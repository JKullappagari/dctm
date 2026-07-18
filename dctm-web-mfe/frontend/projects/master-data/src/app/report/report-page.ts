import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, DestroyRef, effect, inject, input, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormControl, FormGroup, ReactiveFormsModule, UntypedFormGroup } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AgGridAngular } from 'ag-grid-angular';
import { AllCommunityModule, ColDef, ModuleRegistry } from 'ag-grid-community';
import { API_BASE, LookupRow } from '../lookup/lookup.model';

ModuleRegistry.registerModules([AllCommunityModule]);

const REPORTS = 'http://localhost:8084/api/v1/reports';

interface ReportParamDef {
  key: string;
  type: 'int' | 'date' | 'text' | 'bool' | 'loclist';
  label: string;
  optionsPath?: string;
  valueField?: string;
  labelField?: string;
}
interface ReportDef {
  key: string;
  title: string;
  params: ReportParamDef[];
}

/**
 * Generic report page — replaces one legacy SSRS .rdl. Loads the report's filter schema
 * from the reporting service, renders the filters (dropdowns come from the master-data
 * API), runs the underlying stored procedure, and shows the rows in a grid with xlsx
 * export. The key comes from the route (bound via withComponentInputBinding).
 */
@Component({
  selector: 'app-report-page',
  imports: [
    AgGridAngular, ReactiveFormsModule, MatButtonModule, MatIconModule,
    MatFormFieldModule, MatInputModule, MatSelectModule, MatCheckboxModule,
  ],
  template: `
    @if (def(); as report) {
      <div class="page">
        <header><h1>{{ report.title }}</h1></header>

        <form [formGroup]="form" class="filters">
          @for (p of report.params; track p.key) {
            @switch (p.type) {
              @case ('int') {
                <mat-form-field appearance="outline" subscriptSizing="dynamic">
                  <mat-label>{{ p.label }}</mat-label>
                  <mat-select [formControlName]="p.key">
                    <mat-option [value]="0">All</mat-option>
                    @for (o of options()[p.key] ?? []; track o.value) {
                      <mat-option [value]="o.value">{{ o.label }}</mat-option>
                    }
                  </mat-select>
                </mat-form-field>
              }
              @case ('date') {
                <mat-form-field appearance="outline" subscriptSizing="dynamic">
                  <mat-label>{{ p.label }}</mat-label>
                  <input matInput type="date" [formControlName]="p.key" />
                </mat-form-field>
              }
              @case ('bool') {
                <mat-checkbox [formControlName]="p.key">{{ p.label }}</mat-checkbox>
              }
              @default {
                <mat-form-field appearance="outline" subscriptSizing="dynamic">
                  <mat-label>{{ p.label }}{{ p.type === 'loclist' ? ' (comma-sep IDs)' : '' }}</mat-label>
                  <input matInput [formControlName]="p.key" />
                </mat-form-field>
              }
            }
          }
          <button matButton="filled" (click)="run()"><mat-icon>play_arrow</mat-icon> Run</button>
          <button matButton [disabled]="rows().length === 0" (click)="exportExcel()">
            <mat-icon>download</mat-icon> Export
          </button>
        </form>

        @if (ran()) {
          <p class="count">{{ rows().length }} rows</p>
          <ag-grid-angular
            class="grid"
            [rowData]="rows()"
            [columnDefs]="columnDefs()"
            [defaultColDef]="defaultColDef"
            [pagination]="true"
            [paginationPageSize]="50"
          />
        }
      </div>
    } @else {
      <div class="page">
        <p class="status">{{ loadError() ?? 'Loading report…' }}</p>
      </div>
    }
  `,
  styles: `
    .page { padding: 1rem 1.25rem; height: 100%; display: flex; flex-direction: column; box-sizing: border-box; gap: 0.5rem; }
    header h1 { font-size: 1.4rem; font-weight: 500; margin: 0; }
    .filters { display: flex; flex-wrap: wrap; align-items: center; gap: 0.75rem; }
    .count { color: #607d8b; font-size: 0.9rem; margin: 0.25rem 0; }
    .grid { flex: 1; min-height: 380px; }
    .status { color: #607d8b; }
  `,
})
export class ReportPage {
  readonly key = input.required<string>();

  private readonly http = inject(HttpClient);
  private readonly masterBase = inject(API_BASE);
  private readonly snackBar = inject(MatSnackBar);
  private readonly destroyRef = inject(DestroyRef);

  readonly def = signal<ReportDef | null>(null);
  readonly loadError = signal<string | null>(null);
  readonly options = signal<Record<string, { value: unknown; label: string }[]>>({});
  readonly rows = signal<Record<string, unknown>[]>([]);
  readonly columnDefs = signal<ColDef[]>([]);
  readonly ran = signal(false);
  readonly form = new UntypedFormGroup({});
  readonly defaultColDef: ColDef = { sortable: true, filter: true, resizable: true, flex: 1 };

  constructor() {
    // Reload the report definition whenever the route key changes.
    effect(() => {
      const key = this.key();
      this.def.set(null);
      this.loadError.set(null);
      this.http.get<ReportDef[]>(REPORTS).subscribe({
        next: (catalog) => {
          const report = catalog.find((r) => r.key === key);
          if (!report) {
            this.loadError.set(`Report "${key}" was not found.`);
            return;
          }
          this.def.set(report);
          this.rows.set([]);
          this.ran.set(false);

          const controls: Record<string, FormControl> = {};
          for (const p of report.params) {
            controls[p.key] = new FormControl(p.type === 'int' ? 0 : p.type === 'bool' ? false : '');
            if (p.type === 'int' && p.optionsPath) this.loadOptions(p);
          }
          for (const name of Object.keys(this.form.controls)) this.form.removeControl(name);
          for (const [name, ctl] of Object.entries(controls)) this.form.addControl(name, ctl);
        },
        error: (err) =>
          this.loadError.set(
            err.status === 401 ? 'Not signed in.'
            : err.status === 403 ? 'You do not have access to reports.'
            : 'Could not load the report catalog.'),
      });
    });
  }

  private loadOptions(p: ReportParamDef): void {
    this.http.get<LookupRow[]>(`${this.masterBase}/${p.optionsPath}`).subscribe((rows) => {
      this.options.update((cur) => ({
        ...cur,
        [p.key]: rows.map((r) => ({
          value: r[p.valueField ?? 'id'],
          label: String(r[p.labelField ?? 'name'] ?? ''),
        })),
      }));
    });
  }

  run(): void {
    const report = this.def();
    if (!report) return;
    let params = new HttpParams();
    for (const p of report.params) {
      const v = this.form.controls[p.key]?.value;
      params = params.set(p.key, v == null ? '' : String(v));
    }
    this.http
      .get<Record<string, unknown>[]>(`${REPORTS}/${report.key}`, { params })
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (rows) => {
          this.rows.set(rows);
          this.ran.set(true);
          this.columnDefs.set(
            rows.length > 0 ? Object.keys(rows[0]).map((f) => ({ field: f, headerName: f })) : [],
          );
        },
        error: (err) =>
          this.snackBar.open(
            err.status === 403 ? 'You lack access to this report.' : 'Report failed — check the filters.',
            undefined, { duration: 4000 }),
      });
  }

  exportExcel(): void {
    const report = this.def();
    if (!report) return;
    let params = new HttpParams();
    for (const p of report.params) {
      params = params.set(p.key, String(this.form.controls[p.key]?.value ?? ''));
    }
    this.http.get(`${REPORTS}/${report.key}/export`, { params, responseType: 'blob' }).subscribe({
      next: (blob) => {
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `${report.key}.xlsx`;
        a.click();
        URL.revokeObjectURL(url);
      },
      error: () => this.snackBar.open('Export failed.', undefined, { duration: 3500 }),
    });
  }
}

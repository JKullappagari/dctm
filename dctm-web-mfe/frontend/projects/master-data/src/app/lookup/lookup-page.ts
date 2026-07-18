import { Component, DestroyRef, inject, input, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AgGridAngular } from 'ag-grid-angular';
import {
  AllCommunityModule,
  ColDef,
  GridApi,
  GridReadyEvent,
  ModuleRegistry,
  RowDoubleClickedEvent,
} from 'ag-grid-community';
import { LookupApiService } from './lookup-api.service';
import { LookupDialog, LookupDialogData } from './lookup-dialog';
import { LookupConfig, LookupRow } from './lookup.model';

ModuleRegistry.registerModules([AllCommunityModule]);

@Component({
  selector: 'app-lookup-page',
  imports: [AgGridAngular, MatButtonModule, MatIconModule],
  template: `
    <div class="page">
      <header class="toolbar">
        <h1>{{ config().title }}</h1>
        <span class="spacer"></span>
        @if (!config().readOnly) {
          <button matButton="filled" (click)="openDialog()">
            <mat-icon>add</mat-icon> New
          </button>
          @if (!config().noEdit) {
            <button matButton [disabled]="selection().length !== 1" (click)="openDialog(selection()[0])">
              <mat-icon>edit</mat-icon> Edit
            </button>
          }
          <button matButton [disabled]="selection().length === 0" (click)="deleteSelected()">
            <mat-icon>delete</mat-icon> Delete
          </button>
        }
        <button matButton (click)="exportExcel()">
          <mat-icon>download</mat-icon> Export
        </button>
      </header>

      <ag-grid-angular
        class="grid"
        [rowData]="rows()"
        [columnDefs]="columnDefs()"
        [defaultColDef]="defaultColDef"
        [rowSelection]="{ mode: 'multiRow', enableClickSelection: false }"
        [pagination]="true"
        [paginationPageSize]="20"
        (gridReady)="onGridReady($event)"
        (selectionChanged)="onSelectionChanged()"
        (rowDoubleClicked)="onRowDoubleClicked($event)"
      />
    </div>
  `,
  styles: `
    .page {
      display: flex;
      flex-direction: column;
      gap: 0.75rem;
      height: 100%;
      padding: 1rem 1.25rem;
      box-sizing: border-box;
    }
    .toolbar {
      display: flex;
      align-items: center;
      gap: 0.5rem;
    }
    .toolbar h1 {
      font-size: 1.4rem;
      font-weight: 500;
      margin: 0;
    }
    .spacer { flex: 1; }
    .grid { flex: 1; min-height: 420px; }
  `,
})
export class LookupPage {
  /** Provided via route data — one config per legacy .aspx page. */
  readonly config = input.required<LookupConfig>();

  private readonly api = inject(LookupApiService);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);
  private readonly destroyRef = inject(DestroyRef);

  readonly rows = signal<LookupRow[]>([]);
  readonly selection = signal<LookupRow[]>([]);
  readonly columnDefs = signal<ColDef[]>([]);

  readonly defaultColDef: ColDef = { sortable: true, filter: true, resizable: true, flex: 1 };

  private gridApi?: GridApi;

  onGridReady(event: GridReadyEvent): void {
    this.gridApi = event.api;
    this.columnDefs.set(
      this.config().columns.map((c) => ({
        field: c.field,
        headerName: c.headerName,
        ...(c.boolean ? { valueFormatter: (p: { value: unknown }) => (p.value ? 'Yes' : 'No') } : {}),
      })),
    );
    this.reload();
  }

  reload(): void {
    this.api
      .list(this.config().path)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (rows) => this.rows.set(rows),
        error: () => this.notify('Failed to load data — is the Master Data API running?'),
      });
  }

  onSelectionChanged(): void {
    this.selection.set(this.gridApi?.getSelectedRows() ?? []);
  }

  onRowDoubleClicked(event: RowDoubleClickedEvent): void {
    if (event.data && !this.config().readOnly && !this.config().noEdit) {
      this.openDialog(event.data as LookupRow);
    }
  }

  openDialog(row?: LookupRow): void {
    const enrichPath = this.config().editEnrichPath;
    if (row && enrichPath) {
      // The list SP omits data the form needs (e.g. site country/city ids) —
      // fetch the sub-resource and merge before opening.
      const id = row[this.config().idField] as number;
      this.api.enrich(this.config().path, id, enrichPath).subscribe({
        next: (extra) => this.openDialogWithRow({ ...row, ...extra }),
        error: () => this.notify('Failed to load record details.'),
      });
      return;
    }
    this.openDialogWithRow(row);
  }

  private openDialogWithRow(row?: LookupRow): void {
    const data: LookupDialogData = { config: this.config(), row };
    this.dialog
      .open(LookupDialog, { data })
      .afterClosed()
      .subscribe((payload: LookupRow | undefined) => {
        if (!payload) return;
        row ? this.update(row, payload) : this.create(payload);
      });
  }

  private create(payload: LookupRow): void {
    this.api.create(this.config().path, payload).subscribe({
      next: (result) => {
        this.notify(
          result.reactivated
            ? 'A previously deleted entry with this name was restored.'
            : 'Created.',
        );
        this.reload();
      },
      error: (err) => this.notify(err.status === 409 ? 'That name already exists.' : 'Create failed.'),
    });
  }

  private update(row: LookupRow, payload: LookupRow): void {
    const id = row[this.config().idField] as number;
    this.api.update(this.config().path, id, payload).subscribe({
      next: () => {
        this.notify('Saved.');
        this.reload();
      },
      error: (err) => this.notify(err.status === 409 ? 'That name already exists.' : 'Save failed.'),
    });
  }

  deleteSelected(): void {
    const selected = this.selection();
    const names = selected.map((r) => r[this.config().nameField]).join(', ');
    if (!confirm(`Delete ${selected.length} record(s): ${names}?`)) return;

    const ids = selected.map((r) => r[this.config().idField] as number);
    this.api.remove(this.config().path, ids).subscribe({
      next: () => {
        this.notify('Deleted.');
        this.reload();
      },
      error: () => this.notify('Delete failed.'),
    });
  }

  exportExcel(): void {
    // Server-side ClosedXML export — replaces the Infragistics WebExcelExporter.
    // Fetched via HttpClient (Bearer token attached), then saved as a blob download.
    this.api.export(this.config().path).subscribe({
      next: (blob) => {
        const url = URL.createObjectURL(blob);
        const anchor = document.createElement('a');
        anchor.href = url;
        anchor.download = `${this.config().title}.xlsx`;
        anchor.click();
        URL.revokeObjectURL(url);
      },
      error: () => this.notify('Export failed — check your permissions.'),
    });
  }

  private notify(message: string): void {
    this.snackBar.open(message, undefined, { duration: 3500 });
  }
}

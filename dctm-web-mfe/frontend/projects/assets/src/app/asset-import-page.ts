import { HttpClient } from '@angular/common/http';
import { Component, inject, input, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AgGridAngular } from 'ag-grid-angular';
import { AllCommunityModule, ColDef, ModuleRegistry } from 'ag-grid-community';

ModuleRegistry.registerModules([AllCommunityModule]);

const BASE = 'http://localhost:8083/api/v1';

export interface ImportConfig {
  title: string;
  /** Path segment under /import, e.g. 'assets' or 'blades'. */
  path: string;
  hint: string;
}

interface ImportRowResult {
  row: number;
  ref: string;
  status: string;
  message: string | null;
}
interface ImportResult {
  total: number;
  imported: number;
  failed: number;
  rows: ImportRowResult[];
}

/**
 * Generic asset import wizard (Import Asset / Import Blades): download template → fill →
 * upload. Server resolves FK names, inserts via Asset_UpdateNew, returns per-row outcome.
 */
@Component({
  selector: 'app-asset-import-page',
  imports: [AgGridAngular, MatButtonModule, MatIconModule],
  template: `
    <div class="page">
      <header>
        <h1>{{ config().title }}</h1>
        <span class="spacer"></span>
        <button matButton (click)="downloadTemplate()"><mat-icon>download</mat-icon> Template</button>
        <input #fileInput type="file" accept=".xlsx" hidden (change)="onFile($event)" />
        <button matButton="filled" [disabled]="uploading()" (click)="fileInput.click()">
          <mat-icon>upload</mat-icon> {{ uploading() ? 'Importing…' : 'Upload & Import' }}
        </button>
      </header>

      @if (result(); as res) {
        <p class="summary">
          {{ res.total }} rows —
          <span class="ok">{{ res.imported }} imported</span>,
          <span class="err">{{ res.failed }} failed</span>
        </p>
        <ag-grid-angular
          class="grid"
          [rowData]="res.rows"
          [columnDefs]="columnDefs"
          [defaultColDef]="defaultColDef"
          [pagination]="true"
          [paginationPageSize]="20"
        />
      } @else {
        <p class="hint">{{ config().hint }}</p>
      }
    </div>
  `,
  styles: `
    .page { padding: 1rem 1.25rem; height: 100%; display: flex; flex-direction: column; box-sizing: border-box; gap: 0.75rem; }
    header { display: flex; align-items: center; gap: 0.5rem; }
    header h1 { font-size: 1.4rem; font-weight: 500; margin: 0; }
    .spacer { flex: 1; }
    .grid { flex: 1; min-height: 360px; }
    .summary .ok { color: #2e7d32; } .summary .err { color: #c62828; }
    .hint { color: #78909c; max-width: 660px; }
  `,
})
export class AssetImportPage {
  readonly config = input.required<ImportConfig>();

  private readonly http = inject(HttpClient);
  private readonly snackBar = inject(MatSnackBar);

  readonly result = signal<ImportResult | null>(null);
  readonly uploading = signal(false);

  readonly columnDefs: ColDef[] = [
    { field: 'row', headerName: 'Row', width: 90 },
    { field: 'ref', headerName: 'Asset Tag' },
    {
      field: 'status',
      headerName: 'Status',
      cellStyle: (p) => ({ color: p.value === 'imported' ? '#2e7d32' : '#c62828', fontWeight: 500 }),
    },
    { field: 'message', headerName: 'Message', flex: 2 },
  ];
  readonly defaultColDef: ColDef = { sortable: true, filter: true, resizable: true, flex: 1 };

  downloadTemplate(): void {
    this.http.get(`${BASE}/import/${this.config().path}/template`, { responseType: 'blob' }).subscribe({
      next: (blob) => this.saveBlob(blob, `${this.config().path}-template.xlsx`),
      error: () => this.snackBar.open('Template download failed.', undefined, { duration: 3500 }),
    });
  }

  onFile(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) return;
    const form = new FormData();
    form.append('file', file);

    this.uploading.set(true);
    this.http.post<ImportResult>(`${BASE}/import/${this.config().path}`, form).subscribe({
      next: (res) => {
        this.uploading.set(false);
        this.result.set(res);
        this.snackBar.open(`${res.imported} of ${res.total} imported.`, undefined, { duration: 4000 });
      },
      error: (err) => {
        this.uploading.set(false);
        this.snackBar.open(err.status === 403 ? 'You lack the import permission.' : 'Import failed.', undefined, { duration: 4000 });
      },
    });
    input.value = '';
  }

  private saveBlob(blob: Blob, name: string): void {
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = name;
    a.click();
    URL.revokeObjectURL(url);
  }
}

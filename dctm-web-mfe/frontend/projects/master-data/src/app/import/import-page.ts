import { HttpClient } from '@angular/common/http';
import { Component, inject, input, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AgGridAngular } from 'ag-grid-angular';
import { AllCommunityModule, ColDef, ModuleRegistry } from 'ag-grid-community';
import { API_BASE } from '../lookup/lookup.model';

ModuleRegistry.registerModules([AllCommunityModule]);

interface ImportRowResult {
  row: number;
  model: string;
  status: string;
  message: string | null;
}
interface ImportResult {
  total: number;
  imported: number;
  failed: number;
  rows: ImportRowResult[];
}

/** One .xlsx import wizard (retires a legacy Import*.aspx page). */
export interface ImportConfig {
  title: string;
  /** API path under API_BASE, e.g. 'import/applications'. */
  path: string;
  /** Label for the first result column (e.g. 'Application', 'Model'). */
  keyHeader: string;
  hint: string;
  /** File stem for the downloaded template. */
  templateName: string;
}

/**
 * Generic import wizard: download the template, fill it, upload. The server resolves FK
 * names, inserts each row through the matching contract, and returns a per-row outcome
 * (imported / skipped / error) shown in the results grid. One config per legacy page.
 */
@Component({
  selector: 'app-import-page',
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
          <span class="warn">{{ res.total - res.imported - res.failed }} skipped</span>,
          <span class="err">{{ res.failed }} failed</span>
        </p>
        <ag-grid-angular
          class="grid"
          [rowData]="res.rows"
          [columnDefs]="columnDefs()"
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
    .summary .ok { color: #2e7d32; } .summary .warn { color: #ef6c00; } .summary .err { color: #c62828; }
    .hint { color: #78909c; max-width: 640px; }
  `,
})
export class ImportPage {
  readonly config = input.required<ImportConfig>();

  private readonly http = inject(HttpClient);
  private readonly base = inject(API_BASE);
  private readonly snackBar = inject(MatSnackBar);

  readonly result = signal<ImportResult | null>(null);
  readonly uploading = signal(false);

  readonly defaultColDef: ColDef = { sortable: true, filter: true, resizable: true, flex: 1 };
  columnDefs(): ColDef[] {
    return [
      { field: 'row', headerName: 'Row', width: 90 },
      { field: 'model', headerName: this.config().keyHeader },
      {
        field: 'status',
        headerName: 'Status',
        cellStyle: (p) => ({
          color: p.value === 'imported' ? '#2e7d32' : p.value === 'skipped' ? '#ef6c00' : '#c62828',
          fontWeight: 500,
        }),
      },
      { field: 'message', headerName: 'Message', flex: 2 },
    ];
  }

  downloadTemplate(): void {
    this.http.get(`${this.base}/${this.config().path}/template`, { responseType: 'blob' }).subscribe({
      next: (blob) => this.saveBlob(blob, `${this.config().templateName}.xlsx`),
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
    this.http.post<ImportResult>(`${this.base}/${this.config().path}`, form).subscribe({
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

/** Import wizard configs — one per legacy Import*.aspx page. */
export const IMPORT_CONFIGS: Record<string, ImportConfig> = {
  'asset-models': {
    title: 'Import Asset Models',
    path: 'import/asset-models',
    keyHeader: 'Model',
    templateName: 'AssetModels-Template',
    hint:
      'Download the template, fill in one asset model per row (Manufacturer, ModelType, ' +
      'MountType and AirFlowDirection are matched by name), then upload. Rows are ' +
      'independent — invalid rows are reported without blocking the rest.',
  },
  applications: {
    title: 'Import Applications',
    path: 'import/applications',
    keyHeader: 'Application',
    templateName: 'Applications-Template',
    hint:
      'Download the template, fill in one application per row (Business Unit, Application ' +
      'Type, Criticality, Owner and Status are matched by name), then upload. Rows are ' +
      'independent — invalid rows are reported without blocking the rest.',
  },
  'app-asset-map': {
    title: 'Import Asset-App Map',
    path: 'import/app-asset-map',
    keyHeader: 'Application',
    templateName: 'AppAssetMap-Template',
    hint:
      'Download the template and list one Application + HostName per row; both are matched ' +
      'by name. Each application is added to its host’s application map. Importing only ' +
      'adds mappings — it never removes existing ones.',
  },
};

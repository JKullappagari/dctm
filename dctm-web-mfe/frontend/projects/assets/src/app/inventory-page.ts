import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AgGridAngular } from 'ag-grid-angular';
import { AllCommunityModule, ColDef, ICellRendererParams, ModuleRegistry } from 'ag-grid-community';

ModuleRegistry.registerModules([AllCommunityModule]);

const ASSETS = 'http://localhost:8083/api/v1';

interface InventorySession {
  id: string;
  stDateTime: string | null;
  location: string | null;
  site: string | null;
  assetCount: number;
  scannedCount: number;
  missingCount: number;
  overscannedCount: number;
  finalCount: number;
  invUser: string | null;
  deviceName: string | null;
}

/**
 * Inventory Update (InventoryUpdate.aspx): RFID stock-take reconciliation. Enter the
 * location ids scanned by the handheld devices, review each session's counts, and commit
 * to reconcile the scanned assets to their observed locations/status.
 */
@Component({
  selector: 'app-inventory-page',
  imports: [AgGridAngular, ReactiveFormsModule, MatButtonModule, MatIconModule, MatFormFieldModule, MatInputModule],
  template: `
    <div class="page">
      <header>
        <h1>Inventory Update</h1>
        <mat-form-field appearance="outline" subscriptSizing="dynamic" class="locs">
          <mat-label>Location IDs (comma-separated)</mat-label>
          <input matInput [formControl]="locations" placeholder="e.g. 19,21" />
          @if (locations.hasError('pattern') || locations.hasError('required')) {
            <mat-error>Enter one or more location ids, e.g. 19,21</mat-error>
          }
        </mat-form-field>
        <button matButton="filled" [disabled]="locations.invalid" (click)="load()">
          <mat-icon>search</mat-icon> Load Sessions
        </button>
      </header>

      @if (loaded()) {
        <p class="summary">{{ sessions().length }} stock-take session(s)</p>
        <ag-grid-angular
          class="grid"
          [rowData]="sessions()"
          [columnDefs]="columnDefs"
          [defaultColDef]="defaultColDef"
          [pagination]="true"
          [paginationPageSize]="20"
        />
        <p class="hint">
          Commit reconciles a session's scanned assets to their observed locations and
          statuses (misplaced / missing) — it updates asset records, so it is permission-gated.
        </p>
      } @else {
        <p class="hint">
          Stock-take sessions are produced by RFID handheld scanning. Enter the scanned
          location ids and load to review and reconcile them.
        </p>
      }
    </div>
  `,
  styles: `
    .page { padding: 1rem 1.25rem; height: 100%; display: flex; flex-direction: column; box-sizing: border-box; gap: 0.75rem; }
    header { display: flex; align-items: center; gap: 0.75rem; }
    header h1 { font-size: 1.4rem; font-weight: 500; margin: 0; }
    .locs { width: 280px; }
    .grid { flex: 1; min-height: 360px; }
    .hint { color: #78909c; max-width: 680px; }
  `,
})
export class InventoryPage {
  private readonly http = inject(HttpClient);
  private readonly snackBar = inject(MatSnackBar);

  // A non-empty comma-separated list of positive integer location ids.
  readonly locations = new FormControl('', {
    nonNullable: true,
    validators: [Validators.required, Validators.pattern(/^\s*\d+\s*(,\s*\d+\s*)*$/)],
  });
  readonly sessions = signal<InventorySession[]>([]);
  readonly loaded = signal(false);

  readonly columnDefs: ColDef[] = [
    { field: 'site', headerName: 'Site' },
    { field: 'location', headerName: 'Location' },
    { field: 'assetCount', headerName: 'Assets', width: 110 },
    { field: 'scannedCount', headerName: 'Scanned', width: 110 },
    { field: 'missingCount', headerName: 'Missing', width: 110, cellStyle: (p) => (p.value > 0 ? { color: '#c62828' } : null) },
    { field: 'overscannedCount', headerName: 'Over-scanned', width: 130 },
    { field: 'invUser', headerName: 'User' },
    { field: 'deviceName', headerName: 'Device' },
    {
      headerName: '',
      width: 130,
      cellRenderer: (p: ICellRendererParams) => {
        const btn = document.createElement('button');
        btn.textContent = 'Commit';
        btn.className = 'commit-btn';
        btn.onclick = () => this.commit(p.data as InventorySession);
        return btn;
      },
    },
  ];
  readonly defaultColDef: ColDef = { sortable: true, filter: true, resizable: true, flex: 1 };

  load(): void {
    if (this.locations.invalid) return;
    const locs = this.locations.value.replace(/\s/g, '');
    this.http
      .get<InventorySession[]>(`${ASSETS}/inventory`, { params: { locations: locs } })
      .subscribe({
        next: (rows) => {
          this.sessions.set(rows);
          this.loaded.set(true);
        },
        error: () => this.snackBar.open('Failed to load sessions.', undefined, { duration: 3500 }),
      });
  }

  commit(session: InventorySession): void {
    if (!confirm(`Reconcile session at ${session.site} / ${session.location}? This updates asset records.`)) return;
    this.http.post<{ committed: number; message?: string }>(`${ASSETS}/inventory/sessions/${session.id}/commit`, {}).subscribe({
      next: (r) => {
        this.snackBar.open(r.message ?? `Reconciled ${r.committed} asset(s).`, undefined, { duration: 4000 });
        this.load();
      },
      error: (err) => this.snackBar.open(err.status === 403 ? 'You lack the Inventory Update permission.' : 'Commit failed.', undefined, { duration: 4000 }),
    });
  }
}

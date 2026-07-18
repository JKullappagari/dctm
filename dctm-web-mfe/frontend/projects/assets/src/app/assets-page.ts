import { Component, DestroyRef, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialog } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AgGridAngular } from 'ag-grid-angular';
import {
  AllCommunityModule,
  ColDef,
  GridApi,
  GridReadyEvent,
  ModuleRegistry,
  RowSelectedEvent,
} from 'ag-grid-community';
import { debounceTime } from 'rxjs';
import { AssetService, AssetStatus } from './asset.service';
import { LifecycleDialog, dialogDataFor } from './lifecycle-dialog';

ModuleRegistry.registerModules([AllCommunityModule]);

@Component({
  selector: 'app-assets-page',
  imports: [
    AgGridAngular,
    ReactiveFormsModule,
    RouterLink,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatFormFieldModule,
    MatInputModule,
  ],
  template: `
    <div class="page">
      <header class="toolbar">
        <h1>Assets</h1>
        <mat-form-field appearance="outline" class="search" subscriptSizing="dynamic">
          <mat-label>Search ref / name</mat-label>
          <input matInput [formControl]="search" />
        </mat-form-field>
        <button matButton="filled" routerLink="new"><mat-icon>add</mat-icon> New Asset</button>
        <span class="count">{{ total() }} assets</span>
      </header>

      <div class="body">
        <ag-grid-angular
          class="grid"
          [rowData]="rows()"
          [columnDefs]="columnDefs"
          [defaultColDef]="defaultColDef"
          [rowSelection]="{ mode: 'singleRow', checkboxes: false, enableClickSelection: true }"
          [pagination]="true"
          [paginationPageSize]="20"
          (gridReady)="onGridReady($event)"
          (rowSelected)="onRowSelected($event)"
        />

        @if (selected(); as sel) {
          <aside class="lifecycle">
            <h2>Asset #{{ sel.assetId }}</h2>
            <div class="states">
              @for (state of sel.states; track state) {
                <span class="chip">{{ state }}</span>
              }
            </div>
            <h3>Actions</h3>
            @if (sel.availableActions.length === 0) {
              <p class="none">No actions available.</p>
            }
            @for (action of sel.availableActions; track action) {
              <button matButton="outlined" class="action" (click)="runAction(sel.assetId, action)">
                {{ label(action) }}
              </button>
            }
          </aside>
        }
      </div>
    </div>
  `,
  styles: `
    .page { display: flex; flex-direction: column; height: 100%; padding: 1rem 1.25rem; box-sizing: border-box; gap: 0.75rem; }
    .toolbar { display: flex; align-items: center; gap: 1rem; }
    .toolbar h1 { font-size: 1.4rem; font-weight: 500; margin: 0; }
    .search { width: 260px; }
    .count { margin-left: auto; color: #607d8b; font-size: 0.9rem; }
    .body { display: flex; gap: 1rem; flex: 1; min-height: 0; }
    .grid { flex: 1; min-height: 420px; }
    .lifecycle { width: 260px; flex-shrink: 0; background: #fff; border: 1px solid #e0e0e0; border-radius: 10px; padding: 1rem; overflow: auto; }
    .lifecycle h2 { font-size: 1.05rem; margin: 0 0 0.5rem; }
    .lifecycle h3 { font-size: 0.8rem; text-transform: uppercase; letter-spacing: 0.05em; color: #90a4ae; margin: 1rem 0 0.5rem; }
    .states { display: flex; flex-wrap: wrap; gap: 0.35rem; }
    .chip { background: #e3f2fd; color: #1565c0; border-radius: 12px; padding: 0.15rem 0.6rem; font-size: 0.8rem; }
    .action { display: block; width: 100%; margin-bottom: 0.4rem; }
    .none { color: #90a4ae; font-size: 0.85rem; }
  `,
})
export class AssetsPage {
  private readonly api = inject(AssetService);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);
  private readonly destroyRef = inject(DestroyRef);

  readonly rows = signal<Record<string, unknown>[]>([]);
  readonly total = signal(0);
  readonly selected = signal<AssetStatus | null>(null);
  readonly search = new FormControl('', { nonNullable: true });

  private gridApi?: GridApi;

  readonly columnDefs: ColDef[] = [
    { field: 'refNumber', headerName: 'Ref #' },
    { field: 'assetName', headerName: 'Name' },
    { field: 'modelName', headerName: 'Model' },
    { field: 'mfgName', headerName: 'Manufacturer' },
    { field: 'site', headerName: 'Site' },
    { field: 'status', headerName: 'Status' },
  ];
  readonly defaultColDef: ColDef = { sortable: true, filter: true, resizable: true, flex: 1 };

  constructor() {
    this.search.valueChanges
      .pipe(debounceTime(300), takeUntilDestroyed(this.destroyRef))
      .subscribe(() => {
        this.selected.set(null);
        this.reload();
      });
  }

  onGridReady(event: GridReadyEvent): void {
    this.gridApi = event.api;
    this.reload();
  }

  reload(): void {
    this.api
      .search(this.search.value, 1, 200)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (paged) => {
          this.rows.set(paged.items as unknown as Record<string, unknown>[]);
          this.total.set(paged.total);
        },
        error: () => this.notify('Failed to load assets — is the Asset API running?'),
      });
  }

  onRowSelected(event: RowSelectedEvent): void {
    if (!event.node.isSelected() || !event.data) return;
    const id = (event.data as { assetID: number }).assetID;
    this.api.status(id).subscribe((status) => this.selected.set(status));
  }

  label(action: string): string {
    return dialogDataFor(action).label;
  }

  runAction(id: number, action: string): void {
    this.dialog
      .open(LifecycleDialog, { data: dialogDataFor(action) })
      .afterClosed()
      .subscribe((body: Record<string, unknown> | undefined) => {
        if (!body) return;
        this.api.transition(id, action, body).subscribe({
          next: (status) => {
            this.selected.set(status);
            this.notify(`${this.label(action)} applied.`);
            this.reload();
          },
          error: (err) =>
            this.notify(err.status === 409 ? 'That action is not valid for the current state.' : 'Action failed.'),
        });
      });
  }

  private notify(message: string): void {
    this.snackBar.open(message, undefined, { duration: 3500 });
  }
}

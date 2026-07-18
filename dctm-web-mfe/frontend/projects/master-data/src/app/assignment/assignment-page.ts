import { Component, computed, inject, input, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { LookupRow } from '../lookup/lookup.model';
import { AssignmentConfig, AssignmentItem } from './assignment.model';
import { AssignmentService } from './assignment.service';

@Component({
  selector: 'app-assignment-page',
  imports: [MatButtonModule, MatIconModule, MatSelectModule, MatFormFieldModule],
  template: `
    <div class="page">
      <header>
        <h1>{{ config().title }}</h1>
        <mat-form-field appearance="outline" subscriptSizing="dynamic" class="parent">
          <mat-label>{{ config().parentLabel }}</mat-label>
          <mat-select [value]="parentId()" (valueChange)="selectParent($event)">
            @for (p of parents(); track p[config().parentIdField]) {
              <mat-option [value]="p[config().parentIdField]">{{ p[config().parentNameField] }}</mat-option>
            }
          </mat-select>
        </mat-form-field>
      </header>

      @if (parentId() !== null) {
        <div class="dual">
          <div class="list-box">
            <div class="list-title">Available</div>
            <div class="list">
              @for (item of available(); track item.id) {
                <button type="button" class="item" [class.sel]="availSel().has(item.id)" (click)="toggle(availSel(), item.id)">
                  {{ item.name }}
                </button>
              }
            </div>
          </div>

          <div class="controls">
            <button matIconButton [disabled]="availSel().size === 0" (click)="assign()" title="Assign">
              <mat-icon>chevron_right</mat-icon>
            </button>
            <button matIconButton [disabled]="assignedSel().size === 0" (click)="unassign()" title="Remove">
              <mat-icon>chevron_left</mat-icon>
            </button>
          </div>

          <div class="list-box">
            <div class="list-title">Assigned {{ config().itemLabel }}</div>
            <div class="list">
              @for (item of assigned(); track item.id) {
                <button type="button" class="item" [class.sel]="assignedSel().has(item.id)" (click)="toggle(assignedSel(), item.id)">
                  {{ item.name }}
                </button>
              }
            </div>
          </div>
        </div>

        <div class="actions">
          <button matButton="filled" [disabled]="!dirty()" (click)="save()">Save</button>
        </div>
      }
    </div>
  `,
  styles: `
    .page { padding: 1rem 1.25rem; display: flex; flex-direction: column; gap: 1rem; height: 100%; box-sizing: border-box; }
    header { display: flex; align-items: center; gap: 1.5rem; }
    header h1 { font-size: 1.4rem; font-weight: 500; margin: 0; }
    .parent { width: 260px; }
    .dual { display: flex; gap: 1rem; align-items: stretch; flex: 1; min-height: 0; }
    .list-box { flex: 1; display: flex; flex-direction: column; border: 1px solid #e0e0e0; border-radius: 10px; overflow: hidden; background: #fff; }
    .list-title { padding: 0.6rem 0.9rem; background: #f5f5f5; font-weight: 500; font-size: 0.9rem; }
    .list { flex: 1; overflow: auto; padding: 0.4rem; }
    .item { display: block; width: 100%; text-align: left; border: 0; background: none; padding: 0.45rem 0.6rem; border-radius: 6px; cursor: pointer; font-size: 0.9rem; }
    .item:hover { background: #f0f4f8; }
    .item.sel { background: #2196f3; color: #fff; }
    .controls { display: flex; flex-direction: column; justify-content: center; gap: 0.5rem; }
    .actions { display: flex; justify-content: flex-end; }
  `,
})
export class AssignmentPage {
  readonly config = input.required<AssignmentConfig>();

  private readonly api = inject(AssignmentService);
  private readonly snackBar = inject(MatSnackBar);

  readonly parents = signal<LookupRow[]>([]);
  // number for int-keyed parents (BU, site); string for GUID-keyed parents (host).
  readonly parentId = signal<number | string | null>(null);
  readonly available = signal<AssignmentItem[]>([]);
  readonly assigned = signal<AssignmentItem[]>([]);
  readonly availSel = signal<Set<number>>(new Set());
  readonly assignedSel = signal<Set<number>>(new Set());

  // A signal so `dirty` recomputes when the saved baseline is reset after a save.
  private readonly originalAssigned = signal<Set<number>>(new Set());
  readonly dirty = computed(() => {
    const now = new Set(this.assigned().map((i) => i.id));
    const base = this.originalAssigned();
    return now.size !== base.size || [...now].some((id) => !base.has(id));
  });

  constructor() {
    // config() is available after input binding; load parents once.
    queueMicrotask(() => {
      this.api.parents(this.config().parentPath).subscribe((rows) => this.parents.set(rows));
    });
  }

  selectParent(id: number | string): void {
    this.parentId.set(id);
    this.api.load(this.config().path, id).subscribe((view) => {
      this.available.set(view.available);
      this.assigned.set(view.assigned);
      this.originalAssigned.set(new Set(view.assigned.map((i) => i.id)));
      this.availSel.set(new Set());
      this.assignedSel.set(new Set());
    });
  }

  toggle(set: Set<number>, id: number): void {
    const next = new Set(set);
    next.has(id) ? next.delete(id) : next.add(id);
    // Reassign the matching signal.
    if (set === this.availSel()) this.availSel.set(next);
    else this.assignedSel.set(next);
  }

  assign(): void {
    const moving = this.available().filter((i) => this.availSel().has(i.id));
    this.assigned.set([...this.assigned(), ...moving].sort((a, b) => a.name.localeCompare(b.name)));
    this.available.set(this.available().filter((i) => !this.availSel().has(i.id)));
    this.availSel.set(new Set());
  }

  unassign(): void {
    const moving = this.assigned().filter((i) => this.assignedSel().has(i.id));
    this.available.set([...this.available(), ...moving].sort((a, b) => a.name.localeCompare(b.name)));
    this.assigned.set(this.assigned().filter((i) => !this.assignedSel().has(i.id)));
    this.assignedSel.set(new Set());
  }

  save(): void {
    const ids = this.assigned().map((i) => i.id);
    this.api.save(this.config().path, this.parentId()!, ids).subscribe({
      next: () => {
        this.originalAssigned.set(new Set(ids));
        this.snackBar.open('Assignment saved.', undefined, { duration: 3000 });
      },
      error: () => this.snackBar.open('Save failed.', undefined, { duration: 3500 }),
    });
  }
}

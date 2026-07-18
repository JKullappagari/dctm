import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { forkJoin } from 'rxjs';
import { API_BASE, LookupRow } from '../lookup/lookup.model';

const IDENTITY_BASE = 'http://localhost:8082/api/v1';

interface RightItem {
  rightsID: number;
  rights: string;
  granted: boolean;
}

interface ModuleRights {
  moduleId: number;
  module: string;
  rights: RightItem[];
}

interface MainModuleSection {
  title: string;
  modules: ModuleRights[];
}

/**
 * Retires GroupModuleRightsAssignment.aspx: the group ↔ module-rights matrix that
 * drives the security-trimmed menu and every API permission check. Saves go through
 * the legacy per-module SP; only modules whose rights changed are saved.
 */
@Component({
  selector: 'app-group-rights-page',
  imports: [MatButtonModule, MatCheckboxModule, MatFormFieldModule, MatSelectModule],
  template: `
    <div class="page">
      <header>
        <h1>Group Module Rights</h1>
        <mat-form-field appearance="outline" subscriptSizing="dynamic" class="picker">
          <mat-label>Group</mat-label>
          <mat-select [value]="groupId()" (valueChange)="selectGroup($event)">
            @for (g of groups(); track g['groupID']) {
              <mat-option [value]="g['groupID']">{{ g['group'] }}</mat-option>
            }
          </mat-select>
        </mat-form-field>
        <span class="spacer"></span>
        @if (groupId() !== null) {
          <button matButton="filled" [disabled]="dirtyModules().size === 0 || saving()" (click)="save()">
            Save {{ dirtyModules().size > 0 ? '(' + dirtyModules().size + ' modules)' : '' }}
          </button>
        }
      </header>

      @for (section of sections(); track section.title) {
        <div class="section">
          <h2>{{ section.title }}</h2>
          <div class="modules">
            @for (m of section.modules; track m.moduleId) {
              <div class="module" [class.dirty]="dirtyModules().has(m.moduleId)">
                <div class="module-name">{{ m.module }}</div>
                <div class="rights">
                  @for (r of m.rights; track r.rightsID) {
                    <mat-checkbox [checked]="r.granted" (change)="toggle(m, r, $event.checked)">
                      {{ r.rights }}
                    </mat-checkbox>
                  }
                </div>
              </div>
            }
          </div>
        </div>
      }
    </div>
  `,
  styles: `
    .page { padding: 1rem 1.25rem; height: 100%; overflow: auto; box-sizing: border-box; }
    header { display: flex; align-items: center; gap: 1rem; margin-bottom: 1rem; position: sticky; top: 0; background: #f4f6f8; z-index: 5; padding: 0.25rem 0; }
    header h1 { font-size: 1.4rem; font-weight: 500; margin: 0; }
    .picker { width: 240px; }
    .spacer { flex: 1; }
    .section h2 { font-size: 0.85rem; text-transform: uppercase; letter-spacing: 0.06em; color: #78909c; margin: 1rem 0 0.5rem; }
    .modules { display: flex; flex-direction: column; gap: 0.5rem; }
    .module { background: #fff; border: 1px solid #e0e0e0; border-radius: 10px; padding: 0.6rem 0.9rem; }
    .module.dirty { border-color: #2196f3; }
    .module-name { font-weight: 500; margin-bottom: 0.25rem; }
    .rights { display: flex; flex-wrap: wrap; gap: 0 1rem; }
  `,
})
export class GroupRightsPage {
  private readonly http = inject(HttpClient);
  private readonly base = inject(API_BASE);
  private readonly snackBar = inject(MatSnackBar);

  readonly groups = signal<LookupRow[]>([]);
  readonly groupId = signal<number | null>(null);
  readonly sections = signal<MainModuleSection[]>([]);
  readonly dirtyModules = signal<Set<number>>(new Set());
  readonly saving = signal(false);

  constructor() {
    this.http.get<LookupRow[]>(`${this.base}/groups`).subscribe((rows) => this.groups.set(rows));
  }

  selectGroup(id: number): void {
    this.groupId.set(id);
    this.dirtyModules.set(new Set());
    this.http
      .get<MainModuleSection[]>(`${IDENTITY_BASE}/groups/${id}/module-rights`)
      .subscribe((tree) => this.sections.set(tree));
  }

  toggle(module: ModuleRights, right: RightItem, checked: boolean): void {
    right.granted = checked;
    const dirty = new Set(this.dirtyModules());
    dirty.add(module.moduleId);
    this.dirtyModules.set(dirty);
  }

  save(): void {
    const id = this.groupId()!;
    const dirty = [...this.dirtyModules()];
    const puts = dirty.map((moduleId) => {
      const module = this.sections().flatMap((s) => s.modules).find((m) => m.moduleId === moduleId)!;
      const rightIds = module.rights.filter((r) => r.granted).map((r) => r.rightsID);
      return this.http.put<void>(`${IDENTITY_BASE}/groups/${id}/modules/${moduleId}/rights`, { rightIds });
    });

    this.saving.set(true);
    forkJoin(puts).subscribe({
      next: () => {
        this.saving.set(false);
        this.dirtyModules.set(new Set());
        this.snackBar.open(`Saved rights for ${dirty.length} module(s).`, undefined, { duration: 3000 });
      },
      error: () => {
        this.saving.set(false);
        this.snackBar.open('Save failed — check your permissions.', undefined, { duration: 4000 });
      },
    });
  }
}

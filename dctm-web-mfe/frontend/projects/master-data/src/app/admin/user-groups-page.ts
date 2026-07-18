import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { API_BASE, LookupRow } from '../lookup/lookup.model';

const IDENTITY_BASE = 'http://localhost:8082/api/v1';

interface UserGroup {
  groupID: number;
  group: string;
  member: boolean;
}

/**
 * The group-membership slice of ManageUsers.aspx: pick a user, tick the groups they
 * belong to. Identity lifecycle (create user, password, enable/disable) lives in
 * Keycloak; group membership stays in the legacy tables because it drives permissions.
 */
@Component({
  selector: 'app-user-groups-page',
  imports: [MatButtonModule, MatCheckboxModule, MatFormFieldModule, MatSelectModule],
  template: `
    <div class="page">
      <header>
        <h1>User Group Membership</h1>
        <mat-form-field appearance="outline" subscriptSizing="dynamic" class="picker">
          <mat-label>User</mat-label>
          <mat-select [value]="userId()" (valueChange)="selectUser($event)">
            @for (u of users(); track u['userID']) {
              <mat-option [value]="u['userID']">{{ u['loginName'] }} — {{ u['name'] }}</mat-option>
            }
          </mat-select>
        </mat-form-field>
        <span class="spacer"></span>
        @if (userId() !== null) {
          <button matButton="filled" [disabled]="!dirty()" (click)="save()">Save</button>
        }
      </header>

      @if (userId() !== null) {
        <div class="card">
          @for (g of groups(); track g.groupID) {
            <mat-checkbox [checked]="g.member" (change)="g.member = $event.checked; dirty.set(true)">
              {{ g.group }}
            </mat-checkbox>
          }
        </div>
        <p class="hint">User accounts and passwords are managed in Keycloak; membership here controls legacy module rights.</p>
      }
    </div>
  `,
  styles: `
    .page { padding: 1rem 1.25rem; box-sizing: border-box; }
    header { display: flex; align-items: center; gap: 1rem; margin-bottom: 1rem; }
    header h1 { font-size: 1.4rem; font-weight: 500; margin: 0; }
    .picker { width: 320px; }
    .spacer { flex: 1; }
    .card { background: #fff; border: 1px solid #e0e0e0; border-radius: 10px; padding: 0.9rem 1.1rem; display: flex; flex-direction: column; gap: 0.25rem; max-width: 460px; }
    .hint { color: #90a4ae; font-size: 0.82rem; max-width: 460px; }
  `,
})
export class UserGroupsPage {
  private readonly http = inject(HttpClient);
  private readonly base = inject(API_BASE);
  private readonly snackBar = inject(MatSnackBar);

  readonly users = signal<LookupRow[]>([]);
  readonly userId = signal<number | null>(null);
  readonly groups = signal<UserGroup[]>([]);
  readonly dirty = signal(false);

  constructor() {
    this.http.get<LookupRow[]>(`${this.base}/users`).subscribe((rows) => this.users.set(rows));
  }

  selectUser(id: number): void {
    this.userId.set(id);
    this.dirty.set(false);
    this.http
      .get<UserGroup[]>(`${IDENTITY_BASE}/users/${id}/groups`)
      .subscribe((groups) => this.groups.set(groups));
  }

  save(): void {
    const groupIds = this.groups().filter((g) => g.member).map((g) => g.groupID);
    this.http.put<void>(`${IDENTITY_BASE}/users/${this.userId()}/groups`, { groupIds }).subscribe({
      next: () => {
        this.dirty.set(false);
        this.snackBar.open('Membership saved.', undefined, { duration: 3000 });
      },
      error: () => this.snackBar.open('Save failed — check your permissions.', undefined, { duration: 4000 }),
    });
  }
}

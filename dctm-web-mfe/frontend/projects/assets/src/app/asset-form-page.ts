import { Location } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, computed, inject, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';

const MASTER = 'http://localhost:8081/api/v1';
const ASSETS = 'http://localhost:8083/api/v1';

type Row = Record<string, any>;

/**
 * Retires CreateAsset.aspx (core fields): manufacturer → model cascade, site →
 * location cascade (locations assigned to the site), owner/tech/orientation, rack
 * position. Saves via the legacy Asset_UpdateNew SP; its configurable uniqueness
 * rule and MessageCode feedback surface as the error banner.
 */
@Component({
  selector: 'app-asset-form-page',
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule],
  template: `
    <div class="page">
      <h1>Create Asset</h1>
      <form [formGroup]="form" class="grid">
        <mat-form-field appearance="outline">
          <mat-label>Ref Number</mat-label>
          <input matInput formControlName="refNumber" maxlength="50" required />
          @if (form.controls.refNumber.hasError('required')) {
            <mat-error>Ref Number is required</mat-error>
          }
        </mat-form-field>
        <mat-form-field appearance="outline">
          <mat-label>Asset Name</mat-label>
          <input matInput formControlName="assetName" maxlength="100" />
        </mat-form-field>

        <mat-form-field appearance="outline">
          <mat-label>Manufacturer</mat-label>
          <mat-select formControlName="mfgId">
            @for (m of manufacturers(); track m['mfgID']) {
              <mat-option [value]="m['mfgID']">{{ m['mfgName'] }}</mat-option>
            }
          </mat-select>
        </mat-form-field>
        <mat-form-field appearance="outline">
          <mat-label>Model</mat-label>
          <mat-select formControlName="modelId" required>
            @for (m of filteredModels(); track m['modelID']) {
              <mat-option [value]="m['modelID']">{{ m['modelName'] }}</mat-option>
            }
          </mat-select>
          @if (form.controls.modelId.hasError('required')) {
            <mat-error>Model is required</mat-error>
          }
        </mat-form-field>

        <mat-form-field appearance="outline">
          <mat-label>Site</mat-label>
          <mat-select formControlName="siteId" (valueChange)="loadLocations($event)" required>
            @for (s of sites(); track s['siteID']) {
              <mat-option [value]="s['siteID']">{{ s['site'] }}</mat-option>
            }
          </mat-select>
          @if (form.controls.siteId.hasError('required')) {
            <mat-error>Site is required</mat-error>
          }
        </mat-form-field>
        <mat-form-field appearance="outline">
          <mat-label>Location</mat-label>
          <mat-select formControlName="locationId" required>
            @for (l of locations(); track l.id) {
              <mat-option [value]="l.id">{{ l.name }}</mat-option>
            }
          </mat-select>
          @if (form.controls.locationId.hasError('required')) {
            <mat-error>Location is required</mat-error>
          }
        </mat-form-field>

        <mat-form-field appearance="outline">
          <mat-label>Owner</mat-label>
          <mat-select formControlName="ownerId">
            <mat-option [value]="null">—</mat-option>
            @for (o of owners(); track o['ownerID']) {
              <mat-option [value]="o['ownerID']">{{ o['ownerFirstName'] }} {{ o['ownerLastName'] }}</mat-option>
            }
          </mat-select>
        </mat-form-field>
        <mat-form-field appearance="outline">
          <mat-label>Orientation</mat-label>
          <mat-select formControlName="orientation">
            <mat-option [value]="''">—</mat-option>
            @for (o of orientations(); track o['orientationID']) {
              <mat-option [value]="o['orientationName']">{{ o['orientationName'] }}</mat-option>
            }
          </mat-select>
        </mat-form-field>

        <mat-form-field appearance="outline">
          <mat-label>Rack / Stand</mat-label>
          <input matInput formControlName="rackOrStand" maxlength="50" />
        </mat-form-field>
        <mat-form-field appearance="outline">
          <mat-label>Start Position (U)</mat-label>
          <input matInput type="number" inputmode="numeric" formControlName="startPos" min="0"
                 (keydown)="onlyInteger($event)" />
          @if (form.controls.startPos.hasError('min')) {
            <mat-error>Start Position cannot be negative</mat-error>
          }
        </mat-form-field>
        <mat-form-field appearance="outline">
          <mat-label>Height (RUs)</mat-label>
          <input matInput type="number" inputmode="numeric" formControlName="noOfRUs" min="1"
                 (keydown)="onlyInteger($event)" />
          @if (form.controls.noOfRUs.hasError('min')) {
            <mat-error>Height must be at least 1</mat-error>
          }
        </mat-form-field>
      </form>

      @if (error()) {
        <p class="error">{{ error() }}</p>
      }
      <div class="actions">
        <button matButton (click)="cancel()">Cancel</button>
        <button matButton="filled" [disabled]="form.invalid || saving()" (click)="save()">Create</button>
      </div>
    </div>
  `,
  styles: `
    .page { padding: 1rem 1.25rem; max-width: 880px; box-sizing: border-box; }
    h1 { font-size: 1.4rem; font-weight: 500; margin: 0 0 1rem; }
    .grid { display: grid; grid-template-columns: 1fr 1fr; gap: 0.25rem 1rem; }
    .actions { display: flex; justify-content: flex-end; gap: 0.5rem; margin-top: 0.5rem; }
    .error { color: #c62828; }
  `,
})
export class AssetFormPage {
  private readonly http = inject(HttpClient);
  private readonly location = inject(Location);
  private readonly snackBar = inject(MatSnackBar);

  readonly manufacturers = signal<Row[]>([]);
  readonly models = signal<Row[]>([]);
  readonly sites = signal<Row[]>([]);
  readonly locations = signal<{ id: number; name: string }[]>([]);
  readonly owners = signal<Row[]>([]);
  readonly orientations = signal<Row[]>([]);
  readonly error = signal<string | null>(null);
  readonly saving = signal(false);

  readonly form = new FormGroup({
    refNumber: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    assetName: new FormControl(''),
    mfgId: new FormControl<number | null>(null),
    modelId: new FormControl<number | null>(null, [Validators.required]),
    siteId: new FormControl<number | null>(null, [Validators.required]),
    locationId: new FormControl<number | null>(null, [Validators.required]),
    ownerId: new FormControl<number | null>(null),
    orientation: new FormControl(''),
    rackOrStand: new FormControl(''),
    startPos: new FormControl<number | null>(null, [Validators.min(0)]),
    noOfRUs: new FormControl<number | null>(null, [Validators.min(1)]),
  });

  private readonly mfgId = signal<number | null>(null);
  readonly filteredModels = computed(() => {
    const mfg = this.mfgId();
    return mfg == null ? this.models() : this.models().filter((m) => m['mfgID'] === mfg);
  });

  constructor() {
    this.http.get<Row[]>(`${MASTER}/manufacturers`).subscribe((r) => this.manufacturers.set(r));
    this.http.get<Row[]>(`${MASTER}/asset-models`).subscribe((r) => this.models.set(r));
    this.http.get<Row[]>(`${MASTER}/sites`).subscribe((r) => this.sites.set(r));
    this.http.get<Row[]>(`${MASTER}/owners`).subscribe((r) => this.owners.set(r));
    this.http.get<Row[]>(`${MASTER}/ref/orientations`).subscribe((r) => this.orientations.set(r));

    this.form.controls.mfgId.valueChanges.subscribe((v) => {
      this.mfgId.set(v);
      this.form.controls.modelId.setValue(null);
    });
  }

  /**
   * Keeps type=number inputs to whole numbers — the native control otherwise accepts
   * 'e', '+', '-' and '.' keystrokes, which the U-position/RU-height fields must reject.
   */
  onlyInteger(event: KeyboardEvent): void {
    const allowed = ['Backspace', 'Delete', 'Tab', 'Enter', 'Escape', 'Home', 'End',
      'ArrowLeft', 'ArrowRight', 'ArrowUp', 'ArrowDown'];
    if (allowed.includes(event.key) || event.ctrlKey || event.metaKey) return;
    if (!/^\d$/.test(event.key)) event.preventDefault();
  }

  loadLocations(siteId: number): void {
    this.form.controls.locationId.setValue(null);
    // Locations assigned to the site (same source the legacy site→location filter used).
    this.http
      .get<{ assigned: { id: number; name: string }[] }>(`${MASTER}/assignments/site-locations/${siteId}`)
      .subscribe((v) => this.locations.set(v.assigned));
  }

  save(): void {
    this.error.set(null);
    this.saving.set(true);
    const v = this.form.value;
    this.http
      .post<{ id: number }>(`${ASSETS}/assets`, {
        refNumber: v.refNumber,
        assetName: v.assetName,
        modelId: v.modelId,
        siteId: v.siteId,
        locationId: v.locationId,
        ownerId: v.ownerId,
        orientation: v.orientation,
        rackOrStand: v.rackOrStand,
        startPos: v.startPos,
        noOfRUs: v.noOfRUs,
      })
      .subscribe({
        next: (r) => {
          this.saving.set(false);
          this.snackBar.open(`Asset #${r.id} created.`, undefined, { duration: 3000 });
          this.location.back(); // works both in the shell (/assets/new) and standalone (/new)
        },
        error: (err) => {
          this.saving.set(false);
          this.error.set(err.error?.error ?? 'Create failed.');
        },
      });
  }

  cancel(): void {
    this.location.back();
  }
}

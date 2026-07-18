import { Component, inject, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { LookupApiService } from './lookup-api.service';
import { LookupConfig, LookupField, LookupRow } from './lookup.model';

export interface LookupDialogData {
  config: LookupConfig;
  /** Undefined for "new", the grid row for "edit". */
  row?: LookupRow;
}

@Component({
  selector: 'app-lookup-dialog',
  imports: [
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatCheckboxModule,
    MatSelectModule,
    MatButtonModule,
  ],
  template: `
    <h2 mat-dialog-title>{{ data.row ? 'Edit' : 'New' }} {{ data.config.title }}</h2>
    <mat-dialog-content>
      <form [formGroup]="form" class="lookup-form">
        @for (field of data.config.fields; track field.key) {
          @switch (field.type) {
            @case ('checkbox') {
              <mat-checkbox [formControlName]="field.key">{{ field.label }}</mat-checkbox>
            }
            @case ('textarea') {
              <mat-form-field appearance="outline">
                <mat-label>{{ field.label }}</mat-label>
                <textarea matInput [formControlName]="field.key" rows="3"></textarea>
              </mat-form-field>
            }
            @case ('select') {
              <mat-form-field appearance="outline">
                <mat-label>{{ field.label }}</mat-label>
                <mat-select [formControlName]="field.key">
                  @if (!field.required) {
                    <mat-option [value]="null">—</mat-option>
                  }
                  @for (option of options()[field.key] ?? []; track option.value) {
                    <mat-option [value]="option.value">{{ option.label }}</mat-option>
                  }
                </mat-select>
                <mat-error>{{ errorFor(field) }}</mat-error>
              </mat-form-field>
            }
            @case ('number') {
              <mat-form-field appearance="outline">
                <mat-label>{{ field.label }}</mat-label>
                <input matInput type="number" [formControlName]="field.key"
                       [min]="field.min ?? null" [max]="field.max ?? null" />
                <mat-error>{{ errorFor(field) }}</mat-error>
              </mat-form-field>
            }
            @case ('date') {
              <mat-form-field appearance="outline">
                <mat-label>{{ field.label }}</mat-label>
                <input matInput type="date" [formControlName]="field.key" />
                <mat-error>{{ errorFor(field) }}</mat-error>
              </mat-form-field>
            }
            @default {
              <mat-form-field appearance="outline">
                <mat-label>{{ field.label }}</mat-label>
                <input matInput [formControlName]="field.key" [maxlength]="field.maxLength ?? 200" />
                @if (field.maxLength) {
                  <mat-hint align="end">{{ (form.controls[field.key].value?.length ?? 0) }}/{{ field.maxLength }}</mat-hint>
                }
                <mat-error>{{ errorFor(field) }}</mat-error>
              </mat-form-field>
            }
          }
        }
      </form>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button matButton mat-dialog-close>Cancel</button>
      <button matButton="filled" [disabled]="form.invalid" (click)="save()">Save</button>
    </mat-dialog-actions>
  `,
  styles: `
    .lookup-form {
      display: flex;
      flex-direction: column;
      gap: 0.75rem;
      min-width: 380px;
      padding-top: 0.5rem;
    }
  `,
})
export class LookupDialog {
  readonly data = inject<LookupDialogData>(MAT_DIALOG_DATA);
  private readonly ref = inject(MatDialogRef<LookupDialog>);
  private readonly api = inject(LookupApiService);

  /** Loaded options per select-field key. */
  readonly options = signal<Record<string, { value: unknown; label: string }[]>>({});

  readonly form = new FormGroup(
    Object.fromEntries(this.data.config.fields.map((f) => [f.key, this.buildControl(f)])),
  );

  /** Raw option rows per select field — kept for optionsFilter/disabledWhen rules. */
  private readonly rawRows: Record<string, LookupRow[]> = {};

  constructor() {
    for (const field of this.data.config.fields) {
      if (field.type !== 'select' || !field.optionsPath) continue;

      if (field.optionsFilter || field.disabledWhen) {
        // Client-side rule: load the full list once, then filter/enable. A static rule
        // (e.g. Audit Cycle = Rooms only) has no dependsOn; a dynamic one (Location
        // parent hierarchy) re-applies whenever the driving field changes.
        this.loadOptions(field, undefined, () => this.applyRules(field));
        if (field.dependsOn) {
          this.form.controls[field.dependsOn].valueChanges.subscribe(() => {
            this.form.controls[field.key].setValue(null);
            this.applyRules(field);
          });
        }
        continue;
      }

      if (!field.dependsOn) {
        this.loadOptions(field);
        continue;
      }

      const parent = this.form.controls[field.dependsOn];

      // Server-side cascade: (re)load when the parent changes, clear a stale choice,
      // and load immediately when editing a row that already has a parent value.
      parent.valueChanges.subscribe((parentValue) => {
        this.form.controls[field.key].setValue(null);
        this.options.update((current) => ({ ...current, [field.key]: [] }));
        if (parentValue != null) this.loadOptions(field, parentValue);
      });
      if (parent.value != null) this.loadOptions(field, parent.value);
    }
  }

  private loadOptions(field: LookupField, parentValue?: unknown, done?: () => void): void {
    const params =
      field.optionsParam && parentValue != null
        ? { [field.optionsParam]: String(parentValue) }
        : undefined;
    this.api.list(field.optionsPath!, params).subscribe((rows) => {
      this.rawRows[field.key] = rows;
      this.options.update((current) => ({
        ...current,
        [field.key]: rows.map((r) => ({
          value: r[field.optionValue ?? 'id'],
          label: String(r[field.optionLabel ?? 'name'] ?? ''),
        })),
      }));
      done?.();
      // Rule-driven fields may depend on this list to resolve ids → names.
      for (const other of this.data.config.fields) {
        if (other !== field && (other.optionsFilter || other.disabledWhen)) this.applyRules(other);
      }
    });
  }

  private applyRules(field: LookupField): void {
    const rows = this.rawRows[field.key] ?? [];
    const form = this.form.getRawValue() as Record<string, unknown>;
    const control = this.form.controls[field.key];

    if (field.disabledWhen?.(form, this.rawRows)) {
      if (control.value != null) control.setValue(null, { emitEvent: false });
      control.disable({ emitEvent: false });
    } else {
      control.enable({ emitEvent: false });
    }

    const visible = field.optionsFilter
      ? rows.filter((r) => field.optionsFilter!(r, form, this.rawRows))
      : rows;
    // Drop a selection that the new rule no longer allows.
    if (
      control.value != null &&
      !visible.some((r) => r[field.optionValue ?? 'id'] === control.value)
    ) {
      control.setValue(null, { emitEvent: false });
    }
    this.options.update((current) => ({
      ...current,
      [field.key]: visible.map((r) => ({
        value: r[field.optionValue ?? 'id'],
        label: String(r[field.optionLabel ?? 'name'] ?? ''),
      })),
    }));
  }

  private buildControl(field: LookupField): FormControl {
    const initial = this.data.row?.[field.rowKey ?? field.key];
    const validators = field.required ? [Validators.required] : [];
    if (field.maxLength) validators.push(Validators.maxLength(field.maxLength));
    if (field.email) validators.push(Validators.email);
    if (field.pattern) validators.push(Validators.pattern(field.pattern));
    if (field.min != null) validators.push(field.type === 'number' ? Validators.min(field.min) : Validators.minLength(field.min));
    if (field.max != null && field.type === 'number') validators.push(Validators.max(field.max));

    switch (field.type) {
      case 'checkbox':
        return new FormControl<boolean>(!!initial);
      case 'select':
        return new FormControl<unknown>(initial ?? null, validators);
      case 'number':
        return new FormControl<number | null>((initial as number) ?? null, validators);
      case 'date': {
        // <input type=date> wants yyyy-MM-dd; the row may hold a full ISO datetime.
        const iso = initial ? String(initial).slice(0, 10) : '';
        return new FormControl<string>(iso, validators);
      }
      default:
        return new FormControl<string>((initial as string) ?? '', validators);
    }
  }

  /** First applicable validation message for a field's control. */
  errorFor(field: LookupField): string {
    const c = this.form.controls[field.key];
    if (c.hasError('required')) return `${field.label} is required`;
    if (c.hasError('email')) return 'Enter a valid email address';
    if (c.hasError('maxlength')) return `${field.label} must be ${field.maxLength} characters or fewer`;
    if (c.hasError('minlength')) return `${field.label} must be at least ${field.min} characters`;
    if (c.hasError('min')) return `${field.label} must be at least ${field.min}`;
    if (c.hasError('max')) return `${field.label} must be at most ${field.max}`;
    if (c.hasError('pattern')) return field.patternError ?? `${field.label} is not in the expected format`;
    return '';
  }

  save(): void {
    // Convert date fields (yyyy-MM-dd) to ISO datetime for the API.
    const value: Record<string, unknown> = { ...this.form.getRawValue() };
    for (const field of this.data.config.fields) {
      if (field.type === 'date' && value[field.key]) {
        value[field.key] = new Date(value[field.key] as string).toISOString();
      }
    }
    this.ref.close(value);
  }
}

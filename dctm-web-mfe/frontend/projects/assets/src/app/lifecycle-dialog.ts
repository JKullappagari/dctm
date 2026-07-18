import { Component, inject } from '@angular/core';
import {
  AbstractControl, FormControl, FormGroup, ReactiveFormsModule, ValidationErrors, Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

export interface LifecycleDialogData {
  action: string;
  label: string;
  /** bar/decommission need extra fields. */
  needsDateRange: boolean;
  needsExpiry: boolean;
  needsCard: boolean;
}

const LABELS: Record<string, string> = {
  writeoff: 'Write Off',
  reinstate: 'Reinstate',
  restrict: 'Restrict',
  'de-restrict': 'Remove Restriction',
  bar: 'Bar (period)',
  'un-bar': 'Remove Bar',
  decommission: 'Decommission',
  recommission: 'Recommission',
  'assign-rfid': 'Assign RFID Card',
  'deassign-rfid': 'De-assign RFID Card',
};

/** Group validator: the bar period's To date must be on or after the From date. */
function dateOrderValidator(group: AbstractControl): ValidationErrors | null {
  const from = group.get('fromDate')?.value;
  const to = group.get('toDate')?.value;
  return from && to && to < from ? { dateOrder: true } : null;
}

export function dialogDataFor(action: string): LifecycleDialogData {
  return {
    action,
    label: LABELS[action] ?? action,
    needsDateRange: action === 'bar',
    needsExpiry: action === 'decommission',
    needsCard: action === 'assign-rfid',
  };
}

@Component({
  selector: 'app-lifecycle-dialog',
  imports: [ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatButtonModule],
  template: `
    <h2 mat-dialog-title>{{ data.label }}</h2>
    <mat-dialog-content>
      <form [formGroup]="form" class="lc-form">
        @if (data.needsCard) {
          <mat-form-field appearance="outline">
            <mat-label>RFID Card Number</mat-label>
            <input matInput formControlName="cardNumber" required />
            @if (form.controls.cardNumber.hasError('required')) {
              <mat-error>Card number is required</mat-error>
            }
          </mat-form-field>
        }
        @if (data.needsDateRange) {
          <mat-form-field appearance="outline">
            <mat-label>From</mat-label>
            <input matInput type="date" formControlName="fromDate" required />
            @if (form.controls.fromDate.hasError('required')) {
              <mat-error>From date is required</mat-error>
            }
          </mat-form-field>
          <mat-form-field appearance="outline">
            <mat-label>To</mat-label>
            <input matInput type="date" formControlName="toDate" required />
            @if (form.controls.toDate.hasError('required')) {
              <mat-error>To date is required</mat-error>
            }
          </mat-form-field>
          @if (form.hasError('dateOrder')) {
            <p class="lc-error">To date must be on or after the From date.</p>
          }
        }
        @if (data.needsExpiry) {
          <mat-form-field appearance="outline">
            <mat-label>Expiry Date</mat-label>
            <input matInput type="date" formControlName="expiryDate" required />
            @if (form.controls.expiryDate.hasError('required')) {
              <mat-error>Expiry date is required</mat-error>
            }
          </mat-form-field>
        }
        <mat-form-field appearance="outline">
          <mat-label>Reason</mat-label>
          <textarea matInput formControlName="reason" rows="2"></textarea>
        </mat-form-field>
      </form>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button matButton mat-dialog-close>Cancel</button>
      <button matButton="filled" [disabled]="form.invalid" (click)="confirm()">{{ data.label }}</button>
    </mat-dialog-actions>
  `,
  styles: `
    .lc-form { display: flex; flex-direction: column; gap: 0.5rem; min-width: 360px; padding-top: 0.5rem; }
    .lc-error { color: #c62828; font-size: 0.8rem; margin: -0.25rem 0 0; }
  `,
})
export class LifecycleDialog {
  readonly data = inject<LifecycleDialogData>(MAT_DIALOG_DATA);
  private readonly ref = inject(MatDialogRef<LifecycleDialog>);

  readonly form = new FormGroup(
    {
      reason: new FormControl(''),
      // Conditionally required per action (set below).
      cardNumber: new FormControl(''),
      fromDate: new FormControl(''),
      toDate: new FormControl(''),
      expiryDate: new FormControl(''),
    },
    { validators: this.data.needsDateRange ? [dateOrderValidator] : [] },
  );

  constructor() {
    if (this.data.needsCard) this.form.controls.cardNumber.addValidators(Validators.required);
    if (this.data.needsDateRange) {
      this.form.controls.fromDate.addValidators(Validators.required);
      this.form.controls.toDate.addValidators(Validators.required);
    }
    if (this.data.needsExpiry) this.form.controls.expiryDate.addValidators(Validators.required);
    this.form.updateValueAndValidity();
  }

  confirm(): void {
    if (this.form.invalid) return;
    const v = this.form.value;
    const body: Record<string, unknown> = { reason: v.reason };
    if (this.data.needsCard) body['cardNumber'] = v.cardNumber;
    if (this.data.needsDateRange) {
      body['fromDate'] = new Date(v.fromDate!).toISOString();
      body['toDate'] = new Date(v.toDate!).toISOString();
    }
    if (this.data.needsExpiry && v.expiryDate) body['expiryDate'] = new Date(v.expiryDate).toISOString();
    this.ref.close(body);
  }
}

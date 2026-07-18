import { TestBed } from '@angular/core/testing';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { LifecycleDialog, dialogDataFor } from './lifecycle-dialog';

function setup(action: string) {
  const close = vi.fn();
  TestBed.configureTestingModule({
    providers: [
      { provide: MAT_DIALOG_DATA, useValue: dialogDataFor(action) },
      { provide: MatDialogRef, useValue: { close } },
    ],
  });
  const fixture = TestBed.createComponent(LifecycleDialog);
  fixture.detectChanges();
  return { dialog: fixture.componentInstance, close };
}

describe('LifecycleDialog conditional validation', () => {
  it('write-off needs no extra fields — valid immediately', () => {
    const { dialog, close } = setup('writeoff');
    expect(dialog.form.valid).toBe(true);
    dialog.form.controls.reason.setValue('End of life');
    dialog.confirm();
    expect(close).toHaveBeenCalledWith(expect.objectContaining({ reason: 'End of life' }));
  });

  it('assign-rfid requires a card number', () => {
    const { dialog, close } = setup('assign-rfid');
    expect(dialog.form.invalid).toBe(true);
    dialog.confirm();
    expect(close).not.toHaveBeenCalled();

    dialog.form.controls.cardNumber.setValue('CARD-42');
    expect(dialog.form.valid).toBe(true);
  });

  it('bar requires both dates and enforces To >= From', () => {
    const { dialog } = setup('bar');
    expect(dialog.form.invalid).toBe(true);

    dialog.form.controls.fromDate.setValue('2026-07-10');
    dialog.form.controls.toDate.setValue('2026-07-01'); // before From
    expect(dialog.form.hasError('dateOrder')).toBe(true);

    dialog.form.controls.toDate.setValue('2026-07-20');
    expect(dialog.form.valid).toBe(true);
  });

  it('decommission requires an expiry date', () => {
    const { dialog } = setup('decommission');
    expect(dialog.form.invalid).toBe(true);
    dialog.form.controls.expiryDate.setValue('2026-12-31');
    expect(dialog.form.valid).toBe(true);
  });
});

import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { LookupDialog, LookupDialogData } from './lookup-dialog';
import { LookupConfig } from './lookup.model';

const config: LookupConfig = {
  path: 'owners',
  title: 'Owner',
  idField: 'ownerID',
  nameField: 'ownerFirstName',
  columns: [{ field: 'ownerFirstName', headerName: 'First Name' }],
  fields: [
    { key: 'firstName', label: 'First Name', type: 'text', required: true, maxLength: 10 },
    { key: 'email', label: 'Email', type: 'text', email: true },
    {
      key: 'color', label: 'Colour', type: 'text',
      pattern: '^#?[0-9A-Fa-f]{6}$', patternError: 'hex please',
    },
    { key: 'count', label: 'Count', type: 'number', min: 0 },
    { key: 'startDate', label: 'Start', type: 'date' },
  ],
};

function setup(data: LookupDialogData) {
  const closeSpy = vi.fn();
  TestBed.configureTestingModule({
    providers: [
      provideHttpClient(),
      provideHttpClientTesting(),
      { provide: MAT_DIALOG_DATA, useValue: data },
      { provide: MatDialogRef, useValue: { close: closeSpy } },
    ],
  });
  const fixture = TestBed.createComponent(LookupDialog);
  fixture.detectChanges();
  return { fixture, dialog: fixture.componentInstance, close: closeSpy };
}

describe('LookupDialog validation', () => {
  it('is invalid until the required field is filled', () => {
    const { dialog } = setup({ config });
    expect(dialog.form.invalid).toBe(true);
    expect(dialog.errorFor(config.fields[0])).toBe('First Name is required');

    dialog.form.controls['firstName'].setValue('Ada');
    expect(dialog.form.valid).toBe(true);
  });

  it('rejects an invalid email', () => {
    const { dialog } = setup({ config });
    dialog.form.controls['firstName'].setValue('Ada');
    dialog.form.controls['email'].setValue('not-an-email');
    expect(dialog.form.invalid).toBe(true);
    expect(dialog.errorFor(config.fields[1])).toBe('Enter a valid email address');

    dialog.form.controls['email'].setValue('ada@example.com');
    expect(dialog.form.valid).toBe(true);
  });

  it('enforces maxLength', () => {
    const { dialog } = setup({ config });
    dialog.form.controls['firstName'].setValue('this-is-way-too-long');
    expect(dialog.errorFor(config.fields[0])).toBe('First Name must be 10 characters or fewer');
  });

  it('enforces a hex-colour pattern with its custom message', () => {
    const { dialog } = setup({ config });
    dialog.form.controls['firstName'].setValue('Ada');
    dialog.form.controls['color'].setValue('nope');
    expect(dialog.errorFor(config.fields[2])).toBe('hex please');

    dialog.form.controls['color'].setValue('#3366FF');
    expect(dialog.form.valid).toBe(true);
  });

  it('enforces a numeric minimum', () => {
    const { dialog } = setup({ config });
    dialog.form.controls['firstName'].setValue('Ada');
    dialog.form.controls['count'].setValue(-5);
    expect(dialog.errorFor(config.fields[3])).toBe('Count must be at least 0');
  });

  it('seeds initial values from the edited row', () => {
    const { dialog } = setup({ config, row: { firstName: 'Grace', email: 'g@x.io' } });
    expect(dialog.form.controls['firstName'].value).toBe('Grace');
    expect(dialog.form.valid).toBe(true);
  });

  it('save() closes with an ISO datetime for date fields', () => {
    const { dialog, close } = setup({ config });
    dialog.form.controls['firstName'].setValue('Ada');
    dialog.form.controls['startDate'].setValue('2026-07-18');
    dialog.save();
    expect(close).toHaveBeenCalledTimes(1);
    const value = close.mock.calls[0][0];
    expect(value.firstName).toBe('Ada');
    expect(value.startDate).toBe(new Date('2026-07-18').toISOString());
  });

  afterEach(() => {
    // Drain any option requests select fields may have issued (none here).
    TestBed.inject(HttpTestingController).verify();
  });
});

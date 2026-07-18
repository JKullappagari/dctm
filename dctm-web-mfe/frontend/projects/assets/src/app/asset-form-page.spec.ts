import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AssetFormPage } from './asset-form-page';

const MASTER = 'http://localhost:8081/api/v1';

function setup() {
  TestBed.configureTestingModule({
    providers: [
      provideHttpClient(),
      provideHttpClientTesting(),
      provideRouter([]),
      { provide: MatSnackBar, useValue: { open: vi.fn() } },
    ],
  });
  const fixture = TestBed.createComponent(AssetFormPage);
  fixture.detectChanges();
  const http = TestBed.inject(HttpTestingController);
  // The constructor loads five dropdown sources.
  for (const path of ['manufacturers', 'asset-models', 'sites', 'owners', 'ref/orientations']) {
    http.expectOne(`${MASTER}/${path}`).flush([]);
  }
  return { fixture, page: fixture.componentInstance, http };
}

describe('AssetFormPage validation', () => {
  it('is invalid until Ref Number, Model, Site and Location are set', () => {
    const { page, http } = setup();
    expect(page.form.invalid).toBe(true);

    page.form.controls.refNumber.setValue('R-1');
    page.form.controls.modelId.setValue(5);
    page.form.controls.siteId.setValue(2);
    page.form.controls.locationId.setValue(9);
    expect(page.form.valid).toBe(true);
    http.verify();
  });

  it('rejects a negative start position and a zero RU height', () => {
    const { page, http } = setup();
    page.form.controls.startPos.setValue(-1);
    page.form.controls.noOfRUs.setValue(0);
    expect(page.form.controls.startPos.hasError('min')).toBe(true);
    expect(page.form.controls.noOfRUs.hasError('min')).toBe(true);
    http.verify();
  });
});

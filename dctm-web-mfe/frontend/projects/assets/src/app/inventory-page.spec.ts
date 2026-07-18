import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { MatSnackBar } from '@angular/material/snack-bar';
import { InventoryPage } from './inventory-page';

const ASSETS = 'http://localhost:8083/api/v1';

function setup() {
  TestBed.configureTestingModule({
    providers: [
      provideHttpClient(),
      provideHttpClientTesting(),
      { provide: MatSnackBar, useValue: { open: vi.fn() } },
    ],
  });
  const fixture = TestBed.createComponent(InventoryPage);
  fixture.detectChanges();
  return { page: fixture.componentInstance, http: TestBed.inject(HttpTestingController) };
}

describe('InventoryPage location-id validation', () => {
  it('rejects empty and non-numeric input, accepts a comma-separated id list', () => {
    const { page } = setup();
    expect(page.locations.invalid).toBe(true); // empty (required)

    page.locations.setValue('abc');
    expect(page.locations.invalid).toBe(true);

    page.locations.setValue('19, 21');
    expect(page.locations.valid).toBe(true);
  });

  it('does not call the API while the input is invalid', () => {
    const { page, http } = setup();
    page.locations.setValue('oops');
    page.load();
    http.expectNone(() => true);
  });

  it('loads sessions for a valid, whitespace-stripped id list', () => {
    const { page, http } = setup();
    page.locations.setValue(' 19 , 21 ');
    page.load();
    const req = http.expectOne((r) => r.url === `${ASSETS}/inventory`);
    expect(req.request.params.get('locations')).toBe('19,21');
    req.flush([{ id: 's1', site: 'HQ', location: 'R1' }]);
    expect(page.loaded()).toBe(true);
    expect(page.sessions().length).toBe(1);
    http.verify();
  });
});

import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { MatSnackBar } from '@angular/material/snack-bar';
import { API_BASE } from '../lookup/lookup.model';
import { ReportPage } from './report-page';

const REPORTS = 'http://localhost:8084/api/v1/reports';
const catalog = [
  {
    key: 'asset-data-excel', title: 'Asset Data - Excel',
    params: [{ key: 'columnList', type: 'text', label: 'Columns (optional)' }],
  },
  {
    key: 'capacity', title: 'Capacity Report',
    params: [{ key: 'siteId', type: 'int', label: 'Site', optionsPath: 'sites', valueField: 'siteID', labelField: 'site' }],
  },
];

function setup(key: string) {
  TestBed.configureTestingModule({
    providers: [
      provideHttpClient(),
      provideHttpClientTesting(),
      { provide: API_BASE, useValue: 'http://test/api/v1' },
      { provide: MatSnackBar, useValue: { open: vi.fn() } },
    ],
  });
  const fixture = TestBed.createComponent(ReportPage);
  fixture.componentRef.setInput('key', key);
  fixture.detectChanges();
  return { fixture, page: fixture.componentInstance, http: TestBed.inject(HttpTestingController) };
}

describe('ReportPage', () => {
  it('loads the catalog and builds a form control per param', () => {
    const { page, http } = setup('asset-data-excel');
    http.expectOne(REPORTS).flush(catalog);
    expect(page.def()?.title).toBe('Asset Data - Excel');
    expect(page.form.contains('columnList')).toBe(true);
    expect(page.loadError()).toBeNull();
  });

  it('loads dropdown options for int params', () => {
    const { page, http } = setup('capacity');
    http.expectOne(REPORTS).flush(catalog);
    const opts = http.expectOne('http://test/api/v1/sites');
    opts.flush([{ siteID: 1, site: 'HQ' }]);
    expect(page.options()['siteId'][0]).toEqual({ value: 1, label: 'HQ' });
  });

  it('shows a not-signed-in message on 401', () => {
    const { page, http } = setup('capacity');
    http.expectOne(REPORTS).flush('no', { status: 401, statusText: 'Unauthorized' });
    expect(page.loadError()).toBe('Not signed in.');
    expect(page.def()).toBeNull();
  });

  it('shows an access message on 403', () => {
    const { page, http } = setup('capacity');
    http.expectOne(REPORTS).flush('no', { status: 403, statusText: 'Forbidden' });
    expect(page.loadError()).toBe('You do not have access to reports.');
  });

  it('reports an unknown key', () => {
    const { page, http } = setup('does-not-exist');
    http.expectOne(REPORTS).flush(catalog);
    expect(page.loadError()).toContain('was not found');
  });

  it('run() fetches rows and derives column defs', () => {
    const { page, http } = setup('asset-data-excel');
    http.expectOne(REPORTS).flush(catalog);
    page.run();
    const req = http.expectOne((r) => r.url === `${REPORTS}/asset-data-excel`);
    req.flush([{ assetid: 1, assetname: 'srv1' }]);
    expect(page.ran()).toBe(true);
    expect(page.rows().length).toBe(1);
    expect(page.columnDefs().map((c) => c.field)).toEqual(['assetid', 'assetname']);
  });

  afterEach(() => TestBed.inject(HttpTestingController).verify());
});

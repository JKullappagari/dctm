import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { MatSnackBar } from '@angular/material/snack-bar';
import { API_BASE } from '../lookup/lookup.model';
import { IMPORT_CONFIGS, ImportConfig, ImportPage } from './import-page';

const base = 'http://test/api/v1';

function setup(config: ImportConfig) {
  TestBed.configureTestingModule({
    providers: [
      provideHttpClient(),
      provideHttpClientTesting(),
      { provide: API_BASE, useValue: base },
      { provide: MatSnackBar, useValue: { open: vi.fn() } },
    ],
  });
  const fixture = TestBed.createComponent(ImportPage);
  fixture.componentRef.setInput('config', config);
  fixture.detectChanges();
  return { fixture, page: fixture.componentInstance, http: TestBed.inject(HttpTestingController) };
}

describe('ImportPage', () => {
  const config = IMPORT_CONFIGS['applications'];

  it('renders the title and hint from its config', () => {
    const { fixture } = setup(config);
    const el = fixture.nativeElement as HTMLElement;
    expect(el.querySelector('h1')?.textContent).toContain('Import Applications');
    expect(el.querySelector('.hint')?.textContent).toContain('Download the template');
  });

  it('downloads the template from the config path', () => {
    const { page, http } = setup(config);
    page.downloadTemplate();
    const req = http.expectOne(`${base}/import/applications/template`);
    expect(req.request.method).toBe('GET');
    expect(req.request.responseType).toBe('blob');
    req.flush(new Blob(['x']));
  });

  it('uploads the chosen file and shows the per-row result', () => {
    const { page, http } = setup(config);
    const file = new File(['data'], 'apps.xlsx');
    const input = { files: [file], value: 'apps.xlsx' } as unknown as HTMLInputElement;
    page.onFile({ target: input } as unknown as Event);

    const req = http.expectOne(`${base}/import/applications`);
    expect(req.request.method).toBe('POST');
    expect(req.request.body instanceof FormData).toBe(true);
    req.flush({ total: 2, imported: 1, failed: 1, rows: [
      { row: 2, model: 'A', status: 'imported', message: null },
      { row: 3, model: 'B', status: 'error', message: 'bad' },
    ] });

    expect(page.uploading()).toBe(false);
    expect(page.result()?.imported).toBe(1);
    expect(page.result()?.failed).toBe(1);
  });

  it('column header reflects the config keyHeader', () => {
    const { page } = setup(IMPORT_CONFIGS['app-asset-map']);
    const cols = page.columnDefs();
    expect(cols.find((c) => c.field === 'model')?.headerName).toBe('Application');
  });

  afterEach(() => TestBed.inject(HttpTestingController).verify());
});

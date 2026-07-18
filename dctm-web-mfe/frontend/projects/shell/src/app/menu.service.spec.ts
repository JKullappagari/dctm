import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { LEGACY_ROUTE_MAP, MenuService } from './menu.service';

describe('LEGACY_ROUTE_MAP', () => {
  // The four pages enabled in this iteration must resolve to their new routes.
  it.each([
    ['ImportApplications.aspx', '/master/import/applications'],
    ['ImportAppAssetMap.aspx', '/master/import/app-asset-map'],
    ['ApplicationMap.aspx', '/master/assign/app-map'],
    ['AssetDataExcel.aspx', '/master/reports/asset-data-excel'],
  ])('maps %s → %s', (legacy, route) => {
    expect(LEGACY_ROUTE_MAP[legacy]).toBe(route);
  });

  it('maps every value to an absolute /master or /assets route', () => {
    for (const [legacy, route] of Object.entries(LEGACY_ROUTE_MAP)) {
      expect(route.startsWith('/master') || route.startsWith('/assets'), legacy).toBe(true);
    }
  });
});

describe('MenuService', () => {
  it('loads the security-trimmed menu from the identity service', () => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting()],
    });
    const service = TestBed.inject(MenuService);
    const http = TestBed.inject(HttpTestingController);

    let result: unknown;
    service.load().subscribe((r) => (result = r));
    const req = http.expectOne('http://localhost:8082/api/v1/me/menu');
    expect(req.request.method).toBe('GET');
    req.flush([{ title: 'Application', items: [] }]);

    expect(result).toEqual([{ title: 'Application', items: [] }]);
    http.verify();
  });
});

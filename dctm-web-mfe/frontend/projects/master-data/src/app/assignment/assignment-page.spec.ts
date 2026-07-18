import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { MatSnackBar } from '@angular/material/snack-bar';
import { API_BASE } from '../lookup/lookup.model';
import { AssignmentPage } from './assignment-page';
import { AssignmentConfig } from './assignment.model';

const base = 'http://test/api/v1';
const config: AssignmentConfig = {
  path: 'app-map',
  title: 'Application Map',
  parentPath: 'hosts',
  parentIdField: 'hostID',
  parentNameField: 'hostName',
  parentLabel: 'Host',
  itemLabel: 'Applications',
};
const guid = '0c365e30-f0b2-e711-80bd-00155dde2123';

async function setup() {
  TestBed.configureTestingModule({
    providers: [
      provideHttpClient(),
      provideHttpClientTesting(),
      { provide: API_BASE, useValue: base },
      { provide: MatSnackBar, useValue: { open: vi.fn() } },
    ],
  });
  const fixture = TestBed.createComponent(AssignmentPage);
  fixture.componentRef.setInput('config', config);
  fixture.detectChanges();
  const http = TestBed.inject(HttpTestingController);
  await Promise.resolve(); // let the constructor's queueMicrotask parent-load run
  http.expectOne(`${base}/hosts`).flush([{ hostID: guid, hostName: 'srv-1' }]);
  return { fixture, page: fixture.componentInstance, http };
}

describe('AssignmentPage (Application Map, GUID parent)', () => {
  it('loads the parent list on init', async () => {
    const { page, http } = await setup();
    expect(page.parents().length).toBe(1);
    http.verify();
  });

  it('loads assigned/available for the selected GUID host', async () => {
    const { page, http } = await setup();
    page.selectParent(guid);
    http.expectOne(`${base}/assignments/app-map/${guid}`).flush({
      assigned: [{ id: 1, name: 'App One' }],
      available: [{ id: 2, name: 'App Two' }],
    });
    expect(page.parentId()).toBe(guid);
    expect(page.assigned().map((i) => i.id)).toEqual([1]);
    expect(page.available().map((i) => i.id)).toEqual([2]);
    http.verify();
  });

  it('moves an item, becomes dirty, and saves with the GUID parent', async () => {
    const { page, http } = await setup();
    page.selectParent(guid);
    http.expectOne(`${base}/assignments/app-map/${guid}`).flush({
      assigned: [],
      available: [{ id: 2, name: 'App Two' }],
    });

    expect(page.dirty()).toBe(false);
    page.availSel.set(new Set([2]));
    page.assign();
    expect(page.dirty()).toBe(true);
    expect(page.assigned().map((i) => i.id)).toEqual([2]);

    page.save();
    const put = http.expectOne(`${base}/assignments/app-map/${guid}`);
    expect(put.request.method).toBe('PUT');
    expect(put.request.body).toEqual({ ids: [2] });
    put.flush(null);
    expect(page.dirty()).toBe(false);
    http.verify();
  });
});

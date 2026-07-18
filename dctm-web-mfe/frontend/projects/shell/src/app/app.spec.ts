import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { of } from 'rxjs';
import { App } from './app';
import { MenuSection, MenuService } from './menu.service';

const menu: MenuSection[] = [
  {
    title: 'Application',
    items: [
      { moduleID: 40, label: 'Import Applications', legacyUrl: 'ImportApplications.aspx' },
      { moduleID: 30, label: 'Application Map', legacyUrl: 'ApplicationMap.aspx' },
      { moduleID: 99, label: 'Not Migrated', legacyUrl: 'SomethingElse.aspx' },
    ],
  },
];

function configure(isAuthenticated: boolean) {
  const oidc = {
    checkAuth: vi.fn(() => of({ isAuthenticated, userData: { preferred_username: 'admin' } })),
    authorize: vi.fn(),
    logoff: vi.fn(() => of(null)),
  };
  TestBed.configureTestingModule({
    imports: [App],
    providers: [
      provideRouter([]),
      { provide: OidcSecurityService, useValue: oidc },
      { provide: MenuService, useValue: { load: () => of(menu) } },
    ],
  });
  return oidc;
}

describe('Shell App', () => {
  it('redirects to Keycloak when unauthenticated', () => {
    const oidc = configure(false);
    TestBed.createComponent(App);
    expect(oidc.authorize).toHaveBeenCalled();
  });

  it('maps legacy menu urls to migrated routes (unmigrated → null)', () => {
    configure(true);
    const app = TestBed.createComponent(App).componentInstance as unknown as {
      sections: () => { title: string; items: { label: string; route: string | null }[] }[];
      userName: () => string;
    };
    const items = app.sections()[0].items;
    expect(items.find((i) => i.label === 'Import Applications')?.route).toBe('/master/import/applications');
    expect(items.find((i) => i.label === 'Application Map')?.route).toBe('/master/assign/app-map');
    expect(items.find((i) => i.label === 'Not Migrated')?.route).toBeNull();
    expect(app.userName()).toBe('admin');
  });

  it('menu starts closed and the hamburger toggles it', () => {
    configure(true);
    const app = TestBed.createComponent(App).componentInstance as unknown as {
      menuOpen: () => boolean;
      toggleMenu: () => void;
      closeMenu: () => void;
    };
    expect(app.menuOpen()).toBe(false);
    app.toggleMenu();
    expect(app.menuOpen()).toBe(true);
    app.closeMenu();
    expect(app.menuOpen()).toBe(false);
  });
});

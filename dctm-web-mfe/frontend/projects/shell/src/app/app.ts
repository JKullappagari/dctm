import { Component, inject, signal } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { LEGACY_ROUTE_MAP, MenuSection, MenuService } from './menu.service';

interface NavItem {
  label: string;
  route: string | null; // null = module not migrated yet (rendered disabled)
}

interface NavSection {
  title: string;
  items: NavItem[];
}

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  private readonly oidc = inject(OidcSecurityService);
  private readonly menuService = inject(MenuService);

  protected readonly authenticated = signal(false);
  protected readonly userName = signal('');
  protected readonly sections = signal<NavSection[]>([]);
  protected readonly menuOpen = signal(false);

  toggleMenu(): void {
    this.menuOpen.update((open) => !open);
  }

  closeMenu(): void {
    this.menuOpen.set(false);
  }

  constructor() {
    this.oidc.checkAuth().subscribe(({ isAuthenticated, userData }) => {
      this.authenticated.set(isAuthenticated);
      if (!isAuthenticated) {
        // No local login page anymore — Keycloak owns authentication
        // (replaces Login.aspx / iAssetTrackCorporate.aspx).
        this.oidc.authorize();
        return;
      }
      this.userName.set(userData?.preferred_username ?? '');
      this.loadMenu();
    });
  }

  private loadMenu(): void {
    // Security-trimmed menu from the legacy group/module-rights model.
    this.menuService.load().subscribe({
      next: (sections: MenuSection[]) =>
        this.sections.set(
          sections.map((section) => ({
            title: section.title,
            items: section.items.map((item) => ({
              label: item.label,
              route: item.legacyUrl ? (LEGACY_ROUTE_MAP[item.legacyUrl] ?? null) : null,
            })),
          })),
        ),
      error: () => this.sections.set([]),
    });
  }

  logout(): void {
    this.oidc.logoff().subscribe();
  }
}

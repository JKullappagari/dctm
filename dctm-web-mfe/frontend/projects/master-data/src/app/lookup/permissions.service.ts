import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, catchError, map, of, shareReplay } from 'rxjs';

interface ModulePermission {
  module: string;
  rights: string;
}

@Injectable({ providedIn: 'root' })
export class PermissionsService {
  private readonly http = inject(HttpClient);

  // Fetched once and shared. In the shell the auth interceptor supplies the token;
  // standalone (:4201, no token) the call 401s and we fail open — client guards are
  // UX-only defense-in-depth, the API is the real gate.
  private readonly permissions$: Observable<ModulePermission[] | null> = this.http
    .get<ModulePermission[]>('http://localhost:8082/api/v1/me/permissions')
    .pipe(
      catchError(() => of(null)),
      shareReplay(1),
    );

  /** True if the user holds any right on the module, or when permissions are unavailable. */
  canView(module: string | undefined): Observable<boolean> {
    if (!module) return of(true);
    return this.permissions$.pipe(
      map((permissions) =>
        permissions === null ? true : permissions.some((p) => p.module === module),
      ),
    );
  }
}

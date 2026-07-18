import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { map } from 'rxjs';
import { LookupConfig } from './lookup.model';
import { PermissionsService } from './permissions.service';

/**
 * Blocks direct navigation to a module the user has no right on (the security-trimmed
 * menu hides it, but this covers hand-typed URLs). Fails open when permissions can't
 * be loaded — the API enforces server-side regardless.
 */
export const permissionGuard: CanActivateFn = (route) => {
  const config = route.data['config'] as LookupConfig | undefined;
  const permissions = inject(PermissionsService);
  const router = inject(Router);

  return permissions.canView(config?.module).pipe(
    map((allowed) => allowed || router.createUrlTree(['/forbidden'])),
  );
};

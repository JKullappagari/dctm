import { Routes } from '@angular/router';
import { GroupRightsPage } from './admin/group-rights-page';
import { UserGroupsPage } from './admin/user-groups-page';
import { IMPORT_CONFIGS, ImportPage } from './import/import-page';
import { ReportPage } from './report/report-page';
import { ASSIGNMENT_CONFIGS } from './assignment/assignment.model';
import { AssignmentPage } from './assignment/assignment-page';
import { LOOKUP_ENTITIES } from './entities';
import { Forbidden } from './lookup/forbidden';
import { LookupPage } from './lookup/lookup-page';
import { permissionGuard } from './lookup/permission.guard';

/**
 * Exposed through native federation ('./routes') and consumed by the shell.
 * Also used directly when this MFE runs standalone on :4201.
 */
export const MASTER_DATA_ROUTES: Routes = [
  ...LOOKUP_ENTITIES.map((config) => ({
    path: config.path,
    component: LookupPage,
    canActivate: [permissionGuard],
    // Bound to LookupPage's `config` input via withComponentInputBinding().
    data: { config },
  })),
  ...ASSIGNMENT_CONFIGS.map((config) => ({
    path: 'assign/' + config.path,
    component: AssignmentPage,
    data: { config },
  })),
  { path: 'admin/group-rights', component: GroupRightsPage },
  { path: 'admin/user-groups', component: UserGroupsPage },
  ...Object.entries(IMPORT_CONFIGS).map(([key, config]) => ({
    path: 'import/' + key,
    component: ImportPage,
    // Bound to ImportPage's `config` input via withComponentInputBinding().
    data: { config },
  })),
  { path: 'reports/:key', component: ReportPage },
  { path: 'forbidden', component: Forbidden },
  { path: '', pathMatch: 'full' as const, redirectTo: LOOKUP_ENTITIES[0].path },
];

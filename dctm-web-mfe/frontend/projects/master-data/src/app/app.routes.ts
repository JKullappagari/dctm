import { Routes } from '@angular/router';
import { MASTER_DATA_ROUTES } from './master-data.routes';

// Standalone-dev routing for this MFE on :4201; the shell consumes
// MASTER_DATA_ROUTES through federation instead.
export const routes: Routes = MASTER_DATA_ROUTES;

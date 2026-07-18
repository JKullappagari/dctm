import { loadRemoteModule } from '@angular-architects/native-federation';
import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'master',
    loadChildren: () =>
      loadRemoteModule('master-data', './routes').then((m) => m.MASTER_DATA_ROUTES),
  },
  {
    path: 'assets',
    loadChildren: () =>
      loadRemoteModule('assets', './routes').then((m) => m.ASSETS_ROUTES),
  },
  { path: '', pathMatch: 'full', redirectTo: 'master/manufacturers' },
];

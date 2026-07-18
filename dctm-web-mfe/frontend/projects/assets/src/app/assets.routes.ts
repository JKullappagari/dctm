import { Routes } from '@angular/router';
import { AssetFormPage } from './asset-form-page';
import { AssetImportPage, ImportConfig } from './asset-import-page';
import { AssetsPage } from './assets-page';
import { InventoryPage } from './inventory-page';

const importAsset: ImportConfig = {
  title: 'Import Assets',
  path: 'assets',
  hint: 'Download the template, one asset per row (Site + Rack must already exist; Manufacturer/Model/Owner matched by name), then upload. Rows are independent.',
};
const importBlades: ImportConfig = {
  title: 'Import Blades',
  path: 'blades',
  hint: 'Blades are placed into an existing enclosure by ParentAssetTag. The model must be a blade and the parent an enclosure with a free slot.',
};

// Exposed through native federation ('./routes'); also used standalone on :4202.
export const ASSETS_ROUTES: Routes = [
  { path: '', component: AssetsPage },
  { path: 'new', component: AssetFormPage },
  { path: 'import/assets', component: AssetImportPage, data: { config: importAsset } },
  { path: 'import/blades', component: AssetImportPage, data: { config: importBlades } },
  { path: 'inventory', component: InventoryPage },
];

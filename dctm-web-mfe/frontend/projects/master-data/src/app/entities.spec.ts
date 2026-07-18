import { LOOKUP_ENTITIES } from './entities';
import { ASSIGNMENT_CONFIGS } from './assignment/assignment.model';
import { IMPORT_CONFIGS } from './import/import-page';

/**
 * Config-driven sanity checks: because nearly every master-data page is data (a
 * LookupConfig / AssignmentConfig / ImportConfig rendered by a generic component), these
 * guard the whole surface against malformed entries in one place.
 */
describe('LOOKUP_ENTITIES', () => {
  it('has unique route paths', () => {
    const paths = LOOKUP_ENTITIES.map((e) => e.path);
    expect(new Set(paths).size).toBe(paths.length);
  });

  it('every entity has id/name fields, a title and at least one column', () => {
    for (const e of LOOKUP_ENTITIES) {
      expect(e.path, `path for ${e.title}`).toBeTruthy();
      expect(e.title, `title for ${e.path}`).toBeTruthy();
      expect(e.idField, `idField for ${e.path}`).toBeTruthy();
      expect(e.nameField, `nameField for ${e.path}`).toBeTruthy();
      expect(e.columns.length, `columns for ${e.path}`).toBeGreaterThan(0);
    }
  });

  it('every field has a key, label and known type', () => {
    const types = ['text', 'textarea', 'checkbox', 'select', 'number', 'date'];
    for (const e of LOOKUP_ENTITIES) {
      for (const f of e.fields) {
        expect(f.key, `key in ${e.path}`).toBeTruthy();
        expect(f.label, `label for ${e.path}.${f.key}`).toBeTruthy();
        expect(types, `type for ${e.path}.${f.key}`).toContain(f.type);
      }
    }
  });

  it('select fields declare an options source', () => {
    for (const e of LOOKUP_ENTITIES) {
      for (const f of e.fields.filter((x) => x.type === 'select')) {
        expect(f.optionsPath, `optionsPath for ${e.path}.${f.key}`).toBeTruthy();
        expect(f.optionValue, `optionValue for ${e.path}.${f.key}`).toBeTruthy();
        expect(f.optionLabel, `optionLabel for ${e.path}.${f.key}`).toBeTruthy();
      }
    }
  });

  it('non-read-only entities that are editable have at least one field', () => {
    for (const e of LOOKUP_ENTITIES.filter((x) => !x.readOnly)) {
      expect(e.fields.length, `fields for ${e.path}`).toBeGreaterThan(0);
    }
  });
});

describe('ASSIGNMENT_CONFIGS', () => {
  it('includes the Application Map (host → applications) page', () => {
    const appMap = ASSIGNMENT_CONFIGS.find((c) => c.path === 'app-map');
    expect(appMap).toBeDefined();
    expect(appMap!.parentPath).toBe('hosts');
    expect(appMap!.parentIdField).toBe('hostID');
  });

  it('has unique paths and complete parent metadata', () => {
    const paths = ASSIGNMENT_CONFIGS.map((c) => c.path);
    expect(new Set(paths).size).toBe(paths.length);
    for (const c of ASSIGNMENT_CONFIGS) {
      expect(c.parentPath && c.parentIdField && c.parentNameField && c.parentLabel).toBeTruthy();
    }
  });
});

describe('IMPORT_CONFIGS', () => {
  it('covers asset models, applications and the asset-app map', () => {
    expect(Object.keys(IMPORT_CONFIGS).sort()).toEqual(
      ['app-asset-map', 'applications', 'asset-models'],
    );
  });

  it('each import config has a title, api path, hint and template name', () => {
    for (const [key, c] of Object.entries(IMPORT_CONFIGS)) {
      expect(c.title, `title for ${key}`).toBeTruthy();
      expect(c.path.startsWith('import/'), `path for ${key}`).toBe(true);
      expect(c.hint, `hint for ${key}`).toBeTruthy();
      expect(c.templateName, `templateName for ${key}`).toBeTruthy();
    }
  });
});

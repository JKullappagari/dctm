import { InjectionToken } from '@angular/core';

/** Base URL of the Master Data Service. */
export const API_BASE = new InjectionToken<string>('API_BASE', {
  factory: () => 'http://localhost:8081/api/v1',
});

export interface LookupField {
  /** JSON property on the write DTO, e.g. 'mfgName'. */
  key: string;
  /**
   * Row property to read the initial value from when it differs from `key`
   * (e.g. write 'firstName' but the list row exposes 'ownerFirstName').
   */
  rowKey?: string;
  label: string;
  type: 'text' | 'textarea' | 'checkbox' | 'select' | 'number' | 'date';
  required?: boolean;
  maxLength?: number;
  /** text/number: reject values below/above (numbers) or shorter than (text) this. */
  min?: number;
  max?: number;
  /** text only: require a valid email address. */
  email?: boolean;
  /** text only: a regex the value must match, with a human-readable error. */
  pattern?: string;
  patternError?: string;
  /** select only: API path whose list provides the options, e.g. 'divisions'. */
  optionsPath?: string;
  /** select only: option row property used as the value, e.g. 'divisionID'. */
  optionValue?: string;
  /** select only: option row property used as the label, e.g. 'division'. */
  optionLabel?: string;
  /** select only: key of the parent field this select cascades from (e.g. city ← country). */
  dependsOn?: string;
  /** select only: query-param name that carries the parent value, e.g. 'countryId'. */
  optionsParam?: string;
  /**
   * select only: client-side option filter, re-evaluated when the dependsOn field
   * changes. Receives the raw option row, the current form value, and every select's
   * raw rows (to resolve ids → names). E.g. Location parent rules: Room has no parent,
   * Row under Room, Rack under Row/Room.
   */
  optionsFilter?: (
    row: LookupRow,
    form: Record<string, unknown>,
    rows: Record<string, LookupRow[]>,
  ) => boolean;
  /** select only: disables (and clears) the control when true, re-evaluated with dependsOn. */
  disabledWhen?: (form: Record<string, unknown>, rows: Record<string, LookupRow[]>) => boolean;
}

export interface LookupColumn {
  /** JSON property on the row, e.g. 'mfgName'. */
  field: string;
  headerName: string;
  /** Render 1/0 as Yes/No (legacy int flags). */
  boolean?: boolean;
}

/**
 * One legacy "Pattern A" CRUD page (WebDataGrid + edit form + Excel export)
 * described as data. Each entry retires one .aspx page.
 */
export interface LookupConfig {
  /** API resource path, e.g. 'manufacturers'. */
  path: string;
  title: string;
  /** Row id property, e.g. 'mfgID'. */
  idField: string;
  /** Row display-name property used in confirmations. */
  nameField: string;
  /**
   * Legacy tblModule.Module name gating this page. When set, the route guard
   * blocks users without any right on it (defense-in-depth over the trimmed menu;
   * the API is the real enforcer). Omit for entities with no legacy module.
   */
  module?: string;
  /** Read-only page (e.g. User Search) — hides New/Edit/Delete, keeps Export. */
  readOnly?: boolean;
  /** Insert-only page (e.g. Audit Cycle, whose Update SP can't update) — hides Edit. */
  noEdit?: boolean;
  /**
   * Sub-resource fetched and merged into the row before editing, for entities
   * whose list SP omits data the form needs (e.g. 'geo' → GET /sites/{id}/geo
   * supplies countryID/cityID that iAssetTrack_Sp_Site_List does not return).
   */
  editEnrichPath?: string;
  columns: LookupColumn[];
  fields: LookupField[];
}

export type LookupRow = Record<string, unknown>;

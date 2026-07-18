import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE, LookupRow } from './lookup.model';

export interface CreateResult {
  id: number;
  /** True when the API resurrected a soft-deleted record with the same name. */
  reactivated: boolean;
}

@Injectable({ providedIn: 'root' })
export class LookupApiService {
  private readonly http = inject(HttpClient);
  private readonly base = inject(API_BASE);

  list(path: string, params?: Record<string, string | number>): Observable<LookupRow[]> {
    return this.http.get<LookupRow[]>(`${this.base}/${path}`, { params });
  }

  /** Fetches a row sub-resource, e.g. sites/{id}/geo. */
  enrich(path: string, id: number, subPath: string): Observable<LookupRow> {
    return this.http.get<LookupRow>(`${this.base}/${path}/${id}/${subPath}`);
  }

  create(path: string, payload: LookupRow): Observable<CreateResult> {
    return this.http.post<CreateResult>(`${this.base}/${path}`, payload);
  }

  update(path: string, id: number, payload: LookupRow): Observable<void> {
    return this.http.put<void>(`${this.base}/${path}/${id}`, payload);
  }

  remove(path: string, ids: number[]): Observable<void> {
    return this.http.delete<void>(`${this.base}/${path}`, { params: { ids: ids.join(',') } });
  }

  /** Downloads through HttpClient so the auth interceptor attaches the Bearer token. */
  export(path: string): Observable<Blob> {
    return this.http.get(`${this.base}/${path}/export`, { responseType: 'blob' });
  }
}

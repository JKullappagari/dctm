import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE, LookupRow } from '../lookup/lookup.model';
import { AssignmentView } from './assignment.model';

@Injectable({ providedIn: 'root' })
export class AssignmentService {
  private readonly http = inject(HttpClient);
  private readonly base = inject(API_BASE);

  parents(path: string): Observable<LookupRow[]> {
    return this.http.get<LookupRow[]>(`${this.base}/${path}`);
  }

  load(path: string, parentId: number | string): Observable<AssignmentView> {
    return this.http.get<AssignmentView>(`${this.base}/assignments/${path}/${parentId}`);
  }

  save(path: string, parentId: number | string, ids: number[]): Observable<void> {
    return this.http.put<void>(`${this.base}/assignments/${path}/${parentId}`, { ids });
  }
}

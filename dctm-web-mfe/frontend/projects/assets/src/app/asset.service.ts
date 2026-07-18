import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

const BASE = 'http://localhost:8083/api/v1/assets';

export interface AssetListRow {
  assetID: number;
  refNumber: string | null;
  assetName: string | null;
  modelName: string | null;
  mfgName: string | null;
  site: string | null;
  status: string | null;
  currentRFIDCardNumber: string | null;
}

export interface PagedAssets {
  total: number;
  page: number;
  size: number;
  items: AssetListRow[];
}

export interface AssetStatus {
  assetId: number;
  states: string[];
  availableActions: string[];
}

@Injectable({ providedIn: 'root' })
export class AssetService {
  private readonly http = inject(HttpClient);

  search(q: string, page: number, size: number): Observable<PagedAssets> {
    return this.http.get<PagedAssets>(BASE, { params: { q, page, size } });
  }

  status(id: number): Observable<AssetStatus> {
    return this.http.get<AssetStatus>(`${BASE}/${id}/status`);
  }

  /** Runs a lifecycle transition; returns the new status/actions. */
  transition(id: number, action: string, body: Record<string, unknown>): Observable<AssetStatus> {
    if (action === 'deassign-rfid') {
      return this.http.delete<AssetStatus>(`${BASE}/${id}/rfid-card`, {
        params: { reason: String(body['reason'] ?? '') },
      });
    }
    const path = action === 'assign-rfid' ? 'rfid-card' : action;
    return this.http.post<AssetStatus>(`${BASE}/${id}/${path}`, body);
  }
}

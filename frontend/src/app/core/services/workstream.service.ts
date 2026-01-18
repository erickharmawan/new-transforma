import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Workstream, CreateWorkstreamDto, UpdateWorkstreamDto } from '../models/workstream.model';

@Injectable({
  providedIn: 'root'
})
export class WorkstreamService {
  private apiUrl = 'http://localhost:5056/api/workstream';

  constructor(private http: HttpClient) {}

  getWorkstreamsByProject(projectId: string): Observable<Workstream[]> {
    return this.http.get<Workstream[]>(`${this.apiUrl}/project/${projectId}`);
  }

  getWorkstream(id: string): Observable<Workstream> {
    return this.http.get<Workstream>(`${this.apiUrl}/${id}`);
  }

  createWorkstream(workstream: CreateWorkstreamDto): Observable<Workstream> {
    return this.http.post<Workstream>(this.apiUrl, workstream);
  }

  updateWorkstream(id: string, workstream: UpdateWorkstreamDto): Observable<Workstream> {
    return this.http.put<Workstream>(`${this.apiUrl}/${id}`, workstream);
  }

  deleteWorkstream(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}

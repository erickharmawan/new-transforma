import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Output, CreateOutputDto, UpdateOutputDto } from '../models/output.model';

@Injectable({
  providedIn: 'root'
})
export class OutputService {
  private apiUrl = 'http://localhost:5056/api/output';

  constructor(private http: HttpClient) {}

  getOutputsByWorkstream(workstreamId: string): Observable<Output[]> {
    return this.http.get<Output[]>(`${this.apiUrl}/workstream/${workstreamId}`);
  }

  getOutput(id: string): Observable<Output> {
    return this.http.get<Output>(`${this.apiUrl}/${id}`);
  }

  createOutput(dto: CreateOutputDto): Observable<Output> {
    return this.http.post<Output>(this.apiUrl, dto);
  }

  updateOutput(id: string, dto: UpdateOutputDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto);
  }

  deleteOutput(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}

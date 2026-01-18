import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Milestone, CreateMilestone, UpdateMilestone } from '../models/milestone.model';

@Injectable({
  providedIn: 'root'
})
export class MilestoneService {
  private apiUrl = 'http://localhost:5056/api/milestone';

  constructor(private http: HttpClient) {}

  getMilestonesByProject(projectId: string): Observable<Milestone[]> {
    return this.http.get<Milestone[]>(`${this.apiUrl}/project/${projectId}`);
  }

  getMyMilestones(): Observable<Milestone[]> {
    return this.http.get<Milestone[]>(`${this.apiUrl}/my-milestones`);
  }

  getMilestone(id: string): Observable<Milestone> {
    return this.http.get<Milestone>(`${this.apiUrl}/${id}`);
  }

  createMilestone(milestone: CreateMilestone): Observable<Milestone> {
    return this.http.post<Milestone>(this.apiUrl, milestone);
  }

  updateMilestone(id: string, milestone: UpdateMilestone): Observable<Milestone> {
    return this.http.put<Milestone>(`${this.apiUrl}/${id}`, milestone);
  }

  approveMilestone(id: string, isApproved: boolean, reason?: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/${id}/approve`, { isApproved, rejectionReason: reason });
  }

  deleteMilestone(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}

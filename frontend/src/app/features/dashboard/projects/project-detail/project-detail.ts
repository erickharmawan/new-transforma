import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzDescriptionsModule } from 'ng-zorro-antd/descriptions';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { ProjectService } from '../../../../core/services/project.service';
import { Project } from '../../../../core/models/project.model';

@Component({
  selector: 'app-project-detail',
  standalone: true,
  imports: [
    CommonModule,
    DatePipe,
    NzTabsModule,
    NzCardModule,
    NzButtonModule,
    NzIconModule,
    NzTagModule,
    NzDescriptionsModule,
    NzSpinModule
  ],
  templateUrl: './project-detail.component.html',
  styleUrls: ['./project-detail.component.scss']
})
export class ProjectDetailComponent implements OnInit {
  projectId: string = '';
  project: Project | null = null;
  loading = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private projectService: ProjectService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.projectId = this.route.snapshot.params['id'];
    console.log('ngOnInit - projectId:', this.projectId);
    this.loadProject();
  }

  loadProject(): void {
    this.loading = true;
    console.log('Loading project:', this.projectId);
    this.projectService.getProject(this.projectId).subscribe({
      next: (data) => {
        console.log('Project data received:', data);
        this.project = data;
        this.loading = false;
        console.log('Loading set to false, project:', this.project);
        this.cdr.detectChanges();
        console.log('Change detection triggered');
        console.log('Should display:', !this.loading && !!this.project);
      },
      error: (error) => {
        console.error('Error loading project:', error);
        this.loading = false;
        this.router.navigate(['/dashboard/projects']);
      }
    });
  }

  getStatusColor(status: string): string {
    const statusColors: { [key: string]: string } = {
      'Active': 'success',
      'Completed': 'blue',
      'Inactive': 'error',
      'On Hold': 'warning'
    };
    return statusColors[status] || 'default';
  }

  goBack(): void {
    this.router.navigate(['/dashboard/projects']);
  }
}

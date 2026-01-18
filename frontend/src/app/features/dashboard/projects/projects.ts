import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzMessageService } from 'ng-zorro-antd/message';
import { ProjectService } from '../../../core/services/project.service';
import { Project } from '../../../core/models/project.model';
import { finalize } from 'rxjs/operators';
import { ProjectFormModalComponent } from './project-form-modal/project-form-modal';

@Component({
  selector: 'app-projects',
  imports: [
    CommonModule, 
    NzTableModule, 
    NzButtonModule, 
    NzIconModule, 
    NzTagModule,
    NzPopconfirmModule,
    ProjectFormModalComponent
  ],
  templateUrl: './projects.html',
  styleUrl: './projects.scss',
})
export class ProjectsComponent implements OnInit {
  projects: Project[] = [];
  loading = false;
  
  // Stats
  totalProjects = 0;
  activeProjects = 0;
  completedProjects = 0;
  inactiveProjects = 0;
  onHoldProjects = 0;
  
  // Modal state
  isModalVisible = false;
  selectedProject: Project | null = null;

  constructor(
    private projectService: ProjectService,
    private message: NzMessageService,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) {
  }

  ngOnInit(): void {
    this.loadProjects();
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

  updateStats(): void {
    this.totalProjects = this.projects.length;
    this.activeProjects = this.projects.filter(p => p.status === 'Active').length;
    this.completedProjects = this.projects.filter(p => p.status === 'Completed').length;
    this.inactiveProjects = this.projects.filter(p => p.status === 'Inactive').length;
    this.onHoldProjects = this.projects.filter(p => p.status === 'On Hold').length;
  }

  loadProjects(): void {
    if (this.loading) {
      console.log('Already loading, skipping...');
      return;
    }
    
    this.loading = true;
    console.log('Loading projects...');
    this.projectService.getAllProjects().subscribe({
      next: (data) => {
        console.log('Projects received:', data);
        this.projects = data;
        this.updateStats();
        this.loading = false;
        this.cdr.detectChanges();
        console.log('Loading complete, projects:', this.projects.length);
      },
      error: (error) => {
        console.error('Error loading projects:', error);
        this.message.error('Failed to load projects');
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  showCreateModal(): void {
    this.selectedProject = null;
    this.isModalVisible = true;
  }

  showEditModal(project: Project): void {
    this.selectedProject = project;
    this.isModalVisible = true;
  }

  viewProject(project: Project): void {
    this.router.navigate(['/dashboard/projects', project.id]);
  }

  handleModalSuccess(): void {
    setTimeout(() => {
      this.loadProjects();
    });
  }

  deleteProject(project: Project): void {
    this.projectService.deleteProject(project.id).subscribe({
      next: () => {
        this.message.success('Project deleted successfully');
        this.loadProjects();
      },
      error: (error) => {
        console.error('Error deleting project:', error);
        this.message.error('Failed to delete project');
      }
    });
  }
}

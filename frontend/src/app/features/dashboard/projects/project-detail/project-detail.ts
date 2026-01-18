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
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzTooltipModule } from 'ng-zorro-antd/tooltip';import { NzProgressModule } from 'ng-zorro-antd/progress';import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProjectService } from '../../../../core/services/project.service';
import { WorkstreamService } from '../../../../core/services/workstream.service';
import { OutputService } from '../../../../core/services/output.service';
import { MilestoneService } from '../../../../core/services/milestone.service';
import { Project } from '../../../../core/models/project.model';
import { Workstream, CreateWorkstreamDto } from '../../../../core/models/workstream.model';
import { Output, CreateOutputDto } from '../../../../core/models/output.model';
import { Milestone, CreateMilestone, UpdateMilestone } from '../../../../core/models/milestone.model';

@Component({
  selector: 'app-project-detail',
  standalone: true,
  imports: [
    CommonModule,
    DatePipe,
    FormsModule,
    ReactiveFormsModule,
    NzTabsModule,
    NzCardModule,
    NzButtonModule,
    NzIconModule,
    NzTagModule,
    NzDescriptionsModule,
    NzSpinModule,
    NzBadgeModule,
    NzTableModule,
    NzModalModule,
    NzFormModule,
    NzInputModule,
    NzSelectModule,
    NzDatePickerModule,
    NzPopconfirmModule,
    NzTooltipModule,
    NzProgressModule
  ],
  templateUrl: './project-detail.component.html',
  styleUrls: ['./project-detail.component.scss']
})
export class ProjectDetailComponent implements OnInit {
  projectId: string = '';
  project: Project | null = null;
  loading = false;
  
  // Workstreams
  workstreams: Workstream[] = [];
  loadingWorkstreams = false;
  workstreamModalVisible = false;
  workstreamForm: FormGroup;
  isEditMode = false;
  editingWorkstreamId: string | null = null;
  expandedWorkstreamId: string | null = null;

  // Outputs
  outputs: { [workstreamId: string]: Output[] } = {};
  loadingOutputs: { [workstreamId: string]: boolean } = {};
  outputModalVisible = false;
  outputForm: FormGroup;
  isEditModeOutput = false;
  editingOutputId: string | null = null;
  currentWorkstreamId: string | null = null;

  // Milestones
  milestones: Milestone[] = [];
  loadingMilestones = false;
  milestoneDetailModalVisible = false;
  selectedMilestone: Milestone | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private projectService: ProjectService,
    private workstreamService: WorkstreamService,
    private outputService: OutputService,
    private milestoneService: MilestoneService,
    private fb: FormBuilder,
    private message: NzMessageService,
    private cdr: ChangeDetectorRef
  ) {
    this.workstreamForm = this.fb.group({
      name: ['', [Validators.required]],
      description: [''],
      status: ['Active', [Validators.required]],
      startDate: [null],
      endDate: [null]
    });

    this.outputForm = this.fb.group({
      name: ['', [Validators.required]],
      description: ['']
    });
  }

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
        this.cdr.detectChanges();
        console.log('Loading set to false, change detection triggered');
        
        // Load workstreams in next tick to avoid ExpressionChangedAfterItHasBeenCheckedError
        setTimeout(() => {
          this.loadWorkstreams();
          this.loadMilestones();
        }, 0);
      },
      error: (error) => {
        console.error('Error loading project:', error);
        this.loading = false;
        this.cdr.detectChanges();
        this.message.error('Failed to load project');
        setTimeout(() => {
          this.router.navigate(['/dashboard/projects']);
        }, 1500);
      }
    });
  }

  loadWorkstreams(): void {
    this.loadingWorkstreams = true;
    this.workstreamService.getWorkstreamsByProject(this.projectId).subscribe({
      next: (data) => {
        this.workstreams = data;
        this.loadingWorkstreams = false;
      },
      error: (error) => {
        console.error('Error loading workstreams:', error);
        this.loadingWorkstreams = false;
        this.message.error('Failed to load workstreams');
      }
    });
  }

  loadMilestones(): void {
    this.loadingMilestones = true;
    this.milestoneService.getMilestonesByProject(this.projectId).subscribe({
      next: (data) => {
        this.milestones = data;
        this.loadingMilestones = false;
      },
      error: (error) => {
        console.error('Error loading milestones:', error);
        this.loadingMilestones = false;
        this.message.error('Failed to load milestones');
      }
    });
  }

  openAddWorkstreamModal(): void {
    this.isEditMode = false;
    this.editingWorkstreamId = null;
    this.workstreamForm.reset({
      status: 'Active'
    });
    this.workstreamModalVisible = true;
  }

  openEditWorkstreamModal(workstream: Workstream): void {
    this.isEditMode = true;
    this.editingWorkstreamId = workstream.id;
    this.workstreamForm.patchValue({
      name: workstream.name,
      description: workstream.description,
      status: workstream.status,
      startDate: workstream.startDate ? new Date(workstream.startDate) : null,
      endDate: workstream.endDate ? new Date(workstream.endDate) : null
    });
    this.workstreamModalVisible = true;
  }

  handleWorkstreamModalOk(): void {
    if (this.workstreamForm.valid) {
      const formValue = this.workstreamForm.value;
      const dto: CreateWorkstreamDto = {
        projectId: this.projectId,
        name: formValue.name,
        description: formValue.description || undefined,
        status: formValue.status,
        startDate: formValue.startDate ? new Date(formValue.startDate).toISOString() : undefined,
        endDate: formValue.endDate ? new Date(formValue.endDate).toISOString() : undefined
      };

      if (this.isEditMode && this.editingWorkstreamId) {
        this.workstreamService.updateWorkstream(this.editingWorkstreamId, dto).subscribe({
          next: () => {
            this.message.success('Workstream updated successfully');
            this.workstreamModalVisible = false;
            this.loadWorkstreams();
          },
          error: (error) => {
            console.error('Error updating workstream:', error);
            this.message.error('Failed to update workstream');
          }
        });
      } else {
        this.workstreamService.createWorkstream(dto).subscribe({
          next: () => {
            this.message.success('Workstream created successfully');
            this.workstreamModalVisible = false;
            this.loadWorkstreams();
          },
          error: (error) => {
            console.error('Error creating workstream:', error);
            this.message.error('Failed to create workstream');
          }
        });
      }
    } else {
      Object.values(this.workstreamForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  handleWorkstreamModalCancel(): void {
    this.workstreamModalVisible = false;
  }

  deleteWorkstream(id: string): void {
    this.workstreamService.deleteWorkstream(id).subscribe({
      next: () => {
        this.message.success('Workstream deleted successfully');
        this.loadWorkstreams();
      },
      error: (error) => {
        console.error('Error deleting workstream:', error);
        this.message.error('Failed to delete workstream');
      }
    });
  }

  toggleWorkstream(workstreamId: string): void {
    if (this.expandedWorkstreamId === workstreamId) {
      this.expandedWorkstreamId = null;
    } else {
      this.expandedWorkstreamId = workstreamId;
      this.loadOutputs(workstreamId);
    }
  }

  loadOutputs(workstreamId: string): void {
    this.loadingOutputs[workstreamId] = true;
    this.outputService.getOutputsByWorkstream(workstreamId).subscribe({
      next: (data) => {
        this.outputs[workstreamId] = data;
        this.loadingOutputs[workstreamId] = false;
      },
      error: (error) => {
        console.error('Error loading outputs:', error);
        this.loadingOutputs[workstreamId] = false;
        this.message.error('Failed to load outputs');
      }
    });
  }

  openAddOutputModal(workstreamId: string): void {
    this.currentWorkstreamId = workstreamId;
    this.isEditModeOutput = false;
    this.editingOutputId = null;
    this.outputForm.reset();
    this.outputModalVisible = true;
  }

  openEditOutputModal(output: Output): void {
    this.currentWorkstreamId = output.workstreamId;
    this.isEditModeOutput = true;
    this.editingOutputId = output.id;
    this.outputForm.patchValue({
      name: output.name,
      description: output.description
    });
    this.outputModalVisible = true;
  }

  handleOutputModalOk(): void {
    if (this.outputForm.valid && this.currentWorkstreamId) {
      const formValue = this.outputForm.value;
      const dto: CreateOutputDto = {
        workstreamId: this.currentWorkstreamId,
        name: formValue.name,
        description: formValue.description || undefined
      };

      if (this.isEditModeOutput && this.editingOutputId) {
        this.outputService.updateOutput(this.editingOutputId, dto).subscribe({
          next: () => {
            this.message.success('Output updated successfully');
            this.outputModalVisible = false;
            this.loadOutputs(this.currentWorkstreamId!);
            this.loadWorkstreams(); // Refresh output count
          },
          error: (error) => {
            console.error('Error updating output:', error);
            this.message.error('Failed to update output');
          }
        });
      } else {
        this.outputService.createOutput(dto).subscribe({
          next: () => {
            this.message.success('Output created successfully');
            this.outputModalVisible = false;
            this.loadOutputs(this.currentWorkstreamId!);
            this.loadWorkstreams(); // Refresh output count
          },
          error: (error) => {
            console.error('Error creating output:', error);
            this.message.error('Failed to create output');
          }
        });
      }
    } else {
      Object.values(this.outputForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  handleOutputModalCancel(): void {
    this.outputModalVisible = false;
  }

  deleteOutput(output: Output): void {
    this.outputService.deleteOutput(output.id).subscribe({
      next: () => {
        this.message.success('Output deleted successfully');
        this.loadOutputs(output.workstreamId);
        this.loadWorkstreams(); // Refresh output count
      },
      error: (error) => {
        console.error('Error deleting output:', error);
        this.message.error('Failed to delete output');
      }
    });
  }

  getMilestoneStatusColor(status: string): string {
    const statusColors: { [key: string]: string } = {
      'not-yet': 'default',
      'on-progress': 'processing',
      'done': 'success'
    };
    return statusColors[status] || 'default';
  }

  getMilestoneStatusLabel(status: string): string {
    const statusLabels: { [key: string]: string } = {
      'not-yet': 'Not Yet',
      'on-progress': 'On Progress',
      'done': 'Done'
    };
    return statusLabels[status] || status;
  }

  openMilestoneDetail(milestone: Milestone): void {
    this.selectedMilestone = milestone;
    this.milestoneDetailModalVisible = true;
  }

  closeMilestoneDetail(): void {
    this.milestoneDetailModalVisible = false;
    this.selectedMilestone = null;
  }

  approveMilestone(milestoneId: string): void {
    this.milestoneService.approveMilestone(milestoneId, true).subscribe({
      next: () => {
        this.message.success('Milestone approved successfully');
        this.loadMilestones();
        if (this.selectedMilestone?.id === milestoneId) {
          this.closeMilestoneDetail();
        }
      },
      error: (error) => {
        console.error('Error approving milestone:', error);
        this.message.error('Failed to approve milestone');
      }
    });
  }

  rejectMilestone(milestoneId: string): void {
    this.milestoneService.approveMilestone(milestoneId, false).subscribe({
      next: () => {
        this.message.success('Milestone rejected');
        this.loadMilestones();
        if (this.selectedMilestone?.id === milestoneId) {
          this.closeMilestoneDetail();
        }
      },
      error: (error) => {
        console.error('Error rejecting milestone:', error);
        this.message.error('Failed to reject milestone');
      }
    });
  }

  getApprovalStatusColor(status: string | undefined): string {
    const statusColors: { [key: string]: string } = {
      'pending': 'warning',
      'approved': 'success',
      'rejected': 'error'
    };
    return statusColors[status || 'pending'] || 'default';
  }

  getApprovalStatusLabel(status: string | undefined): string {
    const statusLabels: { [key: string]: string } = {
      'pending': 'Pending',
      'approved': 'Approved',
      'rejected': 'Rejected'
    };
    return statusLabels[status || 'pending'] || 'Pending';
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

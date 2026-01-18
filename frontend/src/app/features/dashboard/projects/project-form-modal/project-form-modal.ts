import { Component, Input, Output, EventEmitter, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { ProjectService } from '../../../../core/services/project.service';
import { Project } from '../../../../core/models/project.model';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'app-project-form-modal',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzModalModule,
    NzFormModule,
    NzInputModule,
    NzButtonModule,
    NzDatePickerModule,
    NzSelectModule
  ],
  templateUrl: './project-form-modal.html',
  styleUrl: './project-form-modal.scss'
})
export class ProjectFormModalComponent implements OnInit, OnChanges {
  @Input() visible = false;
  @Input() project: Project | null = null;
  @Output() visibleChange = new EventEmitter<boolean>();
  @Output() onSuccess = new EventEmitter<void>();

  form!: FormGroup;
  isSubmitting = false;

  statusOptions = [
    { label: 'Active', value: 'Active' },
    { label: 'Inactive', value: 'Inactive' },
    { label: 'Completed', value: 'Completed' },
    { label: 'On Hold', value: 'On Hold' }
  ];

  constructor(
    private fb: FormBuilder,
    private projectService: ProjectService,
    private message: NzMessageService
  ) {
    this.initForm();
  }

  ngOnInit(): void {
    // Initial load
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['project'] && this.project) {
      this.form.patchValue({
        name: this.project.name,
        description: this.project.description,
        code: this.project.code,
        startDate: this.project.startDate ? new Date(this.project.startDate) : null,
        endDate: this.project.endDate ? new Date(this.project.endDate) : null,
        status: this.project.status
      });
    } else if (changes['visible'] && this.visible && !this.project) {
      // Reset form when opening for create
      this.form.reset({ status: 'Active' });
    }
  }

  initForm(): void {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(200)]],
      description: [''],
      code: ['', [Validators.required, Validators.maxLength(50)]],
      startDate: [null],
      endDate: [null],
      status: ['Active', Validators.required]
    });
  }

  get isEditMode(): boolean {
    return !!this.project;
  }

  get modalTitle(): string {
    return this.isEditMode ? 'Edit Project' : 'Create New Project';
  }

  handleCancel(): void {
    this.visible = false;
    this.visibleChange.emit(false);
    this.form.reset({ status: 'Active' });
  }

  handleSubmit(): void {
    if (this.form.valid) {
      this.isSubmitting = true;
      const formValue = this.form.value;

      const projectData = {
        name: formValue.name,
        description: formValue.description || null,
        code: formValue.code,
        startDate: formValue.startDate || null,
        endDate: formValue.endDate || null,
        status: formValue.status
      };

      const request = this.isEditMode && this.project
        ? this.projectService.updateProject(this.project.id, projectData)
        : this.projectService.createProject(projectData);

      request.subscribe({
        next: () => {
          this.message.success(
            this.isEditMode ? 'Project updated successfully' : 'Project created successfully'
          );
          this.isSubmitting = false;
          this.handleCancel();
          this.onSuccess.emit();
        },
        error: (error) => {
          console.error('Error saving project:', error);
          this.message.error('Failed to save project. Please try again.');
          this.isSubmitting = false;
        }
      });
    } else {
      Object.values(this.form.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }
}

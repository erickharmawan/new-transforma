import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzProgressModule } from 'ng-zorro-antd/progress';
import { MilestoneService } from '../../../core/services/milestone.service';
import { Milestone } from '../../../core/models/milestone.model';

@Component({
  selector: 'app-milestones',
  imports: [CommonModule, NzTableModule, NzButtonModule, NzIconModule, NzTagModule, NzProgressModule],
  templateUrl: './milestones.html',
  styleUrl: './milestones.scss',
})
export class MilestonesComponent implements OnInit {
  milestones: Milestone[] = [];
  loading = false;

  constructor(private milestoneService: MilestoneService) {}

  ngOnInit(): void {
    // Will implement when we have project ID
  }

  getStatusColor(status: string): string {
    const colorMap: Record<string, string> = {
      'not-yet': 'default',
      'on-progress': 'processing',
      'done': 'success'
    };
    return colorMap[status] || 'default';
  }

  getStatusLabel(status: string): string {
    const labelMap: Record<string, string> = {
      'not-yet': 'Not Started',
      'on-progress': 'In Progress',
      'done': 'Completed'
    };
    return labelMap[status] || status;
  }
}

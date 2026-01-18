export interface Milestone {
  id: string;
  name: string;
  description?: string;
  outputId: string;
  outputName: string;
  workstreamId: string;
  workstreamName: string;
  picUserId: string;
  picUserName: string;
  status: MilestoneStatus;
  plannedStartDate?: Date;
  plannedEndDate?: Date;
  actualStartDate?: Date;
  actualEndDate?: Date;
  progress: number;
  isCritical: boolean;
  approvalStatus?: ApprovalStatus;
  approvedAt?: Date;
}

export type MilestoneStatus = 'not-yet' | 'on-progress' | 'done';
export type ApprovalStatus = 'pending' | 'approved' | 'rejected';

export interface CreateMilestone {
  name: string;
  description?: string;
  outputId: string;
  picUserId: string;
  plannedStartDate?: Date;
  plannedEndDate?: Date;
  isCritical: boolean;
}

export interface UpdateMilestone {
  name?: string;
  description?: string;
  status?: MilestoneStatus;
  actualStartDate?: Date;
  actualEndDate?: Date;
  progress?: number;
  notes?: string;
}

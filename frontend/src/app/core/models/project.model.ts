export interface Project {
  id: string;
  name: string;
  description?: string;
  code?: string;
  startDate?: Date;
  endDate?: Date;
  status: string;
  workstreamCount: number;
  milestoneCount: number;
}

export interface CreateProject {
  name: string;
  description?: string;
  code?: string;
  startDate?: Date;
  endDate?: Date;
}

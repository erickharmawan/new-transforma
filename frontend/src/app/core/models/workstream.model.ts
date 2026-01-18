export interface Workstream {
  id: string;
  projectId: string;
  name: string;
  description: string | null;
  status: 'Active' | 'Completed' | 'On Hold' | 'Inactive';
  startDate: string | null;
  endDate: string | null;
  outputCount: number;
  createdAt: string;
  updatedAt: string;
}

export interface CreateWorkstreamDto {
  projectId: string;
  name: string;
  description?: string;
  status: string;
  startDate?: string;
  endDate?: string;
}

export interface UpdateWorkstreamDto {
  name?: string;
  description?: string;
  status?: string;
  startDate?: string;
  endDate?: string;
}

export interface Output {
  id: string;
  name: string;
  description?: string;
  workstreamId: string;
  picLeaderId?: string;
  picLeaderName?: string;
  milestoneCount: number;
}

export interface CreateOutputDto {
  name: string;
  description?: string;
  workstreamId: string;
  picLeaderId?: string;
}

export interface UpdateOutputDto {
  name: string;
  description?: string;
  picLeaderId?: string;
}

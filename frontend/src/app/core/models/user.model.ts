export interface User {
  id: string;
  name: string;
  email: string;
  roleName?: string;
  baseRoleId?: string;
  isActive: boolean;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  user: User;
  policies: string[];
  projectAccesses: ProjectAccess[];
}

export interface ProjectAccess {
  projectId: string;
  projectName: string;
  accessLevel: string;
}

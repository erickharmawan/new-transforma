import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login').then(m => m.LoginComponent)
  },
  {
    path: 'dashboard',
    canActivate: [authGuard],
    loadComponent: () => import('./features/dashboard/dashboard').then(m => m.Dashboard),
    children: [
      {
        path: '',
        redirectTo: 'home',
        pathMatch: 'full'
      },
      {
        path: 'home',
        loadComponent: () => import('./features/dashboard/home/home').then(m => m.HomeComponent)
      },
      {
        path: 'projects',
        loadComponent: () => import('./features/dashboard/projects/projects').then(m => m.ProjectsComponent)
      },
      {
        path: 'projects/:id',
        loadComponent: () => import('./features/dashboard/projects/project-detail/project-detail').then(m => m.ProjectDetailComponent)
      },
      {
        path: 'milestones',
        loadComponent: () => import('./features/dashboard/milestones/milestones').then(m => m.MilestonesComponent)
      },
      {
        path: 'users',
        loadComponent: () => import('./features/dashboard/users/users').then(m => m.UsersComponent)
      },
      {
        path: 'reports',
        loadComponent: () => import('./features/dashboard/reports/reports').then(m => m.ReportsComponent)
      }
    ]
  },
  {
    path: '',
    redirectTo: '/login',
    pathMatch: 'full'
  }
];

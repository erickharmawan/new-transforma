import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzCardModule } from 'ng-zorro-antd/card';

@Component({
  selector: 'app-reports',
  imports: [CommonModule, NzButtonModule, NzIconModule, NzCardModule],
  templateUrl: './reports.html',
  styleUrl: './reports.scss',
})
export class ReportsComponent {}

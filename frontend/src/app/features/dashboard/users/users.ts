import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { User } from '../../../core/models/user.model';

@Component({
  selector: 'app-users',
  imports: [CommonModule, NzTableModule, NzButtonModule, NzIconModule, NzTagModule],
  templateUrl: './users.html',
  styleUrl: './users.scss',
})
export class UsersComponent implements OnInit {
  users: User[] = [];
  loading = false;

  ngOnInit(): void {
    // Will implement user list API later
  }
}

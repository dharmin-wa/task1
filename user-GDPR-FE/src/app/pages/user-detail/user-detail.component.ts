import { Component } from '@angular/core';
import { User } from '../../reusable/modals/user.modal';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrl: './user-detail.component.scss'
})
export class UserDetailComponent {
  isAdmin: boolean = true;
  userData: any = {}; // To store the current user's data (non-admin)
  allUsers: any[] = []; // To store all users (admin)
  
  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  deleteUser(userId:any){
    if (confirm('Are you sure you want to delete this user?')) {
      this.userService.deleteUser(userId).subscribe(
        (response) => {
          alert('User deleted successfully!');
          this.loadUsers();
        },
        (error) => {
          alert('Failed to delete user!');
          console.error(error);
        }
      );
    }
  }

  loadUsers() {
    this.userService.getAllUsers().subscribe(
      (response) => {
        this.allUsers = response.users;
      },
      (error) => {
        alert('Failed to load users!');
      }
    );
  }
}

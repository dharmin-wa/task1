import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { Store } from '@ngrx/store';
import { loadUsers } from '../../store/user.actions';
import { User } from '../../reusable/modals/user.modal';
import { UserState } from '../../store/user.reducer';
import { AuthState } from '../../store/auth.reducer';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.scss'
})
export class UserListComponent {
  users$: Observable<User[]>;
  auth$: Observable<AuthState>;
  isAdmin: boolean = true;

  constructor(private store: Store<{ users: UserState , auth: AuthState}>) {
    this.users$ = store.select(state => state.users.users);
    this.auth$ = this.store.select('auth');
  }

  ngOnInit() {
    this.store.dispatch(loadUsers());
  }

  deleteUser(userId:any){
    if (confirm('Are you sure you want to delete this user?')) {
      // this.userService.deleteUser(userId).subscribe(
      //   (response) => {
      //     alert('User deleted successfully!');
      //     this.loadUsers();
      //   },
      //   (error) => {
      //     alert('Failed to delete user!');
      //     console.error(error);
      //   }
      // );
    }
  }
}

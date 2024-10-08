import { Component } from '@angular/core';
import { map, Observable } from 'rxjs';
import { Store } from '@ngrx/store';
import { loadUserById, loadUsers } from '../../store/user.actions';
import { User } from '../../reusable/modals/user.modal';
import { UserState } from '../../store/user.reducer';
import { AuthState } from '../../store/auth.reducer';
import { EncryptionService } from '../../reusable/services/encryption.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.scss'
})
export class UserListComponent {
  users$: Observable<User[]>;
  auth$: Observable<AuthState>;
  isAdmin$: Observable<boolean>; // Change to observable
  isAdmin = false; // Initialize as false
  id = ''
  id$: Observable<string>; // Change to observable

  selectedUser$: Observable<User | null>;

  constructor(private encryptionService: EncryptionService, private store: Store<{ users: UserState, auth: AuthState }>) {
    this.users$ = store.select(state => state.users.users);
    this.auth$ = this.store.select('auth');
    this.selectedUser$ = this.store.select(state => state.users.selectedUser);

    // Use map to extract isAdmin from auth$
    this.isAdmin$ = this.auth$.pipe(
      map(auth => auth?.user?.user.isAdmin || false)
    );

    // Subscribe to isAdmin$ to update local isAdmin variable
    this.isAdmin$.subscribe(isAdmin => {
      this.isAdmin = isAdmin;
    });

    // Use map to extract isAdmin from auth$
    this.id$ = this.auth$.pipe(
      map(auth => auth?.user?.user.id || '')
    );

    // Subscribe to isAdmin$ to update local isAdmin variable
    this.id$.subscribe(id => {
      this.id = id;
    });
  }

  ngOnInit() {
    if (this.isAdmin) {
      this.store.dispatch(loadUsers());
    }
    else {
      this.store.dispatch(loadUserById({ email: this.id }));
    }
  }

  deleteUser(userId: any) {
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

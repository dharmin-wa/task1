import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { Store } from '@ngrx/store';
import { loadUsers } from '../../store/user.actions';
import { User } from '../../reusable/modals/user.modal';
import { UserState } from '../../store/user.reducer';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.scss'
})
export class UserListComponent {
  users$: Observable<User[]>;

  constructor(private store: Store<{ users: UserState }>) {
    this.users$ = store.select(state => state.users.users);
  }

  ngOnInit() {
    this.store.dispatch(loadUsers());
  }
}

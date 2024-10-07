// src/app/store/user.reducer.ts
import { createReducer, on } from '@ngrx/store';
import { loadUsersSuccess, loadUsersFailure } from './user.actions';
import { User } from '../reusable/modals/user.modal';

export interface UserState {
  users: User[];
  error: string | null;
}

export const initialState: UserState = {
  users: [],
  error: null,
};

export const userReducer = createReducer(
  initialState,
  on(loadUsersSuccess, (state, { users }) => ({
    ...state,
    users: users,
    error: null,
  })),
  on(loadUsersFailure, (state, { error }) => ({
    ...state,
    error: error,
  }))
);

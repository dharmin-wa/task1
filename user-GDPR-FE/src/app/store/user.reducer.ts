// src/app/store/user.reducer.ts
import { createReducer, on } from '@ngrx/store';
import { loadUsersSuccess, loadUsersFailure, loadUserById, loadUserByIdSuccess, loadUserByIdFailure } from './user.actions';
import { User } from '../reusable/modals/user.modal';

export interface UserState {
  users: User[];
  error: string | null;
  loading: boolean;
  selectedUser: User | null;
}

export const initialState: UserState = {
  users: [],
  error: null,
  selectedUser: null,
  loading: false,
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
  })),
  on(loadUserById, state => ({
    ...state,
    loading: true,
    error: null,
  })),
  on(loadUserByIdSuccess, (state, { user }) => ({
    ...state,
    selectedUser: user,
    loading: false,
    error: null,
  })),
  on(loadUserByIdFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error,
  }))
);

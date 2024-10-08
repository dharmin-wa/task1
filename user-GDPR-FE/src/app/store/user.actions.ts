import { createAction, props } from '@ngrx/store';
import {  User } from '../reusable/modals/user.modal';

export const loadUsers = createAction('[User] Load Users');
export const loadUsersSuccess = createAction(
  '[User] Load Users Success',
  props<{ users: User[] }>()
);
export const loadUsersFailure = createAction(
  '[User] Load Users Failure',
  props<{ error: string }>()
);

// Action to initiate loading a user by ID
export const loadUserById = createAction(
  '[User] Load User By ID',
  props<{ email: string }>()
);

// Action when user is loaded successfully
export const loadUserByIdSuccess = createAction(
  '[User] Load User By ID Success',
  props<{ user: User }>()
);

// Action when loading user by ID fails
export const loadUserByIdFailure = createAction(
  '[User] Load User By ID Failure',
  props<{ error: any }>()
);
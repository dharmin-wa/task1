import { createSelector, createFeatureSelector } from '@ngrx/store';
import { UserState } from './user.reducer';

export const selectUserState = createFeatureSelector<UserState>('users');
export const selectAllUsers = createSelector(selectUserState, (state: UserState) => state?.users);
export const selectUserError = createSelector(selectUserState, (state: UserState) => state?.error);

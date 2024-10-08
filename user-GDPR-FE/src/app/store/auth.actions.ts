import { createAction, props } from '@ngrx/store';
import { AuthResponse } from '../reusable/modals/auth.model';
import { User } from '../reusable/modals/user.modal';

export const login = createAction(
    '[Auth API] Login',
    props<{ email: string; password: string }>()
);

export const loginSuccess = createAction(
    '[Auth API] Login Success',
    props<{ authResponse: AuthResponse }>()
);

export const loginFailure = createAction(
    '[Auth API] Login Failure',
    props<{ error: any }>()
);

// Set auth state if already logged in
export const setAuthState = createAction(
    '[Auth API] Set Auth State',
    props<{ authResponse: AuthResponse }>()
  );

// Register action
export const register = createAction(
    '[Auth API] Register',
    props<{ registerRequest: User }>()
);

// Register success
export const registerSuccess = createAction(
    '[Auth API] Register Success',
    props<{ authResponse: User }>()
);

// Register failure
export const registerFailure = createAction(
    '[Auth API] Register Failure',
    props<{ error: any }>()
);
// auth.reducer.ts
import { createReducer, on } from '@ngrx/store';
import { login, loginSuccess, loginFailure, registerFailure, registerSuccess, register, setAuthState } from './auth.actions';
import { AuthResponse } from '../reusable/modals/auth.model';
import { User } from '../reusable/modals/user.modal';

export interface AuthState {
    user: AuthResponse | null;
    createUser: User | null;

    loading: boolean;
    error: any;
}

export const initialAuthState: AuthState = {
    user: null,
    loading: false,
    error: null,
    createUser: null
};

export const authReducer = createReducer(
    initialAuthState,
    on(login, (state) => ({
        ...state,
        loading: true,
        error: null,
    })),
    on(loginSuccess, (state, { authResponse }) => ({
        ...state,
        user: authResponse,
        loading: false,
    })),
    on(loginFailure, (state, { error }) => ({
        ...state,
        error: error.error.message,
        loading: false,
    })),
    on(setAuthState, (state, { authResponse }) => ({
        ...state,
        user: authResponse,
        loading: false,
        error: null,
    })),
    on(register, (state) => ({
        ...state,
        loading: true,
        error: null,
    })),
    on(registerSuccess, (state, { authResponse }) => ({
        ...state,
        createUser: authResponse,
        loading: false,
    })),
    on(registerFailure, (state, { error }) => ({
        ...state,
        error: error,
        loading: false,
    }))
);

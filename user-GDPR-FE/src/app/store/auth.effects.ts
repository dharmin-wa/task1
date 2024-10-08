import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { login, loginSuccess, loginFailure, register, registerSuccess, registerFailure, setAuthState } from './auth.actions';
import { catchError, map, mergeMap, tap } from 'rxjs/operators';
import { of } from 'rxjs';
import { AuthService } from '../reusable/services/auth.service';
import { Router } from '@angular/router';

@Injectable()
export class AuthEffects {
    constructor(private actions$: Actions, private authService: AuthService, private router: Router) { }

    login$ = createEffect(() =>
        this.actions$.pipe(
            ofType(login),
            mergeMap((action) =>
                this.authService.login(action.email, action.password).pipe(
                    map((authResponse) => {
                        this.authService.setToken(authResponse)
                        return loginSuccess({ authResponse })
                    }),
                    catchError((error) => of(loginFailure({ error })))
                )
            )
        )
    );

    // Effect to handle login success and redirect to home
    loginSuccess$ = createEffect(
        () =>
            this.actions$.pipe(
                ofType(loginSuccess),
                tap(() => {
                    this.router.navigate(['/home'])// Redirect to home on success
                })
            ),
        { dispatch: false } // We don't want to dispatch a new action here
    );

    // Check if the user is already logged in (setAuthState action)
    setAuthState$ = createEffect(() =>
        this.actions$.pipe(
            ofType(setAuthState),
            tap(({ authResponse }) => {
                // Save auth details into local storage
                this.authService.setToken(authResponse)
            })
        ),
        { dispatch: false }
    );

    register$ = createEffect(() =>
        this.actions$.pipe(
            ofType(register),
            mergeMap((action) =>
                this.authService.register(action.registerRequest).pipe(
                    map((authResponse) => registerSuccess({ authResponse })),
                    catchError((error) => of(registerFailure({ error })))
                )
            )
        )
    );

    // Redirect to login after successful registration
    registerSuccess$ = createEffect(
        () =>
            this.actions$.pipe(
                ofType(registerSuccess),
                tap(() => this.router.navigate(['/login']))
            ),
        { dispatch: false }
    );
}

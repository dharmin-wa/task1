import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { HttpClient } from '@angular/common/http';
import { loadUsers, loadUsersSuccess, loadUsersFailure, loadUserById, loadUserByIdSuccess, loadUserByIdFailure } from './user.actions';
import { catchError, map, mergeMap, tap } from 'rxjs/operators';
import { of } from 'rxjs';
import { User } from '../reusable/modals/user.modal';
import { AuthService } from '../reusable/services/auth.service';
import { Router } from '@angular/router';

@Injectable()
export class UserEffects {
  private baseUrl = 'http://192.168.1.65:6005/api/User';

  constructor(private actions$: Actions, private http: HttpClient, private authService: AuthService, private router: Router) { }

  loadUsers$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadUsers),
      mergeMap(() =>
        this.http.get<{ users: User[] }>(`${this.baseUrl}/get-all`).pipe(
          map(response => loadUsersSuccess({ users: response.users })),
          catchError(error => of(loadUsersFailure({ error: error.message })))
        )
      )
    )
  );

  loadUserById$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadUserById),
      mergeMap(action =>
        this.http.get<{ user: User }>(`${this.baseUrl}/get-detail?userId=${action.email}`).pipe(
          map(user => loadUserByIdSuccess(user)), // Dispatch success action
          catchError(error => of(loadUserByIdFailure({ error }))) // Dispatch failure action
        )
      )
    )
  );

}

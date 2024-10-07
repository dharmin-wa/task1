import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { HttpClient } from '@angular/common/http';
import { loadUsers, loadUsersSuccess, loadUsersFailure } from './user.actions';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { User } from '../reusable/modals/user.modal';

@Injectable()
export class UserEffects {
  constructor(private actions$: Actions, private http: HttpClient) {}

  loadUsers$ = createEffect(() =>
    this.actions$.pipe(
      ofType(loadUsers),
      mergeMap(() =>
        this.http.get<{ users: User[] }>('https://dummyjson.com/users').pipe(
          map(response => loadUsersSuccess({ users: response.users })),
          catchError(error => of(loadUsersFailure({ error: error.message })))
        )
      )
    )
  );
}

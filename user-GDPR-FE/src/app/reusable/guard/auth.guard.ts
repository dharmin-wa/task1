// auth.guard.ts
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { AuthState } from '../../store/auth.reducer';
import { AuthResponse } from '../modals/auth.model';
import { setAuthState } from '../../store/auth.actions';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private store: Store<{ auth: AuthState }>, private router: Router) {}

  canActivate(): boolean {
    const authToken = localStorage.getItem('authToken');
    const authUser = localStorage.getItem('authUser');

    if (authToken && authUser) {
      const authResponse: AuthResponse = {
        token: authToken,
        user: JSON.parse(authUser),
      };
      // Dispatch the setAuthState action to update the state
      
      this.store.dispatch(setAuthState({ authResponse }));
      return true;
    } else {
      // Redirect to login if not authenticated
      this.router.navigate(['/login']);
      return false;
    }
  }
}

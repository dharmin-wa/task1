import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthState } from './store/auth.reducer';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'user-GDPR';
  auth$: Observable<AuthState>;
  
  constructor(private store: Store<{ auth: AuthState }>) {
    this.auth$ = this.store.select('auth');
  }
}

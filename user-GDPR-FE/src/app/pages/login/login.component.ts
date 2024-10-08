import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { HttpClient } from '@angular/common/http';
import { EncryptionService } from '../../reusable/services/encryption.service';
import { login, setAuthState } from '../../store/auth.actions';
import { AuthState } from '../../store/auth.reducer';
import { Observable } from 'rxjs';
import { AuthResponse } from '../../reusable/modals/auth.model';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})

export class LoginComponent {
  loginForm: FormGroup;
  auth$: Observable<AuthState>;

  constructor(private fb: FormBuilder, private encryptionService: EncryptionService, private store: Store<{ auth: AuthState }>) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
    this.auth$ = this.store.select('auth');
  }


  ngOnInit() {
    // Check if user is already logged in
    const authToken = localStorage.getItem('authToken');
    const authUser = localStorage.getItem('authUser');

    if (authToken && authUser) {
      const authResponse: AuthResponse = {
        token: authToken,
        user: JSON.parse(authUser),
      };
      this.store.dispatch(setAuthState({ authResponse }));
    }
  }

  onSubmit() {
    if (this.loginForm.valid) {
      const encryptedEmail = this.encryptionService.encrypt(this.loginForm.value.username);
      const encryptedPassword = this.encryptionService.encrypt(this.loginForm.value.password);
      this.store.dispatch(login({ email: encryptedEmail, password: encryptedPassword }));
    }
  }
}
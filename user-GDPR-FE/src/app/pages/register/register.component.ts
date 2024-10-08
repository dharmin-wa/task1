import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { EncryptionService } from '../../reusable/services/encryption.service';
import { AuthState } from '../../store/auth.reducer';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { register } from '../../store/auth.actions';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  registerForm: FormGroup;
  auth$: Observable<AuthState>;

  constructor(private fb: FormBuilder, private store: Store<{ auth: AuthState }>, private router: Router, private encryptionService: EncryptionService,) {
    this.registerForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      mobileNo: ['', Validators.required],
      isAdmin: [false],
      isUserConsent: [false, Validators.requiredTrue]
    });
    this.auth$ = this.store.select('auth');

  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      debugger
      const userData = this.registerForm.value;
      userData.email = this.encryptionService.encrypt(userData.email);
      userData.password = this.encryptionService.encrypt(userData.password);
      userData.mobileNo = this.encryptionService.encrypt(userData.mobileNo);
      this.store.dispatch(register({ registerRequest: userData }));
    }
  }
}

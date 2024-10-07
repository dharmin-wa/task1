import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserState } from '../../store/user.reducer';
import { Store } from '@ngrx/store';
import { UserService } from '../../services/user.service';
import { HttpClient } from '@angular/common/http';
import { EncryptionService } from '../../reusable/services/encryption.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  loginForm: FormGroup;

  constructor(private fb: FormBuilder, private router: Router, private http: HttpClient, private encryptionService: EncryptionService) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      const encryptedEmail = this.encryptionService.encrypt(this.loginForm.value.username);
      const encryptedPassword = this.encryptionService.encrypt(this.loginForm.value.password);

      const loginData = {
        email: encryptedEmail,
        password: encryptedPassword
      };

      this.http.post('http://192.168.1.65:6005/api/User/Login', loginData)
        .subscribe(
          (response: any) => {
            console.log('Login successful:', response);
            localStorage.setItem('token', response.token);
            this.router.navigate(['/home']);
          },
          (error) => {
            console.error('Login failed:', error);
            alert(error);
          }
        );
    }
  }
}
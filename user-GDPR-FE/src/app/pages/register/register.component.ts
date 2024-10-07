import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  registerForm: FormGroup;

  constructor(private fb: FormBuilder,private userService: UserService, private router: Router) {
    this.registerForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      mobileNo: ['', Validators.required],
      isAdmin: [false],
      isUserConsent: [false, Validators.requiredTrue]
    });
  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      const userData = this.registerForm.value;
      this.userService.register(userData).subscribe(
        response => {
          alert(response.message); // Display success message
          this.router.navigate(['/login']); // Redirect to login
        },
        error => {
          alert('Registration failed!'); // Handle error
        }
      );
    }
  }
}

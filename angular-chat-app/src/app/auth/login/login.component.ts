import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { UserLoginRepoResponse } from './login.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginForm: FormGroup;
  errorMessage: string = '';
  hide: boolean = true;
  loading = false;
  constructor(private fb: FormBuilder, private authService: AuthService,
    private router: Router, private snackBar: MatSnackBar) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onSubmit() {
    this.loading = true;
    if (this.loginForm.invalid) return;

    this.authService.login(this.loginForm.value).subscribe({
      next: (response: UserLoginRepoResponse) => {
        this.loading = false;
        if (!response.isSuccess) {
          this.showMessages(response.errorMessage, response.validationMessage);
          return;
        }
        this.snackBar.open(response.message, 'Close', { duration: 3000, verticalPosition: 'bottom', horizontalPosition: 'center' });
        if (this.authService.storeToken(response.token)) {
          this.router.navigate(['/chat']);
        } else {
          this.snackBar.open('Login failed: Unable to store token', 'Close', {
            duration: 4000,
            verticalPosition: 'bottom',
            horizontalPosition: 'center'
          });
        }
      },
      error: (err) => {
        this.loading = false;
        this.snackBar.open('Login failed: ' + err.error?.message, 'Close', {
          duration: 4000,
          verticalPosition: 'bottom',
          horizontalPosition: 'center'
        });
      }
    });
  }

  onClear() {
    this.loginForm.reset();
  }
  showMessages(errors: string[], validations: string[]) {
    [...errors, ...validations].forEach(message => {
      this.snackBar.open(message, 'Close', { duration: 3000, verticalPosition: 'bottom', horizontalPosition: 'center', panelClass: ['snack-error']  });
    });
  }
}

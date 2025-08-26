import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SignupService } from './signup.service';
import { UserSignUpRepoResponse } from './signup.model';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss']
})
export class SignupComponent {
  signupForm: FormGroup;
  hide = true;
  loading = false;

  constructor(private fb: FormBuilder, private signupService: SignupService,
    private snackBar: MatSnackBar, private router: Router) {
    this.signupForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      username: ['', Validators.required],
      password: ['', Validators.required],
      createdAt: ['']
    });
  }

  onSubmit() {
    this.loading = true;
    if (this.signupForm.invalid) return;

    this.signupForm.patchValue({ createdAt: new Date().toISOString() });
    this.signupService.createaccount(this.signupForm.value).subscribe({
      next: (response: UserSignUpRepoResponse) => {
        this.loading = false;
        if (!response.isSuccess) {
          this.showMessages(response.errorMessage, response.validationMessage);
          return;
        }
        this.snackBar.open(response.message, 'Close', { duration: 3000, verticalPosition: 'bottom', horizontalPosition: 'center' });
        this.router.navigate(['/chat']);
      },
      error: (err) => {
        this.loading = false;
        this.snackBar.open('User Creation Failed failed: ' + err.error?.message, 'Close', {
          duration: 4000,
          verticalPosition: 'bottom',
          horizontalPosition: 'center'
        });
      }
    });
  }

  onClear() {
    this.signupForm.reset();
  }

  showMessages(errors: string[], validations: string[]) {
    [...errors, ...validations].forEach(message => {
      this.snackBar.open(message, 'Close', { duration: 3000, verticalPosition: 'bottom', horizontalPosition: 'center', panelClass: ['snack-error'] });
    });
  }

}

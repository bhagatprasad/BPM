import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { AccountService } from '../../services/account.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent implements OnInit {

  loginObj = {
    username: '',
    password: '',
    JwtToken: ''
  };

  rememberMe = false;
  isLoading = false;
  errorMessage = '';
  showPassword = false;

  constructor(
    public accountService: AccountService,
    private router: Router
  ) { }

  ngOnInit(): void {
    // Check if user is already authenticated
    const loggedData = localStorage.getItem('AuthenticatedUserResponse');
    if (loggedData) {
      try {
        const authResponse = JSON.parse(loggedData);
        if (authResponse?.jwtToken) {
          this.router.navigateByUrl('/drugs-catalog');
          return;
        }
      } catch (e) {
        // Invalid data, continue to login
      }
    }

    // Load saved username if exists
    const savedUsername = localStorage.getItem('savedUsername');
    if (savedUsername) {
      this.loginObj.username = savedUsername;
      this.rememberMe = true;
    }
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  onLogin(): void {
    if (!this.loginObj.username || !this.loginObj.password) {
      this.errorMessage = 'Please enter both username and password.';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.accountService.authenticateAsync(this.loginObj).subscribe({
      next: (res: any) => {
        console.log('Login response:', res);

        if (res.jwtToken) {
          localStorage.setItem('AuthenticatedUserResponse', JSON.stringify(res));
          window.dispatchEvent(new Event('storage'));
          window.location.href = '/drugs-catalog';
        } else {
          this.errorMessage = 'Invalid username or password. Please try again.';
          console.error(this.errorMessage, res);
          this.isLoading = false;
        }
      },

      error: (err: HttpErrorResponse) => {
        console.error('Login failed', err);
        this.isLoading = false;

        if (err.status === 0) {
          this.errorMessage = 'Unable to connect to the server. Please check your network connection.';
        } else if (err.status === 401) {
          this.errorMessage = 'Invalid username or password. Please try again.';
        } else if (err.status === 403) {
          this.errorMessage = 'Access forbidden. Please contact support.';
        } else if (err.status === 404) {
          this.errorMessage = 'Login service not found. Please try again later.';
        } else if (err.status === 500) {
          this.errorMessage = 'Internal server error. Please try again later.';
        } else {
          this.errorMessage = 'An unexpected error occurred. Please try again.';
        }
      }
    });
  }
}
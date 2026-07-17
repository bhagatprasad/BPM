import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { AccountService } from '../../services/account.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class LoginComponent {

  loginObj = {
    username: '',
    password: ''
  };

  constructor(
    private accountService: AccountService,
    private router: Router
  ) { }

  onLogin(): void {

    this.accountService.authenticateAsync(this.loginObj).subscribe({
      next: (res: any) => {
        console.log('Login successful', res);

        // Store login response or token
        //localStorage.setItem('authenticateResponse', JSON.stringify(res));

        // If your API returns a token
        // localStorage.setItem('token', res.token);

        //this.router.navigateByUrl('/drugs-catelog');
      },

      error: (err: HttpErrorResponse) => {
        console.error('Login failed', err);

        if (err.status === 401) {
          alert('Invalid username or password.');
        } else if (err.status === 500) {
          alert('Internal server error.');
        } else {
          alert('Unable to connect to the server.');
        }
      }
    });
  }
}
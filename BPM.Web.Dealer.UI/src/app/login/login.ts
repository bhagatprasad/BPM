import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiService } from '../../common/services/api.service';

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
    private apiService: ApiService,
    private router: Router
  ) {}

  onLogin(): void {

    this.apiService
      .send<any>('POST', 'http://localhost:5067/api/Account/authenticate', this.loginObj)
      .subscribe({
        next: (res) => {
          localStorage.setItem('token', res.data.token);

          this.router.navigateByUrl('/drugs-catelog');
        },
        error: (err) => {
          console.error('Login failed', err);
        }
      });

  }
}
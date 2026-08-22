using BPM.Web.Operations.UI.Helper;
using BPM.Web.Operations.UI.Models;
using BPM.Web.Operations.UI.Services;
using BPM.Web.Operations.UI.Views.Dashboard;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BPM.Web.Operations.UI.Views.Account
{
    public partial class LoginView : Window
    {
        private readonly IAuthenticateService _authService;
        private readonly SessionManager _sessionManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly HttpClientService _httpClientService;
        private bool _isPasswordVisible = false;

        public LoginView()
        {
            InitializeComponent();

            _serviceProvider = ((App)Application.Current).ServiceProvider;
            _authService = _serviceProvider.GetRequiredService<IAuthenticateService>();
            _sessionManager = _serviceProvider.GetRequiredService<SessionManager>();
            _httpClientService = _serviceProvider.GetRequiredService<HttpClientService>();

            this.Loaded += (s, e) =>
            {
                PasswordBoxControl.Focus();
            };

            PasswordBoxControl.KeyDown += (s, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    LoginButton_Click(s, e);
                }
            };

            PasswordVisibleTextBox.KeyDown += (s, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    LoginButton_Click(s, e);
                }
            };

            EmailTextBox.KeyDown += (s, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    PasswordBoxControl.Focus();
                }
            };

            PasswordBoxControl.PasswordChanged += (s, e) =>
            {
                if (_isPasswordVisible)
                {
                    PasswordVisibleTextBox.Text = PasswordBoxControl.Password;
                }
            };
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var email = EmailTextBox.Text?.Trim();
            var password = GetPassword();

            if (string.IsNullOrWhiteSpace(email))
            {
                ShowError("Please enter your email address.");
                return;
            }

            if (!IsValidEmail(email))
            {
                ShowError("Please enter a valid email address.");
                return;
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                ShowError("Password must be at least 6 characters.");
                return;
            }

            try
            {
                LoginButton.IsEnabled = false;
                LoginButton.Content = "⏳ Signing In...";
                HideError();

                var loginModel = new AuthenticateUserDto
                {
                    Username = email,
                    Password = password
                };

                var response = await _authService.AuthenticateUserAsync(loginModel);

                if (response != null && !string.IsNullOrWhiteSpace(response.JwtToken))
                {
                    var roleName = response.AuthenticateResponseDto?.RoleInfo?.Name;

                    bool isSuperAdmin = roleName == "SuperAdmin";
                    var authDto = response.AuthenticateResponseDto;

                    bool isAuthorized = isSuperAdmin && (authDto?.DealerInfo == null && authDto?.DistributorInfo == null);

                    if (!isAuthorized)
                    {
                        ShowError("You are not authorized to login to this portal.");
                        LoginButton.IsEnabled = true;
                        LoginButton.Content = "Sign In";
                        return;
                    }

                    // IMPORTANT: Update session and token
                    _sessionManager.SetAuthResponse(response);
                    _sessionManager.SetToken(response.JwtToken, response.RefreshToken);

                    // IMPORTANT: Update HttpClient with new token
                    _httpClientService.UpdateToken(response.JwtToken);

                    // Force refresh of authorization header
                    _httpClientService.RefreshAuthorizationHeader();

                    MessageBox.Show($"Welcome, {authDto?.FirstName} {authDto?.LastName}!",
                        "Login Successful",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    var dashboardView = _serviceProvider.GetRequiredService<DashboardView>();
                    dashboardView.Show();
                    this.Close();
                }
                else
                {
                    ShowError(response?.Message ?? "Login failed. Please check your credentials.");
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message ?? "An error occurred during login. Please try again.");
            }
            finally
            {
                LoginButton.IsEnabled = true;
                LoginButton.Content = "Sign In";
            }
        }

        private void TogglePassword_Click(object sender, RoutedEventArgs e)
        {
            _isPasswordVisible = !_isPasswordVisible;

            if (_isPasswordVisible)
            {
                PasswordVisibleTextBox.Text = PasswordBoxControl.Password;
                PasswordBoxControl.Visibility = Visibility.Collapsed;
                PasswordVisibleTextBox.Visibility = Visibility.Visible;
                EyeIcon.Text = "🙈";
                EyeIcon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0d9488"));
                PasswordVisibleTextBox.Focus();
                PasswordVisibleTextBox.CaretIndex = PasswordVisibleTextBox.Text.Length;
            }
            else
            {
                PasswordBoxControl.Password = PasswordVisibleTextBox.Text;
                PasswordVisibleTextBox.Visibility = Visibility.Collapsed;
                PasswordBoxControl.Visibility = Visibility.Visible;
                EyeIcon.Text = "👁️";
                EyeIcon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#94a3b8"));
                PasswordBoxControl.Focus();
            }
        }

        private void PasswordVisibleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Sync when visible text changes
        }

        private string GetPassword()
        {
            if (_isPasswordVisible)
            {
                return PasswordVisibleTextBox.Text;
            }
            else
            {
                return PasswordBoxControl.Password;
            }
        }

        private void ShowError(string message)
        {
            ErrorMessage.Text = message;
            ErrorMessage.Visibility = Visibility.Visible;
        }

        private void HideError()
        {
            ErrorMessage.Visibility = Visibility.Collapsed;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
using BPM.Web.Operations.UI.Helper;
using BPM.Web.Operations.UI.Models;
using BPM.Web.Operations.UI.Services;
using BPM.Web.Operations.UI.Views.Dashboard;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BPM.Web.Operations.UI.Views.Account
{
    public partial class LoginView : Window
    {
        private readonly IAuthenticateService _authService;
        private readonly SessionManager _sessionManager;
        private readonly IServiceProvider _serviceProvider;

        public LoginView()
        {
            InitializeComponent();

            _serviceProvider = ((App)Application.Current).ServiceProvider;
            _authService = _serviceProvider.GetRequiredService<IAuthenticateService>();
            _sessionManager = _serviceProvider.GetRequiredService<SessionManager>();

            PasswordBoxControl.KeyDown += (s, e) =>
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
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var email = EmailTextBox.Text?.Trim();
            var password = PasswordBoxControl.Password;

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
                    bool isAdministratorOrOperator = roleName == "Administrator" || roleName == "Operator";
                    bool isDealer = roleName == "Dealer";
                    bool isDistributor = roleName == "Distributor";

                    var authDto = response.AuthenticateResponseDto;

                    bool isAuthorized = isSuperAdmin && (authDto?.DealerInfo == null && authDto?.DistributorInfo == null);

                    if (!isAuthorized)
                    {
                        ShowError("You are not authorized to login to this portal.");

                        LoginButton.IsEnabled = true;
                        LoginButton.Content = "Sign In";
                        return;
                    }

                    _sessionManager.SetAuthResponse(response);
                    _sessionManager.SetToken(response.JwtToken, response.RefreshToken);

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
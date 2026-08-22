using BPM.Web.Operations.UI.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BPM.Web.Operations.UI.Services
{
    public class NavigationService : INavigationService
    {
        private readonly SessionManager _sessionManager;

        public NavigationService(SessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public void NavigateToLogin()
        {
            _sessionManager.ClearSession();
            var loginWindow = new MainWindow();
            loginWindow.Show();
            CloseCurrentWindow();
        }

        public void NavigateToDashboard()
        {
            // Open main dashboard
            var dashboard = new MainWindow();
            dashboard.Show();
            CloseCurrentWindow();
        }

        public void NavigateToForgotPassword()
        {
            // Implement forgot password window
            MessageBox.Show("Forgot Password functionality coming soon.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowMessage(string message, string title = "Information", MessageBoxButton button = MessageBoxButton.OK)
        {
            MessageBox.Show(message, title, button, MessageBoxImage.Information);
        }

        public void ShowError(string message, string title = "Error")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void CloseCurrentWindow()
        {
            Application.Current.Windows
                .Cast<Window>()
                .FirstOrDefault(w => w.IsActive)
                ?.Close();
        }
    }
}

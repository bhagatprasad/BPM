using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BPM.Web.Operations.UI.Services
{
    public interface INavigationService
    {
        void NavigateToLogin();
        void NavigateToDashboard();
        void NavigateToForgotPassword();
        void ShowMessage(string message, string title = "Information", MessageBoxButton button = MessageBoxButton.OK);
        void ShowError(string message, string title = "Error");
    }
}

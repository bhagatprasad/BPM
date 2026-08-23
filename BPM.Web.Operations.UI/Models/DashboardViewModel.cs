using CommunityToolkit.Mvvm.ComponentModel;

namespace BPM.Web.Operations.UI.Models
{
    public partial class DashboardViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _userName;

        [ObservableProperty]
        private string _userRole;

        [ObservableProperty]
        private string _selectedMenuItem;

        public DashboardViewModel()
        {
            LoadUserInfo();
        }

        private void LoadUserInfo()
        {
            var sessionManager = new Helper.SessionManager();
            var authResponse = sessionManager.GetAuthResponse();

            if (authResponse?.AuthenticateResponseDto != null)
            {
                var user = authResponse.AuthenticateResponseDto;
                UserName = $"{user.FirstName} {user.LastName}";
                UserRole = user.RoleInfo?.Name ?? "User";
            }
        }
    }
}

using System.Windows;
using System.Windows.Controls;

namespace BPM.Web.Operations.UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var email = EmailTextBox.Text;
            var password = PasswordBox.Password;

            // Basic validation
            if (string.IsNullOrWhiteSpace(email))
            {
                ErrorMessage.Text = "Please enter your email address.";
                ErrorMessage.Visibility = Visibility.Visible;
                return;
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                ErrorMessage.Text = "Password must be at least 6 characters.";
                ErrorMessage.Visibility = Visibility.Visible;
                return;
            }

            // TODO: Call your API here

            // For demo, just show success
            MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Close login window and open main dashboard
            // var dashboard = new DashboardWindow();
            // dashboard.Show();
            // this.Close();
        }
    }
}
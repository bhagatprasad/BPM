// Views/Shared/TopBarView.xaml.cs
using System;
using System.Windows;
using System.Windows.Controls;

namespace BPM.Web.Operations.UI.Views.Shared
{
    public partial class TopBarView : UserControl
    {
        public TopBarView()
        {
            InitializeComponent();
            SetGreeting();
        }

        private void SetGreeting()
        {
            var hour = DateTime.Now.Hour;
            var greeting = hour switch
            {
                < 12 => "Good Morning",
                < 17 => "Good Afternoon",
                _ => "Good Evening"
            };

            GreetingText.Text = $"Welcome | {greeting}";
            DateTimeText.Text = DateTime.Now.ToString("dddd, MMMM d, yyyy");
        }

        private void DarkModeToggle_Checked(object sender, RoutedEventArgs e)
        {
            // Dark mode implementation placeholder
        }

        private void DarkModeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            // Dark mode implementation placeholder
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            // Profile menu implementation placeholder
        }
    }
}
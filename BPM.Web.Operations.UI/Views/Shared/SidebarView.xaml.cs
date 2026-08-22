// Views/Shared/SidebarView.xaml.cs
using System;
using System.Windows;
using System.Windows.Controls;

namespace BPM.Web.Operations.UI.Views.Shared
{
    public partial class SidebarView : UserControl
    {
        public event EventHandler<string> MenuItemSelected;
        public event EventHandler LogoutRequested;

        public SidebarView()
        {
            InitializeComponent();
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string menuItem)
            {
                MenuItemSelected?.Invoke(this, menuItem);
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LogoutRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
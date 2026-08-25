using BPM.Web.Operations.UI.Helper;
using BPM.Web.Operations.UI.Models;
using BPM.Web.Operations.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BPM.Web.Operations.UI.Views.Dashboard
{
    public partial class UserView : UserControl
    {
        private List<UserDto> _allUsers = new List<UserDto>();
        private List<UserDto> _filteredUsers = new List<UserDto>();
        private int _currentPage = 1;
        private int _pageSize = 14;
        private string _currentRole = "All";
        private string _currentSearchText = string.Empty;
        private bool _isDataLoaded = false;
        private bool _isLoading = false;

        private readonly IUserService _userService;
        private readonly SessionManager _sessionManager;

        public UserView()
        {
            InitializeComponent();

            var serviceProvider = ((App)Application.Current).ServiceProvider;
            _userService = serviceProvider.GetRequiredService<IUserService>();
            _sessionManager = serviceProvider.GetRequiredService<SessionManager>();

            // Load data immediately after initialization
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (!_isDataLoaded && !_isLoading)
                {
                    LoadUsers();
                }
            }), System.Windows.Threading.DispatcherPriority.Background);
        }

        private async void LoadUsers()
        {
            try
            {
                if (_isDataLoaded || _isLoading)
                {
                    return;
                }

                if (UsersGrid == null)
                {
                    System.Diagnostics.Debug.WriteLine("UsersGrid is null!");
                    return;
                }

                _isLoading = true;
                this.Cursor = Cursors.Wait;

                var authResponse = _sessionManager.GetAuthResponse();
                if (authResponse == null || string.IsNullOrWhiteSpace(authResponse.JwtToken))
                {
                    MessageBox.Show("Please login again.", "Session Expired", MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.Cursor = Cursors.Arrow;
                    _isLoading = false;
                    return;
                }

                var response = await _userService.GetAllUsersListAsync();

                if (response != null && response.Any())
                {
                    _allUsers = response.ToList();
                }
                else
                {
                    _allUsers = new List<UserDto>();
                }

                _isDataLoaded = true;
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
                _isLoading = false;
            }
        }

        private void ApplyFilters()
        {
            var filtered = _allUsers.AsEnumerable();

            // Apply role filter
            if (_currentRole != "All")
            {
                filtered = filtered.Where(u => u.RoleInfo?.Name == _currentRole);
            }

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(_currentSearchText))
            {
                var searchText = _currentSearchText.ToLower();
                filtered = filtered.Where(u =>
                    (u.FirstName?.ToLower().Contains(searchText) ?? false) ||
                    (u.LastName?.ToLower().Contains(searchText) ?? false) ||
                    (u.Email?.ToLower().Contains(searchText) ?? false) ||
                    (u.RoleInfo?.Name?.ToLower().Contains(searchText) ?? false) ||
                    (u.DealerInfo?.DealershipName?.ToLower().Contains(searchText) ?? false)
                );
            }

            _filteredUsers = filtered.ToList();

            UpdateUserCount();
            _currentPage = 1;
            LoadPage();
        }

        private void LoadPage()
        {
            if (UsersGrid == null)
            {
                System.Diagnostics.Debug.WriteLine("UsersGrid is null in LoadPage!");
                return;
            }

            var pagedData = _filteredUsers
                .Skip((_currentPage - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();

            UsersGrid.ItemsSource = pagedData;

            var totalPages = (int)Math.Ceiling((double)_filteredUsers.Count / _pageSize);

            if (PageInfo != null)
            {
                PageInfo.Text = $"{_currentPage} / {Math.Max(1, totalPages)}";
            }

            if (RecordsInfo != null)
            {
                RecordsInfo.Text = $"Showing {pagedData.Count} of {_filteredUsers.Count} records (Page {_currentPage} of {Math.Max(1, totalPages)})";
            }

            if (PreviousPageButton != null)
            {
                PreviousPageButton.IsEnabled = _currentPage > 1;
            }

            if (NextPageButton != null)
            {
                NextPageButton.IsEnabled = _currentPage < totalPages;
            }
        }

        private void UpdateUserCount()
        {
            if (UserCountText != null)
            {
                UserCountText.Text = _filteredUsers.Count.ToString();
            }
        }

        #region Event Handlers

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            _currentSearchText = textBox?.Text ?? string.Empty;
            ApplyFilters();
        }

        private void RoleFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox?.SelectedItem is ComboBoxItem item)
            {
                _currentRole = item.Tag?.ToString() ?? "All";
                ApplyFilters();
            }
        }

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            _isDataLoaded = false;
            await Task.Run(() => LoadUsers());
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadPage();
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            var totalPages = (int)Math.Ceiling((double)_filteredUsers.Count / _pageSize);
            if (_currentPage < totalPages)
            {
                _currentPage++;
                LoadPage();
            }
        }

        private void UsersGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void UsersGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (UsersGrid.SelectedItem is UserDto selectedUser)
            {
                ToggleExpandRow(selectedUser.UserId);
            }
        }

        private void ExpandRow_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag == null) return;

            var userId = Guid.Parse(button.Tag.ToString());
            ToggleExpandRow(userId);
        }

        #endregion

        #region Expand/Collapse Logic

        private void ToggleExpandRow(Guid userId)
        {
            var row = FindRowByUserId(userId);
            if (row == null) return;

            if (row.DetailsVisibility == Visibility.Visible)
            {
                row.DetailsVisibility = Visibility.Collapsed;
                UpdateExpandIcon(userId, false);
            }
            else
            {
                // Collapse any previously expanded row
                foreach (var item in UsersGrid.Items)
                {
                    var existingRow = UsersGrid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                    if (existingRow != null && existingRow.DetailsVisibility == Visibility.Visible)
                    {
                        existingRow.DetailsVisibility = Visibility.Collapsed;
                        var existingUser = existingRow.Item as UserDto;
                        if (existingUser != null)
                        {
                            UpdateExpandIcon(existingUser.UserId, false);
                        }
                    }
                }

                row.DetailsVisibility = Visibility.Visible;
                row.IsSelected = true;
                UpdateExpandIcon(userId, true);
            }
        }

        private DataGridRow FindRowByUserId(Guid userId)
        {
            foreach (var item in UsersGrid.Items)
            {
                var row = UsersGrid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (row != null)
                {
                    var user = row.Item as UserDto;
                    if (user != null && user.UserId == userId)
                    {
                        return row;
                    }
                }
            }
            return null;
        }

        private void UpdateExpandIcon(Guid userId, bool isExpanded)
        {
            var row = FindRowByUserId(userId);
            if (row == null) return;

            var expandButton = FindVisualChild<Button>(row);
            if (expandButton != null)
            {
                var icon = FindVisualChild<TextBlock>(expandButton);
                if (icon != null)
                {
                    icon.Text = isExpanded ? "▼" : "▶";
                    icon.Foreground = isExpanded ?
                        new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0d9488")) :
                        new SolidColorBrush((Color)ColorConverter.ConvertFromString("#64748b"));
                }
            }
        }

        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild)
                {
                    return typedChild;
                }
                var result = FindVisualChild<T>(child);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        #endregion

        // Public method to force refresh from parent
        public void RefreshData()
        {
            _isDataLoaded = false;
            LoadUsers();
        }
    }
}
using BPM.Web.Operations.UI.Helper;
using BPM.Web.Operations.UI.Models;
using BPM.Web.Operations.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace BPM.Web.Operations.UI.Views.Dashboard
{
    public partial class DistributorView : UserControl
    {
        private List<DistributorDto> _allDistributors = new List<DistributorDto>();
        private List<DistributorDto> _filteredDistributors = new List<DistributorDto>();
        private int _currentPage = 1;
        private int _pageSize = 14;
        private string _currentStatus = "All";
        private string _currentSearchText = string.Empty;
        private bool _isDataLoaded = false;
        private bool _isLoading = false;

        private readonly IDistributorService _distributorService;
        private readonly SessionManager _sessionManager;

        public DistributorView()
        {
            try
            {
                InitializeComponent();

                var serviceProvider = ((App)Application.Current).ServiceProvider;
                _distributorService = serviceProvider.GetRequiredService<IDistributorService>();
                _sessionManager = serviceProvider.GetRequiredService<SessionManager>();

                // Load data immediately after initialization
                Dispatcher.BeginInvoke(new Action(async () =>
                {
                    try
                    {
                        if (!_isDataLoaded && !_isLoading)
                        {
                            await LoadDistributorsAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading distributors: {ex.Message}",
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }), System.Windows.Threading.DispatcherPriority.Background);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing DistributorView: {ex.Message}",
                    "Initialization Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Loads distributors from the API.
        /// </summary>
        private async Task LoadDistributorsAsync()
        {
            if (_isDataLoaded || _isLoading)
            {
                return;
            }

            if (DistributorsGrid == null)
            {
                System.Diagnostics.Debug.WriteLine("DistributorsGrid is null!");
                return;
            }

            try
            {
                _isLoading = true;
                this.Cursor = Cursors.Wait;

                var authResponse = _sessionManager.GetAuthResponse();
                if (authResponse == null || string.IsNullOrWhiteSpace(authResponse.JwtToken))
                {
                    MessageBox.Show("Please login again.", "Session Expired", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var response = await _distributorService.GetDistributorListAsync();

                _allDistributors = (response != null && response.Any())
                    ? response.ToList()
                    : new List<DistributorDto>();

                _isDataLoaded = true;
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading distributors: {ex.Message}",
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
            try
            {
                var filtered = _allDistributors.AsEnumerable();

                // Apply status filter
                if (_currentStatus == "Active")
                {
                    filtered = filtered.Where(d => d.IsActive);
                }
                else if (_currentStatus == "Inactive")
                {
                    filtered = filtered.Where(d => !d.IsActive);
                }

                // Apply search filter
                if (!string.IsNullOrWhiteSpace(_currentSearchText))
                {
                    var searchText = _currentSearchText.ToLower();
                    filtered = filtered.Where(d =>
                        (d.DistributorCode?.ToLower().Contains(searchText) ?? false) ||
                        (d.DistributorName?.ToLower().Contains(searchText) ?? false) ||
                        (d.ContactPerson?.ToLower().Contains(searchText) ?? false) ||
                        (d.Email?.ToLower().Contains(searchText) ?? false) ||
                        (d.Phone?.ToLower().Contains(searchText) ?? false) ||
                        (d.City?.ToLower().Contains(searchText) ?? false) ||
                        (d.State?.ToLower().Contains(searchText) ?? false) ||
                        (d.GSTNumber?.ToLower().Contains(searchText) ?? false)
                    );
                }

                _filteredDistributors = filtered.ToList();

                UpdateDistributorCount();
                _currentPage = 1;
                LoadPage();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApplyFilters error: {ex.Message}");
            }
        }

        private void LoadPage()
        {
            try
            {
                if (DistributorsGrid == null)
                {
                    System.Diagnostics.Debug.WriteLine("DistributorsGrid is null in LoadPage!");
                    return;
                }

                var pagedData = _filteredDistributors
                    .Skip((_currentPage - 1) * _pageSize)
                    .Take(_pageSize)
                    .ToList();

                DistributorsGrid.ItemsSource = pagedData;

                var totalPages = (int)Math.Ceiling((double)_filteredDistributors.Count / _pageSize);

                if (PageInfo != null)
                {
                    PageInfo.Text = $"{_currentPage} / {Math.Max(1, totalPages)}";
                }

                if (RecordsInfo != null)
                {
                    RecordsInfo.Text = $"Showing {pagedData.Count} of {_filteredDistributors.Count} records (Page {_currentPage} of {Math.Max(1, totalPages)})";
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
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadPage error: {ex.Message}");
            }
        }

        private void UpdateDistributorCount()
        {
            try
            {
                if (DistributorCountText != null)
                {
                    DistributorCountText.Text = _filteredDistributors.Count.ToString();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UpdateDistributorCount error: {ex.Message}");
            }
        }

        #region CRUD Operations

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new DistributorDialog();
                dialog.Owner = Window.GetWindow(this);

                if (dialog.ShowDialog() == true)
                {
                    // Refresh the list after adding
                    _isDataLoaded = false;
                    await LoadDistributorsAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening add dialog: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button && button.Tag is Guid id)
                {
                    var distributor = _allDistributors.FirstOrDefault(d => d.Id == id);
                    if (distributor != null)
                    {
                        var dialog = new DistributorDialog(distributor);
                        dialog.Owner = Window.GetWindow(this);

                        if (dialog.ShowDialog() == true)
                        {
                            // Refresh the list after updating
                            _isDataLoaded = false;
                            await LoadDistributorsAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening edit dialog: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button && button.Tag is Guid id)
                {
                    var distributor = _allDistributors.FirstOrDefault(d => d.Id == id);
                    if (distributor == null) return;

                    var result = MessageBox.Show(
                        $"Are you sure you want to delete distributor '{distributor.DistributorName}'?",
                        "Confirm Delete",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        await _distributorService.DeleteDistributorById(id);

                        // Refresh the list after deleting
                        _isDataLoaded = false;
                        await LoadDistributorsAsync();

                        MessageBox.Show("Distributor deleted successfully.", "Success",
                                      MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting distributor: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Event Handlers

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var textBox = sender as TextBox;
                _currentSearchText = textBox?.Text ?? string.Empty;
                ApplyFilters();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SearchBox_TextChanged error: {ex.Message}");
            }
        }

        private void StatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var comboBox = sender as ComboBox;
                if (comboBox?.SelectedItem is ComboBoxItem item)
                {
                    _currentStatus = item.Tag?.ToString() ?? "All";
                    ApplyFilters();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"StatusFilter_SelectionChanged error: {ex.Message}");
            }
        }

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _isDataLoaded = false;
                await LoadDistributorsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentPage > 1)
                {
                    _currentPage--;
                    LoadPage();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"PreviousPage_Click error: {ex.Message}");
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var totalPages = (int)Math.Ceiling((double)_filteredDistributors.Count / _pageSize);
                if (_currentPage < totalPages)
                {
                    _currentPage++;
                    LoadPage();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"NextPage_Click error: {ex.Message}");
            }
        }

        private void DistributorsGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                e.Row.Header = (e.Row.GetIndex() + 1).ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DistributorsGrid_LoadingRow error: {ex.Message}");
            }
        }

        #endregion

        // Public method to force refresh from parent
        public async void RefreshData()
        {
            try
            {
                _isDataLoaded = false;
                await LoadDistributorsAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"RefreshData error: {ex.Message}");
            }
        }
    }
}
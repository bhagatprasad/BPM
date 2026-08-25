using BPM.Web.Operations.UI.Helper;
using BPM.Web.Operations.UI.Models;
using BPM.Web.Operations.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BPM.Web.Operations.UI.Views.Dashboard
{
    public partial class DealerView : UserControl
    {
        private List<DealerDto> _allDealers = new List<DealerDto>();
        private List<DealerDto> _filteredDealers = new List<DealerDto>();
        private int _currentPage = 1;
        private int _pageSize = 14;
        private string _currentStatus = "All";
        private string _currentSearchText = string.Empty;
        private bool _isDataLoaded = false;
        private bool _isLoading = false;

        private readonly IDealerService _dealerService;
        private readonly SessionManager _sessionManager;

        public DealerView()
        {
            try
            {
                InitializeComponent();

                var serviceProvider = ((App)Application.Current).ServiceProvider;
                _dealerService = serviceProvider.GetRequiredService<IDealerService>();
                _sessionManager = serviceProvider.GetRequiredService<SessionManager>();

                // Load data immediately after initialization.
                // This BeginInvoke callback still runs on the UI thread (the Dispatcher
                // it was queued on), so it is safe to touch UI elements here.
                Dispatcher.BeginInvoke(new Action(async () =>
                {
                    try
                    {
                        if (!_isDataLoaded && !_isLoading)
                        {
                            await LoadDealersAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading dealers: {ex.Message}",
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }), System.Windows.Threading.DispatcherPriority.Background);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing DealerView: {ex.Message}",
                    "Initialization Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Loads dealers from the API. This method is UI-thread only:
        /// it touches Cursor/DataGrid/etc. directly, so it must always be
        /// invoked (and awaited) from the UI thread — never from Task.Run
        /// or any background thread.
        /// </summary>
        private async Task LoadDealersAsync()
        {
            if (_isDataLoaded || _isLoading)
            {
                return;
            }

            if (DealersGrid == null)
            {
                System.Diagnostics.Debug.WriteLine("DealersGrid is null!");
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

                // The actual network/IO call is awaited here, which frees the UI
                // thread while it runs, then safely resumes back on the UI thread
                // (WPF's SynchronizationContext handles that automatically).
                var response = await _dealerService.GetAllDealersAsync();

                _allDealers = (response != null && response.Any())
                    ? response.ToList()
                    : new List<DealerDto>();

                _isDataLoaded = true;
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dealers: {ex.Message}",
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
                var filtered = _allDealers.AsEnumerable();

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
                        (d.DealershipName?.ToLower().Contains(searchText) ?? false) ||
                        (d.ContactPerson?.ToLower().Contains(searchText) ?? false) ||
                        (d.Email?.ToLower().Contains(searchText) ?? false) ||
                        (d.Phone?.ToLower().Contains(searchText) ?? false) ||
                        (d.City?.ToLower().Contains(searchText) ?? false) ||
                        (d.State?.ToLower().Contains(searchText) ?? false) ||
                        (d.GSTNumber?.ToLower().Contains(searchText) ?? false)
                    );
                }

                _filteredDealers = filtered.ToList();

                UpdateDealerCount();
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
                if (DealersGrid == null)
                {
                    System.Diagnostics.Debug.WriteLine("DealersGrid is null in LoadPage!");
                    return;
                }

                var pagedData = _filteredDealers
                    .Skip((_currentPage - 1) * _pageSize)
                    .Take(_pageSize)
                    .ToList();

                DealersGrid.ItemsSource = pagedData;

                var totalPages = (int)Math.Ceiling((double)_filteredDealers.Count / _pageSize);

                if (PageInfo != null)
                {
                    PageInfo.Text = $"{_currentPage} / {Math.Max(1, totalPages)}";
                }

                if (RecordsInfo != null)
                {
                    RecordsInfo.Text = $"Showing {pagedData.Count} of {_filteredDealers.Count} records (Page {_currentPage} of {Math.Max(1, totalPages)})";
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

        private void UpdateDealerCount()
        {
            try
            {
                if (DealerCountText != null)
                {
                    DealerCountText.Text = _filteredDealers.Count.ToString();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UpdateDealerCount error: {ex.Message}");
            }
        }

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
                // IMPORTANT: call this directly on the UI thread — do NOT wrap it in
                // Task.Run. LoadDealersAsync touches UI elements (Cursor, DataGrid)
                // and already awaits the network call internally, so Task.Run would
                // move that UI access onto a background thread and crash the app.
                _isDataLoaded = false;
                await LoadDealersAsync();
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
                var totalPages = (int)Math.Ceiling((double)_filteredDealers.Count / _pageSize);
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

        private void DealersGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                e.Row.Header = (e.Row.GetIndex() + 1).ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DealersGrid_LoadingRow error: {ex.Message}");
            }
        }

        #endregion

        // Public method to force refresh from parent
        public async void RefreshData()
        {
            try
            {
                _isDataLoaded = false;
                await LoadDealersAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"RefreshData error: {ex.Message}");
            }
        }
    }
}
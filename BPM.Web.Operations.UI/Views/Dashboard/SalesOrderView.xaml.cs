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
    public partial class SalesOrderView : UserControl
    {
        private List<SalesOrderDto> _allOrders = new List<SalesOrderDto>();
        private List<SalesOrderDto> _filteredOrders = new List<SalesOrderDto>();
        private int _currentPage = 1;
        private int _pageSize = 14;
        private string _currentStatus = "All";
        private string _currentSearchText = string.Empty;
        private bool _isDataLoaded = false;
        private bool _isLoading = false;

        private readonly ISalesOrderService _salesOrderService;
        private readonly SessionManager _sessionManager;

        public SalesOrderView()
        {
            InitializeComponent();

            var serviceProvider = ((App)Application.Current).ServiceProvider;
            _salesOrderService = serviceProvider.GetRequiredService<ISalesOrderService>();
            _sessionManager = serviceProvider.GetRequiredService<SessionManager>();

            // Load data immediately after initialization
            // Use Dispatcher to ensure UI is ready
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (!_isDataLoaded && !_isLoading)
                {
                    LoadSalesOrders();
                }
            }), System.Windows.Threading.DispatcherPriority.Background);
        }

        private async void LoadSalesOrders()
        {
            try
            {
                if (_isDataLoaded || _isLoading)
                {
                    return;
                }

                if (SalesOrdersGrid == null)
                {
                    System.Diagnostics.Debug.WriteLine("SalesOrdersGrid is null!");
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

                var response = await _salesOrderService.GetAllSalesOrderAsync();

                if (response != null && response.Any())
                {
                    _allOrders = response.ToList();
                }
                else
                {
                    _allOrders = new List<SalesOrderDto>();
                }

                _isDataLoaded = true;
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading sales orders: {ex.Message}",
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
            var filtered = _allOrders.AsEnumerable();

            if (_currentStatus != "All")
            {
                filtered = filtered.Where(o => o.Status == _currentStatus);
            }

            if (!string.IsNullOrWhiteSpace(_currentSearchText))
            {
                var searchText = _currentSearchText.ToLower();
                filtered = filtered.Where(o =>
                    o.SONumber.ToLower().Contains(searchText) 
                );
            }

            _filteredOrders = filtered.ToList();

            UpdateOrderCount();
            _currentPage = 1;
            LoadPage();
        }

        private void LoadPage()
        {
            if (SalesOrdersGrid == null)
            {
                System.Diagnostics.Debug.WriteLine("SalesOrdersGrid is null in LoadPage!");
                return;
            }

            var pagedData = _filteredOrders
                .Skip((_currentPage - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();

            SalesOrdersGrid.ItemsSource = pagedData;

            var totalPages = (int)Math.Ceiling((double)_filteredOrders.Count / _pageSize);

            if (PageInfo != null)
            {
                PageInfo.Text = $"{_currentPage} / {Math.Max(1, totalPages)}";
            }

            if (RecordsInfo != null)
            {
                RecordsInfo.Text = $"Showing {pagedData.Count} of {_filteredOrders.Count} records (Page {_currentPage} of {Math.Max(1, totalPages)})";
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

        private void UpdateOrderCount()
        {
            if (OrderCountText != null)
            {
                OrderCountText.Text = _filteredOrders.Count.ToString();
            }
        }

        #region Event Handlers

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            _currentSearchText = textBox?.Text ?? string.Empty;
            ApplyFilters();
        }

        private void StatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox?.SelectedItem is ComboBoxItem item)
            {
                _currentStatus = item.Tag?.ToString() ?? "All";
                ApplyFilters();
            }
        }

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            _isDataLoaded = false;
            await Task.Run(() => LoadSalesOrders());
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
            var totalPages = (int)Math.Ceiling((double)_filteredOrders.Count / _pageSize);
            if (_currentPage < totalPages)
            {
                _currentPage++;
                LoadPage();
            }
        }

        private void SalesOrdersGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void SalesOrdersGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SalesOrdersGrid.SelectedItem is SalesOrderDto selectedOrder)
            {
                ToggleExpandRow(selectedOrder.Id);
            }
        }

        private void ExpandRow_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag == null) return;

            var orderId = Guid.Parse(button.Tag.ToString());
            ToggleExpandRow(orderId);
        }

        #endregion

        #region Expand/Collapse Logic

        private void ToggleExpandRow(Guid orderId)
        {
            var row = FindRowByOrderId(orderId);
            if (row == null) return;

            if (row.DetailsVisibility == Visibility.Visible)
            {
                row.DetailsVisibility = Visibility.Collapsed;
                UpdateExpandIcon(orderId, false);
            }
            else
            {
                // Collapse any previously expanded row
                foreach (var item in SalesOrdersGrid.Items)
                {
                    var existingRow = SalesOrdersGrid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                    if (existingRow != null && existingRow.DetailsVisibility == Visibility.Visible)
                    {
                        existingRow.DetailsVisibility = Visibility.Collapsed;
                        var existingOrder = existingRow.Item as SalesOrderDto;
                        if (existingOrder != null)
                        {
                            UpdateExpandIcon(existingOrder.Id, false);
                        }
                    }
                }

                row.DetailsVisibility = Visibility.Visible;
                row.IsSelected = true;
                UpdateExpandIcon(orderId, true);
            }
        }

        private DataGridRow FindRowByOrderId(Guid orderId)
        {
            foreach (var item in SalesOrdersGrid.Items)
            {
                var row = SalesOrdersGrid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (row != null)
                {
                    var order = row.Item as SalesOrderDto;
                    if (order != null && order.Id == orderId)
                    {
                        return row;
                    }
                }
            }
            return null;
        }

        private void UpdateExpandIcon(Guid orderId, bool isExpanded)
        {
            var row = FindRowByOrderId(orderId);
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
            LoadSalesOrders();
        }
    }
}
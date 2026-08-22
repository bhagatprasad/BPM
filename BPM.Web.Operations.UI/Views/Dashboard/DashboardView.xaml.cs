// Views/Dashboard/DashboardView.xaml.cs
using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using BPM.Web.Operations.UI.Views.Account;
using BPM.Web.Operations.UI.Views.Dashboard;

namespace BPM.Web.Operations.UI.Views.Dashboard
{
    public partial class DashboardView : Window
    {
        private UserControl _currentContent;

        public DashboardView()
        {
            InitializeComponent();
            LoadDefaultContent();
        }

        private void Sidebar_MenuItemSelected(object sender, string menuItem)
        {
            LoadContent(menuItem);
        }

        private void Sidebar_LogoutRequested(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?",
                                         "Logout",
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var serviceProvider = ((App)Application.Current).ServiceProvider;
                var sessionManager = serviceProvider.GetRequiredService<Helper.SessionManager>();
                sessionManager.ClearSession();

                var loginView = serviceProvider.GetRequiredService<LoginView>();
                loginView.Show();
                this.Close();
            }
        }

        private void LoadDefaultContent()
        {
            LoadContent("Dashboard");
        }

        private void LoadContent(string menuItem)
        {
            UserControl content = null;

            switch (menuItem)
            {
                case "Dashboard":
                    content = CreateDashboardContent();
                    break;
                case "Orders":
                    content = CreateOrdersContent();
                    break;
                case "PurchaseOrders":
                    content = CreatePurchaseOrdersContent();
                    break;
                case "SalesOrders":
                    content = CreateSalesOrdersContent();
                    break;
                case "Users":
                    content = CreateUsersContent();
                    break;
                case "Dealers":
                    content = CreateDealersContent();
                    break;
                case "Drugs":
                    content = CreateDrugsContent();
                    break;
                case "Core":
                    content = CreateCoreContent();
                    break;
                case "Chat":
                    content = CreateChatContent();
                    break;
                case "Email":
                    content = CreateEmailContent();
                    break;
                default:
                    content = CreateDashboardContent();
                    break;
            }

            // Dispose old content if it's a disposable control
            if (_currentContent is IDisposable disposable)
            {
                disposable.Dispose();
            }

            _currentContent = content;
            ContentArea.Content = content;
        }

        private UserControl CreateDashboardContent()
        {
            var grid = new Grid();
            var stackPanel = new StackPanel();

            var statsGrid = new Grid();
            statsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            statsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            statsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            statsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            statsGrid.Margin = new Thickness(0, 0, 0, 20);

            var stats = new[]
            {
                new { Icon = "💰", Title = "Total Revenue", Value = "$124,567", Color = "#0d9488" },
                new { Icon = "📦", Title = "Total Orders", Value = "1,234", Color = "#3b82f6" },
                new { Icon = "👥", Title = "Active Users", Value = "89", Color = "#8b5cf6" },
                new { Icon = "💊", Title = "Drugs in Stock", Value = "456", Color = "#f59e0b" }
            };

            for (int i = 0; i < stats.Length; i++)
            {
                var card = CreateStatCard(stats[i].Icon, stats[i].Title, stats[i].Value, stats[i].Color);
                Grid.SetColumn(card, i);
                statsGrid.Children.Add(card);
            }

            stackPanel.Children.Add(statsGrid);

            var chartCard = new Border
            {
                Style = (Style)FindResource("DashboardCard"),
                Height = 300,
                Margin = new Thickness(0, 0, 0, 20)
            };
            var chartContent = new StackPanel
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };
            chartContent.Children.Add(new TextBlock
            {
                Text = "📈 Monthly Sales Chart",
                FontSize = 18,
                FontWeight = FontWeights.SemiBold,
                Foreground = new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#1e293b"))
            });
            chartContent.Children.Add(new TextBlock
            {
                Text = "Chart will be displayed here",
                FontSize = 14,
                Foreground = new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#94a3b8")),
                Margin = new Thickness(0, 10, 0, 0)
            });
            chartCard.Child = chartContent;
            stackPanel.Children.Add(chartCard);

            var tableCard = new Border
            {
                Style = (Style)FindResource("DashboardCard"),
                Margin = new Thickness(0, 0, 0, 20)
            };
            var tableContent = new StackPanel();
            tableContent.Children.Add(new TextBlock
            {
                Text = "Recent Orders",
                FontSize = 16,
                FontWeight = FontWeights.SemiBold,
                Foreground = new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#1e293b")),
                Margin = new Thickness(0, 0, 0, 15)
            });

            var tableGrid = new Grid();
            tableGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            tableGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            tableGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            tableGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            tableGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var headers = new[] { "Order ID", "Customer", "Drug", "Date", "Status" };
            for (int i = 0; i < headers.Length; i++)
            {
                var textBlock = new TextBlock
                {
                    Text = headers[i],
                    FontWeight = FontWeights.SemiBold,
                    Foreground = new System.Windows.Media.SolidColorBrush(
                        (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#475569")),
                    Margin = new Thickness(0, 0, 0, 10)
                };
                Grid.SetColumn(textBlock, i);
                tableGrid.Children.Add(textBlock);
            }

            var sampleData = new[]
            {
                new[] { "#ORD-001", "John Doe", "Paracetamol", "2024-01-15", "✅ Delivered" },
                new[] { "#ORD-002", "Jane Smith", "Amoxicillin", "2024-01-14", "🚚 In Transit" },
                new[] { "#ORD-003", "Bob Johnson", "Ibuprofen", "2024-01-13", "⏳ Pending" },
                new[] { "#ORD-004", "Alice Brown", "Cetirizine", "2024-01-12", "✅ Delivered" },
                new[] { "#ORD-005", "Charlie Davis", "Omeprazole", "2024-01-11", "🚚 In Transit" }
            };

            for (int row = 0; row < sampleData.Length; row++)
            {
                for (int col = 0; col < sampleData[row].Length; col++)
                {
                    var textBlock = new TextBlock
                    {
                        Text = sampleData[row][col],
                        Foreground = new System.Windows.Media.SolidColorBrush(
                            (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#334155")),
                        Margin = new Thickness(0, 5, 0, 5)
                    };
                    Grid.SetRow(textBlock, row + 1);
                    Grid.SetColumn(textBlock, col);
                    tableGrid.Children.Add(textBlock);
                }
            }

            tableContent.Children.Add(tableGrid);
            tableCard.Child = tableContent;
            stackPanel.Children.Add(tableCard);

            grid.Children.Add(stackPanel);
            return new UserControl { Content = grid };
        }

        private Border CreateStatCard(string icon, string title, string value, string color)
        {
            var card = new Border
            {
                Style = (Style)FindResource("StatCard"),
                Margin = new Thickness(8)
            };

            var content = new StackPanel
            {
                Orientation = System.Windows.Controls.Orientation.Horizontal,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };

            var iconBorder = new Border
            {
                Width = 50,
                Height = 50,
                CornerRadius = new CornerRadius(12),
                Background = new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(color + "20")),
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 15, 0)
            };
            iconBorder.Child = new TextBlock
            {
                Text = icon,
                FontSize = 24,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };

            var textStack = new StackPanel
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };
            textStack.Children.Add(new TextBlock
            {
                Text = value,
                FontSize = 22,
                FontWeight = FontWeights.Bold,
                Foreground = new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#1e293b"))
            });
            textStack.Children.Add(new TextBlock
            {
                Text = title,
                FontSize = 13,
                Foreground = new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#64748b"))
            });

            content.Children.Add(iconBorder);
            content.Children.Add(textStack);
            card.Child = content;

            return card;
        }

        private UserControl CreateOrdersContent()
        {
            return CreatePlaceholderContent("📋 Orders Management", "Manage all orders here");
        }

        private UserControl CreatePurchaseOrdersContent()
        {
            try
            {
                var purchaseOrderView = new PurchaseOrderView();
                return purchaseOrderView;
            }
            catch (Exception ex)
            {
                return CreatePlaceholderContent("❌ Error Loading Purchase Orders", ex.Message);
            }
        }

        private UserControl CreateSalesOrdersContent()
        {
            try
            {
                var salesOrderView = new SalesOrderView();
                // SalesOrderView is a Window, but we need UserControl
                // Convert to UserControl
                var contentControl = new ContentControl
                {
                    Content = salesOrderView.Content
                };
                return new UserControl { Content = contentControl };
            }
            catch (Exception ex)
            {
                return CreatePlaceholderContent("❌ Error Loading Sales Orders", ex.Message);
            }
        }

        private UserControl CreateUsersContent()
        {
            return CreatePlaceholderContent("👥 Users", "Manage users here");
        }

        private UserControl CreateDealersContent()
        {
            return CreatePlaceholderContent("🏪 Dealers", "Manage dealers here");
        }

        private UserControl CreateDrugsContent()
        {
            return CreatePlaceholderContent("💊 Drugs", "Manage drugs inventory here");
        }

        private UserControl CreateCoreContent()
        {
            return CreatePlaceholderContent("⚙️ Core Settings", "Manage core settings here");
        }

        private UserControl CreateChatContent()
        {
            return CreatePlaceholderContent("💬 Chat", "Chat with team members");
        }

        private UserControl CreateEmailContent()
        {
            return CreatePlaceholderContent("✉️ Email", "Manage emails here");
        }

        private UserControl CreatePlaceholderContent(string title, string description)
        {
            var border = new Border
            {
                Style = (Style)FindResource("DashboardCard"),
                Height = 400,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch
            };

            var stackPanel = new StackPanel
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };

            stackPanel.Children.Add(new TextBlock
            {
                Text = "📄",
                FontSize = 48,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center
            });

            stackPanel.Children.Add(new TextBlock
            {
                Text = title,
                FontSize = 22,
                FontWeight = FontWeights.SemiBold,
                Foreground = new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#1e293b")),
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                Margin = new Thickness(0, 15, 0, 5)
            });

            stackPanel.Children.Add(new TextBlock
            {
                Text = description,
                FontSize = 14,
                Foreground = new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#94a3b8")),
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center
            });

            border.Child = stackPanel;
            return new UserControl { Content = border };
        }
    }
}
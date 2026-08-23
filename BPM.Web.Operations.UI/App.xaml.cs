using BPM.Web.Operations.UI.Helper;
using BPM.Web.Operations.UI.Models;
using BPM.Web.Operations.UI.Services;
using BPM.Web.Operations.UI.Views.Account;
using BPM.Web.Operations.UI.Views.Dashboard;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Windows;

namespace BPM.Web.Operations.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        // Exposed so windows/views can resolve services
        public IServiceProvider ServiceProvider => _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Don't tie app lifetime to whichever window happens to open first
            ShutdownMode = ShutdownMode.OnLastWindowClose;

            // Handle unhandled exceptions
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            DispatcherUnhandledException += OnDispatcherUnhandledException;

            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();

            // Check if user is already logged in
            var sessionManager = _serviceProvider.GetRequiredService<SessionManager>();
            if (sessionManager.IsAuthenticated())
            {
                var dashboardView = _serviceProvider.GetRequiredService<DashboardView>();
                dashboardView.Show();
            }
            else
            {
                var loginWindow = _serviceProvider.GetRequiredService<LoginView>();
                loginWindow.Show();
            }
        }

        private void ConfigureServices(ServiceCollection services)
        {
            try
            {
                // Configuration
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                var configPath = Path.Combine(basePath, "appsettings.json");

                if (!File.Exists(configPath))
                {
                    throw new FileNotFoundException($"Configuration file not found: {configPath}");
                }

                var configuration = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                services.AddSingleton<IConfiguration>(configuration);

                // Configure BPMConfig
                var bpmConfig = configuration.GetSection("BPMConfig");
                if (!bpmConfig.Exists())
                {
                    throw new InvalidOperationException("BPMConfig section is missing in appsettings.json");
                }

                services.Configure<BPMConfig>(bpmConfig);

                // Logging
                services.AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddDebug();
                    builder.SetMinimumLevel(LogLevel.Information);
                });

                // Register Services - Singleton for SessionManager
                services.AddSingleton<SessionManager>();

                // Register HttpClientService as Scoped (or Singleton if you prefer)
                services.AddScoped<HttpClientService>();

                // Register Repository Factory and Services
                services.AddScoped<IRepositoryFactory, RepositoryFactory>();
                services.AddScoped<IAuthenticateService, AuthenticateService>();
                services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
                services.AddScoped<ISalesOrderService, SalesOrderService>();
                services.AddScoped<IUserService, UserService>();
                services.AddScoped<IDealerService, DealerService>();
                services.AddSingleton<INavigationService, NavigationService>();

                // ViewModels
                services.AddTransient<LoginViewModel>();
                services.AddTransient<DashboardViewModel>();

                // Views
                services.AddTransient<LoginView>();
                services.AddTransient<DashboardView>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to configure application services: {ex.Message}",
                    "Configuration Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                throw;
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _serviceProvider?.Dispose();
            base.OnExit(e);
        }

        #region Exception Handlers

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            ShowFatalError(exception);
        }

        private void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            ShowFatalError(e.Exception);
        }

        private void ShowFatalError(Exception exception)
        {
            var message = exception?.Message ?? "An unknown error occurred.";
            var stackTrace = exception?.StackTrace ?? "";

            MessageBox.Show(
                $"An unexpected error occurred:\n\n{message}\n\n{stackTrace}",
                "Fatal Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        #endregion
    }
}
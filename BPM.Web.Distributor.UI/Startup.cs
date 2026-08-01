using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using BPM.Web.Distributor.UI.Helpers;
using BPM.Web.Distributor.UI.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BPM.Web.Distributor.UI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // Register Services
        public void ConfigureServices(IServiceCollection services)
        {
          
            services.Configure<BPMConfig>(
                Configuration.GetSection("BPMConfig"));


            services.AddControllersWithViews();


            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddHttpContextAccessor();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";

                    options.Cookie.Name = "BPMAuth";

                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromHours(2);
                });

            services.AddAuthorization();

            services.AddNotyf(config =>
            {
                config.DurationInSeconds = 5;
                config.IsDismissable = true;
                config.Position = NotyfPosition.TopRight;
            });

            services.AddTransient<TokenAuthorizationHttpClientHandler>();

            services.AddHttpClient<IAuthenticateService, AuthenticateService>((provider, client) =>
            {
                var config = Configuration
                    .GetSection("BPMConfig")
                    .Get<BPMConfig>();

                client.BaseAddress = new Uri(config.BaseUrl);
            })
            .AddHttpMessageHandler<TokenAuthorizationHttpClientHandler>();

        }

        // Configure Middleware
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseNotyf();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using BPM.Web.Distributor.UI.Helpers;
using BPM.Web.Distributor.UI.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json.Serialization;

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
            // Configure BPMConfig
            services.Configure<BPMConfig>(Configuration.GetSection("BPMConfig"));

            // Add Controllers with Newtonsoft Json
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            services.AddMvc().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.AddDirectoryBrowser();

            // Add Distributed Memory Cache
            services.AddDistributedMemoryCache();

            // Add Session
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            // Add Http Context Accessor
            services.AddHttpContextAccessor();

            // Add HttpClient
            services.AddHttpClient();

            // Register Services
            services.AddScoped<HttpClientService>();
            services.AddTransient<TokenAuthorizationHttpClientHandler>();
            services.AddHttpClient("AuthorizedClient")
                .AddHttpMessageHandler<TokenAuthorizationHttpClientHandler>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            // Register Repository Factory and Services
            services.AddScoped<IRepositoryFactory, RepositoryFactory>();
            services.AddScoped<IAuthenticateService, AuthenticateService>();
            services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDrugService, DrugService>();
            services.AddScoped<IDealerService, DealerService>();
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<IFeatureService, FeatureService>();
            // Add other services as needed

            // Configure Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.Cookie.Name = "BPMAuth";
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(2);
                options.ReturnUrlParameter = "returnUrl";
            });

            // Add Authorization Policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy("DistributorPortal", policy =>
                {
                    policy.RequireClaim("Portal", "Distributor");
                });
            });

            // Configure Notyf
            services.AddNotyf(config =>
            {
                config.DurationInSeconds = 10;
                config.IsDismissable = true;
                config.Position = NotyfPosition.TopCenter;
            });
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

            // Configure Static Files with Cache Control
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers["Cache-Control"] = "no-cache, no-store";
                    ctx.Context.Response.Headers["Pragma"] = "no-cache";
                    ctx.Context.Response.Headers["Expires"] = "-1";
                }
            });

            app.UseRouting();

            app.UseSession();

            // Important: Authentication must be added after UseSession
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseNotyf();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}
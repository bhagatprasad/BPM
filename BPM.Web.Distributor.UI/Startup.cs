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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddSession();

            services.AddHttpContextAccessor();

            services.Configure<BPMConfig>(
                Configuration.GetSection("BPMConfig"));

            services.AddTransient<TokenAuthorizationHttpClientHandler>();

            services.AddHttpClient("AuthorizedClient")
                    .AddHttpMessageHandler<TokenAuthorizationHttpClientHandler>();

            services.AddScoped<Services.HttpClientService>();

            services.AddScoped<AccountService>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                });
          
        }

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}
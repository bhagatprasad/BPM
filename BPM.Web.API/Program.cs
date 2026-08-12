using BPM.Web.API.CustomFilters;
using BPM.Web.API.GlobalExceptionHandling;
using BPM.Web.API.Models.Data;
using BPM.Web.API.RabbitMQ;
using BPM.Web.API.RabbitMQ.Publisher;
using BPM.Web.API.RabbitMQ.Subscriber;
using BPM.Web.API.Repository;
using BPM.Web.API.Service;
using BPM.Web.API.Services;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// LOGGING CONFIGURATION
// ============================================================
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var log4netConfigFile = new FileInfo("log4net.config");
if (!log4netConfigFile.Exists)
{
    throw new FileNotFoundException("log4net.config file not found.");
}

var logDirectory = Path.Combine(builder.Environment.ContentRootPath, "Logs");
if (!Directory.Exists(logDirectory))
{
    Directory.CreateDirectory(logDirectory);
}

var logPath = Path.Combine(logDirectory, "AppLog.txt");
GlobalContext.Properties["LogFileName"] = logPath;
XmlConfigurator.Configure(log4netConfigFile);
builder.Logging.AddLog4Net();

var startupLogger = builder.Services.BuildServiceProvider()
    .GetRequiredService<ILogger<Program>>();
startupLogger.LogInformation($"Application starting. Log file path: {logPath}");

// ============================================================
// SERVICES
// ============================================================
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ============================================================
// DATABASE CONFIGURATION - UPDATED FOR POSTGRESQL
// ============================================================
builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        // Enable retry on failure for better resilience
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorCodesToAdd: null);

        // Set command timeout (optional)
        npgsqlOptions.CommandTimeout(60);

        // Use query splitting behavior for better performance
        npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);

        // Note: EnableDateTimeKindHandling is not available in all versions
        // The DateTime handling is now done in the DbContext
    });

    // Enable sensitive data logging and detailed errors in development
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

builder.Services.Configure<RabbitMQSettings>(
    builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddScoped<BPMAuthorize>();

// Repositories
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IDealerRepository, DealerRepository>();
builder.Services.AddScoped<IDrugRepository, DrugRepository>();
builder.Services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
builder.Services.AddScoped<IDrugCategoryRepository, DrugCategoryRepository>();
builder.Services.AddScoped<IUserRespository, UserRespository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
builder.Services.AddScoped<IDrugUomRepository, DrugUomRepository>();
builder.Services.AddScoped<IPackagingMasterRepository, PackagingMasterRepository>();
builder.Services.AddScoped<IUserLoginHistoryRepository, UserLoginHistoryRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IDrugFormRepository, DrugFormRepository>();
builder.Services.AddScoped<IDrugPackagingRepository, DrugPackagingRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IUserPasswordHistoryRepository, UserPasswordHistoryRepository>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddScoped<IFeatureRepository, FeatureRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<ISalesOrderRepository, SalesOrderRepository>();
builder.Services.AddScoped<IBillingRepository, BillingRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();

// Services
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IDealerService, DealerService>();
builder.Services.AddScoped<IDrugService, DrugService>();
builder.Services.AddScoped<IManufacturerService, ManufacturerService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IDrugCategoryService, DrugCategoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
builder.Services.AddScoped<IDrugUomService, DrugUomService>();
builder.Services.AddScoped<IPackagingMasterService, PackagingMasterService>();
builder.Services.AddScoped<IDrugFormService, DrugFormService>();
builder.Services.AddScoped<IDrugPackagingService, DrugPackagingService>();
builder.Services.AddScoped<IActivityService, ActivityService>();
builder.Services.AddScoped<IFeatureService, FeatureService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<ISalesOrderService, SalesOrderService>();
builder.Services.AddScoped<IBillingService, BillingService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();

// RabbitMQ
builder.Services.AddSingleton<RabbitMQPublisher>();
builder.Services.AddSingleton<IRabbitMQPublisher>(sp => sp.GetRequiredService<RabbitMQPublisher>());
builder.Services.AddHostedService<PasswordHistorySubscriber>();
builder.Services.AddHostedService<UserLoginHistorySubscriber>();
builder.Services.AddHostedService<RefreshTokenSubscriber>();

// Health Checks
builder.Services.AddHealthChecks().AddCheck<RabbitMQHealthCheck>("rabbitmq");

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();

// ============================================================
// JWT AUTHENTICATION
// ============================================================
var tokenKey = builder.Configuration.GetValue<string>("Jwt:Key");
if (string.IsNullOrEmpty(tokenKey))
{
    throw new InvalidOperationException("JWT Key is not configured in appsettings.json");
}

var key = Encoding.ASCII.GetBytes(tokenKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// ============================================================
// SWAGGER
// ============================================================
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "BPM Web API",
        Description = "API for Business Process Management",
        Contact = new OpenApiContact
        {
            Name = "BPM Team",
            Email = "support@bpm.com"
        }
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// ============================================================
// BUILD APP
// ============================================================
var app = builder.Build();

// ============================================================
// MIDDLEWARE
// ============================================================
var logger = app.Services.GetRequiredService<ILogger<Program>>();
app.ConfigureExceptionHandler(logger);

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "BPM Web API v1");
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

var startupLogger2 = app.Services.GetRequiredService<ILogger<Program>>();
startupLogger2.LogInformation("Application started successfully");

app.Run();
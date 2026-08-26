using System.Text.Json.Serialization;
using BPM.Web.Orders.API.Models.Data;
using BPM.Web.Orders.API.Repository;
using BPM.Web.Orders.API.Services;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Database Context
builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorCodesToAdd: null);
        npgsqlOptions.CommandTimeout(60);
        npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
    });

    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

// ============================================================
// REGISTER HTTPCLIENT TO CALL WEB.API
// ============================================================
var apiBaseUrl = builder.Configuration["Services:ApiBaseUrl"] ?? "https://localhost:7001";

// Register named HttpClient for calling Web.API
builder.Services.AddHttpClient("WebApiClient", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
})
.AddPolicyHandler(GetRetryPolicy())
.AddPolicyHandler(GetCircuitBreakerPolicy());

// Register Repositories (Orders only)
builder.Services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
builder.Services.AddScoped<IPurchaseOrderApprovalRepository, PurchaseOrderApprovalRepository>();
builder.Services.AddScoped<ISalesOrderRepository, SalesOrderRepository>();

// Register Services (Orders only)
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
builder.Services.AddScoped<ISalesOrderService, SalesOrderService>();

// Register HTTP Clients for specific services
builder.Services.AddScoped<IUserServiceClient, UserServiceClient>();
builder.Services.AddScoped<IInventoryServiceClient, InventoryServiceClient>();
builder.Services.AddScoped<IDrugServiceClient, DrugServiceClient>();

// Background Service
builder.Services.AddHostedService<PurchaseOrderDraftCleanupService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthorization();
app.MapControllers();
app.Run();

// ============================================================
// POLLY POLICIES
// ============================================================
static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(
            retryCount: 3,
            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}

static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 5,
            durationOfBreak: TimeSpan.FromSeconds(30));
}
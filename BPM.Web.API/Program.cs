using BPM.Web.API.Models.Data;
using BPM.Web.API.Repository;
using BPM.Web.API.Service;
using BPM.Web.API.Services;
using log4net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

#region Configure Logging

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var logPath = Path.Combine(builder.Environment.ContentRootPath, "Logs", "AppLog.txt");
GlobalContext.Properties["LogFileName"] = logPath;

builder.Logging.AddLog4Net("log4net.config");



#endregion

// Add services to the container.
builder.Services.AddControllers();

// Configure PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Services
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IDealerService, DealerService>();

builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IDealerRepository, DealerRepository>();

builder.Services.AddScoped<IDrugRepository, DrugRepository>();
builder.Services.AddScoped<IDrugService, DrugService>();

builder.Services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
builder.Services.AddScoped<IManufacturerService, ManufacturerService>();

builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<ISupplierService, SupplierService>();

builder.Services.AddScoped<IDrugCategoryRepository, DrugCategoryRepository>();
builder.Services.AddScoped<IDrugCategoryService, DrugCategoryService>();

builder.Services.AddScoped<IUserRespository, UserRespository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IAccountService,AccountService>();

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();

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
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "BPM Web API v1");
        // options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
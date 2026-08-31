using BPM.Web.Drug.API.GlobalExceptionHandling;
using BPM.Web.Drug.API.Models.Data;
using BPM.Web.Drug.API.Repositories;
using BPM.Web.Drug.API.Repositories.Interfaces;
using BPM.Web.Drug.API.Services;
using BPM.Web.Drug.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// SERVICES
builder.Services.AddControllers();


// DATABASE CONFIGURATION - POSTGRESQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});


// DEPENDENCY INJECTION - REPOSITORIES
builder.Services.AddScoped<IDrugRepository, DrugRepository>();
builder.Services.AddScoped<IDrugCategoryRepository, DrugCategoryRepository>();
builder.Services.AddScoped<IDrugFormRepository,DrugFormRepository>();
builder.Services.AddScoped<IDrugUomRepository,DrugUomRepository>();
builder.Services.AddScoped<IDrugPackagingRepository,DrugPackagingRepository>();



// DEPENDENCY INJECTION - SERVICES
builder.Services.AddScoped<IDrugService, DrugService>();
builder.Services.AddScoped<IDrugCategoryService, DrugCategoryService>();
builder.Services.AddScoped<IDrugFormService,DrugFormService>();
builder.Services.AddScoped<IDrugUomService,DrugUomService>();
builder.Services.AddScoped<IDrugPackagingService,DrugPackagingService>();

// JWT AUTHENTICATION
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
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// SWAGGER / API DOCUMENTATION
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "BPM Drug API",
        Description = "API for Drug Management Microservice"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token"
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
            new string[] { }
        }
    });
});


// BUILD APPLICATION
var app = builder.Build();
var logger = app.Services.GetRequiredService<ILogger<Program>>();

app.ConfigureExceptionHandler(logger);


// HTTP REQUEST PIPELINE
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
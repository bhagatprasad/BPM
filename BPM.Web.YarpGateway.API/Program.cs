using Yarp.ReverseProxy.Configuration;
using Microsoft.OpenApi.Models;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Global SSL bypass for development - SIMPLEST AND MOST RELIABLE
ServicePointManager.ServerCertificateValidationCallback +=
    (sender, cert, chain, sslPolicyErrors) => true;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "BPM Web API",
        Description = "API Gateway for Business Process Management",
        Contact = new OpenApiContact
        {
            Name = "BPM Team",
            Email = "support@bpm.com"
        }
    });
});

// Add YARP
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("Development", policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200",
            "http://localhost:5177"
        )
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});

var app = builder.Build();

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
app.UseCors("Development");
app.MapControllers();
app.MapReverseProxy();

app.Run();
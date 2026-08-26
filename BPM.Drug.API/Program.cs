using BPM.Web.Drug.API.Models.Data;
using BPM.Web.Drug.API.Repositories;
using BPM.Web.Drug.API.Services;
using Microsoft.EntityFrameworkCore;

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


// DEPENDENCY INJECTION - SERVICES
builder.Services.AddScoped<IDrugService, DrugService>();
builder.Services.AddScoped<IDrugCategoryService, DrugCategoryService>();


// SWAGGER / API DOCUMENTATION
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// BUILD APPLICATION
var app = builder.Build();


// HTTP REQUEST PIPELINE
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
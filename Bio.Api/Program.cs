using Bio.Application.Extensions;
using Bio.Domain.Enums;
using Bio.Domain.Settings;
using Bio.Infrastructure.Data;
using Bio.Infrastructure.Extensions;
using Microsoft.Extensions.FileProviders;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add Infrastructure and Application Services
builder.Services.AddInfrastructureServices(builder.Configuration.GetConnectionString("DefaultConnection"));


var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.AddSingleton(jwtSettings);
builder.Services.AddApplicationServices(jwtSettings);



var filesPath = Path.Combine(builder.Environment.ContentRootPath, "Files");
if (!Directory.Exists(filesPath))
{
    Directory.CreateDirectory(filesPath);
}

var photosPath = Path.Combine(builder.Environment.ContentRootPath, "Photos");
if (!Directory.Exists(photosPath))
{
    Directory.CreateDirectory(photosPath);
}



// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin() // domain eklenince policy.WithOrigins("https://domain.com") olarak güncelle
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});




// Add Authorization Policies
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireSuperAdmin", policy =>
        policy.RequireClaim(ClaimTypes.Role, nameof(UserRole.SuperAdmin)))
    .AddPolicy("RequireAdmin", policy =>
        policy.RequireClaim(ClaimTypes.Role, nameof(UserRole.SuperAdmin), nameof(UserRole.Admin)))
    .AddPolicy("RequireJobSeeker", policy =>
        policy.RequireClaim(ClaimTypes.Role, nameof(UserRole.JobSeeker)))
    .AddPolicy("RequireCompany", policy =>
        policy.RequireClaim(ClaimTypes.Role, nameof(UserRole.Company)))
    .AddPolicy("RequireJobSeekerOrAdmin", policy =>
        policy.RequireClaim(ClaimTypes.Role, nameof(UserRole.JobSeeker), nameof(UserRole.Admin), nameof(UserRole.SuperAdmin)))
    .AddPolicy("RequireCompanyOrAdminOrSuperAdmin", policy =>
        policy.RequireClaim(ClaimTypes.Role, nameof(UserRole.Company), nameof(UserRole.Admin), nameof(UserRole.SuperAdmin)))
    .AddPolicy("RequireCompanyOrAJobSeekerOrAdmin", policy =>
        policy.RequireClaim(ClaimTypes.Role, nameof(UserRole.Company), nameof(UserRole.JobSeeker), nameof(UserRole.Admin), nameof(UserRole.SuperAdmin)));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var databaseInitializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
    Console.WriteLine("DatabaseInitializer çalýþtýrýlýyor...");
    await databaseInitializer.SeedSuperAdminAsync();
    Console.WriteLine("DatabaseInitializer tamamlandý.");
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(filesPath),
    RequestPath = "/Files"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(photosPath),
    RequestPath = "/Photos"
});



// Middleware
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll");
app.UseStaticFiles();

app.MapControllers();

app.Run();

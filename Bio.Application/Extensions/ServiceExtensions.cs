using Bio.Application.Dtos.User;
using Bio.Application.Helpers;
using Bio.Application.Interfaces;
using Bio.Application.Mapping;
using Bio.Application.Services;
using Bio.Domain.Interfaces;
using Bio.Domain.Settings;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace Bio.Application.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services, JwtSettings jwtSettings)
        {

            // JWT Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // FluentValidation
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // AutoMapper
            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            // Application Services
            services.AddScoped<IHashingService, HashingHelper>();
            services.AddScoped<ITokenService, TokenHelper>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<ICertificateService, CertificateService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IJobApplicationService, JobApplicationService>();
            services.AddScoped<IJobCategoryService, JobCategoryService>();
            services.AddScoped<IJobPostService, JobPostService>();
            services.AddScoped<IResumeService, ResumeService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IDashboardCompanyService, DashboardCompanyService>();
        }
    }
}

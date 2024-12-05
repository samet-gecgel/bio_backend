using Bio.Domain.Interfaces;
using Bio.Domain.Repositories;
using Bio.Infrastructure.Data;
using Bio.Infrastructure.Helpers;
using Bio.Infrastructure.Persistence;
using Bio.Infrastructure.Persistence.Contexts;
using Bio.Infrastructure.Persistence.Repositories;
using Bio.Infrastructure.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bio.Infrastructure.Extensions
{
    public static class ServiceExtensions 
    {
        public static void AddInfrastructureServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<BioDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IFileService, FileService>(provider =>
            {
                var env = provider.GetRequiredService<IWebHostEnvironment>();
                var filesPath = Path.Combine(env.ContentRootPath, "Files");
                return new FileService(filesPath);
            });

            services.AddScoped<IPhotoService, PhotoService>(provider =>
            {
                var env = provider.GetRequiredService<IWebHostEnvironment>();
                var photosPath = Path.Combine(env.ContentRootPath, "Photos");
                return new PhotoService(photosPath);
            });

            services.AddScoped<DatabaseInitializer>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();


            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<ICertificateRepository, CertificateRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IResumeRepository, ResumeRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
            services.AddScoped<IJobCategoryRepository, JobCategoryRepository>();
            services.AddScoped<IJobPostRepository, JobPostRepository>();
            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IDashboardCompanyRepository, DashboardCompanyRepository>();
        }
    }
}

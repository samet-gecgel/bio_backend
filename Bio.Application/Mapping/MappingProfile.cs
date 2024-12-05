using AutoMapper;
using Bio.Application.Dtos.Admin;
using Bio.Application.Dtos.Certificate;
using Bio.Application.Dtos.Company;
using Bio.Application.Dtos.Dashboard;
using Bio.Application.Dtos.JobApplication;
using Bio.Application.Dtos.JobCategory;
using Bio.Application.Dtos.JobPost;
using Bio.Application.Dtos.News;
using Bio.Application.Dtos.Resume;
using Bio.Application.Dtos.User;
using Bio.Domain.Entities;

namespace Bio.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Admin
            CreateMap<Admin, AdminDto>().ReverseMap();
            CreateMap<Admin, AdminCreateDto>().ReverseMap();
            CreateMap<Admin, AdminUpdateDto>().ReverseMap();
            CreateMap<Admin, AdminPasswordUpdateDto>().ReverseMap();
            CreateMap<Admin, AdminLoginDto>().ReverseMap();

            // User
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserCreateDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();
            CreateMap<User, UserPasswordUpdateDto>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();

            // Company
            CreateMap<Company, CompanyDto>()
                .ForMember(dest => dest.JobPosts, opt => opt.MapFrom(src => src.JobPosts));
            CreateMap<Company, CompanyCreateDto>().ReverseMap();
            CreateMap<Company, CompanyUpdateDto>().ReverseMap();
            CreateMap<Company, CompanyPasswordUpdateDto>().ReverseMap();
            CreateMap<Company, CompanyLoginDto>().ReverseMap();
            CreateMap<Company, PublicCompanyDto>().ReverseMap();

            // Certificate
            CreateMap<Certificate, CertificateDto>().ReverseMap();
            CreateMap<Certificate, CertificateCreateDto>().ReverseMap();
            CreateMap<Certificate, CertificateUpdateDto>().ReverseMap();

            // Resume
            CreateMap<Resume, ResumeDto>().ReverseMap();
            CreateMap<Resume, ResumeCreateDto>().ReverseMap();
            CreateMap<Resume, ResumeUpdateDto>().ReverseMap();

            // JobPost
            CreateMap<JobPost, JobPostDto>()
                .ForMember(dest => dest.ViewCount, opt => opt.MapFrom(src => src.ViewCount))
                .ForMember(dest => dest.ApplicationCount, opt => opt.MapFrom(src => src.JobApplications != null ? src.JobApplications.Count : 0))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.JobCategory.Name))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.CompanyName));
            CreateMap<JobPostCreateDto, JobPost>().ReverseMap();
            CreateMap<JobPost, JobPostUpdateDto>().ReverseMap();
            CreateMap<JobPost, JobPostFilterDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.JobCategory.Name)) 
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.CompanyName))
                .ForMember(dest => dest.Districts, opt => opt.MapFrom(src => src.District))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.PublishedDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.ApplicationDeadline))
                .ForMember(dest => dest.OffDays, opt => opt.MapFrom(src => src.OffDays))
                .ForMember(dest => dest.RequiredEducationLevel, opt => opt.MapFrom(src => src.RequiredEducationLevel))
                .ForMember(dest => dest.JobTypes, opt => opt.MapFrom(src => src.JobType))
                .ForMember(dest => dest.ExperienceLevel, opt => opt.MapFrom(src => src.ExperienceLevel))
                .ForMember(dest => dest.RequiresDrivingLicense, opt => opt.MapFrom(src => src.RequiresDrivingLicense))
                .ForMember(dest => dest.IsDisabledFriendly, opt => opt.MapFrom(src => src.IsDisabledFriendly))
    .ReverseMap();

            // JobApplication
            CreateMap<JobApplication, JobApplicationDto>()
                .ForMember(dest => dest.JobPostTitle, opt => opt.MapFrom(src => src.JobPost.Title))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.JobPost.Company.CompanyName))
                .ForMember(dest => dest.ApplicantName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.ApplicationDate, opt => opt.MapFrom(src => src.ApplicationDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
            CreateMap<JobApplication, JobApplicationDetailDto>()
                    .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.JobPost.Title))
                    .ForMember(dest => dest.ApplicantName, opt => opt.MapFrom(src => src.User.FullName))
                    .ForMember(dest => dest.Certificates, opt => opt.MapFrom(src => src.User.Certificates))
                    .ForMember(dest => dest.ResumeId, opt => opt.MapFrom(src => src.Resume.Id))
                    .ForMember(dest => dest.CoverLetter, opt => opt.MapFrom(src => src.CoverLetter));
            CreateMap<JobApplication, JobApplicationCreateDto>().ReverseMap();
            CreateMap<JobApplication, JobApplicationUpdateDto>().ReverseMap();

            // Job Category
            CreateMap<JobCategory, JobCategoryDto>().ReverseMap();
            CreateMap<JobCategory, JobCategoryCreateDto>().ReverseMap();
            CreateMap<JobCategory, JobCategoryUpdateDto>().ReverseMap();

            //News
            CreateMap<News, NewsDto>()
                .ForMember(dest => dest.AdminName, opt => opt.MapFrom(src => src.Admin.FullName));
            CreateMap<News, NewsCreateDto>().ReverseMap();
            CreateMap<News, NewsUpdateDto>().ReverseMap();

            CreateMap<JobApplication, RecentApplicationDto>()
                .ForMember(dest => dest.JobPostTitle, opt => opt.MapFrom(src => src.JobPost.Title))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.JobPost.Company.CompanyName))
                .ForMember(dest => dest.ApplicantName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.ApplicationDate, opt => opt.MapFrom(src => src.ApplicationDate));


            CreateMap<IGrouping<string, JobApplication>, MostAppliedCompaniesDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Key))
                .ForMember(dest => dest.TotalApplications, opt => opt.MapFrom(src => src.Count()));

        }
    }
}

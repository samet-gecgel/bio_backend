using Bio.Application.Dtos.JobPost;
using Bio.Domain.Enums;

namespace Bio.Application.Dtos.Company
{
    public class CompanyDto
    {
        public Guid Id { get; set; }
        public string TcKimlik { get; set; }
        public string Vkn { get; set; } // vergi kimlik numarası
        public string FullName { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string Position { get; set; }
        public string District { get; set; }
        public EmployeesRange EmployeesInCity { get; set; }
        public EmployeesRange EmployeesInCountry { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public AccountApprovalStatus ApprovalStatus { get; set; }
        public ICollection<JobPostDto> JobPosts { get; set; } = new List<JobPostDto>();
    }

}

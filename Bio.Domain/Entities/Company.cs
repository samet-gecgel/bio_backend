using Bio.Domain.Enums;

namespace Bio.Domain.Entities
{
    public class Company
    {
        public Guid Id { get; set; }
        public string TcKimlik { get; set; }
        public string Vkn { get; set; } // vergi kimlik numarası
        public string FullName { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string District { get; set; }
        public string CompanyName { get; set; }
        public string Position { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public EmployeesRange EmployeesInCity { get; set; }
        public EmployeesRange EmployeesInCountry { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public AccountApprovalStatus ApprovalStatus { get; set; }
        public UserRole Role { get; set; }

        public ICollection<JobPost> JobPosts { get; set; } = new List<JobPost>();
    }
}

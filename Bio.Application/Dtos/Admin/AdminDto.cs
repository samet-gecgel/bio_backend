using Bio.Domain.Enums;

namespace Bio.Application.Dtos.Admin
{
    public class AdminDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? Updated { get; set; }
        public UserRole Role { get; set; }
    }
}

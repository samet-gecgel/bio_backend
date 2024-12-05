using Bio.Domain.Enums;

namespace Bio.Domain.Entities
{
    public class Admin
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } 
        public UserRole Role { get; set; }
        public ICollection<News> News { get; set; } = new List<News>();
    }
}

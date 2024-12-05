namespace Bio.Domain.Entities
{
    public class Certificate
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string CertificateName { get; set; }
        public string Institution { get; set; }
        public DateTime IssueDate { get; set; }
        public string FilePath { get; set; }
        public User User { get; set; }
    }
}

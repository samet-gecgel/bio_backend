namespace Bio.Application.Dtos.Certificate
{
    public class CertificateDto
    {
        public Guid Id { get; set; }
        public string CertificateName { get; set; }
        public string Institution { get; set; }
        public DateTime IssueDate { get; set; }
        public string FilePath { get; set; }
    }
}

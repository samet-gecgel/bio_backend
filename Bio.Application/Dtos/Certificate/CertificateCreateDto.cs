namespace Bio.Application.Dtos.Certificate
{
    public class CertificateCreateDto
    {
        public string CertificateName { get; set; }
        public string Institution { get; set; }
        public DateTime IssueDate { get; set; }
    }
}

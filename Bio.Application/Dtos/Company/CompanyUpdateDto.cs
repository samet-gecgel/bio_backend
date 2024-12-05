using Bio.Domain.Enums;

namespace Bio.Application.Dtos.Company
{
    public class CompanyUpdateDto
    {
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
    }

}

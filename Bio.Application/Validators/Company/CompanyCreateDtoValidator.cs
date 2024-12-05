using Bio.Application.Dtos.Company;
using FluentValidation;

namespace Bio.Application.Validators.Company
{
    public class CompanyCreateDtoValidator : AbstractValidator<CompanyCreateDto>
    {
        public CompanyCreateDtoValidator() 
        {
            RuleFor(c => c.TcKimlik).NotEmpty().WithMessage("Tc kimlik alanı numarası boş olamaz")
                .Length(11).WithMessage("Tc Kimlik numarası 11 haneli olmalıdır.")
                .Matches("^[0-9]*$").WithMessage("T.C. Kimlik numarası sadece rakamlardan oluşmalıdır.");

            RuleFor(c => c.Vkn)
            .NotEmpty().WithMessage("Vergi kimlik numarası alanı boş olamaz.")
            .MaximumLength(11).WithMessage("Vergi kimlik numarası en fazla 11 haneli olmalıdır.")
            .MinimumLength(8).WithMessage("Vergi kimlik numarası en az 8 haneli olmalıdır.")
            .Matches("^[0-9]*$").WithMessage("Vergi kimlik numarası sadece rakamlardan oluşmalıdır.");

            RuleFor(c => c.FullName)
            .NotEmpty().WithMessage("İsim ve Soyisim alanı boş olamaz.")
            .MaximumLength(100).WithMessage("İsim ve Soyisim en fazla 100 karakter olabilir.");

            RuleFor(c => c.Phone)
            .NotEmpty().WithMessage("Telefon numarası alanı boş olamaz.")
            .Matches(@"^\d{10,15}$").WithMessage("Telefon numarası geçerli bir formatta olmalıdır (10-15 rakam).");

            RuleFor(c => c.BirthDate)
            .NotEmpty().WithMessage("Doğum tarihi alanı boş olamaz.")
            .LessThan(DateTime.Now.AddYears(-18)).WithMessage("Kayıt olabilmek için en az 18 yaşında olmalısınız.");


            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("Email adresi alanı boş olamaz.")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");

            RuleFor(c => c.CompanyName)
                .NotEmpty().WithMessage("Şirket adı alanı boş olamaz.")
                .MaximumLength(200).WithMessage("Şirket adı en fazla 200 karakter olmalıdır.");

            RuleFor(c => c.Position)
                .NotEmpty().WithMessage("Pozisyon bilgisi alanı boş olamaz.");
            
            RuleFor(c => c.District)
                .NotEmpty().WithMessage("İlçe bilgisi alanı boş olamaz.");

            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("Şifre alanı boş olamaz.")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.");

            RuleFor(c => c.EmployeesInCity)
                .IsInEnum().WithMessage("Geçersiz çalışan aralığı seçimi.");

            RuleFor(c => c.EmployeesInCountry)
                .IsInEnum().WithMessage("Geçersiz çalışan aralığı seçimi.");
        }
    }
}

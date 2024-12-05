using FluentValidation;
using Bio.Application.Dtos.User;

namespace Bio.Application.Validators.User
{
    public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
    {
        public UserCreateDtoValidator()
        {
            RuleFor(u => u.FullName)
                .NotEmpty().WithMessage("Ad ve soyad alanı boş olamaz.")
                .MaximumLength(100).WithMessage("Ad ve soyad en fazla 100 karakter olabilir.");

            RuleFor(u => u.TcKimlik)
                .NotEmpty().WithMessage("TC Kimlik numarası boş olamaz.")
                .Length(11).WithMessage("TC Kimlik numarası 11 karakter olmalıdır.")
                .Matches("^[0-9]+$").WithMessage("TC Kimlik numarası yalnızca sayılardan oluşmalıdır.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email adresi boş olamaz.")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");

            RuleFor(u => u.Phone)
                .NotEmpty().WithMessage("Telefon numarası boş olamaz.")
                .Matches(@"^\+?\d{10,15}$").WithMessage("Geçerli bir telefon numarası giriniz.");

            RuleFor(u => u.BirthDate)
                .NotEmpty().WithMessage("Doğum tarihi boş olamaz.")
                .LessThan(DateTime.UtcNow.AddYears(-18)).WithMessage("Kullanıcı en az 18 yaşında olmalıdır.");

            RuleFor(u => u.District)
                .NotEmpty().WithMessage("İlçe bilgisi boş olamaz.")
                .MaximumLength(100).WithMessage("İlçe bilgisi en fazla 100 karakter olabilir.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Şifre alanı boş olamaz.")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.");
        }
    }
}
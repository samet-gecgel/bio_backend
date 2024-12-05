using AutoMapper;
using Bio.Application.Dtos.Admin;
using Bio.Application.Dtos.JobPost;
using Bio.Application.Dtos.User;
using Bio.Application.Helpers;
using Bio.Application.Interfaces;
using Bio.Domain.Entities;
using Bio.Domain.Enums;
using Bio.Domain.Interfaces;
using Bio.Domain.Repositories;
using BlogProject.Application.Result;
using System.Net;

namespace Bio.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IHashingService _hashingService;
        private readonly IUnitOfWork _unitOfWork;

        public AdminService(IAdminRepository adminRepository, IMapper mapper, ITokenService tokenService, IHashingService hashingService , IUnitOfWork unitOfWork)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
            _tokenService = tokenService;
            _hashingService = hashingService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<AdminDto>> CreateAdminAsync(AdminCreateDto adminCreateDto)
        {
            var existingAdmin = await _adminRepository.GetByEmailAsync(adminCreateDto.Email);
            if(existingAdmin != null)
            {
                return ServiceResult<AdminDto>.Fail("Bu email adresi zaten kullanılıyor.");
            }

            _hashingService.CreatePasswordHash(adminCreateDto.Password, out var passwordHash, out var passwordSalt);

            var admin = _mapper.Map<Admin>(adminCreateDto);
            admin.PasswordHash = passwordHash;
            admin.PasswordSalt = passwordSalt;

            admin.Role = UserRole.Admin;
            admin.CreatedAt = DateTime.Now;

            await _adminRepository.AddAsync(admin);

            var adminDto = _mapper.Map<AdminDto>(admin);

            await _unitOfWork.SaveChangesAsync();
            
            return ServiceResult<AdminDto>.SuccessAsCreated(adminDto);
        }

        public async Task<ServiceResult> DeleteAdminAsync(Guid id)
        {
            var admin = await _adminRepository.GetByIdAsync(id);
            if(admin == null)
            {
                return ServiceResult.Fail("Admin bulunamadı", HttpStatusCode.NotFound);
            }

            _adminRepository.Delete(admin);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
        public async Task<ServiceResult<AdminDto>> GetAdminByIdAsync(Guid id)
        {
            var admin = await _adminRepository.GetByIdAsync(id);

            if(admin == null)
            {
                return ServiceResult<AdminDto>.Fail("Admin bulunamadı.", HttpStatusCode.NotFound);
            }

            var adminDto = _mapper.Map<AdminDto>(admin);

            return ServiceResult<AdminDto>.Success(adminDto);
        }

        public async Task<ServiceResult<IEnumerable<AdminDto>>> GetAllAdminAsync()
        {
            var admins = await _adminRepository.FindAllAsync(a => a.Role == UserRole.Admin);

            if (admins == null || !admins.Any())
            {
                return ServiceResult<IEnumerable<AdminDto>>.Fail("Henüz kayıtlı bir admin bulunamadı");
            }

            var adminDtos = _mapper.Map<IEnumerable<AdminDto>>(admins);

            return ServiceResult<IEnumerable<AdminDto>>.Success(adminDtos);
        }

        public async Task<ServiceResult> UpdateAdminAsync(Guid id, AdminUpdateDto adminUpdateDto)
        {
            var admin = await _adminRepository.GetByIdAsync(id);
            if(admin == null)
            {
                return ServiceResult.Fail("Admin bulunamadı", HttpStatusCode.NotFound);
            }

            _mapper.Map(adminUpdateDto, admin);
            admin.UpdatedAt = DateTime.Now;

            _adminRepository.Update(admin);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> UpdateAdminPasswordAsync(Guid id, AdminPasswordUpdateDto passwordUpdateDto)
        {
            var admin = await _adminRepository.GetByIdAsync(id);
            if (admin == null)
            {
                return ServiceResult.Fail("Admin bulunamadı", HttpStatusCode.NotFound);
            }

            if (!_hashingService.VerifyPasswordHash(passwordUpdateDto.CurrentPassword, admin.PasswordHash, admin.PasswordSalt))
            {
                return ServiceResult.Fail("Geçerli parola hatalı", HttpStatusCode.BadRequest);
            }

            _hashingService.CreatePasswordHash(passwordUpdateDto.NewPassword, out var newPasswordHash, out var newPasswordSalt);
            admin.PasswordHash = newPasswordHash;
            admin.PasswordSalt = newPasswordSalt;

            _adminRepository.Update(admin);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult<string>> LoginAsync(AdminLoginDto adminLoginDto)
        {
            var admin = await _adminRepository.GetByEmailAsync(adminLoginDto.Email);
            if (admin == null)
            {
                return ServiceResult<string>.Fail("Geçersiz email veya şifre.", HttpStatusCode.Unauthorized);
            }

            var isPasswordValid = _hashingService.VerifyPasswordHash(adminLoginDto.Password, admin.PasswordHash, admin.PasswordSalt);

            if (!isPasswordValid)
            {
                return ServiceResult<string>.Fail("Geçersiz email veya şifre.", HttpStatusCode.Unauthorized);
            }

            var token = _tokenService.CreateToken(admin);

            return ServiceResult<string>.Success(token, HttpStatusCode.OK);
        }

        public async Task<ServiceResult<IEnumerable<AdminDto>>> GetAdminsPagedAsync(int pageNumber, int pageSize)
        {
            var (pagedAdmins, totalItems) = await _adminRepository.GetAllAdminPagedAsync(pageNumber, pageSize);
            if (pagedAdmins == null || !pagedAdmins.Any())
            {
                return ServiceResult<IEnumerable<AdminDto>>.Fail("Henüz kayıtlı bir admin bulunamadı.");
            }

            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var adminDtos = _mapper.Map<IEnumerable<AdminDto>>(pagedAdmins);

            return ServiceResult<IEnumerable<AdminDto>>.SuccessWithPagination(adminDtos, totalItems, totalPages);
        }
    }
}

using AutoMapper;
using Bio.Application.Dtos.Admin;
using Bio.Application.Dtos.Company;
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
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IJobPostRepository _jobPostRepository;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IHashingService _hashingService;
        private readonly IUnitOfWork _unitOfWork;

        public CompanyService(ICompanyRepository companyRepository, IMapper mapper, ITokenService tokenService, IHashingService hashingService, IJobPostRepository jobPostRepository , IUnitOfWork unitOfWork)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
            _tokenService = tokenService;
            _hashingService = hashingService;
            _jobPostRepository = jobPostRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> ApproveCompanyAsync(Guid id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null)
            {
                return ServiceResult.Fail("Firma bulunamadı", HttpStatusCode.NotFound);
            }

            company.ApprovalStatus = AccountApprovalStatus.Approved;
            _companyRepository.Update(company);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult<CompanyDto>> CreateCompanyAsync(CompanyCreateDto companyCreateDto)
        {
            var existingCompanyEmail = await _companyRepository.GetByEmailAsync(companyCreateDto.Email);
            if (existingCompanyEmail != null)
            {
                return ServiceResult<CompanyDto>.Fail("Bu email adresi zaten kullanılıyor.");
            }

            var existingCompanyTc = await _companyRepository.FindAsync(c => c.TcKimlik == companyCreateDto.TcKimlik);
            if (existingCompanyTc != null) 
            {
                return ServiceResult<CompanyDto>.Fail("Bu TC kimlik numarası zaten kullanılıyor.");
            }

            var existingCompanyVkn = await _companyRepository.FindAsync(c => c.Vkn == companyCreateDto.Vkn);
            if (existingCompanyVkn != null)
            {
                return ServiceResult<CompanyDto>.Fail("Bu Vergi kimlik numarası zaten kullanılıyor.");
            }

            var existingCompanyPhone = await _companyRepository.FindAsync(c => c.Phone == companyCreateDto.Phone);
            if (existingCompanyPhone != null) 
            {
                return ServiceResult<CompanyDto>.Fail("Bu telefon numarası zaten kullanılıyor.");
            }

            var existingCompanyName = await _companyRepository.FindAsync(c => c.CompanyName == companyCreateDto.CompanyName);
            if (existingCompanyName != null) 
            {
                return ServiceResult<CompanyDto>.Fail("Bu şirket adı zaten kullanılıyor.");
            }



            _hashingService.CreatePasswordHash(companyCreateDto.Password, out var passwordHash, out var passwordSalt);

            var company = _mapper.Map<Company>(companyCreateDto);
            company.PasswordHash = passwordHash;
            company.PasswordSalt = passwordSalt;
            company.ApprovalStatus = AccountApprovalStatus.Pending;
            company.Role = UserRole.Company;
            company.CreatedAt = DateTime.Now;

            await _companyRepository.AddAsync(company);

            var companyDto = _mapper.Map<CompanyDto>(company);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<CompanyDto>.SuccessAsCreated(companyDto);
        }

        public async Task<ServiceResult> DeleteCompanyAsync(Guid id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null)
            {
                return ServiceResult.Fail("Firma bulunamadı", HttpStatusCode.NotFound);
            }

            var jobPosts = await _jobPostRepository.GetByCompanyIdAsync(id);
            foreach (var jobPost in jobPosts)
            {
                _jobPostRepository.Delete(jobPost);
            }

            _companyRepository.Delete(company);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }


        public async Task<ServiceResult<IEnumerable<CompanyDto>>> GetAllCompaniesAsync()
        {
            var companies = await _companyRepository.GetAllAsync();
            if (companies == null || !companies.Any())
            {
                return ServiceResult<IEnumerable<CompanyDto>>.Fail("Henüz kayıtlı bir firma bulunamadı.");
            }

            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            return ServiceResult<IEnumerable<CompanyDto>>.Success(companiesDto);
        }

        public async Task<ServiceResult<IEnumerable<CompanyDto>>> GetCompaniesByApprovalStatusAsync(AccountApprovalStatus approvalStatus)
        {
            var companies = await _companyRepository.FindAllAsync(c => c.ApprovalStatus == approvalStatus);

            if (companies == null || !companies.Any())
            {
                return ServiceResult<IEnumerable<CompanyDto>>.Fail("Belirtilen onay durumuna sahip firma bulunamadı.", HttpStatusCode.NotFound);
            }

            var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            return ServiceResult<IEnumerable<CompanyDto>>.Success(companyDtos);
        }

        public async Task<ServiceResult<CompanyDto>> GetCompanyByIdAsync(Guid id)
        {
            var company = await _companyRepository.GetCompanyWithJobPosts(id);
            if (company == null)
            {
                return ServiceResult<CompanyDto>.Fail("Firma bulunamadı", HttpStatusCode.NotFound);
            }

            var companyDto = _mapper.Map<CompanyDto>(company);

            return ServiceResult<CompanyDto>.Success(companyDto);
        }

        public async Task<ServiceResult<IEnumerable<PublicCompanyDto>>> GetPublicCompaniesAsync()
        {
            var companies = await _companyRepository.GetAllAsync();
            if (companies == null || !companies.Any())
            {
                return ServiceResult<IEnumerable<PublicCompanyDto>>.Fail("Henüz kayıtlı bir firma bulunamadı.");
            }

            var publicCompanyDtos = _mapper.Map<IEnumerable<PublicCompanyDto>>(companies);

            return ServiceResult<IEnumerable<PublicCompanyDto>>.Success(publicCompanyDtos);
        }

        public async Task<ServiceResult<string>> LoginAsync(CompanyLoginDto companyLoginDto)
        {
            var company = await _companyRepository.FindAsync(c => c.Email == companyLoginDto.Email);
            if (company == null)
            {
                return ServiceResult<string>.Fail("Geçersiz email veya şifre.", HttpStatusCode.Unauthorized);
            }

            var isPasswordValid = _hashingService.VerifyPasswordHash(companyLoginDto.Password, company.PasswordHash, company.PasswordSalt);
            if (!isPasswordValid)
            {
                return ServiceResult<string>.Fail("Geçersiz email veya şifre.", HttpStatusCode.Unauthorized);
            }

            var token = _tokenService.CreateToken(company);

            return ServiceResult<string>.Success(token, HttpStatusCode.OK);
        }

        public async Task<ServiceResult> RejectCompanyAsync(Guid id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null)
            {
                return ServiceResult.Fail("Firma bulunamadı", HttpStatusCode.NotFound);
            }

            company.ApprovalStatus = AccountApprovalStatus.Rejected;
            _companyRepository.Update(company);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> UpdateCompanyAsync(Guid id, CompanyUpdateDto companyUpdateDto)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null)
            {
                return ServiceResult.Fail("Firma bulunamadı", HttpStatusCode.NotFound);
            }

            _mapper.Map(companyUpdateDto, company);
            company.UpdatedAt = DateTime.Now;

            _companyRepository.Update(company);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> UpdateCompanyPasswordAsync(Guid id, CompanyPasswordUpdateDto passwordUpdateDto)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null)
            {
                return ServiceResult.Fail("Şirket bulunamadı", HttpStatusCode.NotFound);
            }

            if (!_hashingService.VerifyPasswordHash(passwordUpdateDto.CurrentPassword, company.PasswordHash, company.PasswordSalt))
            {
                return ServiceResult.Fail("Geçerli parola hatalı", HttpStatusCode.BadRequest);
            }

            _hashingService.CreatePasswordHash(passwordUpdateDto.NewPassword, out var newPasswordHash, out var newPasswordSalt);
            company.PasswordHash = newPasswordHash;
            company.PasswordSalt = newPasswordSalt;

            _companyRepository.Update(company);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult<IEnumerable<CompanyDto>>> GetCompaniesPagedAsync(int pageNumber, int pageSize)
        {
            var (pagedCompanies, totalItems) = await _companyRepository.GetAllCompanyPagedAsync(pageNumber, pageSize);

            if (pagedCompanies == null || !pagedCompanies.Any())
            {
                return ServiceResult<IEnumerable<CompanyDto>>.Fail("Henüz kayıtlı bir şirket bulunamadı.");
            }

            var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(pagedCompanies);

            return ServiceResult<IEnumerable<CompanyDto>>.Success(companyDtos);
        }


    }
}

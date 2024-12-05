using AutoMapper;
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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICertificateRepository _certificateRepository;
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IResumeRepository _resumeRepository; 
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IHashingService _hashingService;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IMapper mapper, ITokenService tokenService, IHashingService hashingService, ICertificateRepository certificateRepository, IJobApplicationRepository jobApplicationRepository, IResumeRepository resumeRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenService = tokenService;
            _hashingService = hashingService;
            _certificateRepository = certificateRepository;
            _jobApplicationRepository = jobApplicationRepository;
            _resumeRepository = resumeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<UserDto>> CreateUserAsync(UserCreateDto userCreateDto)
        {
            var existingUserEmail = await _userRepository.GetEmailAsync(userCreateDto.Email);
            if (existingUserEmail != null)
            {
                return ServiceResult<UserDto>.Fail("Bu email adresi zaten kullanılıyor.");
            }

            var existingUserPhone = await _userRepository.FindAsync(u => u.Phone == userCreateDto.Phone);
            if (existingUserPhone != null)
            {
                return ServiceResult<UserDto>.Fail("Bu telefon numarası zaten kullanılıyor");
            }

            var existingUserTc = await _userRepository.FindAsync(u => u.TcKimlik == userCreateDto.TcKimlik);
            if (existingUserTc != null) 
            {
                return ServiceResult<UserDto>.Fail("Bu TC kimlik numarası zaten kullanılıyor");
            }

            _hashingService.CreatePasswordHash(userCreateDto.Password, out var passwordHash, out var passwordSalt);

            var user = _mapper.Map<User>(userCreateDto);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.ApprovalStatus = AccountApprovalStatus.Pending;
            user.Role = UserRole.JobSeeker;
            user.CreatedAt = DateTime.Now;

            await _userRepository.AddAsync(user);

            var userDto = _mapper.Map<UserDto>(user);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<UserDto>.SuccessAsCreated(userDto);
        }

        public async Task<ServiceResult> DeleteUserAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ServiceResult.Fail("Kullanıcı bulunamadı", HttpStatusCode.NotFound);
            }

            var jobApplications = await _jobApplicationRepository.GetByUserIdAsync(userId);
            foreach (var application in jobApplications)
            {
                _jobApplicationRepository.Delete(application);
            }

            var certificates = await _certificateRepository.GetByUserIdAsync(userId);
            foreach (var certificate in certificates)
            {
                _certificateRepository.Delete(certificate);
            }

            var resumes = await _resumeRepository.GetByUserIdAsync(userId);
            foreach (var resume in resumes)
            {
                _resumeRepository.Delete(resume);
            }

            _userRepository.Delete(user);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }


        public async Task<ServiceResult<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();

            if(users == null || !users.Any())
            {
                return ServiceResult<IEnumerable<UserDto>>.Fail("Henüz bir kullanıcı kaydı bulunamadı.");
            }

            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);

            return ServiceResult<IEnumerable<UserDto>>.Success(userDtos);
        }

        public async Task<ServiceResult<UserDto>> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetUserWithResumesCertificatesAndJobApplication(userId);
            if (user == null)
            {
                return ServiceResult<UserDto>.Fail("Kullanıcı bulunamadı.", HttpStatusCode.NotFound);
            }

            var userDto = _mapper.Map<UserDto>(user);

            return ServiceResult<UserDto>.Success(userDto);
        }

        public async Task<ServiceResult<IEnumerable<UserDto>>> GetUsersByApprovalStatusAsync(AccountApprovalStatus status)
        {
            var users = await _userRepository.FindAllAsync(u => u.ApprovalStatus == status);
            if (users == null || !users.Any())
            {
                return ServiceResult<IEnumerable<UserDto>>.Fail("Belirtilen onay durumunda kullanıcı bulunamadı.");
            }

            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);

            return ServiceResult<IEnumerable<UserDto>>.Success(userDtos);
        }

        public async Task<ServiceResult<IEnumerable<UserDto>>> GetUsersPagedAsync(int pageNumber, int pageSize)
        {
            var (pagedUsers, totalItems) = await _userRepository.GetAllUserPagedAsync(pageNumber, pageSize);

            if (pagedUsers == null || !pagedUsers.Any())
            {
                return ServiceResult<IEnumerable<UserDto>>.Fail("Henüz kayıtlı bir kullanıcı bulunamadı.");
            }

            var userDtos = _mapper.Map<IEnumerable<UserDto>>(pagedUsers);

            return ServiceResult<IEnumerable<UserDto>>.Success(userDtos);
        }


        public async Task<ServiceResult> UpdateUserAsync(Guid id, UserUpdateDto userUpdateDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return ServiceResult.Fail("Kullanıcı bulunamadı.", HttpStatusCode.NotFound);
            }

            _mapper.Map(userUpdateDto, user);
            user.UpdatedAt = DateTime.Now;

            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> UpdateUserPasswordAsync(Guid id, UserPasswordUpdateDto passwordUpdateDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return ServiceResult.Fail("Kullanıcı bulunamadı", HttpStatusCode.NotFound);
            }

            if (!_hashingService.VerifyPasswordHash(passwordUpdateDto.CurrentPassword, user.PasswordHash, user.PasswordSalt))
            {
                return ServiceResult.Fail("Geçerli parola hatalı", HttpStatusCode.BadRequest);
            }

            _hashingService.CreatePasswordHash(passwordUpdateDto.NewPassword, out var newPasswordHash, out var newPasswordSalt);
            user.PasswordHash = newPasswordHash;
            user.PasswordSalt = newPasswordSalt;

            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult<string>> LoginAsync(UserLoginDto userLoginDto)
        {
            var user = await _userRepository.FindAsync(u => u.Email == userLoginDto.Email);
            if (user == null)
            {
                return ServiceResult<string>.Fail("Geçersiz email veya şifre.", HttpStatusCode.Unauthorized);
            }

            var isPasswordValid = _hashingService.VerifyPasswordHash(userLoginDto.Password, user.PasswordHash, user.PasswordSalt);
            if (!isPasswordValid)
            {
                return ServiceResult<string>.Fail("Geçersiz email veya şifre.", HttpStatusCode.Unauthorized);
            }

            var token = _tokenService.CreateToken(user);

            return ServiceResult<string>.Success(token, HttpStatusCode.OK);
        }

        public async Task<ServiceResult> ApproveUserAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return ServiceResult.Fail("Kullanıcı bulunamadı", HttpStatusCode.NotFound);
            }

            user.ApprovalStatus = AccountApprovalStatus.Approved;
            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> RejectUserAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return ServiceResult.Fail("Kullanıcı bulunamadı", HttpStatusCode.NotFound);
            }

            user.ApprovalStatus = AccountApprovalStatus.Rejected;
            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

    }
}

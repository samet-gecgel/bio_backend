using Bio.Domain.Entities;
using Bio.Domain.Enums;
using Bio.Domain.Interfaces;
using Bio.Domain.Repositories;
using Bio.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bio.Infrastructure.Data
{
    public class DatabaseInitializer
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IHashingService _hashingService;
        private readonly IUnitOfWork _unitOfWork;

        public DatabaseInitializer(IAdminRepository adminRepository, IHashingService hashingService, IUnitOfWork unitOfWork)
        {
            _adminRepository = adminRepository;
            _hashingService = hashingService;
            _unitOfWork = unitOfWork;
        }

        public async Task SeedSuperAdminAsync()
        {
            var existingSuperAdmin = await _adminRepository.FindAsync(a => a.Role == UserRole.SuperAdmin);

            if (existingSuperAdmin != null)
            {
                return;
            }

            var superAdmin = new Admin
            {
                FullName = "Super Admin",
                Email = "superadmin@domain.com",
                Role = UserRole.SuperAdmin,
                Department = "IT",
                CreatedAt = DateTime.Now
            };

            _hashingService.CreatePasswordHash("SuperAdmin123!", out var passwordHash, out var passwordSalt);
            superAdmin.PasswordHash = passwordHash;
            superAdmin.PasswordSalt = passwordSalt;

            await _adminRepository.AddAsync(superAdmin);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
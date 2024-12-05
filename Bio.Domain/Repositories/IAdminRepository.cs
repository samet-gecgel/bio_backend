using Bio.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bio.Domain.Repositories
{
    public interface IAdminRepository : IBaseRepository<Admin>
    {
        Task<Admin> GetByEmailAsync(string email);
        Task<(IEnumerable<Admin>, int)> GetAllAdminPagedAsync(int pageNumber, int pageSize);
    }
}

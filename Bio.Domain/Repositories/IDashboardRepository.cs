using Bio.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bio.Domain.Repositories
{
    public interface IDashboardRepository
    {
        // Kullanıcılar
        Task<int> GetTotalUsersAsync();
        Task<int> GetApprovedUsersAsync();
        Task<int> GetNewUsersLast30DaysAsync();

        // Şirketler
        Task<int> GetTotalCompaniesAsync();
        Task<int> GetApprovedCompaniesAsync();
        Task<int> GetNewCompaniesLast30DaysAsync();

        // Başvurular
        Task<int> GetTotalApplicationsAsync();
        Task<List<JobApplication>> GetRecentApplicationsAsync();
        Task<List<IGrouping<string, JobApplication>>> GetTopCompaniesLast30DaysAsync();
        Task<List<KeyValuePair<string, int>>> GetJobApplicationsLast7DaysAsync();

        // İş İlanları
        Task<int> GetTotalJobPostsAsync();
        Task<int> GetActiveJobPostsAsync();
        Task<int> GetNewJobPostsLast30DaysAsync();
    }
}

using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces
{
    public interface ICollegeRepository
    {
        Task<List<College>> GetAllAsync();

        Task<College?> GetByIdAsync(int id);

        Task<College?> GetByCodeAsync(string collegeCode);

        Task<List<College>> GetPendingAsync();

        Task AddAsync(College college);

        Task SaveChangesAsync();
    }
}
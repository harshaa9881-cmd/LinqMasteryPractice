using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces
{
    public interface IPlacementOfficerRepository
    {
        Task<List<PlacementOfficer>> GetAllAsync();

        Task<List<PlacementOfficer>> GetPendingAsync();

        Task<PlacementOfficer?> GetByIdAsync(int id);

        Task<PlacementOfficer?> GetByUserIdAsync(string userId);

        Task AddAsync(PlacementOfficer officer);

        Task SaveChangesAsync();
    }
}
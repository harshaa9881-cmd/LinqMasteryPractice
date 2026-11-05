using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetByEmailAsync(string email);

        Task<ApplicationUser?> GetByIdAsync(string userId);
    }
}
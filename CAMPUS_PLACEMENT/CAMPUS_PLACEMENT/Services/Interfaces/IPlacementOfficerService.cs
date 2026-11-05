
using CampusPlacementManagement.DTOs.PlacementOfficer;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface IPlacementOfficerService
    {
        Task<(bool Success, string Message, PlacementOfficerResponseDto? Data)>
            RegisterAsync(PlacementOfficerRegisterDto dto);

        Task<List<PlacementOfficerResponseDto>> GetAllAsync();

        Task<List<PlacementOfficerResponseDto>> GetPendingAsync();

        Task<PlacementOfficerResponseDto?> GetByIdAsync(
            int id, string userId, bool isSuperAdmin);

        Task<(bool Success, string Message)> ApproveAsync(
            int id, string approvedBy);

        Task<(bool Success, string Message)> RejectAsync(int id);

        Task<(bool Success, string Message)> SuspendAsync(int id);

        Task<PlacementOfficerResponseDto?> GetMeAsync(string userId);
    }
}
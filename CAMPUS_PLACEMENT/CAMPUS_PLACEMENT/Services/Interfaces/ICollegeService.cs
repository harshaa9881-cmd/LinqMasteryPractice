using CampusPlacementManagement.DTOs.College;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface ICollegeService
    {
        Task<(bool Success, string Message, CollegeResponseDto? Data)>
            RegisterAsync(CollegeDto dto);

        Task<List<CollegeResponseDto>> GetAllAsync();

        Task<List<CollegeResponseDto>> GetPendingAsync();

        Task<CollegeResponseDto?> GetByIdAsync(
            int id, string userId, bool isSuperAdmin);

        Task<(bool Success, string Message)> ApproveAsync(int id);

        Task<(bool Success, string Message)> RejectAsync(int id);

        Task<(bool Success, string Message)> SuspendAsync(int id);

        Task<(bool Success, string Message)> UpdateAsync(
            int id, CollegeDto dto);
    }
}
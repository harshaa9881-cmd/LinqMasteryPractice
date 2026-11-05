
using CampusPlacementManagement.DTOs.Company;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<(bool Success, string Message, CompanyResponseDto? Data)>
            RegisterAsync(CompanyDto dto);

        Task<List<CompanyResponseDto>> GetAllAsync();

        Task<List<CompanyResponseDto>> GetPendingAsync();

        Task<CompanyResponseDto?> GetByIdAsync(
            int id, string userId, bool isSuperAdmin);

        Task<(bool Success, string Message)> ApproveAsync(int id);

        Task<(bool Success, string Message)> RejectAsync(int id);

        Task<(bool Success, string Message)> SuspendAsync(int id);

        Task<(bool Success, string Message)> UpdateAsync(
            int id, string userId, bool isSuperAdmin, CompanyDto dto);
    }
}
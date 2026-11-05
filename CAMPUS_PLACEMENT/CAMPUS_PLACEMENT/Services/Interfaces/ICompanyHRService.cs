using CampusPlacementManagement.DTOs.CompanyHR;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface ICompanyHRService
    {
        Task<(bool Success, string Message, CompanyHRResponseDto? Data)>
            RegisterAsync(CompanyHRRegisterDto dto);

        Task<List<CompanyHRResponseDto>> GetAllAsync();

        Task<List<CompanyHRResponseDto>> GetPendingAsync();

        Task<(bool Success, string Message)> ApproveAsync(
            int id, string approvedBy);

        Task<(bool Success, string Message)> RejectAsync(int id);

        Task<CompanyHRResponseDto?> GetMeAsync(string userId);
    }
}
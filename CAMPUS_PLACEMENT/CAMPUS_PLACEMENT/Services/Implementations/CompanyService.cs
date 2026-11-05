
using CampusPlacementManagement.DTOs.Company;
using CampusPlacementManagement.Enums;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services.Implementations
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ICompanyHRRepository _companyHRRepository;

        public CompanyService(
            ICompanyRepository companyRepository,
            ICompanyHRRepository companyHRRepository)
        {
            _companyRepository = companyRepository;
            _companyHRRepository = companyHRRepository;
        }

        public async Task<(bool Success, string Message, CompanyResponseDto? Data)>
            RegisterAsync(CompanyDto dto)
        {
            var company = new Company
            {
                CompanyName = dto.CompanyName.Trim(),
                CompanyEmail = dto.CompanyEmail.Trim(),
                Website = dto.Website,
                Industry = dto.Industry.Trim(),
                Address = dto.Address.Trim(),
                Description = dto.Description,
                Status = RecordStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _companyRepository.AddAsync(company);
            await _companyRepository.SaveChangesAsync();

            return (
                true,
                "Company registration submitted successfully.",
                MapCompany(company)
            );
        }

        public async Task<List<CompanyResponseDto>> GetAllAsync()
        {
            var companies = await _companyRepository.GetAllAsync();

            return companies.Select(MapCompany).ToList();
        }

        public async Task<List<CompanyResponseDto>> GetPendingAsync()
        {
            var companies = await _companyRepository.GetPendingAsync();

            return companies.Select(MapCompany).ToList();
        }

        public async Task<CompanyResponseDto?> GetByIdAsync(
            int id, string userId, bool isSuperAdmin)
        {
            var company = await _companyRepository.GetByIdAsync(id);

            if (company == null)
            {
                return null;
            }

            if (!isSuperAdmin)
            {
                var companyHR = await _companyHRRepository
                    .GetByUserIdAsync(userId);

                if (companyHR == null || companyHR.CompanyId != id)
                {
                    return null;
                }
            }

            return MapCompany(company);
        }

        public async Task<(bool Success, string Message)> ApproveAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);

            if (company == null)
            {
                return (false, "Company not found.");
            }

            company.Status = RecordStatus.Approved;

            await _companyRepository.SaveChangesAsync();

            return (true, "Company approved successfully.");
        }

        public async Task<(bool Success, string Message)> RejectAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);

            if (company == null)
            {
                return (false, "Company not found.");
            }

            company.Status = RecordStatus.Rejected;

            await _companyRepository.SaveChangesAsync();

            return (true, "Company rejected successfully.");
        }

        public async Task<(bool Success, string Message)> SuspendAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);

            if (company == null)
            {
                return (false, "Company not found.");
            }

            company.Status = RecordStatus.Suspended;

            await _companyRepository.SaveChangesAsync();

            return (true, "Company suspended successfully.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(
            int id, string userId, bool isSuperAdmin, CompanyDto dto)
        {
            var company = await _companyRepository.GetByIdAsync(id);

            if (company == null)
            {
                return (false, "Company not found.");
            }

            if (!isSuperAdmin)
            {
                var companyHR = await _companyHRRepository
                    .GetByUserIdAsync(userId);

                if (companyHR == null || companyHR.CompanyId != id)
                {
                    return (false, "You cannot update this company.");
                }
            }

            company.CompanyName = dto.CompanyName.Trim();
            company.CompanyEmail = dto.CompanyEmail.Trim();
            company.Website = dto.Website;
            company.Industry = dto.Industry.Trim();
            company.Address = dto.Address.Trim();
            company.Description = dto.Description;

            await _companyRepository.SaveChangesAsync();

            return (true, "Company updated successfully.");
        }

        private static CompanyResponseDto MapCompany(Company company)
        {
            return new CompanyResponseDto
            {
                CompanyId = company.CompanyId,
                CompanyName = company.CompanyName,
                CompanyEmail = company.CompanyEmail,
                Website = company.Website,
                Industry = company.Industry,
                Address = company.Address,
                Description = company.Description,
                Status = company.Status,
                CreatedAt = company.CreatedAt
            };
        }
    }
}
using Microsoft.AspNetCore.Identity;
using CampusPlacementManagement.Constants;
using CampusPlacementManagement.DTOs.CompanyHR;
using CampusPlacementManagement.Enums;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services.Implementations
{
    public class CompanyHRService : ICompanyHRService
    {
        private readonly ICompanyHRRepository _companyHRRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public CompanyHRService(
            ICompanyHRRepository companyHRRepository,
            ICompanyRepository companyRepository,
            UserManager<ApplicationUser> userManager)
        {
            _companyHRRepository = companyHRRepository;
            _companyRepository = companyRepository;
            _userManager = userManager;
        }

        public async Task<(bool Success, string Message, CompanyHRResponseDto? Data)>
            RegisterAsync(CompanyHRRegisterDto dto)
        {
            var company = await _companyRepository.GetByIdAsync(dto.CompanyId);

            if (company == null ||
                company.Status != RecordStatus.Approved)
            {
                return (false, "Selected company is not approved or active.", null);
            }

            var existingUser = await _userManager.FindByEmailAsync(dto.Email);

            if (existingUser != null)
            {
                return (false, "Email is already registered.", null);
            }

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                AccountStatus = AccountStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var userResult = await _userManager.CreateAsync(
                user, dto.Password);

            if (!userResult.Succeeded)
            {
                return (
                    false,
                    string.Join(" ", userResult.Errors.Select(e => e.Description)),
                    null
                );
            }

            var companyHR = new CampusPlacementManagement.Models.CompanyHR
            {
                UserId = user.Id,
                CompanyId = dto.CompanyId,
                Designation = dto.Designation
            };

            await _companyHRRepository.AddAsync(companyHR);
            await _companyHRRepository.SaveChangesAsync();

            companyHR.User = user;
            companyHR.Company = company;

            return (
                true,
                "Company HR registration submitted for approval.",
                MapCompanyHR(companyHR)
            );
        }

        public async Task<List<CompanyHRResponseDto>> GetAllAsync()
        {
            var companyHRs = await _companyHRRepository.GetAllAsync();

            return companyHRs.Select(MapCompanyHR).ToList();
        }

        public async Task<List<CompanyHRResponseDto>> GetPendingAsync()
        {
            var companyHRs = await _companyHRRepository.GetPendingAsync();

            return companyHRs.Select(MapCompanyHR).ToList();
        }

        public async Task<(bool Success, string Message)> ApproveAsync(
            int id, string approvedBy)
        {
            var companyHR = await _companyHRRepository.GetByIdAsync(id);

            if (companyHR == null)
            {
                return (false, "Company HR not found.");
            }

            if (companyHR.Company.Status != RecordStatus.Approved)
            {
                return (false, "The associated company is not approved.");
            }

            companyHR.ApprovedBy = approvedBy;
            companyHR.ApprovedAt = DateTime.UtcNow;

            companyHR.User.AccountStatus = AccountStatus.Approved;
            companyHR.User.UpdatedAt = DateTime.UtcNow;

            var userResult = await _userManager.UpdateAsync(companyHR.User);

            if (!userResult.Succeeded)
            {
                return (
                    false,
                    string.Join(" ", userResult.Errors.Select(e => e.Description))
                );
            }

            if (!await _userManager.IsInRoleAsync(
                companyHR.User, UserRoles.CompanyHR))
            {
                var roleResult = await _userManager.AddToRoleAsync(
                    companyHR.User, UserRoles.CompanyHR);

                if (!roleResult.Succeeded)
                {
                    return (
                        false,
                        string.Join(" ", roleResult.Errors.Select(e => e.Description))
                    );
                }
            }

            await _companyHRRepository.SaveChangesAsync();

            return (true, "Company HR approved successfully.");
        }

        public async Task<(bool Success, string Message)> RejectAsync(int id)
        {
            var companyHR = await _companyHRRepository.GetByIdAsync(id);

            if (companyHR == null)
            {
                return (false, "Company HR not found.");
            }

            companyHR.User.AccountStatus = AccountStatus.Rejected;
            companyHR.User.UpdatedAt = DateTime.UtcNow;

            await _userManager.UpdateAsync(companyHR.User);
            await _companyHRRepository.SaveChangesAsync();

            return (true, "Company HR rejected successfully.");
        }

        public async Task<CompanyHRResponseDto?> GetMeAsync(string userId)
        {
            var companyHR = await _companyHRRepository
                .GetByUserIdAsync(userId);

            return companyHR == null ? null : MapCompanyHR(companyHR);
        }

        private static CompanyHRResponseDto MapCompanyHR(
             CampusPlacementManagement.Models.CompanyHR companyHR)
        {
            return new CompanyHRResponseDto
            {
                CompanyHRId = companyHR.CompanyHRId,
                UserId = companyHR.UserId,
                FullName = companyHR.User.FullName,
                Email = companyHR.User.Email!,
                PhoneNumber = companyHR.User.PhoneNumber!,
                CompanyId = companyHR.CompanyId,
                CompanyName = companyHR.Company.CompanyName,
                Designation = companyHR.Designation,
                ApprovedBy = companyHR.ApprovedBy,
                ApprovedAt = companyHR.ApprovedAt,
                Status = companyHR.User.AccountStatus
            };
        }
    }
}
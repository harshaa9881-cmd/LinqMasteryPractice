
using CampusPlacementManagement.Constants;
using CampusPlacementManagement.DTOs.PlacementOfficer;
using CampusPlacementManagement.Enums;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CampusPlacementManagement.Services.Implementations
{
    public class PlacementOfficerService : IPlacementOfficerService
    {
        private readonly IPlacementOfficerRepository _officerRepository;
        private readonly ICollegeRepository _collegeRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public PlacementOfficerService(
            IPlacementOfficerRepository officerRepository,
            ICollegeRepository collegeRepository,
            UserManager<ApplicationUser> userManager)
        {
            _officerRepository = officerRepository;
            _collegeRepository = collegeRepository;
            _userManager = userManager;
        }

        public async Task<(bool Success, string Message, PlacementOfficerResponseDto? Data)>
            RegisterAsync(PlacementOfficerRegisterDto dto)
        {
            var college = await _collegeRepository.GetByIdAsync(dto.CollegeId);

            if (college == null ||
                college.Status != RecordStatus.Approved)
            {
                return (false, "Selected college is not approved or active.", null);
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

            var officer = new PlacementOfficer
            {
                UserId = user.Id,
                CollegeId = dto.CollegeId,
                Designation = dto.Designation,
                Status = RecordStatus.Pending
            };

            await _officerRepository.AddAsync(officer);
            await _officerRepository.SaveChangesAsync();

            officer.User = user;
            officer.College = college;

            return (
                true,
                "Placement officer registration submitted for approval.",
                MapOfficer(officer)
            );
        }

        public async Task<List<PlacementOfficerResponseDto>> GetAllAsync()
        {
            var officers = await _officerRepository.GetAllAsync();

            return officers.Select(MapOfficer).ToList();
        }

        public async Task<List<PlacementOfficerResponseDto>> GetPendingAsync()
        {
            var officers = await _officerRepository.GetPendingAsync();

            return officers.Select(MapOfficer).ToList();
        }

        public async Task<PlacementOfficerResponseDto?> GetByIdAsync(
            int id, string userId, bool isSuperAdmin)
        {
            var officer = await _officerRepository.GetByIdAsync(id);

            if (officer == null)
            {
                return null;
            }

            if (!isSuperAdmin && officer.UserId != userId)
            {
                return null;
            }

            return MapOfficer(officer);
        }

        public async Task<(bool Success, string Message)> ApproveAsync(
            int id, string approvedBy)
        {
            var officer = await _officerRepository.GetByIdAsync(id);

            if (officer == null)
            {
                return (false, "Placement officer not found.");
            }

            if (officer.College.Status != RecordStatus.Approved)
            {
                return (false, "The associated college is not approved.");
            }

            officer.Status = RecordStatus.Approved;
            officer.ApprovedBy = approvedBy;
            officer.ApprovedAt = DateTime.UtcNow;

            officer.User.AccountStatus = AccountStatus.Approved;
            officer.User.UpdatedAt = DateTime.UtcNow;

            var userResult = await _userManager.UpdateAsync(officer.User);

            if (!userResult.Succeeded)
            {
                return (
                    false,
                    string.Join(" ", userResult.Errors.Select(e => e.Description))
                );
            }

            if (!await _userManager.IsInRoleAsync(
                officer.User, UserRoles.PlacementOfficer))
            {
                var roleResult = await _userManager.AddToRoleAsync(
                    officer.User, UserRoles.PlacementOfficer);

                if (!roleResult.Succeeded)
                {
                    return (
                        false,
                        string.Join(" ", roleResult.Errors.Select(e => e.Description))
                    );
                }
            }

            await _officerRepository.SaveChangesAsync();

            return (true, "Placement officer approved successfully.");
        }

        public async Task<(bool Success, string Message)> RejectAsync(int id)
        {
            var officer = await _officerRepository.GetByIdAsync(id);

            if (officer == null)
            {
                return (false, "Placement officer not found.");
            }

            officer.Status = RecordStatus.Rejected;
            officer.User.AccountStatus = AccountStatus.Rejected;
            officer.User.UpdatedAt = DateTime.UtcNow;

            await _userManager.UpdateAsync(officer.User);
            await _officerRepository.SaveChangesAsync();

            return (true, "Placement officer rejected successfully.");
        }

        public async Task<(bool Success, string Message)> SuspendAsync(int id)
        {
            var officer = await _officerRepository.GetByIdAsync(id);

            if (officer == null)
            {
                return (false, "Placement officer not found.");
            }

            officer.Status = RecordStatus.Suspended;
            officer.User.AccountStatus = AccountStatus.Suspended;
            officer.User.UpdatedAt = DateTime.UtcNow;

            await _userManager.UpdateAsync(officer.User);
            await _officerRepository.SaveChangesAsync();

            return (true, "Placement officer suspended successfully.");
        }

        public async Task<PlacementOfficerResponseDto?> GetMeAsync(string userId)
        {
            var officer = await _officerRepository.GetByUserIdAsync(userId);

            return officer == null ? null : MapOfficer(officer);
        }

        private static PlacementOfficerResponseDto MapOfficer(
            PlacementOfficer officer)
        {
            return new PlacementOfficerResponseDto
            {
                PlacementOfficerId = officer.PlacementOfficerId,
                UserId = officer.UserId,
                FullName = officer.User.FullName,
                Email = officer.User.Email!,
                PhoneNumber = officer.User.PhoneNumber!,
                CollegeId = officer.CollegeId,
                CollegeName = officer.College.CollegeName,
                Designation = officer.Designation,
                Status = officer.Status,
                ApprovedBy = officer.ApprovedBy,
                ApprovedAt = officer.ApprovedAt
            };
        }
    }
}
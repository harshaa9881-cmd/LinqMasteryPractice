using CampusPlacementManagement.DTOs.College;
using CampusPlacementManagement.Enums;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services.Implementations
{
    public class CollegeService : ICollegeService
    {
        private readonly ICollegeRepository _collegeRepository;
        private readonly IPlacementOfficerRepository _officerRepository;

        public CollegeService(
            ICollegeRepository collegeRepository,
            IPlacementOfficerRepository officerRepository)
        {
            _collegeRepository = collegeRepository;
            _officerRepository = officerRepository;
        }

        public async Task<(bool Success, string Message, CollegeResponseDto? Data)>
            RegisterAsync(CollegeDto dto)
        {
            var existingCollege = await _collegeRepository
                .GetByCodeAsync(dto.CollegeCode.Trim());

            if (existingCollege != null)
            {
                return (false, "College code already exists.", null);
            }

            var college = new College
            {
                CollegeName = dto.CollegeName.Trim(),
                CollegeCode = dto.CollegeCode.Trim(),
                UniversityName = dto.UniversityName,
                OfficialEmail = dto.OfficialEmail.Trim(),
                PhoneNumber = dto.PhoneNumber.Trim(),
                Address = dto.Address.Trim(),
                Website = dto.Website,
                Status = RecordStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _collegeRepository.AddAsync(college);
            await _collegeRepository.SaveChangesAsync();

            return (true, "College registration submitted successfully.",
                MapCollege(college));
        }

        public async Task<List<CollegeResponseDto>> GetAllAsync()
        {
            var colleges = await _collegeRepository.GetAllAsync();

            return colleges.Select(MapCollege).ToList();
        }

        public async Task<List<CollegeResponseDto>> GetPendingAsync()
        {
            var colleges = await _collegeRepository.GetPendingAsync();

            return colleges.Select(MapCollege).ToList();
        }

        public async Task<CollegeResponseDto?> GetByIdAsync(
            int id, string userId, bool isSuperAdmin)
        {
            var college = await _collegeRepository.GetByIdAsync(id);

            if (college == null)
            {
                return null;
            }

            if (!isSuperAdmin)
            {
                var officer = await _officerRepository
                    .GetByUserIdAsync(userId);

                if (officer == null || officer.CollegeId != id)
                {
                    return null;
                }
            }

            return MapCollege(college);
        }

        public async Task<(bool Success, string Message)> ApproveAsync(int id)
        {
            var college = await _collegeRepository.GetByIdAsync(id);

            if (college == null)
            {
                return (false, "College not found.");
            }

            college.Status = RecordStatus.Approved;

            await _collegeRepository.SaveChangesAsync();

            return (true, "College approved successfully.");
        }

        public async Task<(bool Success, string Message)> RejectAsync(int id)
        {
            var college = await _collegeRepository.GetByIdAsync(id);

            if (college == null)
            {
                return (false, "College not found.");
            }

            college.Status = RecordStatus.Rejected;

            await _collegeRepository.SaveChangesAsync();

            return (true, "College rejected successfully.");
        }

        public async Task<(bool Success, string Message)> SuspendAsync(int id)
        {
            var college = await _collegeRepository.GetByIdAsync(id);

            if (college == null)
            {
                return (false, "College not found.");
            }

            college.Status = RecordStatus.Suspended;

            await _collegeRepository.SaveChangesAsync();

            return (true, "College suspended successfully.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(
            int id, CollegeDto dto)
        {
            var college = await _collegeRepository.GetByIdAsync(id);

            if (college == null)
            {
                return (false, "College not found.");
            }

            var existingCollege = await _collegeRepository
                .GetByCodeAsync(dto.CollegeCode.Trim());

            if (existingCollege != null &&
                existingCollege.CollegeId != id)
            {
                return (false, "College code already exists.");
            }

            college.CollegeName = dto.CollegeName.Trim();
            college.CollegeCode = dto.CollegeCode.Trim();
            college.UniversityName = dto.UniversityName;
            college.OfficialEmail = dto.OfficialEmail.Trim();
            college.PhoneNumber = dto.PhoneNumber.Trim();
            college.Address = dto.Address.Trim();
            college.Website = dto.Website;

            await _collegeRepository.SaveChangesAsync();

            return (true, "College updated successfully.");
        }

        private static CollegeResponseDto MapCollege(College college)
        {
            return new CollegeResponseDto
            {
                CollegeId = college.CollegeId,
                CollegeName = college.CollegeName,
                CollegeCode = college.CollegeCode,
                UniversityName = college.UniversityName,
                OfficialEmail = college.OfficialEmail,
                PhoneNumber = college.PhoneNumber,
                Address = college.Address,
                Website = college.Website,
                Status = college.Status,
                CreatedAt = college.CreatedAt
            };
        }
    }
}
using Microsoft.EntityFrameworkCore;
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Enums;

namespace CampusPlacementManagement.Repositories.Implementations
{
    public class CollegeRepository : ICollegeRepository
    {
        private readonly ApplicationDbContext _context;

        public CollegeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<College>> GetAllAsync()
        {
            return _context.Colleges.ToListAsync();
        }

        public Task<College?> GetByIdAsync(int id)
        {
            return _context.Colleges
                .FirstOrDefaultAsync(c => c.CollegeId == id);
        }

        public Task<College?> GetByCodeAsync(string collegeCode)
        {
            return _context.Colleges
                .FirstOrDefaultAsync(c =>
                    c.CollegeCode.ToUpper() == collegeCode.ToUpper());
        }

        public Task<List<College>> GetPendingAsync()
        {
            return _context.Colleges
                .Where(c => c.Status == RecordStatus.Pending)
                .ToListAsync();
        }

        public async Task AddAsync(College college)
        {
            await _context.Colleges.AddAsync(college);
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
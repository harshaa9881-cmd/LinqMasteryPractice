using Microsoft.EntityFrameworkCore;
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Enums;

namespace CampusPlacementManagement.Repositories.Implementations
{
    public class PlacementOfficerRepository : IPlacementOfficerRepository
    {
        private readonly ApplicationDbContext _context;

        public PlacementOfficerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<PlacementOfficer>> GetAllAsync()
        {
            return _context.PlacementOfficers
                .Include(p => p.User)
                .Include(p => p.College)
                .ToListAsync();
        }

        public Task<List<PlacementOfficer>> GetPendingAsync()
        {
            return _context.PlacementOfficers
                .Include(p => p.User)
                .Include(p => p.College)
                .Where(p => p.Status == RecordStatus.Pending)
                .ToListAsync();
        }

        public Task<PlacementOfficer?> GetByIdAsync(int id)
        {
            return _context.PlacementOfficers
                .Include(p => p.User)
                .Include(p => p.College)
                .FirstOrDefaultAsync(p => p.PlacementOfficerId == id);
        }

        public Task<PlacementOfficer?> GetByUserIdAsync(string userId)
        {
            return _context.PlacementOfficers
                .Include(p => p.User)
                .Include(p => p.College)
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task AddAsync(PlacementOfficer officer)
        {
            await _context.PlacementOfficers.AddAsync(officer);
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
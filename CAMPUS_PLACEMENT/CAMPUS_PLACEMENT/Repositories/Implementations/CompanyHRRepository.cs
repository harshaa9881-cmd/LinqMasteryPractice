using Microsoft.EntityFrameworkCore;
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Enums;

namespace CampusPlacementManagement.Repositories.Implementations
{
    public class CompanyHRRepository : ICompanyHRRepository
    {
        private readonly ApplicationDbContext _context;

        public CompanyHRRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<CompanyHR>> GetAllAsync()
        {
            return _context.CompanyHRs
                .Include(h => h.User)
                .Include(h => h.Company)
                .ToListAsync();
        }

        public Task<List<CompanyHR>> GetPendingAsync()
        {
            return _context.CompanyHRs
                .Include(h => h.User)
                .Include(h => h.Company)
                .Where(h => h.User.AccountStatus == AccountStatus.Pending)
                .ToListAsync();
        }

        public Task<CompanyHR?> GetByIdAsync(int id)
        {
            return _context.CompanyHRs
                .Include(h => h.User)
                .Include(h => h.Company)
                .FirstOrDefaultAsync(h => h.CompanyHRId == id);
        }

        public Task<CompanyHR?> GetByUserIdAsync(string userId)
        {
            return _context.CompanyHRs
                .Include(h => h.User)
                .Include(h => h.Company)
                .FirstOrDefaultAsync(h => h.UserId == userId);
        }

        public async Task AddAsync(CompanyHR companyHR)
        {
            await _context.CompanyHRs.AddAsync(companyHR);
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
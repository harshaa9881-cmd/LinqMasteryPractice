using Microsoft.EntityFrameworkCore;
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Enums;

namespace CampusPlacementManagement.Repositories.Implementations
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext _context;

        public CompanyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<Company>> GetAllAsync()
        {
            return _context.Companies.ToListAsync();
        }

        public Task<Company?> GetByIdAsync(int id)
        {
            return _context.Companies
                .FirstOrDefaultAsync(c => c.CompanyId == id);
        }

        public Task<List<Company>> GetPendingAsync()
        {
            return _context.Companies
                .Where(c => c.Status == RecordStatus.Pending)
                .ToListAsync();
        }

        public async Task AddAsync(Company company)
        {
            await _context.Companies.AddAsync(company);
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
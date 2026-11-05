using Microsoft.EntityFrameworkCore;
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;

namespace CampusPlacementManagement.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            return _context.Users
                .FirstOrDefaultAsync(u => u.NormalizedEmail == email.ToUpper());
        }

        public Task<ApplicationUser?> GetByIdAsync(string userId)
        {
            return _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
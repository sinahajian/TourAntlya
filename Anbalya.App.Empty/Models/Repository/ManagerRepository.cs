using System;
using Microsoft.EntityFrameworkCore;
using Models.DbContexts;
using Models.Interface;

namespace Models.Repository
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly TourDbContext _context;

        public ManagerRepository(TourDbContext context)
        {
            _context = context;
        }

        public async Task<ManagerDto?> AuthenticateAsync(string userName, string password, CancellationToken ct = default)
        {
            var manager = await _context.Managers
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.UserName == userName, ct);

            if (manager is null)
            {
                return null;
            }

            if (!string.Equals(manager.Password, password, StringComparison.Ordinal))
            {
                return null;
            }

            return ManagerDto.FromEntity(manager);
        }

        public async Task<ManagerDto?> GetByUserNameAsync(string userName, CancellationToken ct = default)
        {
            var manager = await _context.Managers
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.UserName == userName, ct);

            return manager is null ? null : ManagerDto.FromEntity(manager);
        }
    }
}

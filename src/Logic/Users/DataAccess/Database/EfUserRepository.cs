using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Logic.Users.DataAccess.Database.DbContexts;
using Logic.Users.DataAccess.Interfaces;
using Logic.Users.Models;
using Logic.Users.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Logic.Users.DataAccess.Database
{
    public class EfUserRepository : IUserRepository
    {
        private readonly UsersDbContext _context;

        public EfUserRepository(UsersDbContext context)
        {
            _context = context;
        }

        private IQueryable<User> UsersQueryable => _context.Users.AsNoTracking();

        public async Task<User> CreateUserAsync(User user, CancellationToken cancellationToken = default)
        {
            var entry = _context.Add(user);
            await _context.SaveChangesAsync(cancellationToken);
            return entry.Entity;
        }

        public async Task<bool> CheckUniquePhoneAsync(string phone, CancellationToken cancellationToken = default)
        {
            return await UsersQueryable.AllAsync(user => user.Phone != phone, cancellationToken);
        }

        public async Task<bool> CheckUniqueEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await UsersQueryable.AllAsync(user => user.Email != email, cancellationToken);
        }

        public async Task<User?> TryLoginAsync(string phone, string password,
            CancellationToken cancellationToken = default)
        {
            var user = await UsersQueryable.FirstOrDefaultAsync(
                user => user.Phone == phone && user.Password == password, cancellationToken);
            if (user is null)
                return null;
            user = UserMutationHelper.GetUserWithUpdatedLastLogin(DateTime.UtcNow, user);
            var entry = _context.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
            return entry.Entity;
        }

        public async Task<User?> FindUserByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Users.FindAsync(new object[]
            {
                id
            }, cancellationToken);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using PractiseEfCoreWIthSP.DataContext;
using PractiseEfCoreWIthSP.Models.Domains;
using PractiseEfCoreWIthSP.Repositories.IRepositories;

namespace PractiseEfCoreWIthSP.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PractiseDataContext _context;

        public UserRepository(PractiseDataContext context)
        {
            _context = context;
        }

        public async Task<User> AddUser(User user, CancellationToken cancellationToken)
        {
            await _context.Users.AddAsync(user,cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return user;

        }

        public async Task<User> GetUserByEmailAddress(string EmailAdddress, CancellationToken cancellationToken)
        {
            var user = await _context.Users.Where(u => u.EmailAddress.Equals(EmailAdddress) && u.IsDeleted.Equals(false)).FirstOrDefaultAsync(cancellationToken);
            return user;
        }
    }
}

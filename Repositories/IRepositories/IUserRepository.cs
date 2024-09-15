using PractiseEfCoreWIthSP.Models.Domains;

namespace PractiseEfCoreWIthSP.Repositories.IRepositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAddress(string EmailAdddress, CancellationToken cancellationToken);
        Task<User> AddUser(User user, CancellationToken cancellationToken);
    }
}

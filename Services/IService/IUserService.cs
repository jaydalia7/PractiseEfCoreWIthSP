using PractiseEfCoreWIthSP.Models.ViewModels;

namespace PractiseEfCoreWIthSP.Services.IService
{
    public interface IUserService
    {
        Task<WebApiReponseModel> CreateUser(UserAddModel userAddModel, CancellationToken cancellationToken);
        Task<WebApiReponseModel> Login(Loginmodel loginmodel, CancellationToken cancellationToken);
    }
}

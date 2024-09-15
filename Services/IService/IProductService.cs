using PractiseEfCoreWIthSP.Models.ViewModels;

namespace PractiseEfCoreWIthSP.Services.IService
{
    public interface IProductService
    {
        Task<WebApiReponseModel> CreateProduct(AddProductModel addProductModel, CancellationToken cancellationToken);
        Task<WebApiReponseModel> GetAllProduct(CancellationToken cancellationToken);
        Task<WebApiReponseModel> UpdateProduct(int productId, AddProductModel updateProductModel, CancellationToken cancellationToken);
        Task<WebApiReponseModel> RemoveProduct(int productId, CancellationToken cancellationToken);
    }
}

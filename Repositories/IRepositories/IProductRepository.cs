using Microsoft.AspNetCore.Components.Web;
using PractiseEfCoreWIthSP.Models.Domains;

namespace PractiseEfCoreWIthSP.Repositories.IRepositories
{
    public interface IProductRepository
    {
        Task<bool> CheckProductByName(string productName, CancellationToken cancellationToken);
        Task<Product> AddProduct(Product productData, CancellationToken cancellationToken);
        Task<List<Product>> GetAllProducts(CancellationToken cancellationToken);
        Task<Product> GetProductById(int id, CancellationToken cancellationToken);
        Task<Product> UpdateProductData(Product product, CancellationToken cancellationToken);
        Task<bool> RemoveProduct(Product product, CancellationToken cancellationToken);
    }
}

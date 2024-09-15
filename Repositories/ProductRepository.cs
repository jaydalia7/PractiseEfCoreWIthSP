using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PractiseEfCoreWIthSP.DataContext;
using PractiseEfCoreWIthSP.Models.Domains;
using PractiseEfCoreWIthSP.Repositories.IRepositories;

namespace PractiseEfCoreWIthSP.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly PractiseDataContext _context;

        public ProductRepository(PractiseDataContext context)
        {
            _context = context;
        }

        public async Task<Product> AddProduct(Product productData, CancellationToken cancellationToken)
        {
            await _context.Products.AddAsync(productData, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return productData;
        }

        public async Task<bool> CheckProductByName(string productName, CancellationToken cancellationToken)
        {
            /*Without Sp*/
            //var product = await _context.Products.Where(product => product.Name.Equals(productName)).FirstOrDefaultAsync(cancellationToken);
            //if (product == null)
            //{
            //    return false;
            //}
            //else
            //{
            //    return true;
            //}

            /*With Sp*/
            var product = await _context.Products.FromSqlRaw("exec GetProductByName {0}", productName).ToListAsync(cancellationToken);
            if (product == null || product.Count <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<List<Product>> GetAllProducts(CancellationToken cancellationToken)
        {
            /*Without Sp*/
            //var products = await _context.Products.Where(p => p.IsDeleted.Equals(false)).ToListAsync(cancellationToken);
            //return products;

            /*With Sp*/
            var products = await _context.Products.FromSqlRaw("exec GetAllProducts").ToListAsync(cancellationToken);
            return products;

        }

        public async Task<Product> GetProductById(int id, CancellationToken cancellationToken)
        {
            /*Without Sp*/
            //var product = await _context.Products.AsNoTracking().Where(p => p.Id == id && p.IsDeleted.Equals(false)).FirstOrDefaultAsync(cancellationToken);
            //return product;

            /*With Sp*/
            var product = await _context.Products.FromSqlRaw("exec GetProductById {0}", id).ToListAsync(cancellationToken);
            return product.FirstOrDefault();

        }

        public async Task<bool> RemoveProduct(Product product, CancellationToken cancellationToken)
        {
            /*Without Sp*/
            //product.IsDeleted = true;
            //_context.Products.Update(product);
            //var count = await _context.SaveChangesAsync(cancellationToken);
            //return count > 0;

            /*With Sp*/
            var count = await _context.Database.ExecuteSqlRawAsync("Exec UpatedOrRemoveProduct @ProductId={0}, @Action='delete'", product.Id);
            return count > 0;
        }

        public async Task<Product> UpdateProductData(Product product, CancellationToken cancellationToken)
        {
            /*Without Sp*/
            //_context.Products.Update(product);
            //var count = await _context.SaveChangesAsync(cancellationToken);
            //return count > 0 ? product : null;

            /*With Sp*/
            var count = await _context.Database.ExecuteSqlRawAsync("Exec UpatedOrRemoveProduct @ProductId={0}, @ProductName={1}, @ProductDescription = {2},@ProductPrice = {3}, @Action='update'", product.Id,product.Name,product.Description,product.Price);
            return count > 0 ? product : null;
        }
    }
}

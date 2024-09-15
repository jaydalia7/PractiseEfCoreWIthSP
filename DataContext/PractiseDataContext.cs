

using Microsoft.EntityFrameworkCore;
using PractiseEfCoreWIthSP.Models.Domains;

namespace PractiseEfCoreWIthSP.DataContext
{
    public class PractiseDataContext : DbContext
    {
        public PractiseDataContext(DbContextOptions<PractiseDataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PurchasedProduct> PurchasedProducts { get; set; }
        public DbSet<SellProduct> SellProducts { get; set; }
    }
}

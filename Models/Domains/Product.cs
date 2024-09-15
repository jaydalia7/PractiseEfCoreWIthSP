using System.ComponentModel.DataAnnotations;

namespace PractiseEfCoreWIthSP.Models.Domains
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<PurchasedProduct> PurchasedProducts { get; set; }
        public ICollection<SellProduct> SellProducts { get; set; }

    }
}

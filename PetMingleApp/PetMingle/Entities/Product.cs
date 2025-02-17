namespace PetMingle.Entities
{
    public class Product
    {
        public int ID { get; set; }
        
        public string? Name { get; set; }

        public int CompanyId { get; set; }

        public Company? Company { get; set; }
        
        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public string? ProductCoverUrl { get; set; }

        public ICollection<ProductCategory>? ProductCategories { get; set; }

        public ICollection<Review>?Reviews { get; set; }
    }
}

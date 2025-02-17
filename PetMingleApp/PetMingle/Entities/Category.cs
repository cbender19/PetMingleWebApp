namespace PetMingle.Entities
{
    public class Category
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; }
    }
}

namespace PetMingle.Entities
{
    public class Cart
    {
        public int ID { get; set; }

        public string UserID { get; set; }

        public ApplicationUser User { get; set; }

        public ICollection<CartItem> CartItems { get; set; }
    }
}

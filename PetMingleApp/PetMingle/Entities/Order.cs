namespace PetMingle.Entities
{
    public class Order
    {
        public int ID { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public DateTime OrderDate { get; set; }

        public string Status { get; set; }

        public decimal TotalPrice { get; set; }

        public string ShippingAddress { get; set; }

        public ICollection<OrderItem> OrderItem { get; set; }

        public void UpdateOrder()
        {
            if (DateTime.Compare(OrderDate, DateTime.Now) <= 0)
            {
                Status = "Arrived";
            }
            else
            {
                Status = "In transit";
            }
        }
    }
}

using Microsoft.AspNetCore.Identity;

namespace PetMingle.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime RegistrationDate { get; set; }

        public ICollection<Order>? Orders { get; set; }

        public ICollection<Review>? Reviews { get; set; }

        public Cart Cart { get; set; }
    }
}

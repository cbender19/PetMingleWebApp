
using Microsoft.AspNetCore.Identity;
using PetMingle.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Authorization;



namespace PetMingle.Data.Repositories.Customer
{
    public class UserRepository : IDisposable
    {
        private readonly DataContext _ctx;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthenticationStateProvider _authenticationStateAsync;

        public UserRepository(DataContext ctx, UserManager<ApplicationUser> userManager,
             AuthenticationStateProvider authenticationStateProvider)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _authenticationStateAsync = authenticationStateProvider ?? throw new ArgumentNullException(nameof(authenticationStateProvider));
        }
        public void Dispose()
        {
            _ctx.Dispose();
        }


        public async Task<ApplicationUser> GetUserAsync()
        {
            var authState = await _authenticationStateAsync.GetAuthenticationStateAsync();
            var user = authState?.User;

            if (user == null || !user.Identity.IsAuthenticated)
            {
                throw new InvalidOperationException("No authenticated user found.");
            }

            var userId = user.Claims.FirstOrDefault()?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidOperationException("User ID claim is missing or invalid.");
            }

            var applicationUser = await _userManager.Users
                .Include(u => u.Reviews)
                .Include(u => u.Orders)
                .Include(u => u.Cart)
                .ThenInclude(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (applicationUser == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            if (applicationUser.Cart == null)
            {
                applicationUser.Cart = new Cart { UserID = applicationUser.Id, CartItems = new List<CartItem>() };
                _ctx.Carts.Add(applicationUser.Cart);
                await _ctx.SaveChangesAsync();
            }

            return applicationUser;
        }

        public async Task<CartItem> AddToCartAsync(Product product, ApplicationUser user)
        {
            if (user == null || string.IsNullOrEmpty(user.Id))
            {
                throw new InvalidOperationException("User is not authenticated. Please log in to add items to the cart.");
            }
            if (product == null) throw new ArgumentNullException(nameof(product));

            // Validate user exists in the database
            var existingUser = await _ctx.Users
                .Include(u => u.Cart)
                .ThenInclude(c => c.CartItems)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (existingUser == null)
            {
                throw new InvalidOperationException($"User with ID {user.Id} does not exist.");
            }

            var cart = existingUser.Cart ?? new Cart
            {
                UserID = existingUser.Id,
                CartItems = new List<CartItem>()
            };

            if (cart.ID == 0) // New cart
            {
                _ctx.Carts.Add(cart);
            }

            var existingCartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == product.ID);
            if (existingCartItem != null)
            {
                // Update quantity for existing item
                existingCartItem.Quantity += 1;
            }
            else
            {
                // Add new item to cart
                var newCartItem = new CartItem
                {
                    CartId = cart.ID,
                    ProductId = product.ID,
                    Quantity = 1
                };
                cart.CartItems.Add(newCartItem);
                _ctx.CartItems.Add(newCartItem);
            }

            await _ctx.SaveChangesAsync(); // Save changes once
            return cart.CartItems.First(ci => ci.ProductId == product.ID);
        }


        public async Task RemoveItemFromCart(CartItem item)
        {
            //Remove the item
            _ctx.CartItems.Remove(item);

            await _ctx.SaveChangesAsync();

        }

        public async Task UpdateItem(CartItem updatedItem)
        {
            if (updatedItem == null) throw new ArgumentNullException(nameof(updatedItem));

            var oldItem = await _ctx.CartItems.FindAsync(updatedItem.ID);
            if (oldItem == null)
            {
                throw new InvalidOperationException("The item to update does not exist.");
            }

            oldItem.Quantity = updatedItem.Quantity;
            await _ctx.SaveChangesAsync();
        }




        public async Task DecrementStockAsync(IEnumerable<CartItem> cartItems)
        {
            foreach (var item in cartItems)
            {
                var product = await _ctx.Products.FirstOrDefaultAsync(p => p.ID == item.ProductId);
                if (product != null)
                {
                    Console.WriteLine($"Before Update - Product ID: {product.ID}, Stock: {product.StockQuantity}");
                    product.StockQuantity -= item.Quantity;
                    if (product.StockQuantity < 0) product.StockQuantity = 0; // Avoid negative stock

                    Console.WriteLine($"After Update - Product ID: {product.ID}, Stock: {product.StockQuantity}");

                    // Detach the product to avoid tracking conflicts
                    _ctx.Entry(product).State = EntityState.Detached;
                }
            }

            await _ctx.SaveChangesAsync();
        }

        // Clear all items from the user's cart
        public async Task ClearCartAsync(ApplicationUser user)
        {
            if (user?.Cart?.CartItems != null)
            {
                // Detach referenced Product entities to avoid tracking conflicts
                foreach (var cartItem in user.Cart.CartItems)
                {
                    _ctx.Entry(cartItem.Product).State = EntityState.Detached;
                }

                // Remove cart items
                _ctx.CartItems.RemoveRange(user.Cart.CartItems);
                user.Cart.CartItems.Clear();

                await _ctx.SaveChangesAsync();
            }
        }


    }


}

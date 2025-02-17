using System.Threading.Tasks;

namespace PetMingle.Services
{
    public class CartServices
    {
        public event Action OnChange;

        private bool _showCart = false;
        private bool _startAnimation = false;

        public bool ShowCart
        {
            get => _showCart;
            set
            {
                _showCart = value;
                NotifyStateChanged();
            }
        }

        public bool StartAnimation
        {
            get => _startAnimation;
            set
            {
                _startAnimation = value;
                NotifyStateChanged();
            }
        }

        public async Task OpenCartWithAutoCloseAsync(int timeoutInSeconds = 5)
        {
            OpenCart();
            await Task.Delay(1000); // Wait for the specified duration
            CloseCart();
        }

        public void OpenCart()
        {
            StartAnimation = false;
            ShowCart = true;
        }

        public void CloseCart()
        {
            StartAnimation = true;
            ShowCart = false;
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}


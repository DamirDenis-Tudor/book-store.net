using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Persistence.DTO.Order;
using PresentationClient.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using static PresentationClient.Pages.PaymentDetails;

namespace PresentationClient.Pages
{
    public partial class ViewCart
    {
        [Inject]
        private ICartService CartService { get; set; }

        private ObservableCollection<OrderProductData> Products { get; set; }

        protected override void OnInitialized()
        {
            Products = new ObservableCollection<OrderProductData>();
           
        }

        private bool _isDataLoaded = false;
        private bool _cartEmpty = true;
		private decimal ProductsTotalPrice { get; set; } = 0;
        private decimal TotalPrice { get; set; } = 0;
        private const decimal _deliveryFeeForOrder = 11.99m;
        private decimal _deliveryFee = 0; 

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && !_isDataLoaded)
            {
                var cart = await CartService.GetCart();
                foreach (var product in cart)
                {
                    Products.Add(product);
                }
                UpdateTotalPrices();
				_isDataLoaded = true;
                _cartEmpty = !Products.Any();

				StateHasChanged();
            }
        }

        private void UpdateTotalPrices()
        {
			ProductsTotalPrice = Products.Sum(prod => prod.Price * prod.OrderQuantity);
            if (ProductsTotalPrice != 0 && ProductsTotalPrice < 300)
                _deliveryFee = _deliveryFeeForOrder;
            else
                _deliveryFee = 0;
			TotalPrice = ProductsTotalPrice + _deliveryFee;
		}

        //TODO: Refactor this solution
        public void RefreshView()
		{
            UpdateTotalPrices();
			_cartEmpty = !Products.Any() || !Products.Where(prod=> prod.OrderQuantity > 0).Any();
			StateHasChanged();
		}
    }
}

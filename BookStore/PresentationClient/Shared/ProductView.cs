using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Persistence.DTO.Order;
using Persistence.DTO.Product;
using PresentationClient.Services;

namespace PresentationClient.Shared
{
    public partial class ProductView
    {
        [Inject]
        private ICartService CartService { get; set; }

        [Parameter]
        public ProductDto? Product { get; set; }

        protected async Task AddToCart() => await CartService.AddToCart(Product);
    }
}

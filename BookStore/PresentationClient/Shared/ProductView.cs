using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Persistence.DTO.Order;
using Persistence.DTO.Product;

namespace PresentationClient.Shared
{
    public partial class ProductView
    {
        [Inject]
        private ProtectedLocalStorage LocalStorage { get; set; }

        [Parameter]
        public ProductDto? Product { get; set; }

        protected async Task AddToCart()
        {
            if (Product != null)
            {
                var result = await LocalStorage.GetAsync<List<OrderProductData>>("cart");
                List<OrderProductData> products;
                if (!result.Success || result.Value == null)
                    products = new List<OrderProductData>();
                else
                {
                    products = result.Value;
                    
                }
                bool found = false;
                products.ForEach(prod => { if (prod.ProductName == Product.Name) { prod.OrderQuantity = prod.OrderQuantity + 1; found = true; }});

                if (!found)
                {
                    OrderProductData orderProductDto = new OrderProductData() { ProductName = Product.Name, Description = Product.Description, Link = Product.Link, Price = Product.Price, OrderQuantity = 1 };
                    products.Add(orderProductDto);
                }
                products.ForEach(Console.WriteLine);
                await LocalStorage.SetAsync("cart", products);
            }
        }
    }
}

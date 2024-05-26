using Microsoft.AspNetCore.Components;
using Persistence.DTO.Order;
using PresentationClient.Services;

namespace PresentationClient.Shared
{
	public partial class ProductCart
	{
		[Inject]
		private ICartService Cart { get; set; }

		[Parameter]
		public OrderProductData? Product { get; set; }

		[Parameter]
		public Action? RefreshView { get; set; }

		private bool _enabled = true;

		private void IncreaseQuantity()
		{
			Product.OrderQuantity++;
			Cart.UpdateProduct(Product);
			RefreshView?.Invoke();
		}

		private void DecreaseQuantity()
		{
			Product.OrderQuantity--;
			if (Product.OrderQuantity == 0)
			{
				Cart.DeleteProduct(Product);
				_enabled = false;
				//StateHasChanged();
			}
			else
				Cart.UpdateProduct(Product);
			RefreshView?.Invoke();
		}
	}
}

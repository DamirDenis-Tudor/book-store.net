using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Persistence.DTO.Product;

namespace PresentationProvider.Shared
{
	public partial class ProductView
	{

		[Inject]
		public NavigationManager NavigationManager { get; set; }

		[Parameter]
		public ProductDto? Product { get; set; }

		private void ProductEdit()
		{
			NavigationManager.NavigateTo($"/update-product?product={Product.Name.Replace("\n", "%0A")}");
		}
	}
}

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Persistence.DTO.Product;

namespace PresentationProvider.Shared
{
	public partial class ProductView
	{

		[Parameter]
		public ProductDto? Product { get; set; }

	}
}

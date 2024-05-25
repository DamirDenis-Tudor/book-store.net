using Microsoft.AspNetCore.Components;
using Persistence.DAL;
using Persistence.DTO.Product;

namespace PresentationClient.Pages
{
	public partial class Index
	{
		protected string test = "";
		public class ProductCategory
		{
			public string Name { get; set; }
		}

		private List<ProductCategory> Categories = new List<ProductCategory>
		{
			new ProductCategory{ Name = "Jucarii"},
			new ProductCategory{ Name = "Haine"},
			new ProductCategory{ Name = "Mancare"},
			new ProductCategory{ Name = "Custi"},
		};

		private IList<ProductDto> Products = PersistenceAccess.Instance.ProductRepository.GetAllProducts().SuccessValue;

		protected void orderOptionSelected()
		{
			test = "";
		}
	}
}

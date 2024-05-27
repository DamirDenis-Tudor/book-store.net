using Business.BAL;
using Persistence.DAL;
using Persistence.DTO.Product;

namespace PresentationClient.Entities
{
	public class ProductsScope
	{
		public static ProductsScope Instance => new();
        private ProductsScope() {  }
        public IList<ProductDto> Products { get; set; } = BusinessFacade.Instance.InventoryService.GetInventory().SuccessValue;
		public IList<string> Categories() => Products.Select(prod => prod.Category).Distinct().ToList();
	}
}
using Persistence.DAL;
using Persistence.DTO.Product;

namespace PresentationAdmin.Entities
{
	public class ProductsScope
	{
		public IList<ProductDto> Products { get; set; } = PersistenceFacade.Instance.ProductRepository.GetAllProducts().SuccessValue;
		public IList<string> Categories { get; set; } = PersistenceFacade.Instance.ProductRepository.GetCategories().SuccessValue;
	}
}

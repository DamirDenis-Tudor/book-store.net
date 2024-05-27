using Persistence.DTO.Order;
using Persistence.DTO.Product;

namespace PresentationClient.Services
{
	public interface ICartService
	{
		Task<List<OrderProductData>> GetCart();

		Task AddToCart(ProductDto product);
		void UpdateProduct(OrderProductData newProduct);
		void DeleteProduct(OrderProductData newProduct);
		void ClearCart();
	}
}

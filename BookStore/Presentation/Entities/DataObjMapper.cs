using Persistence.DTO.Order;
using Persistence.DTO.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Entities
{
	public class DataObjMapper
	{

		/// <summary>
		/// Map the details of the product form <see cref="OrderProductDto"/> to a <see cref="ProductDto"/> for the display component
		/// </summary>
		/// <param name="prod">The order product data object</param>
		/// <returns>The resulted product object</returns>
		public static ProductDto ConvertToProductDto(OrderProductDto prod)
		{
			return new ProductDto
			{
				Name = prod.ProductName,
				Description = prod.Description,
				Price = prod.Price,
				Link = prod.Link,
				Quantity = prod.OrderQuantity,
				Category = ""
			};
		}
		
		/// <summary>
		/// Map the details of the product form <see cref="OrderProductDto"/> to a <see cref="ProductDto"/> for the display component
		/// </summary>
		/// <param name="prod">The order product data object</param>
		/// <returns>The resulted product object</returns>
		public static ProductDto ConvertToProductDto(ProductStatsDto prod)
		{
			return new ProductDto
			{
				Name = prod.ProductDto.Name,
				Description = prod.ProductDto.Description,
				Price = prod.TotalRevenue,
				Link = prod.ProductDto.Link,
				Quantity = prod.TotalItemsSold,
				Category = prod.ProductDto.Category
			};
		}
	}
}

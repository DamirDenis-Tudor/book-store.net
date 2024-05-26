using Business.BAL;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Persistence.DAL;
using Persistence.DTO.Product;
using PresentationAdmin.Service;

namespace PresentationAdmin.Entities
{
	public class ProductsScope
	{
		/*private IUserLoginService _userData;
		private ProtectedLocalStorage ll;
		public ProductsScope(IUserLoginService userData)
		{
			_userData = userData;
		}*/


		public IList<ProductStatsDto> Products { get { return BusinessFacade.Instance.InventoryService.GetInventoryStats().SuccessValue; } set { Products = value; } }
		//public IList<ProductStatsDto> Products { get { return BusinessFacade.Instance.InventoryService.GetInventoryStats(await _userData.GetUsername()).SuccessValue; } set; }
		public IList<string> Categories()
		{
			List<string> cat = new List<string>();
			foreach (var item in Products)
				cat.Add(item.ProductDto.Category);
			return cat.Distinct().ToList();
		}
	}
}

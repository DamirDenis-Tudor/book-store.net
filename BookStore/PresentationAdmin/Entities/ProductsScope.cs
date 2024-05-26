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
		private IUserLoginService _userData;
		private ProtectedLocalStorage ll;
		public ProductsScope(IUserLoginService userData, ProtectedLocalStorage l)
		{
			_userData = userData;
			ll = l;
		}


		//public IList<ProductStatsDto> Products { get { return BusinessFacade.Instance.InventoryService.GetInventoryStats(await _userData.GetUsername()).SuccessValue; } set; }
		//public IList<ProductStatsDto> Products { get { return BusinessFacade.Instance.InventoryService.GetInventoryStats(await _userData.GetUsername()).SuccessValue; } set; }
		public IList<string> Categories { get; set; } = PersistenceFacade.Instance.ProductRepository.GetCategories().SuccessValue;

		public async Task<IList<ProductStatsDto>> GetProducts()
		{
			//var result = BusinessFacade.Instance.InventoryService.GetInventoryStats(await _userData.GetUsername());
			//var dwd = await ll.GetAsync<string>("sessiontoken");
			return PersistenceFacade.Instance.ProductRepository.GetAllProductsStats().SuccessValue;
			//return result.IsSuccess ? result.SuccessValue : new List<ProductStatsDto>(); 
		}/*public async Task<IList<ProductStatsDto>> GetProducts()
		{
			//var result = BusinessFacade.Instance.InventoryService.GetInventoryStats(await _userData.GetUsername());
			var dwd = await _userData.GetUsername();
			return PersistenceFacade.Instance.ProductRepository.GetAllProductsStats().SuccessValue;
			//return result.IsSuccess ? result.SuccessValue : new List<ProductStatsDto>(); 
		}*/
	}
}

using Microsoft.AspNetCore.Components;
using Persistence.DTO.Product;
using PresentationAdmin.Entities;
using System.Collections.ObjectModel;

namespace PresentationAdmin.Pages
{
	public partial class Home
	{
		[Inject]
		protected ProductsScope ProductsScope { get; set; }
		protected ObservableCollection<ProductStatsDto> DisplayProducts { get; set; }

		protected decimal? _priceRangeMin, _priceRangeMax;
		protected decimal? PriceRangeMin
		{
			get => _priceRangeMin;
			set
			{
				_priceRangeMin = value;
				if (_serach == null)
				{
					if (Category == null)
						DisplayProducts = new ObservableCollection<ProductStatsDto>(ProductsScope.GetProducts().Result.Where(p => p.TotalRevenue >= _priceRangeMin));
					else
					{
						CategoryFilter();
						DisplayProducts = new ObservableCollection<ProductStatsDto>(DisplayProducts.Where(p => p.TotalRevenue >= _priceRangeMin));
					}
				}
			}
		}
		protected decimal? PriceRangeMax
		{
			get => _priceRangeMax;
			set
			{
				_priceRangeMax = value;
				if (_serach == null)
				{
					if (Category == null)
						DisplayProducts = new ObservableCollection<ProductStatsDto>(ProductsScope.GetProducts().Result.Where(p => p.TotalRevenue <= _priceRangeMax));
					else
					{
						CategoryFilter();
						DisplayProducts = new ObservableCollection<ProductStatsDto>(DisplayProducts.Where(p => p.TotalRevenue <= _priceRangeMax));
					}
				}
			}
		}

		private string? _category = null;
		[SupplyParameterFromQuery]
		protected string? Category
		{
			get
			{
				return _category;
			}
			set
			{
				_category = value;
				CategoryFilter();
			}
		}
		private string? _serach = null;
		[SupplyParameterFromQuery(Name = "search")]
		protected string? Search
		{
			get => _serach;
			set
			{
				_serach = value;
				if (_serach != null)
					DisplayProducts = new ObservableCollection<ProductStatsDto>(ProductsScope.GetProducts().Result.Where(p => p.Name.Contains(_serach)));
			}
		}

		protected override void OnInitialized()
		{
			Console.WriteLine(_serach);
			if (_serach == null)
				DisplayProducts = new ObservableCollection<ProductStatsDto>(ProductsScope.GetProducts().Result);
			else
				DisplayProducts = new ObservableCollection<ProductStatsDto>(ProductsScope.GetProducts().Result.Where(p => p.Name.Contains(_serach)));
			PriceRangeMax = DisplayProducts.Max(prod => prod.TotalRevenue);
			PriceRangeMin = DisplayProducts.Min(prod => prod.TotalRevenue);
		}

		protected void OnSortOrderChange(ChangeEventArgs e)
		{
			string selectedMode = e.Value.ToString();
			switch (selectedMode)
			{
				case "name":
					DisplayProducts = new ObservableCollection<ProductStatsDto>(DisplayProducts.OrderBy(prod => prod.Name));
					break;
				case "price":
					DisplayProducts = new ObservableCollection<ProductStatsDto>(DisplayProducts.OrderBy(prod => prod.TotalRevenue));
					break;
				default:
					break;
			}

		}

		private void CategoryFilter()
		{
			/*if (Category != null)
				DisplayProducts = new ObservableCollection<ProductStatsDto>(ProductsScope.GetProducts().Result.Where(p => p. == Category));
			else*/
				DisplayProducts = new ObservableCollection<ProductStatsDto>(ProductsScope.GetProducts().Result);
		}

		public void OnPriceRangeChangeMin(decimal? price)
		{
			DisplayProducts = new ObservableCollection<ProductStatsDto>(ProductsScope.GetProducts().Result.Where(p => p.TotalRevenue >= price));
		}
		protected void OnPriceRangeChangeMax(decimal? price)
		{
			DisplayProducts = new ObservableCollection<ProductStatsDto>(ProductsScope.GetProducts().Result.Where(p => p.TotalRevenue <= price));
		}
	}
}

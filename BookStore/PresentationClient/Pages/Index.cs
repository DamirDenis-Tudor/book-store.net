using Microsoft.AspNetCore.Components;
using Persistence.DTO.Product;
using PresentationClient.Entities;
using System.Collections.ObjectModel;

namespace PresentationClient.Pages
{
    public partial class Index
	{
		[Inject]
		protected ProductsScope ProductsScope { get; set; }
        protected ObservableCollection<ProductDto> DisplayProducts { get; set; }

        protected decimal? _priceRangeMin, _priceRangeMax;
        protected decimal? PriceRangeMin
		{
			get => _priceRangeMin; 
			set{
				_priceRangeMin = value;
				if (_serach == null)
				{
					if (Category == null)
					DisplayProducts = new ObservableCollection<ProductDto>(ProductsScope.Products.Where(p => p.Price >= _priceRangeMin));
				else
				{
					CategoryFilter();
					DisplayProducts = new ObservableCollection<ProductDto>(DisplayProducts.Where(p => p.Price >= _priceRangeMin));
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
                    DisplayProducts = new ObservableCollection<ProductDto>(ProductsScope.Products.Where(p => p.Price <= _priceRangeMax));
                else
                {
                    CategoryFilter();
                    DisplayProducts = new ObservableCollection<ProductDto>(DisplayProducts.Where(p => p.Price <= _priceRangeMax));
                }
            }
        }
        }

        private string? _category = null;
		[SupplyParameterFromQuery]
		protected string? Category { get {
				return _category;
			} set
			{
				_category = value;
				CategoryFilter();
            }
		}
        private string? _serach = null;
		[SupplyParameterFromQuery(Name = "search")]
		protected string? Search { get => _serach;
			 set{
				_serach = value;
				if (_serach != null)
					DisplayProducts = new ObservableCollection<ProductDto>(ProductsScope.Products.Where(p => p.Name.Contains(_serach)));
            }
		}

		protected override void OnInitialized()
		{
			Console.WriteLine(_serach);
            if(_serach == null)
				DisplayProducts = new ObservableCollection<ProductDto>(ProductsScope.Products);
			else
				DisplayProducts = new ObservableCollection<ProductDto>(ProductsScope.Products.Where(p => p.Name.Contains(_serach)));
			PriceRangeMax = DisplayProducts.Max(prod => prod.Price);
			PriceRangeMin = DisplayProducts.Min(prod => prod.Price);
        }

		protected void OnSortOrderChange(ChangeEventArgs e)
		{
			string selectedMode = e.Value.ToString();
			switch(selectedMode)
			{
				case "name":
                    DisplayProducts = new ObservableCollection<ProductDto>(DisplayProducts.OrderBy(prod => prod.Name));
					break;
				case "price":
                    DisplayProducts = new ObservableCollection<ProductDto>(DisplayProducts.OrderBy(prod => prod.Price));
					break;
				default:
                    break;
            }
            
        }

		private void CategoryFilter()
		{
            if (Category != null)
                DisplayProducts = new ObservableCollection<ProductDto>(ProductsScope.Products.Where(p => p.Category == Category));
            else
                DisplayProducts = new ObservableCollection<ProductDto>(ProductsScope.Products);
        }

		public void OnPriceRangeChangeMin(decimal? price)
		{
            DisplayProducts = new ObservableCollection<ProductDto>(ProductsScope.Products.Where(p => p.Price >= price));
        }
		protected void OnPriceRangeChangeMax(decimal? price)
		{
            DisplayProducts = new ObservableCollection<ProductDto>(ProductsScope.Products.Where(p => p.Price <= price));
        }
    }
}

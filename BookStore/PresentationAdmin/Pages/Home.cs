/**************************************************************************
 *                                                                        *
 *  File:        Home.cs                                                  *
 *  Copyright:   (c) 2024, Asmarandei Catalin                             *
 *  Website:     https://github.com/DamirDenis-Tudor/BookStore.NET        *
 *  Description: The main page where the products statistics              *
 *  are displayed                                                         *
 *                                                                        *
 *  This program is free software; you can redistribute it and/or modify  *
 *  it under the terms of the GNU General Public License as published by  *
 *  the Free Software Foundation. This program is distributed in the      *
 *  hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 *  the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 *  PURPOSE. See the GNU General Public License for more details.         *
 *                                                                        *
 **************************************************************************/


using Microsoft.AspNetCore.Components;
using Persistence.DTO.Product;
using PresentationAdmin.Entities;
using System.Collections.ObjectModel;

namespace PresentationAdmin.Pages
{
    /// <summary>
    /// Where the products statistics are displayed, filtred, sorted and the admin has access to admin action buttons
    /// </summary>
    public partial class Home
	{
        /// <summary>
        /// The products singleton utility class for getting the products
        /// </summary>
        [Inject]
		protected ProductsScope ProductsScope { get; set; }
        /// <summary>
        /// The products that are displayed on the page, being dynamicly updated
        /// </summary>
        protected ObservableCollection<ProductStatsDto> DisplayProducts { get; set; }

        /// <summary>
        /// The price range for filtering the products by price
        /// </summary>
        protected decimal? _priceRangeMin, _priceRangeMax;
        /// <summary>
        /// The minimum price of the filter, if the value is changed and is valid the products will be filtered
        /// </summary>
        protected decimal? PriceRangeMin
		{
			get => _priceRangeMin;
			set
			{
				if (value < 0)
					_priceRangeMin = 0;
				else if (value > _priceRangeMax)
					_priceRangeMin = _priceRangeMax;
				else
					_priceRangeMin = value;
				if (_serach == null)
				{
					if (Category == null)
						DisplayProducts = new ObservableCollection<ProductStatsDto>(ProductsScope.Products.Where(p => p.TotalRevenue >= _priceRangeMin));
					else
					{
						CategoryFilter();
						DisplayProducts = new ObservableCollection<ProductStatsDto>(DisplayProducts.Where(p => p.TotalRevenue >= _priceRangeMin));
					}
				}
			}
		}
        /// <summary>
        /// The maximum price of the filter, if the value is changed and is valid the products will be filtered
        /// </summary>
        protected decimal? PriceRangeMax
		{
			get => _priceRangeMax;
			set
			{
				if (value < 0)
					_priceRangeMax = 0;
				else if (value < _priceRangeMin)
					_priceRangeMax = _priceRangeMin;
				else
					_priceRangeMax = value;
				if (_serach == null)
				{
					if (Category == null)
						DisplayProducts = new ObservableCollection<ProductStatsDto>(ProductsScope.Products.Where(p => p.TotalRevenue <= _priceRangeMax));
					else
					{
						CategoryFilter();
						DisplayProducts = new ObservableCollection<ProductStatsDto>(DisplayProducts.Where(p => p.TotalRevenue <= _priceRangeMax));
					}
				}
			}
		}

        /// <summary>
        /// The category of the products that are displayed
        /// </summary>
        private string? _category = null;
        /// <summary>
        /// The category of the product for filtering displayed products by category
        /// </summary>
        [SupplyParameterFromQuery(Name = "category")]
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
        /// <summary>
        /// The search query for filtering the products by name
        /// </summary>
        private string? _serach = null;
        /// <summary>
        /// The search query for filtering the products by name
        /// </summary>
        [SupplyParameterFromQuery(Name = "search")]
		protected string? Search
		{
			get => _serach;
			set
			{
				_serach = value;
				if (_serach != null)
					DisplayProducts = new ObservableCollection<ProductStatsDto>(ProductsScope.Products.Where(p => p.ProductDto.Name.Contains(_serach)));
			}
		}

        /// <summary>
        /// When the page initializes the products are filtered for search value, if it is the case,
		/// and the price range is set for maximum product price and minimum product price(default values for the price filter)
        /// </summary>
        protected override void OnInitialized()
		{
			Console.WriteLine(_serach);
			if (_serach == null)
				DisplayProducts = new ObservableCollection<ProductStatsDto>(ProductsScope.Products);
			else
				DisplayProducts = new ObservableCollection<ProductStatsDto>(ProductsScope.Products.Where(p => p.ProductDto.Name.Contains(_serach)));
			PriceRangeMax = DisplayProducts.Max(prod => prod.TotalRevenue);
			PriceRangeMin = DisplayProducts.Min(prod => prod.TotalRevenue);
		}

        /// <summary>
        /// Event called when the user changes the sorting mode
        /// Matches the selected mode with the sorting mode and sorts the products
        /// </summary>
        /// <param name="e">The change event raised</param>
        protected void OnSortOrderChange(ChangeEventArgs e)
		{
			string selectedMode = e.Value.ToString();
			switch (selectedMode)
			{
				case "name":
					DisplayProducts = new ObservableCollection<ProductStatsDto>(DisplayProducts.OrderBy(prod => prod.ProductDto.Name));
					break;
				case "price":
					DisplayProducts = new ObservableCollection<ProductStatsDto>(DisplayProducts.OrderBy(prod => prod.TotalRevenue));
					break;
				default:
					break;
			}

		}

        /// <summary>
        /// Filters the products by category if there is any category selected
        /// </summary>
        private void CategoryFilter()
		{
			if (Category != null)
				DisplayProducts = new ObservableCollection<ProductStatsDto>(ProductsScope.Products.Where(p => p.ProductDto.Category == Category));
			else
				DisplayProducts = new ObservableCollection<ProductStatsDto>(ProductsScope.Products);
		}

        /// <summary>
        /// Event called when the user changes the minimum price value of the filter
        /// </summary>
        /// <param name="price">The new value of the price</param>
        public void OnPriceRangeChangeMin(decimal? price)
		{
			DisplayProducts = new ObservableCollection<ProductStatsDto>(ProductsScope.Products.Where(p => p.TotalRevenue >= price));
		}
        /// <summary>
        /// Event called when the user changes the maximum price value of the filter
        /// </summary>
        /// <param name="price">The new value of the price</param>
        protected void OnPriceRangeChangeMax(decimal? price)
		{
			DisplayProducts = new ObservableCollection<ProductStatsDto>(ProductsScope.Products.Where(p => p.TotalRevenue <= price));
		}
	}
}

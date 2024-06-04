/**************************************************************************
 *                                                                        *
 *  File:        Index.cs                                                 *
 *  Copyright:   (c) 2024, Asmarandei Catalin                             *
 *  Website:     https://github.com/DamirDenis-Tudor/BookStore.NET        *
 *  Description: The main page where the products are displayed           *
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
using Presentation.Entities;
using System.Collections.ObjectModel;

namespace Presentation.Components;

/// <summary>
/// Where the products are displayed, filtred, sorted
/// </summary>
public partial class HomeComponent
{
    /// <summary>
    /// The products repository
    /// </summary>
    [Inject]
    private IProductsScope ProductsScope { get; set; } = null!;

    /// <summary>
    /// When displaying the products, the child content is the side part of the card that could differ from use to use
    /// </summary>
    [Parameter]
    public RenderFragment<ProductDto> ChildContent { get; set; } = null!;

    /// <summary>
    /// A dashboard that will appear above the categories section if used
    /// </summary>
    [Parameter]
    public RenderFragment? DashboardActionBar { get; set; } = null;

    /// <summary>
    /// The base path of the page using this compoent
    /// It must start with /
    /// </summary>
    [Parameter]
    public string? PathBase { get; set; } = "";

    /// <summary>
    /// The products that are displayed on the page being dynamicly updated 
    /// </summary>
    private ObservableCollection<ProductDto> DisplayProducts { get; set; } = null!;

    /// <summary>
    /// The price range for filtering the products by price
    /// </summary>
    private decimal? _priceRangeMin;

    /// <summary>
    /// The price range for filtering the products by price
    /// </summary>
    private decimal? _priceRangeMax;

    /// <summary>
    /// The minimum price of the filter, if the value is changed and is valid the products will be filtered
    /// </summary>
    private decimal? PriceRangeMin
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

            if (_search != null) return;

            if (Category != null)
            {
                CategoryFilter();
                DisplayProducts = new ObservableCollection<ProductDto>
                    (DisplayProducts.Where(p => p.Price >= _priceRangeMin));
                return;
            }

            DisplayProducts = new ObservableCollection<ProductDto>
                (ProductsScope.GetProducts().Where(p => p.Price >= _priceRangeMin));
        }
    }

    /// <summary>
    /// The maximum price of the filter, if the value is changed and is valid the products will be filtered
    /// </summary>
    private decimal? PriceRangeMax
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

            if (_search != null) return;

            if (Category == null)
            {
                CategoryFilter();
                DisplayProducts = new ObservableCollection<ProductDto>
                    (DisplayProducts.Where(p => p.Price <= _priceRangeMax));
                return;
            }

            DisplayProducts = new ObservableCollection<ProductDto>
                (ProductsScope.GetProducts().Where(p => p.Price <= _priceRangeMax));
        }
    }

    /// <summary>
    /// The category of the product for filtering displayed products by category
    /// </summary>
    private string? _category;

    /// <summary>
    /// The category the products will be sorted by taken from the html query, if value is changed the products will be filtered
    /// </summary>
    [SupplyParameterFromQuery(Name = "category")]
    protected string? Category
    {
        get => _category;
        set
        {
            _category = value;
            CategoryFilter();
        }
    }

    /// <summary>
    /// The name of the product that the user is searching for
    /// </summary>
    private string? _search;

    /// <summary>
    /// Value taken from the html query, if value is changed, the products will be filtered for matching with the name
    /// </summary>
    [SupplyParameterFromQuery(Name = "search")]
    public string? Search
    {
        get => _search;
        set
        {
            _search = value;
            if (_search != null)
                DisplayProducts = new ObservableCollection<ProductDto>
                    (ProductsScope.GetProducts().Where(p => p.ProductInfoDto.Name.Contains(_search)));
        }
    }

    /// <summary>
    /// When the page initializes the products are filtered for search value, if it is the case,
    /// and the price range is set for maximum product price and minimum product price(default values for the price filter)
    /// </summary>
    protected override void OnInitialized()
    {
        if (_search == null)
            DisplayProducts = new ObservableCollection<ProductDto>(ProductsScope.GetProducts());
        else
            DisplayProducts = new ObservableCollection<ProductDto>
                (ProductsScope.GetProducts().Where(p => p.ProductInfoDto.Name.Contains(_search)));
        if (ProductsScope.GetProducts().Count > 0)
        {
            PriceRangeMax = DisplayProducts.Max(prod => prod.Price);
            PriceRangeMin = DisplayProducts.Min(prod => prod.Price);
        }

    }

    /// <summary>
    /// Event called when the user changes the sorting mode
    /// Matches the selected mode with the sorting mode and sorts the products
    /// </summary>
    /// <param name="e">The change event raised</param>
    private void OnSortOrderChange(ChangeEventArgs e)
    {
        if (e.Value == null) return;

        var selectedMode = e.Value.ToString();
        switch (selectedMode)
        {
            case "name":
                DisplayProducts = new ObservableCollection<ProductDto>(DisplayProducts.OrderBy(prod => prod.ProductInfoDto.Name));
                break;
            case "price":
                DisplayProducts = new ObservableCollection<ProductDto>(DisplayProducts.OrderBy(prod => prod.Price));
                break;
        }
    }

    /// <summary>
    /// Filters the products by category if there is any category selected
    /// </summary>
    private void CategoryFilter()
    {
        if (Category != null)
            DisplayProducts = new ObservableCollection<ProductDto>
                (ProductsScope.GetProducts().Where(p => p.ProductInfoDto.Category == Category));
        else
            DisplayProducts = new ObservableCollection<ProductDto>(ProductsScope.GetProducts());
    }
}
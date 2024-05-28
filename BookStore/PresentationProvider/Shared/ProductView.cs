/**************************************************************************
 *                                                                        *
 *  File:        ProductView.cs                                           *
 *  Copyright:   (c) 2024, Asmarandei Catalin                             *
 *  Website:     https://github.com/DamirDenis-Tudor/BookStore.NET        *
 *  Description: Component that will be displayed for viewing             *
 *      the products                                                      *
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

namespace PresentationProvider.Shared
{
    /// <summary>
    /// Used for dispalying the products
    /// </summary>
    public partial class ProductView
    {
        /// <summary>
        /// Navigation manager for redirecting the user to the update product page
        /// </summary>
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// The product received form the parent page to be displayed
        /// </summary>
        [Parameter]
        public ProductDto? Product { get; set; }

        /// <summary>
        /// Event called when the provider wants to edit the product
        /// He will be redirected to the update page with the product name in query
        /// </summary>
        private void ProductEdit()
        {
            NavigationManager.NavigateTo($"/update-product?product={Product.Name.Replace("\n", "%0A")}");
        }
    }
}

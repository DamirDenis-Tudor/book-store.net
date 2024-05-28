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
using PresentationClient.Services;

namespace PresentationClient.Shared
{
    /// <summary>
    /// Used for dispalying the products
    /// </summary>
    public partial class ProductView
    {
        /// <summary>
        /// The cart service for operating with the user cart
        /// </summary>
        [Inject]
        private ICartService CartService { get; set; } = null!;

        /// <summary>
        /// The product received form the parent page to be displayed
        /// </summary>
        [Parameter]
        public ProductDto? Product { get; set; }

        /// <summary>
        /// Adds the product to the cart using the service
        /// </summary>
        /// <returns></returns>
        private async Task AddToCart() => await CartService.AddToCart(Product);
    }
}
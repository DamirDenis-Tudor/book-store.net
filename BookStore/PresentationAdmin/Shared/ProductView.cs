/**************************************************************************
 *                                                                        *
 *  File:        ProductView.cs                                           *
 *  Copyright:   (c) 2024, Asmarandei Catalin                             *
 *  Website:     https://github.com/DamirDenis-Tudor/BookStore.NET        *
 *  Description: Component that will be displayed for viewing             *
 *  the products                                                          *
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

namespace PresentationAdmin.Shared
{
    /// <summary>
    /// Used for displaying the products
    /// </summary>
    public partial class ProductView
    {
        /// <summary>
        /// The product stats received form the parent page to be displayed
        /// </summary>s
        [Parameter]
        public ProductDto? Product { get; set; }
    }
}

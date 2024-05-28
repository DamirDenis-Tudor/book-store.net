/**************************************************************************
 *                                                                        *
 *  File:        ProductCart.cs                                           *
 *  Copyright:   (c) 2024, Asmarandei Catalin                             *
 *  Website:     https://github.com/DamirDenis-Tudor/BookStore.NET        *
 *  Description: Component that will be displayed for items in cart       *
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
using Persistence.DTO.Order;
using PresentationClient.Services;

namespace PresentationClient.Shared
{
    /// <summary>
    /// The product that is displayed in view cart
	/// Used for increasing and decreasing the quantity of the product
    /// </summary>
    public partial class ProductCart
	{
        /// <summary>
        /// The cart service for operating with the user cart
        /// </summary>
        [Inject]
		private ICartService Cart { get; set; }
		/// <summary>
		/// The product got paramter from the view cart page
		/// </summary>
		[Parameter]
		public OrderProductData? Product { get; set; }

        /// <summary>
        /// Function received from the view cart page that wil handle the refresh when the user change the quantity
        /// </summary>
        [Parameter]
		public Action? RefreshView { get; set; }

		/// <summary>
		/// If the product is enabled and vaild, if not it will not be dispalyed on the page
		/// </summary>
		private bool _enabled = true;

        /// <summary>
        /// Called when the user increase the product quantity
        /// Increase, then updates the product in cart and notifys the parent page
        /// </summary>
        private void IncreaseQuantity()
		{
			Product.OrderQuantity++;
			Cart.UpdateProduct(Product);
			RefreshView?.Invoke();
		}

        /// <summary>
        /// Called when the user decrease the product quantity
        /// Decrease, validates the new quantity of the product, then updates it in cart and notifys the parent page
        /// </summary>
        private void DecreaseQuantity()
		{
			Product.OrderQuantity--;
			if (Product.OrderQuantity == 0)
			{
				Cart.DeleteProduct(Product);
				_enabled = false;
				//StateHasChanged();
			}
			else
				Cart.UpdateProduct(Product);
			RefreshView?.Invoke();
		}
	}
}

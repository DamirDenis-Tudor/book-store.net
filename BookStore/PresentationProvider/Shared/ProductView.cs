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


using Business.BAL;
using Common;
using Microsoft.AspNetCore.Components;
using Persistence.DTO.Product;
using Presentation.Components;
using Presentation.Services;

namespace PresentationProvider.Shared
{
    /// <summary>
    /// Used for displaying the products
    /// </summary>
    public partial class ProductView
    {
        /// <summary>
        /// Navigation manager for redirecting the user to the update product page
        /// </summary>
        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;
        /// <summary>
        /// Business facade for interacting with the business layer
        /// </summary>
        [Inject]
        public BusinessFacade Business { get; set; } = null!;
        /// <summary>
        /// User login service for getting the login session token
        /// </summary>
        [Inject]
        public IUserLoginService UserLogin { get; set; } = null!;

        /// <summary>
        /// The product received form the parent page to be displayed
        /// </summary>
        [Parameter]
        public ProductDto Product { get; set; } = null!;

        /// <summary>
        /// Event called when the provider wants to edit the product
        /// He will be redirected to the update page with the product name in a query
        /// </summary>
        private void ProductEdit() => NavigationManager
            .NavigateTo($"/update-product?product={Product.ProductInfoDto.Name.Replace("\n", "%0A")}");

        /// <summary>
        /// Confirm dialog for removing the product
        /// </summary>
        private ConfirmPopUp confirmationDialog;

        /// <summary>
        /// Called when the provider hits the delete product button
        /// </summary>
        private void ProductRemove()
        {
            confirmationDialog.ShowDialog();
        }

        /// <summary>
        /// Callback for the confirmation dialog
        /// </summary>
        /// <param name="confirmed">If the user choose confim or cancel</param>
        private async Task OnConfirmClose(bool confirmed)
        {
            if (confirmed)
            {
                var token = await UserLogin.GetToken();
                var result = Business.AuthService.GetUsername(token);
                if(!result.IsSuccess)
                {
                    Logger.Instance.GetLogger<ProductView>().LogError(result.Message);
                    return;
                }
                Business.InventoryService.DeleteProduct(result.SuccessValue, Product.ProductInfoDto.Name);
                NavigationManager.NavigateTo("/home", true);
            }
        }

    }
}

/**************************************************************************
 *                                                                        *
 *  File:        AddProduct.cs                                            *
 *  Copyright:   (c) 2024, Asmarandei Catalin                             *
 *  Website:     https://github.com/DamirDenis-Tudor/BookStore.NET        *
 *  Description: The page where the provider can add new products         *
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
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Persistence.DTO.Product;
using PresentationProvider.Service;
using System.ComponentModel.DataAnnotations;

namespace PresentationProvider.Pages
{
    /// <summary>
    /// Add new product to the system
    /// </summary>
    public partial class AddProduct
    {
        /// <summary>
        /// The user login service for getting the token of the user
        /// </summary>
        [Inject]
        public IUserLoginService UserData { get; set; }
        /// <summary>
        /// The navigation manager for redirecting the user to the home page
        /// </summary>
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        /// <summary>
        /// The business facade singleton
        /// </summary>
        [Inject]
        public BusinessFacade Business { get; set; }

        /// <summary>
        /// The informations introduced by the provider for adding a new product
        /// </summary>
        public record ProductData
        {
            /// <summary>
            /// The name of the product
            /// </summary>
            [Required(ErrorMessage = "Completeaza campul \"Nume produs\""), MinLength(10, ErrorMessage = "Descrierea trebuie sa aiba minim 30 caractere")]
            public string Name { get; set; }
            /// <summary>
            /// The price of the product
            /// </summary>
            [Required(ErrorMessage = "Completeaza campul \"Pret\"")]
            public decimal Price { get; set; } = 1;
            /// <summary>
            /// The quantity of the product
            /// </summary>
            [Required(ErrorMessage = "Completeaza campul \"Cantitate\"")]
            public int Quantity { get; set; } = 1;
            /// <summary>
            /// The category of the product
            /// </summary>
            [Required(ErrorMessage = "Completeaza campul \"Categorie produs\""), MinLength(5, ErrorMessage = "Descrierea trebuie sa aiba minim 30 caractere")]
            public string Category { get; set; }
            [Required(ErrorMessage = "Completeaza campul \"Descriere produs\""), MinLength(5, ErrorMessage = "Descrierea trebuie sa aiba minim 50 caractere")]
            public string Description { get; set; }
            /// <summary>
            /// The link to the photo of the product
            /// </summary>
            public string? Photo { get; set; }

            /// <summary>
            /// Convert the object to a <see cref="ProductDto"/>
            /// </summary>
            /// <returns>The resulted object</returns>
            public ProductDto ConverToDto()
            {
                return new ProductDto()
                {
                    Name = Name,
                    Price = Price,
                    Quantity = Quantity,
                    Category = Category,
                    Description = Description,
					Link = Photo
				};
            }
        }

        /// <summary>
        /// The product that is mapped to the form
        /// </summary>
		ProductData product = new ProductData();

        /// <summary>
        /// Event called when the provider submit the form
        /// Validates the form and if it is valid, the product is added to the system
        /// If the process was successful the provider is redirected to the home page
        /// </summary>
        /// <param name="editContext"></param>
        /// <returns></returns>
        private async Task PlaceOrder (EditContext editContext)
        {
            if (editContext.Validate())
            {
                var sessionToken = await UserData.GetToken();
                if (sessionToken != null)
                {
                    var username = Business.AuthService.GetUsername(sessionToken);

					Business.InventoryService.RegisterProduct(username.SuccessValue, product.ConverToDto());
                    NavigationManager.NavigateTo("/home");
				}
            }
        }
    }
}

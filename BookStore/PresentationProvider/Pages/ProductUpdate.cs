/**************************************************************************
 *                                                                        *
 *  File:        ProductUpdate.cs                                                 *
 *  Copyright:   (c) 2024, Asmarandei Catalin                             *
 *  Website:     https://github.com/DamirDenis-Tudor/BookStore.NET        *
 *  Description: The page where the provider can edit the product price   *
 *  and availebl quantity                                                 *
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
using PresentationProvider.Entities;
using PresentationProvider.Service;
using System.ComponentModel.DataAnnotations;

namespace PresentationProvider.Pages
{
    /// <summary>
    /// Product edit page lets the provider to edit the product price and quantity and if there is any change notifys the server
    /// </summary>
    public partial class ProductUpdate
    {
        /// <summary>
        /// The navigation manager for redirecting the user to the home page
        /// </summary>
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        /// <summary>
        /// The business facade singleton
        /// </summary>
        [Inject]
        public BusinessFacade Business { get; set; }
        /// <summary>
        /// The user login service for getting the token of the user
        /// </summary>
        [Inject]
        public IUserLoginService UserData { get; set; }
        /// <summary>
        /// Utility class for getting the stored the products
        /// </summary>
        [Inject]
        public ProductsScope ProductsScope { get; set; }

        /// <summary>
        /// Product name that will be modified
        /// </summary>
        [SupplyParameterFromQuery(Name = "product")]
        public string ProductName { get; set; }

        /// <summary>
        /// The datas of the product for the form
        /// </summary>
        public record ProductData
        {
            /*[Required(ErrorMessage = "Completeaza campul \"Nume produs\""), MinLength(30, ErrorMessage = "Descrierea trebuie sa aiba minim 30 caractere")]
			public string Name { get; set; }*/
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
            /*[Required(ErrorMessage = "Completeaza campul \"Categorie produs\""), MinLength(30, ErrorMessage = "Descrierea trebuie sa aiba minim 30 caractere")]
			public string Category { get; set; }
			[Required(ErrorMessage = "Completeaza campul \"Descriere produs\""), MinLength(50, ErrorMessage = "Descrierea trebuie sa aiba minim 50 caractere")]
			public string Description { get; set; }
			public string? Photo { get; set; }*/

            /// <summary>
            /// Maps the product data from the <see cref="ProductDto"/>
            /// </summary>
            /// <param name="prod">The product that the properties will be mapped from</param>
            public void Deserialize(ProductDto prod)
            {
                /*Name = prod.Name;*/
                Price = prod.Price;
                Quantity = prod.Quantity;
                /*Category = prod.Category;
				Description = prod.Description;
				Photo = prod.Link;*/
            }

            /*public ProductDto ConverToDto()
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
			}*/
        }

        /// <summary>
        /// The product that is binded to the form for editing
        /// </summary>
        private ProductData _modifyProduct = new ProductData();

        /// <summary>
        /// On initialization searches for the product by using the product name from the query
        /// If the product name is invalid the provider is redirected to home page
        /// </summary>
        protected override void OnInitialized()
        {
            var result = ProductsScope.Products.Where(prod => prod.Name == ProductName.Replace("%0A", "\n")).FirstOrDefault();
            if (result.Equals(default))
                NavigationManager.NavigateTo("/home");
            else
                _modifyProduct.Deserialize(result);
        }

        /// <summary>
        /// Event called when the user submit the form
        /// Checkes if the datas form the form are valide
        /// If there is any change in price or quantity the server is notified with the new values
        /// </summary>
        /// <param name="editContext"></param>
        private async void Submit(EditContext editContext)
        {
            if (editContext.Validate())
            {
                var sessionToken = await UserData.GetToken();
                if (sessionToken != null)
                {
                    var username = Business.AuthService.GetUsername(sessionToken);
                    var remoteInfo = ProductsScope.Products.Where(prod => prod.Name == ProductName.Replace("%0A", "\n")).FirstOrDefault();
                    if (remoteInfo.Price != _modifyProduct.Price)
                        Business.InventoryService.UpdateProductPrice(username.SuccessValue, ProductName.Replace("%0A", "\n"), _modifyProduct.Price);

                    if (remoteInfo.Quantity != _modifyProduct.Quantity)
                        Business.InventoryService.UpdateProductStocks(username.SuccessValue, ProductName.Replace("%0A", "\n"), _modifyProduct.Quantity);
                    NavigationManager.NavigateTo("/home", true);
                }
            }
        }

        /// <summary>
        /// Checks if the product details are different between 2 objects
        /// </summary>
        /// <param name="remote">The product form the server, <see cref="ProductDto"/></param>
        /// <param name="local">The modified product, <see cref="ProductData"/></param>
        /// <returns></returns>
        private bool DifferenceBillDetails(ProductDto remote, ProductData local)
        {
            return remote == null ||
                remote.Quantity != local.Quantity ||
                remote.Price != local.Price;
        }
    }
}

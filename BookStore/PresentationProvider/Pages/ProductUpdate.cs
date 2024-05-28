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
using System.ComponentModel.DataAnnotations;
using PresentationProvider.Services;

namespace PresentationProvider.Pages;

/// <summary>
/// Product edit page lets the provider edit the product price and quantity, and if there is any change, notify the server
/// </summary>
public partial class ProductUpdate
{
    /// <summary>
    /// The navigation manager for redirecting the user to the home page
    /// </summary>
    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// The business facade singleton
    /// </summary>
    [Inject]
    public BusinessFacade Business { get; set; } = null!;

    /// <summary>
    /// The user login service for getting the token of the user
    /// </summary>
    [Inject]
    public IUserLoginService UserData { get; set; } = null!;

    /// <summary>
    /// Utility class for getting the stored the products
    /// </summary>
    [Inject]
    public ProductsScope ProductsScope { get; set; } = null!;

    /// <summary>
    /// Product name that will be modified
    /// </summary>
    [SupplyParameterFromQuery(Name = "product")]
    private string ProductName { get; set; } = null!;

    /// <summary>
    /// The datas of the product for the form
    /// </summary>
    public record ProductData
    {
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
        /// Maps the product data from the <see cref="ProductDto"/>
        /// </summary>
        /// <param name="prod">The product that the properties will be mapped from</param>
        public void Deserialize(ProductDto prod)
        {
            Price = prod.Price;
            Quantity = prod.Quantity;
        }
    }

    /// <summary>
    /// The product that is binded to the form for editing
    /// </summary>
    private readonly ProductData _modifyProduct = new();

    /// <summary>
    /// On initialization searches for the product by using the product name from the query
    /// If the product name is invalid, the provider is redirected to the home page
    /// </summary>
    protected override void OnInitialized()
    {
        var result = ProductsScope.Products
            .FirstOrDefault(prod => prod.Name == ProductName.Replace("%0A", "\n"));
        if (result != null && result.Equals(default))
            NavigationManager.NavigateTo("/home");
        else if (result != null) _modifyProduct.Deserialize(result);
    }

    /// <summary>
    /// Event called when the user submits the form
    /// Checks if the data's form the form are valide
    /// If there is any change in price or quantity the server is notified with the new values
    /// </summary>
    /// <param name="editContext"></param>
    private async void Submit(EditContext editContext)
    {
        if (!editContext.Validate()) return;

        var sessionToken = await UserData.GetToken();

        if (sessionToken == null) return;

        var username = Business.AuthService.GetUsername(sessionToken);

        var remoteInfo = ProductsScope.Products
            .FirstOrDefault(prod => prod.Name == ProductName.Replace("%0A", "\n"));

        if (remoteInfo != null && remoteInfo.Price != _modifyProduct.Price)
            Business.InventoryService.UpdateProductPrice(username.SuccessValue,
                ProductName.Replace("%0A", "\n"), _modifyProduct.Price);

        if (remoteInfo != null && remoteInfo.Quantity != _modifyProduct.Quantity)
            Business.InventoryService.UpdateProductStocks(username.SuccessValue,
                ProductName.Replace("%0A", "\n"), _modifyProduct.Quantity);

        NavigationManager.NavigateTo("/home", true);
    }
}
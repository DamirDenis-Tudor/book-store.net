/**************************************************************************
 *                                                                        *
 *  File:        PaymentDetails.cs                                        *
 *  Copyright:   (c) 2024, Asmarandei Catalin                             *
 *  Website:     https://github.com/DamirDenis-Tudor/BookStore.NET        *
 *  Description: The service for operating with the cart. The cart is     *
 *  stored in the local session store                                     *
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
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Persistence.DTO.Order;
using Persistence.DTO.Product;

namespace PresentationClient.Services;

/// <summary>
/// The cart is stored in the local session store
/// </summary>
public class CartServiceLocal(ProtectedLocalStorage localStorage, BusinessFacade businessFacade) : ICartService
{

    
    private async Task<string> GetUsername()
    {
        var token = await localStorage.GetAsync<string>("sessiontoken");
        if (!token.Success) return "";

        var username = businessFacade.AuthService.GetUsername(token.Value);
        return username.IsSuccess ? username.SuccessValue : "";
    }
    
    public async Task<List<OrderProductData>> GetCart()
    {
          
        var username = await GetUsername();
        if (username == "") return [];
        
        var result = await localStorage.GetAsync<List<OrderProductData>>($"cart{username}");
        return (!result.Success || result.Value == null) ? [] : result.Value;
    }
    
    public async Task AddToCart(ProductDto? product)
    {
        if (product == null) return;
        
        var username = await GetUsername();
        if (username == "") return;
        
        var result = await localStorage.GetAsync<List<OrderProductData>>($"cart{username}");

        List<OrderProductData> products;

        if (!result.Success || result.Value == null)
            products = [];
        else
            products = result.Value;

        var found = false;
        products.ForEach(prod =>
        {
            if (prod.Product != product) return;
            if(prod.OrderQuantity + 1 <= prod.Product.Quantity)
                prod.OrderQuantity += 1;
            found = true;
        });

        if (!found)
        {
            var orderProductDto = new OrderProductData
            {
                Product= product, OrderQuantity = 1
            };
            products.Add(orderProductDto);
        }
        await localStorage.SetAsync($"cart{username}", products);
    }

    /// <summary>
    /// The product that will be changed with the new one is matched by product name
    /// </summary>
    /// <param name="newProduct">The newer version of the product</param>
    public async void UpdateProduct(OrderProductData newProduct)
    {
        var products = await GetCart();

        var index = products.FindIndex(prod => prod.Product == newProduct.Product);

        products[index] = newProduct;
        
        var username = await GetUsername();
        if (username == "") return;

        await localStorage.SetAsync($"cart{username}", products);
    }

    public async void DeleteProduct(OrderProductData newProduct)
    {
        var products = await GetCart();

        var index = products.FindIndex(prod => prod.Product == newProduct.Product);

        products.RemoveAt(index);
        
        var username = await GetUsername();
        if (username == "") return;

        await localStorage.SetAsync($"cart{username}", products);
    }

    public async void ClearCart()
    {
        var username = await GetUsername();
        if (username == "") return;
        await localStorage.DeleteAsync($"cart{username}");
    }
}
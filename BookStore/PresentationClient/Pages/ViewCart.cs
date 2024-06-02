/**************************************************************************
 *                                                                        *
 *  File:        PaymentDetails.cs                                        *
 *  Copyright:   (c) 2024, Asmarandei Catalin                             *
 *  Website:     https://github.com/DamirDenis-Tudor/BookStore.NET        *
 *  Description: The page where the user can view his cart, modify it     *
 *      and proceed to place the order                                    *
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
using System.Collections.ObjectModel;

namespace PresentationClient.Pages;

/// <summary>
/// ViewCart, the cart is displayed, can be modified, see information about the costs and the user can proceed to place the order
/// </summary>
public partial class ViewCart
{
    /// <summary>
    /// The cart service for operating with the user cart
    /// </summary>
    [Inject]
    private ICartService CartService { get; set; } = null!;

    /// <summary>
    /// The displayed products on the page from cart
    /// </summary>
    private ObservableCollection<OrderProductData> Products { get; set; } = null!;

    /// <summary>
    /// Initialize the product list empty until the datas is loaded
    /// In this case on the page will appear some dummy products
    /// </summary>
    protected override void OnInitialized() => Products = [];

    /// <summary>
    /// If the datas are loaded from the cart
    /// </summary>
    private bool _isDataLoaded;

    /// <summary>
    /// If there is no product in the cart
    /// </summary>
    private bool _cartEmpty = true;

    /// <summary>
    /// The total price of the products
    /// </summary>
    private decimal ProductsTotalPrice { get; set; }

    /// <summary>
    /// The total price of the order, with taxes and delivery fee
    /// </summary>
    private decimal TotalPrice { get; set; }

    /// <summary>
    /// The normal delivery fee for an order
    /// </summary>
    private const decimal DeliveryFeeForOrder = 11.99m;

    /// <summary>
    /// The delivery fee applyed to the order
    /// </summary>
    private decimal _deliveryFee;

    /// <summary>
    /// Fetches the products form the cart updates the price and the flags.
    /// The dummy products will be replaced on the page
    /// <param name="firstRender">If the page is rendered for the first time</param>
    /// <returns>Async task</returns>
    /// </summary>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!(firstRender && !_isDataLoaded)) return;

        var cart = await CartService.GetCart();
        if (cart.Count == 0) return;

        cart.ForEach(Products.Add);

        UpdateTotalPrices();
        _isDataLoaded = cart.Count != 0;
        _cartEmpty = !Products.Any();

        StateHasChanged();
    }

    /// <summary>
    /// Calculates the prices of the products and the total price of the order
    /// </summary>
    private void UpdateTotalPrices()
    {
        ProductsTotalPrice = Products.Sum(prod => prod.Product.Price * prod.OrderQuantity);
        if (ProductsTotalPrice != 0 && ProductsTotalPrice < 300)
            _deliveryFee = DeliveryFeeForOrder;
        else
            _deliveryFee = 0;
        TotalPrice = ProductsTotalPrice + _deliveryFee;
    }

    /// <summary>
    /// Refreshes when the user modifies the quantity of a product in cart
    /// </summary>
    private void RefreshView()
    {
        UpdateTotalPrices();
        Products = new ObservableCollection<OrderProductData>(Products.Where(prod => prod.OrderQuantity > 0));
        _cartEmpty = !Products.Any();
        StateHasChanged();
    }
}
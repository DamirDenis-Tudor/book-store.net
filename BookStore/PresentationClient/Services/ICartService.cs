/**************************************************************************
 *                                                                        *
 *  File:        PaymentDetails.cs                                        *
 *  Copyright:   (c) 2024, Asmarandei Catalin                             *
 *  Website:     https://github.com/DamirDenis-Tudor/BookStore.NET        *
 *  Description: Used for operating with the products cart                *
 *                                                                        *
 *  This program is free software; you can redistribute it and/or modify  *
 *  it under the terms of the GNU General Public License as published by  *
 *  the Free Software Foundation. This program is distributed in the      *
 *  hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 *  the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 *  PURPOSE. See the GNU General Public License for more details.         *
 *                                                                        *
 **************************************************************************/


using Persistence.DTO.Order;
using Persistence.DTO.Product;

namespace PresentationClient.Services;

/// <summary>
/// The cart service for operating with the user cart
/// </summary>
public interface ICartService
{
    /// <summary>
    /// Get all the products from the cart
    /// <see cref="OrderProductData"/>
    /// </summary>
    /// <returns>The list of products from the cart</returns>
    Task<List<OrderProductData>> GetCart();

    /// <summary>
    /// Add a product to the cart
    /// </summary>
    /// <param name="product">The product that will be added</param>
    /// <returns></returns>
    Task AddToCart(ProductDto? product);

    /// <summary>
    /// Update a product from the cart with a newer version
    /// </summary>
    /// <param name="newProduct">The newer version of the product</param>
    void UpdateProduct(OrderProductData newProduct);

    /// <summary>
    /// Delete a product from the cart
    /// </summary>
    /// <param name="newProduct"></param>
    void DeleteProduct(OrderProductData newProduct);

    /// <summary>
    /// Delete all the products from the cart
    /// </summary>
    void ClearCart();
}
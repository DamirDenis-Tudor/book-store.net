/**************************************************************************
 *                                                                        *
 *  File:        OrderProductDatas.cs                                     *
 *  Copyright:   (c) 2024, Asmarandei Catalin                             *
 *  Website:     https://github.com/DamirDenis-Tudor/BookStore.NET        *
 *  Description: Used for the datas of products in order                  *
 *                                                                        *
 *  This program is free software; you can redistribute it and/or modify  *
 *  it under the terms of the GNU General Public License as published by  *
 *  the Free Software Foundation. This program is distributed in the      *
 *  hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 *  the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 *  PURPOSE. See the GNU General Public License for more details.         *
 *                                                                        *
 **************************************************************************/

namespace Persistence.DTO.Order;

/// <summary>
/// Used for the datas of products in order
/// </summary>
public record OrderProductData
{
    /// <summary>
    /// The product name
    /// </summary>
    public required string ProductName { get; init; }
    /// <summary>
    /// The product description
    /// </summary>
    public required string Description { get; init; }
    /// <summary>
    /// The product price
    /// </summary>
    public decimal Price { get; init; }
    /// <summary>
    /// The product quantity
    /// </summary>
    public required int OrderQuantity { get; set; }
    /// <summary>
    /// Link to the product photo
    /// </summary>
    public string? Link { get; init; }
}
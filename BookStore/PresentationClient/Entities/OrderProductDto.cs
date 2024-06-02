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

using Persistence.DTO.Product;

namespace Persistence.DTO.Order;

/// <summary>
/// Used for the datas of products in order
/// </summary>
public record OrderProductData
{
    /// <summary>
    /// Product ordered
    /// </summary>
    public required ProductDto Product { get; init; }
    /// <summary>
    /// The product quantity
    /// </summary>
    public required int OrderQuantity { get; set; }
}
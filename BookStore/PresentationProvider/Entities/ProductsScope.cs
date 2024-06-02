﻿/**************************************************************************
 *                                                                        *
 *  File:        ProductsScope.cs                                         *
 *  Copyright:   (c) 2024, Asmarandei Catalin                             *
 *  Website:     https://github.com/DamirDenis-Tudor/BookStore.NET        *
 *  Description: Utitly singleton class used for getting the products     *
 *  from the server and their categories                                  *
 *  order                                                                 *
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
using Persistence.DTO.Product;
using Presentation.Entities;

namespace PresentationProvider.Entities
{
    /// <summary>
    /// Utitly singleton class used for getting the products from the server and their categories
    /// </summary>
    public class ProductsScope : IProductsScope
	{
		public IList<ProductDto> GetProducts() => BusinessFacade.Instance.InventoryService.GetInventory().SuccessValue;

		public IList<string> GetCategories() => GetProducts().Select(prod => prod.ProductInfoDto.Category).Distinct().ToList();
	}
}
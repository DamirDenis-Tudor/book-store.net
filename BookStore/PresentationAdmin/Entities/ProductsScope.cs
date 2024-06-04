/**************************************************************************
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

namespace PresentationAdmin.Entities
{
	/// <summary>
	/// Utitly singleton class used for getting the products from the server and their categories
	/// </summary>
	public class ProductsScope : IProductsScope
    {
		/// <summary>
		/// The list of product statistics fatched from the server
		/// </summary>
		public IList<ProductDto> GetProducts()
		{
			var result = BusinessFacade.Instance.InventoryService.GetInventoryStats();
			if (!result.IsSuccess) return new List<ProductDto>();
			return result.SuccessValue.Select(DataObjMapper.ConvertToProductDto).ToList();
		}
		/// <summary>
		/// Returns the list of all the categories for the products
		/// </summary>
		/// <returns>All the categories of the products</returns>
		public IList<string> GetCategories() { if (!GetProducts().Any()) return new List<string>(); return GetProducts().Select(prod => prod.ProductInfoDto.Category).Distinct().ToList(); }

	}
}
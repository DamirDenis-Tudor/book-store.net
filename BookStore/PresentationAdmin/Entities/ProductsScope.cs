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

namespace PresentationAdmin.Entities
{
    /// <summary>
    /// Utitly singleton class used for getting the products from the server and their categories
    /// </summary>
    public class ProductsScope
    {
        /// <summary>
        /// The list of product statistics fatched from the server
        /// </summary>
        public IList<ProductStatsDto> Products
        {
            get { return BusinessFacade.Instance.InventoryService.GetInventoryStats().SuccessValue; }
            set { Products = value; }
        }

        //public IList<ProductStatsDto> Products { get { return BusinessFacade.Instance.InventoryService.GetInventoryStats(await _userData.GetUsername()).SuccessValue; } set; }
        
        /// <summary>
        /// Returns the list of all the categories for the products
        /// </summary>
        /// <returns>All the categories of the products</returns>
        public IEnumerable<string> Categories()
        {
            var cat = Products.Select(item => item.ProductDto.Category).ToList();
            return cat.Distinct().ToList();
        }
    }
}
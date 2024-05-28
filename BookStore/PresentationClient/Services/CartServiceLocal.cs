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


using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using Persistence.DTO.Order;
using Persistence.DTO.Product;

namespace PresentationClient.Services
{
    /// <summary>
    /// The cart is stored in the local session store
    /// </summary>
    public class CartServiceLocal : ICartService
    {
		private readonly ProtectedLocalStorage _localStorage;

		public CartServiceLocal(ProtectedLocalStorage localStorage)
		{
			_localStorage = localStorage;
		}

		public async Task<List<OrderProductData>> GetCart()
		{
			var result = await _localStorage.GetAsync<List<OrderProductData>>("cart");
			return (!result.Success || result.Value == null) ? new List<OrderProductData>() : result.Value;
		}

		public async Task AddToCart(ProductDto product)
		{
			if (product != null)
			{
				var result = await _localStorage.GetAsync<List<OrderProductData>>("cart");
				List<OrderProductData> products;
				if (!result.Success || result.Value == null)
					products = new List<OrderProductData>();
				else
				{
					products = result.Value;

				}
				bool found = false;
				products.ForEach(prod => { if (prod.ProductName == product.Name) { prod.OrderQuantity = prod.OrderQuantity + 1; found = true; } });

				if (!found)
				{
					OrderProductData orderProductDto = new OrderProductData() { ProductName = product.Name, Description = product.Description, Link = product.Link, Price = product.Price, OrderQuantity = 1 };
					products.Add(orderProductDto);
				}
				products.ForEach(Console.WriteLine);
				await _localStorage.SetAsync("cart", products);
			}
		}
        /// <summary>
        /// The product that will be changed with the new one are matched by product name
        /// </summary>
        /// <param name="newProduct">The newer version of the product</param>
        public async void UpdateProduct(OrderProductData newProduct)
		{
			List<OrderProductData> products = await GetCart();
			int index = products.FindIndex(prod => prod.ProductName == newProduct.ProductName);
			products[index] = newProduct;

			await _localStorage.SetAsync("cart", products);
		}
		public async void DeleteProduct(OrderProductData newProduct)
		{
			List<OrderProductData> products = await GetCart();
			int index = products.FindIndex(prod => prod.ProductName == newProduct.ProductName);
			products.RemoveAt(index);

			await _localStorage.SetAsync("cart", products);
		}

		public async void ClearCart()
		{
			await _localStorage.DeleteAsync("cart");
		}

	}
}

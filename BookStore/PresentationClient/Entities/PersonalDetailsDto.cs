/**************************************************************************
 *                                                                        *
 *  File:        PersonalDetailsDto.cs                                    *
 *  Copyright:   (c) 2024, Asmarandei Catalin                             *
 *  Website:     https://github.com/DamirDenis-Tudor/BookStore.NET        *
 *  Description: Used for user personal details datas for                 * 
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

using Persistence.DTO.Bill;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Entities
{
    /// <summary>
    /// Used for user personal details datas for order
    /// </summary>
    public class PersonalDetailsDto
	{
        /// <summary>
        /// The user address
        /// </summary>
        [Required]
		public string Address { get; set; }
        /// <summary>
        /// The user telephone number
        /// </summary>
        [Required]
		public string Telephone { get; set; }
        /// <summary>
        /// The user country
        /// </summary>
        [Required]
		public string Country { get; set; }
        /// <summary>
        /// The user city
        /// </summary>
        [Required]
		public string City { get; set; }
        /// <summary>
        /// The user postal code
        /// </summary>
		[Required]
		public string PostalCode { get; set; }

        /// <summary>
        /// Mappes the object parameters to a <see cref="BillDto"/> object
        /// </summary>
        /// <returns>The resulted object</returns>
		public void Deserialize(BillDto bill)
		{
			Address = bill.Address;
			Telephone = bill.Telephone;
			Country = bill.Country;
			City = bill.City;
			PostalCode = bill.PostalCode;
		}
	}
}

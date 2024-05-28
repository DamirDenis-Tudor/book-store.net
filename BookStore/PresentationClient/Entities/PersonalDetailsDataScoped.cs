/**************************************************************************
 *                                                                        *
 *  File:        PersonalDetailsDataScoped.cs                             *
 *  Copyright:   (c) 2024, Asmarandei Catalin                             *
 *  Website:     https://github.com/DamirDenis-Tudor/BookStore.NET        *
 *  Description: Used for persisting the user personal details for        * 
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

namespace PresentationClient.Entities
{
    /// <summary>
    /// Used for persisting the user personal details for order
    /// </summary>
    public class PersonalDetailsDataScoped
	{
        /// <summary>
        /// The user address
        /// </summary>
        [Required]
		public string? Address { get; set; } = null;
        /// <summary>
        /// The user telephone number
        /// </summary>
        [Required]
		public string? Telephone { get; set; } = null;
        /// <summary>
        /// The user country
        /// </summary>
        [Required]
		public string? Country { get; set; } = null;
        /// <summary>
        /// The user city
        /// </summary>
        [Required]
		public string? City { get; set; } = null;
        /// <summary>
        /// The user postal code
        /// </summary>
        [Required]
		public string? PostalCode { get; set; } = null;
        /// <summary>
        /// Validation by checking if all the parameters are field, if not, returns false
        /// </summary>
        /// <returns>If the personal details are valid</returns>
		public bool IsValid()
		{
			return Address != null && Telephone != null && Country != null && City != null && PostalCode != null;
		}

        /// <summary>
        /// Mappes the object parameters to a <see cref="BillDto"/> object
        /// </summary>
        /// <returns>The resulted object</returns>
		public BillDto ConvertToDto()
		{
			return new BillDto() {
				Address = Address ?? "",
				Country = Country ?? "",
				City = City ?? "",
				PostalCode = PostalCode ?? "",
				Telephone = Telephone ?? ""
			};
		}
	}
}

/**************************************************************************
 *                                                                        *
 *  File:        UserLogin.cs                                        *
 *  Copyright:   (c) 2024, Asmarandei Catalin                             *
 *  Website:     https://github.com/DamirDenis-Tudor/BookStore.NET        *
 *  Description: Data object used for user login                          *
 *                                                                        *
 *  This program is free software; you can redistribute it and/or modify  *
 *  it under the terms of the GNU General Public License as published by  *
 *  the Free Software Foundation. This program is distributed in the      *
 *  hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 *  the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 *  PURPOSE. See the GNU General Public License for more details.         *
 *                                                                        *
 **************************************************************************/


using System.ComponentModel.DataAnnotations;
using Business.BTO;
using Presentation.Services;

namespace Presentation.Entities
{
    /// <summary>
    /// Used for user login informations
    /// </summary>
    public class UserLogin
    {
	    /// <summary>
	    /// User username
	    /// </summary>
	    [Required(ErrorMessage = "Introduceti username-ul contului")]
	    public string Username { get; set; } = null!;

		/// <summary>
		/// User password
		/// </summary>
		[Required(ErrorMessage = "Introduceti parola contului")]
		public string Password { get; set; } = null!;

        /// <summary>
        /// Mapps the object properties to a <see cref="UserLoginBto"/> object
        /// </summary>
        /// <returns>The generated BTO object</returns>
        public UserLoginBto ConverToBto()
		{
			return new UserLoginBto() { Username = Sanitizer.SanitizeString(Username), Password = Sanitizer.SanitizeString(Password) };
		}
	}
}

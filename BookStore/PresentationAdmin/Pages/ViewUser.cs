/**************************************************************************
 *                                                                        *
 *  File:        PaymentDetails.cs                                        *
 *  Copyright:   (c) 2024, Asmarandei Catalin                             *
 *  Website:     https://github.com/DamirDenis-Tudor/BookStore.NET        *
 *  Description: The page where the admin can view all the users and      *
 *      delete them                                                       *
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
using Common;
using Microsoft.AspNetCore.Components;
using Persistence.DTO.User;
using PresentationAdmin.Service;

namespace PresentationAdmin.Pages
{
    /// <summary>
    /// The admin can view all the users registred to the platform and delete them
    /// </summary>
    public partial class ViewUser
    {
        /// <summary>
        /// Business singleton instance
        /// </summary>
		[Inject]
		public BusinessFacade Business { get; set; }
        /// <summary>
        /// The user login datas
        /// </summary>
		[Inject]
		public IUserLoginService UserData { get; set; }

        /// <summary>
        /// A list with all the users that are registred to the platform
        /// In case of empty list the page will display some dummy user cards
        /// </summary>
		private IList<UserInfoDto> Users {
            get {
                var result = Business.UsersService.GetAllUsers("admin_12345");
                if (!result.IsSuccess)
                {
                    Logger.Instance.GetLogger<ViewUser>().LogError(result.Message);
                    return new List<UserInfoDto>();
				}
                else
                    return result.SuccessValue;
            } 
        }
    }
}

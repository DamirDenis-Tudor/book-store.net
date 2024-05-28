/**************************************************************************
 *                                                                        *
 *  File:        UserView.cs                                              *
 *  Copyright:   (c) 2024, Asmarandei Catalin                             *
 *  Website:     https://github.com/DamirDenis-Tudor/BookStore.NET        *
 *  Description: Component that will be displayed for every user          *
 *      registred                                                         *
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

namespace PresentationAdmin.Shared
{
    /// <summary>
    /// Used for displaying the user information and deleting them by admin
    public partial class UserView
    {
        /// <summary>
        /// The user received from the parent page
        /// </summary>
        [Parameter]
        public UserInfoDto User { get; set; }

        /// <summary>
        /// The user service for operating with the user data
        /// </summary>
        [Inject]
        public IUserLoginService UserData { get; set; }

        /// <summary>
        /// The business facade singleton for operating with the business logic
        /// </summary>
        [Inject]
        public BusinessFacade Business { get; set; }
        /// <summary>
        /// The navigation manager for redirecting the user
        /// </summary>
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Called when the admin wants to delete the user
        /// Deletes the user and refreshes the page
        /// </summary>
        public async void Delete()
        {
            var username = Business.AuthService.GetUsername(await UserData.GetToken());
            if (!username.IsSuccess)
                Logger.Instance.GetLogger<UserView>().LogError(username.Message);
            var result = Business.UsersService.DeleteUser(username.SuccessValue, User.Username);
            if (!result.IsSuccess)
                Logger.Instance.GetLogger<UserView>().LogError(result.Message);
            else
                NavigationManager.NavigateTo("/users", true);

        }
    }
}

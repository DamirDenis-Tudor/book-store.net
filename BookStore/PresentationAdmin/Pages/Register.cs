/**************************************************************************
 *                                                                        *
 *  File:        Registers.cs                                             *
 *  Copyright:   (c) 2024, Asmarandei Catalin                             *
 *  Website:     https://github.com/DamirDenis-Tudor/BookStore.NET        *
 *  Description: The page where the admin can register new providers      *
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
using Microsoft.AspNetCore.Components.Forms;
using Persistence.DTO.User;
using PresentationAdmin.Service;
using System.ComponentModel.DataAnnotations;

namespace PresentationAdmin.Pages
{
    /// <summary>
    /// Takes the details given by the admin, validates them and registers a new provider
    /// </summary>
    public partial class Register
    {
        /// <summary>
        /// The navigation manager for redirecting the user
        /// </summary>
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        /// <summary>
        /// The business facade singleton
        /// </summary>
        [Inject]
        public BusinessFacade Business { get; set; }
        /// <summary>
        /// The user login service for storing the token of the user
        /// </summary>
        [Inject]
        public IUserLoginService UserData { get; set; }

        /// <summary>
        /// The details used for creating a provider
        /// </summary>
        public class UserInfoData
        {
            /// <summary>
            /// The first name of the new user
            /// </summary>
            [Required]
            public string FirstName { get; set; }
            /// <summary>
            /// The last name of the new user
            /// </summary>
            [Required]
            public string LastName { get; set; }
            /// <summary>
            /// The username of the new user
            /// </summary>
            [Required]
            public string Username { get; set; }
            /// <summary>
            /// The password of the new user
            /// </summary>
            [Required]
            public string Password { get; set; }
            /// <summary>
            /// The email of the new user
            /// </summary>
            [Required]
            public string Email { get; set; }
        }

        /// <summary>
        /// The details of the new user that are mapped to the form
        /// </summary>
        UserInfoData user = new UserInfoData();

        /// <summary>
        /// Event called when the admin submit the register form
        /// If the information entered are vaild the user is registered and redirects the admin to the home page
        /// </summary>
        /// <param name="editContext">Edit contex of the form</param>
        private async void RegisterSubmit(EditContext editContext)
        {
            if (editContext.Validate())
            {
                var username = Business.AuthService.GetUsername(await UserData.GetToken());
                var result = Business.UsersService.RegisterProvider(username.SuccessValue, new UserRegisterDto()
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = user.Password,
                    Username = user.Username,
                    UserType = "PROVIDER"
                });
                if (!result.IsSuccess)
                    Logger.Instance.GetLogger<Register>().LogError(result.Message);
                else
                    NavigationManager.NavigateTo("/home");
            }
        }
    }
}

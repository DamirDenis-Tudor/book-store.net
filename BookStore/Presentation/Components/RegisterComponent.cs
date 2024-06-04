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
using Business.BAO;
using Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Persistence.DTO.User;
using Presentation.Services;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Components
{
    /// <summary>
    /// Takes the details given by the admin, validates them and registers a new provider
    /// </summary>
    public partial class RegisterComponent
    {
        /// <summary>
        /// The type of user that will be registered
        /// </summary>
        [Parameter]
        public LoginMode LoginMode { get; set; }
        /// <summary>
        /// The navigation manager for redirecting the user
        /// </summary>
        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// The business facade singleton
        /// </summary>
        [Inject]
        public BusinessFacade Business { get; set; } = null!;

        /// <summary>
        /// The user login service for storing the token of the user
        /// </summary>
        [Inject]
        public IUserLoginService UserData { get; set; } = null!;

		/// <summary>
		/// The error message that will be displayed if the loggin fails
		/// </summary>
		private string _loggingError = "";
		/// <summary>
		/// The success message that will be displayed if the loggin is successful
		/// </summary>
		private string _loggingSuccess = "";

		/// <summary>
		/// The details used for creating a provider
		/// </summary>
		public class UserInfoData
        {
            /// <summary>
            /// The first name of the new user
            /// </summary>
            [Required]
            public string FirstName { get; set; } = null!;

            /// <summary>
            /// The last name of the new user
            /// </summary>
            [Required]
            public string LastName { get; set; } = null!;

            /// <summary>
            /// The username of the new user
            /// </summary>
            [Required]
            public string Username { get; set; } = null!;

            /// <summary>
            /// The password of the new user
            /// </summary>
            [Required]
            public string Password { get; set; } = null!;

            /// <summary>
            /// The email of the new user
            /// </summary>
            [Required]
            public string Email { get; set; } = null!;

            public UserRegisterDto ConvertToDto(LoginMode userType)
            {
                string type = "CLIENT";
                if(userType == LoginMode.Provider)
                    type = "PROVIDER";
                return new UserRegisterDto()
                {
                    Email = Sanitizer.SanitizeString(Email),
                    FirstName = Sanitizer.SanitizeString(FirstName),
                    LastName = Sanitizer.SanitizeString(LastName),
                    Password = Sanitizer.SanitizeString(Password),
                    Username = Sanitizer.SanitizeString(Username),
                    UserType = type
                };
            }
        }

        /// <summary>
        /// The details of the new user that are mapped to the form
        /// </summary>
        private UserInfoData _user = new();

        /// <summary>
        /// Event called when the admin submit the register form
        /// If the information entered are vaild the user is registered and redirects the admin to the home page
        /// </summary>
        /// <param name="editContext">Edit contex of the form</param>
        private async void RegisterSubmit(EditContext editContext)
        {
            if (!editContext.Validate()) return;

            var username = Business.AuthService.GetUsername(await UserData.GetToken());

            Result<VoidResult, BaoErrorType> result = null!;

            if (LoginMode == LoginMode.Provider)
            {
                result = Business.UsersService.RegisterProvider(username.SuccessValue, _user.ConvertToDto(LoginMode));
            }
            if (LoginMode == LoginMode.Client)
            {
                result = Business.UsersService.RegisterClient(_user.ConvertToDto(LoginMode));
            }

            if (!result.IsSuccess)
            {
                Logger.Instance.GetLogger<RegisterComponent>().LogError(result.Message);
                _loggingError = result.Message;
            }
            else
            {
                _loggingSuccess = result.Message;
                NavigationManager.NavigateTo("/home");
            }
        }

		/// <summary>
		/// If the user has a invalidation message of the loggin and he changes the value of the field the message will be cleared
		/// </summary>
		/// <param name="sender">The form</param>
		/// <param name="e">The event raised for field changed</param>
		private void OnFieldChange(object? sender, FieldChangedEventArgs e)
		{
			_loggingError = "";
			_loggingSuccess = "";
		}
	}
}

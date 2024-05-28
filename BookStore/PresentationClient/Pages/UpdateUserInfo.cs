/**************************************************************************
 *                                                                        *
 *  File:        PaymentDetails.cs                                        *
 *  Copyright:   (c) 2024, Asmarandei Catalin                             *
 *  Website:     https://github.com/DamirDenis-Tudor/BookStore.NET        *
 *  Description: The page where the user can modify his account           *
 *      informations                                                      *
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
using Persistence.DAL;
using Persistence.DTO.Bill;
using Persistence.DTO.User;
using PresentationClient.Service;
using System.ComponentModel.DataAnnotations;

namespace PresentationClient.Pages
{
    /// <summary>
    /// Take the infroamtions about the account from the database, check for changes and update them
    /// </summary>
	public partial class UpdateUserInfo
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
        /// The user login service for getting the token of the user
        /// </summary>
        [Inject]
        public IUserLoginService UserData { get; set; }

        /// <summary>
        /// The account informations that the user will get in the form
        /// </summary>
        public class UserInfoData
        {
            [Required]
            public string FirstName { get; set; }
            [Required]
            public string LastName { get; set; }
			[Required, MinLength(5, ErrorMessage = "Username-ul trebuie sa fie mai lung de 5 caractere")]
			public string Username { get; set; }
            [Required, MinLength(3, ErrorMessage = "Parola trebuie sa fie mai lunga de 3 caractere")]
            public string Password { get; set; }
            [Required]
            public string Email { get; set; }

            /// <summary>
            /// Mappes the user informations from an DTO to the object properties
            /// <see cref="UserInfoDto"/>
            /// </summary>
            /// <param name="dto">The DTO object that will be mapped from</param>
            public void Deserialize(UserInfoDto dto)
            {
                FirstName = dto.FirstName;
				LastName = dto.LastName;
				Username = dto.Username;
				Email = dto.Email;
            }
            /// <summary>
            /// Mappes the user informations from the object properties to a DTO
            /// </summary>
            /// <returns>The resulted <see cref="UserInfoDto"/> object</returns>
            public UserRegisterDto ConverToDto()
            {
                return new UserRegisterDto()
                { Email = Email, FirstName = FirstName, LastName = LastName, Password = Password, Username = Username, UserType="CLIENT" };
            }
		}

        /// <summary>
        /// The user informations that are mapped to the form
        /// </summary>
        private UserInfoData _user = new UserInfoData();

        /// <summary>
        /// Gets the user informations from the database and mappes them to the form
        /// </summary>
        /// <param name="firstRender">If the page is rendered for the first time</param>
        /// <returns>Async Task</returns>
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				var sessionToken = await UserData.GetToken();
				if (sessionToken != null)
				{
					var username = Business.AuthService.GetUsername(sessionToken);
					if (username.IsSuccess)
					{
						var result = Business.UsersService.GetUserInfo(username.SuccessValue);
						if (result.IsSuccess)
						{
							_user.Deserialize(result.SuccessValue);
							StateHasChanged();
						}
					}
				}
			}
		}

        /// <summary>
        /// Event called when the user submit the update account inforamtion form
        /// If there is any difference between the informations stored in the database and the ones entered by the user, 
        /// the informations are updated.
        /// The user is logged out(in case he updates his username or password) and redirected to the loggin page
        /// </summary>
        /// <param name="editContext"></param>
		private async void RegisterSubmit(EditContext editContext)
        {
            if (editContext.Validate())
            {
				var sessionToken = await UserData.GetToken();
                if (sessionToken != null)
                {
                    var username = Business.AuthService.GetUsername(sessionToken);
                    var remoteInfo = Business.UsersService.GetUserInfo(username.SuccessValue);
                    if (DifferenceBillDetails(remoteInfo.SuccessValue, _user))
                    {
                        Business.UsersService.UpdateUser(username.SuccessValue, _user.ConverToDto());
                        Business.AuthService.Logout(sessionToken);
                        NavigationManager.NavigateTo("/", true);
					}
				}
            }
        }

        /// <summary>
        /// Checks if two <see cref="UserInfoDto"/> objects have different between informations
        /// </summary>
        /// <param name="remote">The first object for check</param>
        /// <param name="local">The secound object for check</param>
        /// <returns>true If there is any difference</returns>
		private bool DifferenceBillDetails(UserInfoDto remote, UserInfoData local)
		{
            return remote == null || remote.Email != local.FirstName ||
                remote.FirstName != local.FirstName ||
                remote.LastName != local.LastName ||
                remote.Username != local.Username;
		}
	}
}

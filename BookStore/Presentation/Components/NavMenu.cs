/**************************************************************************
 *                                                                        *
 *  File:        NavMenu.cs                                               *
 *  Copyright:   (c) 2024, Asmarandei Catalin                             *
 *  Website:     https://github.com/DamirDenis-Tudor/BookStore.NET        *
 *  Description: Header navigation bar that appears on every page of the  * 
 *  single page aplication                                                *
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
using Microsoft.Extensions.Logging;
using Presentation.Services;

namespace Presentation.Components
{
    /// <summary>
    /// The logic of the navigation bar for logging out the user and searching for products by name
    /// </summary>
    public partial class NavMenu
    {
        [Parameter]
        public bool ShowCart { get; set; } = false;

        [Parameter]
        public bool ShowDashboard { get; set; } = false;
        [Parameter]
        public bool ShowAddProduct { get; set; } = false;

        /// <summary>
        /// Inject the navigation manager for redirecting the user to the login page
        /// </summary>
        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// Inject the user login service for getting the token of the user
        /// </summary>
        [Inject]
        private IUserLoginService UserData { get; set; } = null!;

        /// <summary>
        /// Inject the business layer singleton
        /// </summary>
        [Inject]
        public BusinessFacade Business { get; set; } = null!;

        /// <summary>
        /// The name of the product that the user is searching for
        /// </summary>
        private string? _search;
        
        /// <summary>
        /// When there is a value setted the user is redirected to the home page with a html query with the search value
        /// </summary>
        private string? Serach
        {
            get => _search;
            set
            {
                _search = value;
                if (_search != null)
                    _search = Sanitizer.SanitizeString(_search);
                NavigationManager.NavigateTo($"/?search={_search}");
            }
        }

        /// <summary>
        /// If the user is validly logged in for displaying the acourding action buttons
        /// </summary>
        private bool _loggedIn = true;

        /// <summary>
        /// Performing the logout action for the user
        /// Logging out the user forms the business layer if it was successful then clear the session token and redirect to login page
        /// </summary>
        public async void Logout()
        {
            var result = Business.AuthService.Logout(await UserData.GetToken());
            switch (result.IsSuccess)
            {
                case false:
                    Logger.Instance.GetLogger<NavMenu>().LogError(result.Message);
                    break;
                case true:
                    UserData.ClearSession();
                    _loggedIn = false;
                    Logger.Instance.GetLogger<NavMenu>().LogError(result.Message);

                    NavigationManager.NavigateTo("/login", true);
                    break;
            }
        }
    }
}

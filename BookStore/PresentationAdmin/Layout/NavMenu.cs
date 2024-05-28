﻿/**************************************************************************
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


using Microsoft.AspNetCore.Components;
using Business.BAL;
using Common;
using PresentationAdmin.Service;

namespace PresentationAdmin.Layout
{
    /// <summary>
    /// The logic of the navigation bar for logging out the user and searching for products by name
    /// </summary>
    public partial class NavMenu
    {
        /// <summary>
        /// Inject the navigation manager for redirecting the user to the login page
        /// </summary>
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        /// <summary>
        /// Inject the user login service for getting the token of the user
        /// </summary>
        [Inject]
        private IUserLoginService UserData { get; set; }
        /// <summary>
        /// Inject the business layer singleton
        /// </summary>
		[Inject]
		public BusinessFacade Business { get; set; }

        /// <summary>
        /// The name of the product that the user is searching for
        /// </summary>
		private string? _search;
        /// <summary>
        /// When there is a value setted the user is redirected to the home page with a html query with the search value
        /// </summary>
        protected string? Serach
        {
            get => _search;
            set
            {
                _search = value;
                NavigationManager.NavigateTo($"/home?search={_search}");
            }
        }

        /// <summary>
        /// If the user is valid logged in for displaying the acourding action buttons
        /// </summary>
        private bool _loggedIn = true;

        /// <summary>
        /// Performing the logout action for the user
        /// Logging out the user form the business layer, if it was successful then clear the session token and redirect to login page
        /// </summary>
        public async void Logout()
        {
            var result = Business.AuthService.Logout(await UserData.GetToken());
            if (!result.IsSuccess)
                Logger.Instance.GetLogger<NavMenu>().LogError(result.Message);
            else if (result.IsSuccess)
            {
                UserData.ClearSession();
                _loggedIn = false;

                NavigationManager.NavigateTo("/", true);
            }
            
        }
    }
}

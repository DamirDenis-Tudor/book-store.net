/**************************************************************************
 *                                                                        *
 *  File:        MainLayout.cs                                            *
 *  Copyright:   (c) 2024, Asmarandei Catalin                             *
 *  Website:     https://github.com/DamirDenis-Tudor/BookStore.NET        *
 *  Description: Loggin authorization in the main layout of the one page application*
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
using PresentationClient.Services;

namespace PresentationClient.Layout
{
    /// <summary>
    /// Main Layout, check if the user has a valid loggin session token for accesing the website
    /// </summary>
    public partial class MainLayout
    {
        /// <summary>
        /// Injection of the business facade singleton
        /// </summary>
		[Inject]
        public BusinessFacade Business { get; set; }
        /// <summary>
        /// Injection of the user login service where the token is available for the user
        /// </summary>
		[Inject]
        public IUserLoginService UserData { get; set; }

        /// <summary>
        /// Check if the user is authenticated
        /// </summary>
        private bool? _isAuthenticated = null;
        /// <summary>
        /// Check if the page is rendered for the first time
        /// </summary>
        private bool _isFirstRender = true;

        /// <summary>
        /// Checks if the user has a session token saved and checkes in the business layer if the token is valid
        /// </summary>
        /// <param name="firstRender">If the page is randered for the first time</param>
        /// <returns></returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (_isFirstRender)
            {
                _isFirstRender = false;

                var sessionToken = await UserData.GetToken();
                if (sessionToken != null)
                {
                    var checkResult = Business.AuthService.CheckSession(sessionToken);
                    if (!checkResult.IsSuccess)
                    {
                        Logger.Instance.GetLogger<MainLayout>().LogError(checkResult.Message);
                        _isAuthenticated = false;
                    }
                    else
                        _isAuthenticated = true;
                }
                else
                    _isAuthenticated = false;

                StateHasChanged();
            }
        }
    }
}

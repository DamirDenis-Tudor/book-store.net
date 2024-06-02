/**************************************************************************
 *                                                                        *
 *  File:        AccountDashboard.cs                                      *
 *  Copyright:   (c) 2024, Asmarandei Catalin                             *
 *  Website:     https://github.com/DamirDenis-Tudor/BookStore.NET        *
 *  Description: The page where the user can see details about his        *
 *      account like orders, personal details                             *
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
using Persistence.DAL;
using Persistence.DTO.Order;
using Persistence.DTO.Product;
using Presentation.Services;
using PresentationClient.Services;

namespace PresentationClient.Pages
{
    /// <summary>
    /// Fatches the user orders and displays them
    /// </summary>
    public partial class AccountDashboard
    {
        /// <summary>
        /// The user login service for getting the token of the user
        /// </summary>
        [Inject]
        public IUserLoginService UserData { get; set; } = null!;

        /// <summary>
        /// The business facade singleton
        /// </summary>
        [Inject]
        public BusinessFacade Business { get; set; } = null!;

        /// <summary>
        /// The orders of the user that are displayed on the page
        /// </summary>
        private IList<OrderSessionDto> _orders = new List<OrderSessionDto>();

        /// <summary>
        /// Username that will be displayed
        /// </summary>
        private string _name = "";

        /// <summary>
        /// Using the session token and user makes a call to get the user orders and one to get his full name
        /// </summary>
        /// <param name="firstRender"></param>
        /// <returns></returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                var sessionToken = await UserData.GetToken();
                if (sessionToken == null) return;
                {
                    var username = Business.AuthService.GetUsername(sessionToken);
                    if (username.IsSuccess)
                    {
                        var result = Business.OrderService.GetUserOrders(username.SuccessValue);
                        var user = Business.UsersService.GetUserInfo(username.SuccessValue);
                        _name = $"{user.SuccessValue.FirstName} {user.SuccessValue.LastName}";

                        if (result.IsSuccess)
                            _orders = result.SuccessValue;
                        else
                            Logger.Instance.GetLogger<AccountDashboard>().LogError(result.Message);
                    }
                }

                StateHasChanged();
            }
        }
	}
}
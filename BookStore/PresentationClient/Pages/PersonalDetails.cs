/**************************************************************************
 *                                                                        *
 *  File:        PaymentDetails.cs                                        *
 *  Copyright:   (c) 2024, Asmarandei Catalin                             *
 *  Website:     https://github.com/DamirDenis-Tudor/BookStore.NET        *
 *  Description: The page where the user enters his personal details      *
 *		for placing an order                                             *    
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
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Presentation.Entities;
using PresentationClient.Entities;
using PresentationClient.Services;
using System.ComponentModel.DataAnnotations;

namespace PresentationClient.Pages;

/// <summary>
/// Takes the personal details from the form, validates them, store the data locally and redirects to the payment page
/// </summary>
public partial class PersonalDetails
{
	/// <summary>
	/// The cart service for getting the itmes form the cart
	/// </summary>
	[Inject]
	private ICartService CartService { get; set; } = null!;

	/// <summary>
	/// The business facade singleton
	/// </summary>
	[Inject]
	private BusinessFacade Business { get; set; } = null!;

	/// <summary>
	/// The user login service for getting the token of the user
	/// </summary>
	[Inject]
	private IUserLoginService UserData { get; set; } = null!;

	/// <summary>
	/// The personal details data storage
	/// </summary>
	[Inject]
	private PersonalDetailsDataScoped PersonalDetailsScoped { get; set; } = null!;

	/// <summary>
	/// The navigation manager for redirecting the user
	/// </summary>
	[Inject]
	private NavigationManager NavigationManager { get; set; } = null!;

	/// <summary>
	/// The personal details that are mapped to the form
	/// </summary>
	private readonly PersonalDetailsDto _bill = new();

	/// <summary>
	/// Checks if the user have items in cart, if he does not, the operation is invalid, so he is redirected to the home page
	/// Check if the user have personal details stored in the database, if he does, the details are filled in the form
	/// </summary>
	/// <param name="firstRender">If the page is rendered for the first time</param>
	/// <returns>Async task</returns>
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (!firstRender) return;
		
		var cart = await CartService.GetCart();
		if (cart.Count == 0)
			NavigationManager.NavigateTo("/");
		else
		{
			var sessionToken = await UserData.GetToken();
			if (sessionToken == null) return;
				
			var username = Business.AuthService.GetUsername(sessionToken);
			if (!username.IsSuccess) return;
				
			var result = Business.UsersService.GetUserBillInfo(username.SuccessValue);
			if (!result.IsSuccess) return;
				
			_bill.Deserialize(result.SuccessValue);
			StateHasChanged();
		}
	}

	/// <summary>
	/// Event called when the user submits the personal details form
	/// The informations are validated, if they are correct, the inforamtions are saved locally in <see cref="PersonalDetailsDataScoped.cs"/>
	/// and the user is redirected to the payment page
	/// </summary>
	/// <param name="editContext">The context of the form </param>
	private void PlaceOrder(EditContext editContext)
	{
		if (!editContext.Validate()) return;
	        
		PersonalDetailsScoped.Address = _bill.Address;
		PersonalDetailsScoped.City = _bill.City;
		PersonalDetailsScoped.Country = _bill.Country;
		PersonalDetailsScoped.PostalCode = _bill.PostalCode;
		PersonalDetailsScoped.Telephone = _bill.Telephone;

		if (PersonalDetailsScoped.IsValid())
			NavigationManager.NavigateTo("/pay");
	}
}
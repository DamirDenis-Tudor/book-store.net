/**************************************************************************
 *                                                                        *
 *  File:        UserLoginService.cs                                      *
 *  Copyright:   (c) 2024, Asmarandei Catalin                             *
 *  Website:     https://github.com/DamirDenis-Tudor/BookStore.NET        *
 *  Description: The service for storing the user login session that      *
 *		stores the token in session local storage                         *
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
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace PresentationClient.Services;

/// <summary>
/// The login token is stored in the local session storage
/// </summary>
public class UserLoginService : IUserLoginService
{
	private readonly ProtectedLocalStorage _localStorage;

	public UserLoginService(ProtectedLocalStorage localStorage)
	{
		_localStorage = localStorage;
	}

	private string? Token { get; set; }

	public async Task<string?> GetToken()
	{
		try
		{
			var result = await _localStorage.GetAsync<string?>("sessiontoken");
		
			Token = result.Success ? result.Value : null;
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
		}
		
		return Token;
	}

	public async void SetToken(string? token)
	{
		if (token == null) return;
		await _localStorage.SetAsync("sessiontoken", token);
		Token = token;
	}

	public void ClearSession()
	{
		_localStorage.DeleteAsync("sessiontoken");
	}
}
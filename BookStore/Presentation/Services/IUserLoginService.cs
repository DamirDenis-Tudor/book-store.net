/**************************************************************************
 *                                                                        *
 *  File:        IUserLoginService.cs                                     *
 *  Copyright:   (c) 2024, Asmarandei Catalin                             *
 *  Website:     https://github.com/DamirDenis-Tudor/BookStore.NET        *
 *  Description: The service for storing the user login session           *
 *                                                                        *
 *  This program is free software; you can redistribute it and/or modify  *
 *  it under the terms of the GNU General Public License as published by  *
 *  the Free Software Foundation. This program is distributed in the      *
 *  hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 *  the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 *  PURPOSE. See the GNU General Public License for more details.         *
 *                                                                        *
 **************************************************************************/

namespace Presentation.Services;

/// <summary>
/// Used for storing the user login session token and getting it
/// </summary>
public interface IUserLoginService
{
	/// <summary>
	/// Set and save the token
	/// </summary>
	/// <param name="token"></param>
	void SetToken(string? token);
	/// <summary>
	/// Get the token from storage
	/// </summary>
	/// <returns>The token</returns>
	Task<string?> GetToken();
	//string GetUsername();
	/// <summary>
	/// Clear the token from storage
	/// </summary>
	void ClearSession();
}
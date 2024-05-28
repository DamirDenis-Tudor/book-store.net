/**************************************************************************
 *                                                                        *
 *  File:        Index.cs                                                 *
 *  Copyright:   (c) 2024, Asmarandei Catalin                             *
 *  Website:     https://github.com/DamirDenis-Tudor/BookStore.NET        *
 *  Description: The page where the user can login only if he is not      *
 *		already                                                           *
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
using PresentationAdmin.Entities;
using PresentationAdmin.Service;

namespace PresentationAdmin.Pages;

/// <summary>
/// User login
/// </summary>
public partial class Index
{
    /// <summary>
    /// The information introduced by the user for login
    /// </summary>
    UserLogin User { get; set; } = new UserLogin();

    /// <summary>
    /// The user login service for storing the token of the user
    /// </summary>
    [Inject]
    public IUserLoginService UserData { get; set; } = null!;

    /// <summary>
    /// The navigation manager for redirecting the user to the home page
    /// </summary>
    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// The business facade singleton
    /// </summary>
    [Inject]
    public BusinessFacade Business { get; set; } = null!;

    /// <summary>
    /// The error message that will be displayed if the loggin fails
    /// </summary>
    private string _loginError = "";
    /// <summary>
    /// The success message that will be displayed if the loggin is successful
    /// </summary>
    private string _loginSuccess = "";

    /// <summary>
    /// Event called when the user submit the login form
    /// Makes a call with the given credentials, if the login is successful the session token given is stored 
    /// and the user is redirected to the home page
    /// </summary>
    /// <param name="editContext">The context of the form</param>
    private void LoginSubmit(EditContext editContext)
    {
        editContext.OnFieldChanged += OnFieldChange;
            
        if (!editContext.Validate()) return;
 
        var result = Business.AuthService.Login(User.ConvertToBto(), LoginMode.Admin);
        if (!result.IsSuccess)
        {
            Logger.Instance.GetLogger<Index>().LogError(result.Message);
            _loginError = result.Message;
        }
        else
        {
            _loginSuccess = result.Message;
            UserData.SetToken(result.SuccessValue);

            NavigationManager.NavigateTo("/home", true);
        }
    }

    /// <summary>
    /// If the user has a invalidation message of the loggin and he changes the value of the field the message will be cleared
    /// </summary>
    /// <param name="sender">The form</param>
    /// <param name="e">The event raised for field changed</param>
    private void OnFieldChange(object? sender, FieldChangedEventArgs e)
    {
        _loginError = "";
        _loginSuccess = "";
    }
}
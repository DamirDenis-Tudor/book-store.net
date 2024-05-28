/**************************************************************************
 *                                                                        *
 *  File:        PaymentDetails.cs                                        *
 *  Copyright:   (c) 2024, Asmarandei Catalin                             *
 *  Website:     https://github.com/DamirDenis-Tudor/BookStore.NET        *
 *  Description: The page where the user enters his paymanet details and  *
 *		then he can place the order                                       *
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
using Business.BTO;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Persistence.DTO.Bill;
using PresentationClient.Entities;
using PresentationClient.Services;
using System.ComponentModel.DataAnnotations;

namespace PresentationClient.Pages;

/// <summary>
/// Takes the payment details, validates the payment if everything is ok places the order
/// </summary>
public partial class PaymentDetails
{
    /// <summary>
    /// The card details that the user has to fill in the form
    /// </summary>
    public class Card
    {
        /// <summary>
        /// Card number with the length of 16 digits
        /// </summary>
        [Required, StringLength(16, ErrorMessage = "Numarul cardului trebuie sa aiba o lungime de 16 cifre")]
        public string CardNumber { get; set; } = null!;

        /// <summary>
        /// cardholder name
        /// </summary>
        [Required]
        public string Name { get; set; } = null!;

        /// <summary>
        /// The expiration date of the card
        /// </summary>
        [Required]
        public DateTime ExpirationDate { get; set; } = DateTime.Now;

        /// <summary>
        /// The CVV of the card
        /// </summary>
        [Required, Range(100, 999)]
        public int Cvv { get; set; }
    }

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
    /// The personal details introduced by the user previously
    /// </summary>
    [Inject]
    private PersonalDetailsDataScoped PersonalDetailsScoped { get; set; } = null!;

    /// <summary>
    /// The navigation manager for redirecting the user to the home page
    /// </summary>
    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// The card details that are binded to the form
    /// </summary>
    private readonly Card _card = new();

    /// <summary>
    /// Verification if the order is valid, if the user has introduced the personal details and the cart is not empty
    /// If not he is redirected to the home page
    /// </summary>
    /// <param name="firstRender">If the page is rendered for the first time</param>
    /// <returns>Async task</returns>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;
        
        var cart = await CartService.GetCart();
        if (cart.Count == 0 || !PersonalDetailsScoped.IsValid())
            NavigationManager.NavigateTo("/");
        
    }

    /// <summary>
    /// Event called when the user submits the payment form
    /// If the form is valid the order is created then placed and the user is redirected to the account page where the order is displayed
    /// Checks if the personal details introduced by the user are different from the ones stored in the database,
    /// If so, the personal details are updated
    /// </summary>
    /// <param name="editContext"> The context of the form </param>
    public async void Pay(EditContext editContext)
    {
        if (!editContext.Validate()) return;
        var cart = await CartService.GetCart();

        var sessionToken = await UserData.GetToken();
        if (sessionToken == null) return;

        var username = Business.AuthService.GetUsername(sessionToken);
        if (!username.IsSuccess) return;

        var orderProducts = new List<OrderItemBto>();
        cart.ForEach(prod =>
            orderProducts.Add(new OrderItemBto
                { ProductName = prod.ProductName, OrderQuantity = prod.OrderQuantity, })
        );

        var order = new OrderBto { Username = username.SuccessValue, OrderItemBtos = orderProducts };

        Business.OrderService.PlaceOrder(order);
        CartService.ClearCart();
            
        if (DifferenceBillDetails(PersonalDetailsScoped.ConvertToDto(), Business.UsersService.GetUserBillInfo(username.SuccessValue).SuccessValue))
            Business.UsersService.UpdateBill(username.SuccessValue, PersonalDetailsScoped.ConvertToDto());
            
        NavigationManager.NavigateTo("/account");
    }

    /// <summary>
    /// Checks if the personal details introduced by the user are different from the ones stored in the database
    /// </summary>
    /// <param name="remote">First personal details object, from the database</param>
    /// <param name="local">Second personal details object, entered by user</param>
    /// <returns></returns>
    private static bool DifferenceBillDetails(BillDto remote, BillDto local)
    {
        return remote.Address != local.Address ||
               remote.City != local.City ||
               remote.Country != local.Country ||
               remote.PostalCode != local.PostalCode ||
               remote.Telephone != local.Telephone;
    }
}
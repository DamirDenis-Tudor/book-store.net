using Business.BAL;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Presentation.Entities;
using PresentationClient.Entities;
using PresentationClient.Service;
using PresentationClient.Services;
using System.ComponentModel.DataAnnotations;

namespace PresentationClient.Pages
{
	public partial class PersonalDetails
	{

		[Inject]
		private ICartService CartService { get; set; }
		[Inject]
		private BusinessFacade Business { get; set; }
		[Inject]
		private IUserLoginService UserData { get; set; }
		[Inject]
		private PersonalDetailsDataScoped PersonalDetailsScoped { get; set; }
		[Inject]
		private NavigationManager NavigationManager { get; set; }

		private PersonalDetailsDto _bill = new PersonalDetailsDto();

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				var cart = await CartService.GetCart();
				if (!cart.Any())
					NavigationManager.NavigateTo("/");
				else
				{
					var sessionToken = await UserData.GetToken();
					if (sessionToken != null)
					{
						var username = Business.AuthService.GetUsername(sessionToken);
						if (username.IsSuccess)
						{
							var result = Business.UsersService.GetUserBillInfo(username.SuccessValue);
							if (result.IsSuccess)
							{
								_bill.Deserialize(result.SuccessValue);
								StateHasChanged();
							}
						}
					}
				}
			}
		}

		private void PlaceOrder(EditContext editContext)
		{
			if (editContext.Validate())
			{
				PersonalDetailsScoped.Address = _bill.Address;
				PersonalDetailsScoped.City = _bill.City;
				PersonalDetailsScoped.Country = _bill.Country;
				PersonalDetailsScoped.PostalCode = _bill.PostalCode;
				PersonalDetailsScoped.Telephone = _bill.Telephone;

				if (PersonalDetailsScoped.IsValid())
					NavigationManager.NavigateTo("/pay");
			}
		}
	}
}

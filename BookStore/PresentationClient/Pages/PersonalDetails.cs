using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PresentationClient.Entities;
using PresentationClient.Services;
using System.ComponentModel.DataAnnotations;

namespace PresentationClient.Pages
{
	public partial class PersonalDetails
	{

		[Inject]
		private ICartService CartService { get; set; }
		[Inject]
		private PersonalDetailsDataScoped PersonalDetailsScoped { get; set; }
		[Inject]
		private NavigationManager NavigationManager { get; set; }

		public class BillDto
		{
			[Required]
			public string Address { get; set; }
			[Required]
			public string Telephone { get; set; }
			[Required]
			public string Country { get; set; }
			[Required]
			public string City { get; set; }
			[Required]
			public string PostalCode { get; set; }
		}

		BillDto bill = new BillDto();

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				var cart = await CartService.GetCart();
				if (!cart.Any())
					NavigationManager.NavigateTo("/");
			}
		}

		void PlaceOrder(EditContext editContext)
		{
			if (editContext.Validate())
			{
				PersonalDetailsScoped.Address = bill.Address;
				PersonalDetailsScoped.City = bill.City;
				PersonalDetailsScoped.Country = bill.Country;
				PersonalDetailsScoped.PostalCode = bill.PostalCode;
				PersonalDetailsScoped.Telephone = bill.Telephone;

				if (PersonalDetailsScoped.IsValid())
					NavigationManager.NavigateTo("/pay");
			}
		}
	}
}

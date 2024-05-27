using Business.BAL;
using Business.BTO;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Persistence.DTO.Bill;
using Persistence.DTO.Order;
using PresentationClient.Entities;
using PresentationClient.Service;
using PresentationClient.Services;
using System.ComponentModel.DataAnnotations;

namespace PresentationClient.Pages
{
	public partial class PaymentDetails
	{
		public class Card
		{
			[Required, StringLength(16, ErrorMessage = "Numarul cardului trebuie sa aiba o lungime de 16 cifre")]
			public string CardNumber { get; set; }
			[Required]
			public string Name { get; set; }
			[Required]
			public DateTime ExpirationDate { get; set; } = DateTime.Now;
			[Required, Range(100, 999)]
			public int Cvv { get; set; }
		}

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

		Card card = new Card();

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				var cart = await CartService.GetCart();
				if (!cart.Any() || !PersonalDetailsScoped.IsValid())
					NavigationManager.NavigateTo("/");
			}
		}

		public async void Pay(EditContext editContext)
		{
			if (editContext.Validate())
			{
				var cart = await CartService.GetCart();
				if(cart != null)
				{
					var sessionToken = await UserData.GetToken();
					if (sessionToken != null)
					{
						var username = Business.AuthService.GetUsername(sessionToken);
						if (username.IsSuccess)
						{
							List<OrderItemBto> orderProducts = new List<OrderItemBto>();
							cart.ForEach(prod => 
								orderProducts.Add(new OrderItemBto()
								{
									ProductName = prod.ProductName,
									OrderQuantity = prod.OrderQuantity,
								}));

							OrderBto order = new OrderBto() { 
								Username = username.SuccessValue,
								OrderItemBtos = orderProducts
							};

							Business.OrderService.PlaceOrder(order);
							CartService.ClearCart();
							if(DifferenceBillDetails(PersonalDetailsScoped.ConvertToDto(), Business.UsersService.GetUserBillInfo(username.SuccessValue).SuccessValue))
								Business.UsersService.UpdateBill(username.SuccessValue, PersonalDetailsScoped.ConvertToDto());
							NavigationManager.NavigateTo("/account");
						}
					}
				}
			}
		}

		private bool DifferenceBillDetails(BillDto remote, BillDto local)
		{
			return remote == null || remote.Address != local.Address || 
				remote.City != local.City || 
				remote.Country != local.Country ||
				remote.PostalCode != local.PostalCode || 
				remote.Telephone != local.Telephone;
		}
	}
}

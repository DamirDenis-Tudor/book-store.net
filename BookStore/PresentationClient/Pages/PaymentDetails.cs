using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Persistence.DTO.Bill;
using Persistence.DTO.Order;
using PresentationClient.Entities;
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
					string sessionCode = "23425";
					decimal totalPrice = 0;
					List<OrderProductDto> orderProducts = new List<OrderProductDto>();
					cart.ForEach(prod => {
						orderProducts.Add(new OrderProductDto()
						{
							ProductName = prod.ProductName,
							Description = prod.Description,
							Link = prod.Link,
							OrderQuantity = prod.OrderQuantity,
							Price = prod.Price,
							SessionCode = sessionCode
						});
						totalPrice += prod.Price * prod.OrderQuantity;
					});
					//TODO: Make the delivery fee logic a separate service to be used by every component
					if (totalPrice < 300)
						totalPrice += 11.99m;

					OrderSessionDto order = new OrderSessionDto()
					{
						Username = "Marius",
						OrderProducts = orderProducts,
						SessionCode = sessionCode,
						Status = "Placed",
						TotalPrice = totalPrice
					};

					BillDto bill = PersonalDetailsScoped.ConvertToDto();

					Console.WriteLine(order);
					NavigationManager.NavigateTo("/account");
				}
			}
		}
	}
}

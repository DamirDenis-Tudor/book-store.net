using Business.BAL;
using Business.BAO.Interfaces;
using Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Persistence.DAL;
using Persistence.DTO.Bill;
using Persistence.DTO.Product;
using Persistence.DTO.User;
using PresentationProvider.Entities;
using PresentationProvider.Service;
using System.ComponentModel.DataAnnotations;

namespace PresentationProvider.Pages
{
	public partial class ProductUpdate
	{
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        public BusinessFacade Business { get; set; }
        [Inject]
        public IUserLoginService UserData { get; set; }
		[Inject]
        public ProductsScope ProductsScope { get; set; }

        [SupplyParameterFromQuery(Name = "product")]
        public string ProductName { get; set; }

		public record ProductData
		{
			/*[Required(ErrorMessage = "Completeaza campul \"Nume produs\""), MinLength(30, ErrorMessage = "Descrierea trebuie sa aiba minim 30 caractere")]
			public string Name { get; set; }*/
			[Required(ErrorMessage = "Completeaza campul \"Pret\"")]
			public decimal Price { get; set; } = 1;
			[Required(ErrorMessage = "Completeaza campul \"Cantitate\"")]
			public int Quantity { get; set; } = 1;
			/*[Required(ErrorMessage = "Completeaza campul \"Categorie produs\""), MinLength(30, ErrorMessage = "Descrierea trebuie sa aiba minim 30 caractere")]
			public string Category { get; set; }
			[Required(ErrorMessage = "Completeaza campul \"Descriere produs\""), MinLength(50, ErrorMessage = "Descrierea trebuie sa aiba minim 50 caractere")]
			public string Description { get; set; }
			public string? Photo { get; set; }*/

            public void Deserialize(ProductDto prod)
            {
                /*Name = prod.Name;*/
				Price = prod.Price;
				Quantity = prod.Quantity;
				/*Category = prod.Category;
				Description = prod.Description;
				Photo = prod.Link;*/
            }

			/*public ProductDto ConverToDto()
			{
				return new ProductDto()
				{
					Name = Name,
					Price = Price,
					Quantity = Quantity,
					Category = Category,
					Description = Description,
					Link = Photo
				};
			}*/
		}

        private ProductData _modifyProduct = new ProductData();

		protected override void OnInitialized()
		{
			var result = ProductsScope.Products.Where(prod => prod.Name == ProductName.Replace("%0A", "\n")).FirstOrDefault();
			if (result.Equals(default))
				NavigationManager.NavigateTo("/home");
			else
				_modifyProduct.Deserialize(result);
		}

		private async void Submit(EditContext editContext)
        {
            if (editContext.Validate())
            {
				var sessionToken = await UserData.GetToken();
                if (sessionToken != null)
                {
                    var username = Business.AuthService.GetUsername(sessionToken);
                    var remoteInfo  = ProductsScope.Products.Where(prod => prod.Name == ProductName.Replace("%0A", "\n")).FirstOrDefault();
					if (remoteInfo.Price != _modifyProduct.Price)
						Business.InventoryService.UpdateProductPrice(username.SuccessValue, ProductName.Replace("%0A", "\n"), _modifyProduct.Price);
					
					if (remoteInfo.Quantity != _modifyProduct.Quantity)
						Business.InventoryService.UpdateProductStocks(username.SuccessValue, ProductName.Replace("%0A", "\n"), _modifyProduct.Quantity);
						NavigationManager.NavigateTo("/home", true);
				}
            }
        }

		private bool DifferenceBillDetails(ProductDto remote, ProductData local)
		{
			return remote == null ||
				remote.Quantity != local.Quantity ||
				remote.Price != local.Price;
		}
	}
}

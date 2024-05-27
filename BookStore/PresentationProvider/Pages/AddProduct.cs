using Business.BAL;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Persistence.DTO.Product;
using PresentationProvider.Service;
using System.ComponentModel.DataAnnotations;

namespace PresentationProvider.Pages
{
    public partial class AddProduct
    {
        [Inject]
        public IUserLoginService UserData { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public BusinessFacade Business { get; set; }

        public record ProductData
        {
            [Required(ErrorMessage = "Completeaza campul \"Nume produs\""), MinLength(30, ErrorMessage = "Descrierea trebuie sa aiba minim 30 caractere")]
            public string Name { get; set; }
            [Required(ErrorMessage = "Completeaza campul \"Pret\"")]
            public decimal Price { get; set; } = 1;
            [Required(ErrorMessage = "Completeaza campul \"Cantitate\"")]
            public int Quantity { get; set; } = 1;
            [Required(ErrorMessage = "Completeaza campul \"Categorie produs\""), MinLength(30, ErrorMessage = "Descrierea trebuie sa aiba minim 30 caractere")]
            public string Category { get; set; }
            [Required(ErrorMessage = "Completeaza campul \"Descriere produs\""), MinLength(50, ErrorMessage = "Descrierea trebuie sa aiba minim 50 caractere")]
            public string Description { get; set; }
            public string? Photo { get; set; }

            public ProductDto ConverToDto()
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
            }
        }

		ProductData product = new ProductData();

        private async Task PlaceOrder (EditContext editContext)
        {
            if (editContext.Validate())
            {
                var sessionToken = await UserData.GetToken();
                if (sessionToken != null)
                {
                    var username = Business.AuthService.GetUsername(sessionToken);

					Business.InventoryService.RegisterProduct(username.SuccessValue, product.ConverToDto());
                    NavigationManager.NavigateTo("/home");
				}
            }
        }
    }
}

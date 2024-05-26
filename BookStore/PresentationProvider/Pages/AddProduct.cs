using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace PresentationProvider.Pages
{
    public partial class AddProduct
    {
        public record ProductDto
        {
            [Required(ErrorMessage = "Completeaza campul \"Nume produs\"")]
            public string Name { get; set; }
            [Required(ErrorMessage = "Completeaza campul \"Pret\"")]
            public decimal Price { get; set; }
            [Required(ErrorMessage = "Completeaza campul \"Cantitate\"")]
            public int Quantity { get; set; }
            [Required(ErrorMessage = "Completeaza campul \"Categorie produs\"")]
            public string Category { get; set; }
            public string? Photo { get; set; }
        }

        ProductDto product = new ProductDto();

        void PlaceOrder (EditContext editContext)
        {
            if (editContext.Validate())
            {
                Console.Write("OrderPalced");
            }
        }
    }
}

using Microsoft.AspNetCore.Components;
using Persistence.DTO.Order;
using PresentationClient.Services;
using System.Collections.ObjectModel;

namespace PresentationClient.Pages;

public partial class ViewCart
{
    [Inject]
    private ICartService CartService { get; set; } = null!;

    private ObservableCollection<OrderProductData> Products { get; set; } = null!;

    protected override void OnInitialized() => Products = [];
    
    private const decimal DeliveryFeeForOrder = 11.99m;
    
    private bool _isDataLoaded;
    private bool _cartEmpty = true;
    private decimal ProductsTotalPrice { get; set; }
    private decimal TotalPrice { get; set; }

    private decimal _deliveryFee; 

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && !_isDataLoaded)
        {
            var cart = await CartService.GetCart();
            cart.ForEach(p => Products.Add(p));

            UpdateTotalPrices();
                
            _isDataLoaded = cart.Count != 0;
            _cartEmpty = !Products.Any();

            StateHasChanged();
        }
    }

    private void UpdateTotalPrices()
    {
        ProductsTotalPrice = Products.Sum(prod => prod.Price * prod.OrderQuantity);
        if (ProductsTotalPrice != 0 && ProductsTotalPrice < 300)
            _deliveryFee = DeliveryFeeForOrder;
        else
            _deliveryFee = 0;
        TotalPrice = ProductsTotalPrice + _deliveryFee;
    }
    
    private void RefreshView()
    {
        UpdateTotalPrices();
        
        _cartEmpty = !Products.Any() || !Products.Any(prod => prod.OrderQuantity > 0);
        
        StateHasChanged();
    }
}
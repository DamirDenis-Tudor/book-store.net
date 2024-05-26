using Business.BAL;
using Common;
using Microsoft.AspNetCore.Components;
using Persistence.DAL;
using Persistence.DTO.Order;
using PresentationClient.Service;

namespace PresentationClient.Pages
{
	public partial class AccountDashboard
	{
        [Inject]
        public IUserLoginService UserData { get; set; }
        [Inject]
        public BusinessFacade Business { get; set; }

		private IList<OrderSessionDto> _orders = new List<OrderSessionDto>();
        private string _name = "";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {

                var sessionToken = await UserData.GetToken();
                if (sessionToken != null)
                {
                    var username = Business.AuthService.GetUsername(sessionToken);
                    if (username.IsSuccess)
                    {
                        var result = Business.OrderService.GetUserOrders(username.SuccessValue);
                        var user =  Business.UsersService.GetUserInfo(username.SuccessValue);
                        _name = $"{user.SuccessValue.FirstName} {user.SuccessValue.LastName}";

						if (result.IsSuccess)
                            _orders = result.SuccessValue;
                        else
                            Logger.Instance.GetLogger<AccountDashboard>().LogError(result.Message);
                    }
                }

                StateHasChanged();
            }
        }
    }
}

using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using Business.BAL;
using Common;
using PresentationAdmin.Service;

namespace PresentationAdmin.Layout
{
    public partial class NavMenu
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        private IUserLoginService UserData { get; set; }
		[Inject]
		public BusinessFacade Business { get; set; }

		private string? _search;
        protected string? Serach
        {
            get => _search;
            set
            {
                _search = value;
                NavigationManager.NavigateTo($"/home?search={_search}");
            }
        }

        private bool _loggedIn = true;

        public async void Logout()
        {
            var result = Business.AuthService.Logout(await UserData.GetToken());
            if (!result.IsSuccess)
                Logger.Instance.GetLogger<NavMenu>().LogError(result.Message);
            else if (result.IsSuccess)
            {
                UserData.ClearSession();
                _loggedIn = false;

                NavigationManager.NavigateTo("/login");
            }
            
        }
    }
}

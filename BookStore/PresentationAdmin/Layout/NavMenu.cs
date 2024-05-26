using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using Business.BAL;
using Common;

namespace PresentationAdmin.Layout
{
    public partial class NavMenu
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        private ProtectedLocalStorage LocalStorage { get; set; }
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
            
			var username = await LocalStorage.GetAsync<string>("username");
			var result = Business.AuthService.Logout(username.Value);
            if (!result.IsSuccess)
                Logger.Instance.GetLogger<NavMenu>().LogError(result.Message);
            else if(result.SuccessValue)
            {
                LocalStorage.DeleteAsync("sessiontoken");
                LocalStorage.DeleteAsync("username");
				_loggedIn = false;

				NavigationManager.NavigateTo("/login");
            }
        }
    }
}

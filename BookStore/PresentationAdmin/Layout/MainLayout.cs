using Business.BAL;
using Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Logging;
using PresentationAdmin.Service;

namespace PresentationAdmin.Layout
{
    public partial class MainLayout
    {
		[Inject]
		public BusinessFacade Business { get; set; }
        [Inject]
		public ProtectedLocalStorage LocalStorage { get; set; }
		[Inject]
		public IUserLoginService UserData { get; set; }

		private bool? _isAuthenticated = null;
        private bool _isFirstRender = true;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (_isFirstRender)
            {
                _isFirstRender = false;

                var sessionToken = await LocalStorage.GetAsync<string>("sessiontoken");
                var username = await LocalStorage.GetAsync<string>("username");


				if (sessionToken.Success && username.Success)
                {
					var checkResult = Business.AuthService.CheckSession(username.Value, sessionToken.Value);
                    if (!checkResult.IsSuccess)
                    {
                        Logger.Instance.GetLogger<MainLayout>().LogError(checkResult.Message);
						_isAuthenticated = false;
					}
                    else
                    {
                        _isAuthenticated = checkResult.SuccessValue;
                    }
					
                }
                else
                    _isAuthenticated = false;

                StateHasChanged();
            }
        }
    }
}

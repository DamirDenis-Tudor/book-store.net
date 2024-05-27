using Business.BAL;
using Common;
using Microsoft.AspNetCore.Components;
using PresentationProvider.Service;

namespace PresentationProvider.Layout
{
    public partial class MainLayout
    {
		[Inject]
		public BusinessFacade Business { get; set; }
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

				var sessionToken = await UserData.GetToken();
				if (sessionToken != null)
				{
					var checkResult = Business.AuthService.CheckSession(sessionToken);
					if (!checkResult.IsSuccess)
					{
						Logger.Instance.GetLogger<MainLayout>().LogError(checkResult.Message);
						_isAuthenticated = false;
					}
					else
						_isAuthenticated = true;
				}
				else
					_isAuthenticated = false;

				StateHasChanged();
			}
		}
	}
}

using Business.BAL;
using Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Presentation.Entities;
using PresentationProvider.Service;

namespace PresentationProvider.Pages
{
	public partial class Index
	{
		UserLogin User { get; set; } = new UserLogin();

		[Inject]
		public IUserLoginService UserData { get; set; }
		[Inject]
		public NavigationManager NavigationManager { get; set; }
		[Inject]
		public BusinessFacade Business { get; set; }

		private string _logginError = "";
		private string _logginSuccess = "";

		private void LoginSubmit(EditContext editContext)
		{
			editContext.OnFieldChanged += OnFieldChange;
			if (editContext.Validate())
			{
				var result = Business.AuthService.Login(User.ConverToBto(), LoginMode.Provider);
				if (!result.IsSuccess)
				{
					Logger.Instance.GetLogger<Index>().LogError(result.Message);
					_logginError = result.Message;
				}
				else
				{
					_logginSuccess = result.Message;
					UserData.SetToken(result.SuccessValue);

					NavigationManager.NavigateTo("/home", true);
				}

			}
		}

		private void OnFieldChange(object? sender, FieldChangedEventArgs e)
		{
			_logginError = "";
			_logginSuccess = "";
		}
	}
}

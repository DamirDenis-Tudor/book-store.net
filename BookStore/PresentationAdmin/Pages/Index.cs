using Business.BAL;
using Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Presentation.Entities;
using PresentationAdmin.Service;

namespace PresentationAdmin.Pages
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

		private async Task LoginSubmit(EditContext editContext)
		{
			editContext.OnFieldChanged += test;
			if (editContext.Validate())
			{
				var result = Business.AuthService.Login(User.ConverToBto());
				//var result = BusinessFacade.Instance.AuthService.Login(new Business.BTO.UserLoginBto() { Username="admin_12345", Password="admin@2024"});
				if (!result.IsSuccess)
				{
					Logger.Instance.GetLogger<Index>().LogError(result.Message);
					_logginError = result.Message;
				}
				else
				{
					_logginSuccess = result.Message;
					UserData.SetToken(result.SuccessValue);
					UserData.SetUsername(User.Username);

					NavigationManager.NavigateTo("/home", true);
				}
				
			}
		}

		private void test(object? sender, FieldChangedEventArgs e)
		{
			_logginError = "";
			_logginSuccess = "";
		}
	}
}

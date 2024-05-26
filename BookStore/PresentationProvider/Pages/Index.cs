using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Presentation.Entities;

namespace PresentationProvider.Pages
{
	public partial class Index
	{
		UserLogin User { get; set; } = new UserLogin();

		[Inject]
		public ProtectedLocalStorage LocalStorage { get; set; }
		[Inject]
		public NavigationManager NavigationManager { get; set; }

		private async Task LoginSubmit(EditContext editContext)
		{
			if (editContext.Validate())
			{
				string token = "token";
				await LocalStorage.SetAsync("sessiontoken", token);
				NavigationManager.NavigateTo("/home", true);
			}
		}
	}
}

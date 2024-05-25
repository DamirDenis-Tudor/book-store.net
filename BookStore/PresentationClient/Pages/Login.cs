using Microsoft.AspNetCore.Components.Forms;
using Presentation.Entities;

namespace PresentationClient.Pages
{
	public partial class Login
	{
		UserLogin user { get; set; } = new UserLogin();

		private async Task LoginSubmit(EditContext editContext)
        {
			if (editContext.Validate())
			{
				string token = "token";
				await ProtectedLocalStorage.SetAsync("sessiontoken", token);
				NavigationManager.NavigateTo("/", true);
			}
		}
	}
}

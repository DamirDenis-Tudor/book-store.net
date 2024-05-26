using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using Common;
using Business.BAL;

namespace PresentationClient.Service
{
	public class UserLoginService : IUserLoginService
	{
		private readonly ProtectedLocalStorage _localStorage;

		public UserLoginService(ProtectedLocalStorage localStorage)
		{
			_localStorage = localStorage;
		}

		private string? Token { get; set; } = null;
		/*private string _username;*/

		public async Task<string?> GetToken()
		{
			var result = await _localStorage.GetAsync<string>("sessiontoken");
			Token = result.Success ? result.Value : null;
			/*_username = _business.AuthService.GetUsername(Token).SuccessValue;*/
			return Token;
		}

		/*public string GetUsername()
		{
			return _username;
		}*/

		public async void SetToken(string token)
		{
			await _localStorage.SetAsync("sessiontoken", token);
			Token = token;
			/*_username = _business.AuthService.GetUsername(token).SuccessValue;*/
		}

		public void ClearSession()
		{
			_localStorage.DeleteAsync("sessiontoken");
		}
	}
}

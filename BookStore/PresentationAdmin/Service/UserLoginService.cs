using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using Common;

namespace PresentationAdmin.Service
{
	public class UserLoginService : IUserLoginService
	{
		private readonly ProtectedLocalStorage _localStorage;

		public UserLoginService(ProtectedLocalStorage localStorage)
		{
			_localStorage = localStorage;
		}

		public async Task<string?> GetToken()
		{
			var result = await _localStorage.GetAsync<string>("sessiontoken");
			return result.Success ? result.Value : null;
		}

		public async Task<string?> GetUsername()
		{
			var result = await _localStorage.GetAsync<string>("username");
			return result.Success ? result.Value : null;
		}

		public async void SetToken(string token)
		{
			await _localStorage.SetAsync("sessiontoken", token);
		}

		public async void SetUsername(string username)
		{
			await _localStorage.SetAsync("username", username);
		}
	}
}

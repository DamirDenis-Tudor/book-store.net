﻿using Microsoft.AspNetCore.Components;

namespace PresentationClient.Layout
{
	public partial class NavMenu
	{
		[Inject]
		private NavigationManager navigationManager { get; set; }

		private string? _search;
		protected string? Serach { get => _search; 
			set{
				_search = value;
				navigationManager.NavigateTo($"/?search={_search}");
            } 
		}

		private bool _loggedIn = true;

		public void Logout()
		{
			_loggedIn = false;
			ProtectedLocalStorage.DeleteAsync("sessiontoken");
			NavigationManager.NavigateTo("/login");
		}
	}
}

using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;

namespace PresentationProvider.Layout
{
    public partial class NavMenu
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        private ProtectedLocalStorage LocalStorage { get; set; }

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

        public void Logout()
        {
            _loggedIn = false;
            LocalStorage.DeleteAsync("sessiontoken");
            NavigationManager.NavigateTo("/login");
        }
    }
}

using Business.BAL;
using Microsoft.AspNetCore.Components;
using Persistence.DTO.User;
using PresentationAdmin.Service;

namespace PresentationAdmin.Shared
{
    public partial class UserView
    {
        [Parameter]
        public BillUserDto User { get; set; }

        [Inject]
        public IUserLoginService UserData { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }


        public async void Delete()
        {
            var result = BusinessFacade.Instance.UsersService.DeleteUser(await UserData.GetUsername(), User.Username);
            NavigationManager.NavigateTo("/users", true);
            
		}
    }
}

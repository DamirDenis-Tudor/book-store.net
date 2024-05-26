using Business.BAL;
using Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Persistence.DTO.User;
using PresentationAdmin.Service;

namespace PresentationAdmin.Shared
{
    public partial class UserView
    {
        [Parameter]
        public UserInfoDto User { get; set; }

        [Inject]
        public IUserLoginService UserData { get; set; }
        [Inject]
        public BusinessFacade Business { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }


        public async void Delete()
        {
			var username = Business.AuthService.GetUsername(await UserData.GetToken());
			if (!username.IsSuccess)
				Logger.Instance.GetLogger<UserView>().LogError(username.Message);
			var result = Business.UsersService.DeleteUser(username.SuccessValue, User.Username);
            if (!result.IsSuccess)
                Logger.Instance.GetLogger<UserView>().LogError(result.Message);
            else
                NavigationManager.NavigateTo("/users", true);
            
		}
    }
}

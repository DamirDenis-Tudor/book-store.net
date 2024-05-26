using Business.BAL;
using Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Persistence.DAL;
using Persistence.DTO.Bill;
using Persistence.DTO.User;
using PresentationClient.Service;
using System.ComponentModel.DataAnnotations;

namespace PresentationClient.Pages
{
	public partial class UpdateUserInfo
	{
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        public BusinessFacade Business { get; set; }
        [Inject]
        public IUserLoginService UserData { get; set; }

        public class UserInfoData
        {
            [Required]
            public string FirstName { get; set; }
            [Required]
            public string LastName { get; set; }
			[Required, MinLength(5, ErrorMessage = "Username-ul trebuie sa fie mai lung de 5 caractere")]
			public string Username { get; set; }
            [Required, MinLength(3, ErrorMessage = "Parola trebuie sa fie mai lunga de 3 caractere")]
            public string Password { get; set; }
            [Required]
            public string Email { get; set; }

            public void Deserialize(UserInfoDto dto)
            {
                FirstName = dto.FirstName;
				LastName = dto.LastName;
				Username = dto.Username;
				Email = dto.Email;
            }
            public UserRegisterDto ConverToDto()
            {
                return new UserRegisterDto()
                { Email = Email, FirstName = FirstName, LastName = LastName, Password = Password, Username = Username, UserType="CLIENT" };
            }
		}

        private UserInfoData _user = new UserInfoData();

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				var sessionToken = await UserData.GetToken();
				if (sessionToken != null)
				{
					var username = Business.AuthService.GetUsername(sessionToken);
					if (username.IsSuccess)
					{
						var result = Business.UsersService.GetUserInfo(username.SuccessValue);
						if (result.IsSuccess)
						{
							_user.Deserialize(result.SuccessValue);
							StateHasChanged();
						}
					}
				}
			}
		}

		private async void RegisterSubmit(EditContext editContext)
        {
            if (editContext.Validate())
            {
				var sessionToken = await UserData.GetToken();
                if (sessionToken != null)
                {
                    var username = Business.AuthService.GetUsername(sessionToken);
                    var remoteInfo = Business.UsersService.GetUserInfo(username.SuccessValue);
                    if (DifferenceBillDetails(remoteInfo.SuccessValue, _user))
                    {
                        Business.UsersService.UpdateUser(username.SuccessValue, _user.ConverToDto());
                        Business.AuthService.Logout(sessionToken);
                        NavigationManager.NavigateTo("/", true);
					}
				}
                /*var result = Business.UsersService.RegisterClient(new UserRegisterDto()
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = user.Password,
                    Username = user.Username,
                    UserType = "CLIENT"
                });
                if (!result.IsSuccess)
                    Logger.Instance.GetLogger<Register>().LogError(result.Message);
                else
                    NavigationManager.NavigateTo("/home");*/
            }
        }

		private bool DifferenceBillDetails(UserInfoDto remote, UserInfoData local)
		{
            return remote == null || remote.Email != local.FirstName ||
                remote.FirstName != local.FirstName ||
                remote.LastName != local.LastName ||
                remote.Username != local.Username;
		}
	}
}

using Business.BAL;
using Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Persistence.DAL;
using Persistence.DTO.User;
using PresentationClient.Service;
using System.ComponentModel.DataAnnotations;

namespace PresentationClient.Pages
{
	public partial class Register
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
            [Required]
            public string Username { get; set; }
            [Required]
            public string Password { get; set; }
            [Required]
            public string Email { get; set; }
        }

        UserInfoData user = new UserInfoData();

        private async void RegisterSubmit(EditContext editContext)
        {
            if (editContext.Validate())
            {
                var result = Business.UsersService.RegisterClient(new UserRegisterDto()
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
                    NavigationManager.NavigateTo("/home");
            }
        }
    }
}

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Persistence.DAL;
using Persistence.DTO.User;
using System.ComponentModel.DataAnnotations;

namespace PresentationClient.Pages
{
	public partial class Register
	{
		[Inject]
		private NavigationManager NavigationManager { get; set; }

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

		private void RegisterSubmit(EditContext editContext)
		{
			if (editContext.Validate())
			{
				PersistenceAccess.Instance.UserRepository.RegisterUser(new UserInfoDto()
				{
					Email = user.Email,
					FirstName = user.FirstName,
					LastName = user.LastName,
					Password = user.Password,
					Username = user.Username,
					UserType = "user"
				});
				PersistenceAccess.Instance.UserRepository.GetAllUsers().SuccessValue.ForEach(Console.WriteLine);
				NavigationManager.NavigateTo("/login");
			}
		}
	}
}

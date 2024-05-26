using Business.BAL;
using Common;
using Microsoft.AspNetCore.Components;
using Persistence.DTO.User;
using PresentationAdmin.Service;

namespace PresentationAdmin.Pages
{
    public partial class ViewUser
    {
		[Inject]
		public BusinessFacade Business { get; set; }
		[Inject]
		public IUserLoginService UserData { get; set; }

		private IList<UserInfoDto> Users {
            get {
                var result = Business.UsersService.GetAllUsers("admin_12345");
                if (!result.IsSuccess)
                {
                    Logger.Instance.GetLogger<ViewUser>().LogError(result.Message);
                    return new List<UserInfoDto>();
				}
                else
                    return result.SuccessValue;
            } 
        }
    }
}

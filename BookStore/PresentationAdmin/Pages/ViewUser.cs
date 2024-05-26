using Business.BAL;
using Common;
using Persistence.DTO.User;

namespace PresentationAdmin.Pages
{
    public partial class ViewUser
    {
        private IList<BillUserDto> Users {
            get {
                var result = BusinessFacade.Instance.UsersService.GetAllUsers("admin");
                if (!result.IsSuccess)
                {
                    Logger.Instance.GetLogger<ViewUser>().LogError(result.Message);
                    return new List<BillUserDto>();
				}
                else
                    return result.SuccessValue;
            } 
        }
    }
}

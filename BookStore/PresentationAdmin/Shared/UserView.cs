using Business.BAL;
using Microsoft.AspNetCore.Components;
using Persistence.DTO.User;

namespace PresentationAdmin.Shared
{
    public partial class UserView
    {
        [Parameter]
        public BillUserDto User { get; set; }

        public void Delete()
        {
            BusinessFacade.Instance.UsersService.DeleteUser("admin", User.Username);
        }
    }
}

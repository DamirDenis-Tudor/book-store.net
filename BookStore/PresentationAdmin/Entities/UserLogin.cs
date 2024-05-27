using Business.BTO;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Entities
{
	public class UserLogin
	{
		[Required(ErrorMessage = "Introduceti username-ul contului")]
		public string Username { get; set; }
		[Required(ErrorMessage = "Introduceti parola contului")]
		public string Password { get; set; }

		public UserLoginBto ConverToBto()
		{
			return new UserLoginBto() { Username = Username, Password = Password };
		}
	}
}

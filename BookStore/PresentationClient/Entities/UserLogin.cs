using System.ComponentModel.DataAnnotations;

namespace Presentation.Entities
{
	public class UserLogin
	{
		[Required(ErrorMessage = "Introduceti username-ul contului")]
		public string Username { get; set; }
		[Required(ErrorMessage = "Introduceti parola contului")]
		public string Password { get; set; }
	}
}

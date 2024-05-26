namespace PresentationAdmin.Service
{
	public interface IUserLoginService
	{
		void SetUsername(string username);
		void SetToken(string token);
		Task<string?> GetToken();
		Task<string?> GetUsername();
	}
}

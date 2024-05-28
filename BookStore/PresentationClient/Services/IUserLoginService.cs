namespace PresentationClient.Service
{
	public interface IUserLoginService
	{
		void SetToken(string token);
		Task<string?> GetToken();
		//string GetUsername();
		void ClearSession();
	}
}

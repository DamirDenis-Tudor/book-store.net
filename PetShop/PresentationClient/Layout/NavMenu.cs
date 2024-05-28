namespace PresentationClient.Layout
{
	public partial class NavMenu
	{
		protected bool loggedIn = true;

		protected void logout()
		{
			loggedIn = false;
		}
	}
}

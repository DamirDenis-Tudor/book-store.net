namespace PresentationProvider.Layout
{
    public partial class MainLayout
    {
        private bool? _isAuthenticated = null;
        private bool _isFirstRender = true;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (_isFirstRender)
            {
                _isFirstRender = false;

                var result = await ProtectedLocalStore.GetAsync<string>("sessiontoken");
                Console.WriteLine(result.Value);
                if (result.Success && result.Value == "token")
                {
                    _isAuthenticated = true;
                }
                else
                {
                    _isAuthenticated = false;
                }

                StateHasChanged();
            }
        }
    }
}

using Business.BAL;
using Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Presentation.Entities;
using Presentation.Services;

namespace Presentation.Components
{
    public partial class LoginComponent
    {

        [Parameter]
        public LoginMode LoginMode { get; set; } = LoginMode.None;
        /// <summary>
        /// The information introduced by the user for login
        /// </summary>
        UserLogin User { get; } = new();

        /// <summary>
        /// The navigation manager for redirecting the user to the home page
        /// </summary>
        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;

        /// <summary>
        /// The user login service for storing the token of the user
        /// </summary>
        [Inject]
        public IUserLoginService UserData { get; set; } = null!;

        /// <summary>
        /// The business facade singleton
        /// </summary>
        [Inject]
        public BusinessFacade Business { get; set; } = null!;

        /// <summary>
        /// If the component enables the authentification option if user don't have an account
        /// </summary>
        [Parameter]
        public bool AuthentificationEnabled { get; set; }

        /// <summary>
        /// The error message that will be displayed if the loggin fails
        /// </summary>
        private string _loggingError = "";
        /// <summary>
        /// The success message that will be displayed if the loggin is successful
        /// </summary>
        private string _loggingSuccess = "";

        /// <summary>
        /// Event called when the user submit the login form
        /// Makes a call with the given credentials, if the login is successful the session token given is stored 
        /// and the user is redirected to the home page
        /// </summary>
        /// <param name="editContext">The context of the form</param>
        private void LoginSubmit(EditContext editContext)
        {
            editContext.OnFieldChanged += OnFieldChange;

            if (!editContext.Validate()) return;

            var result = Business.AuthService.Login(User.ConverToBto(), LoginMode);
            if (!result.IsSuccess)
            {
                Logger.Instance.GetLogger<HomeComponent>().LogError(result.Message);
                _loggingError = result.Message;
            }
            else
            {
                _loggingSuccess = result.Message;
                UserData.SetToken(result.SuccessValue);

                NavigationManager.NavigateTo("/", true);
            }
        }

        /// <summary>
        /// If the user has a invalidation message of the loggin and he changes the value of the field the message will be cleared
        /// </summary>
        /// <param name="sender">The form</param>
        /// <param name="e">The event raised for field changed</param>
        private void OnFieldChange(object? sender, FieldChangedEventArgs e)
        {
            _loggingError = "";
            _loggingSuccess = "";
        }
    }
}

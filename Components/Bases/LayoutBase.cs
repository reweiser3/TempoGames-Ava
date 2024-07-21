using Ava.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.JSInterop;
using System.Security.Claims;
using Ava.Services;

namespace Ava.Components.Bases
{
    /// <summary>
    /// Base layout component for Ava application.
    /// </summary>
    public class LayoutBase : LayoutComponentBase
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected IJSRuntime JSRuntime { get; set; } = default!;

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        [Inject]
        protected ILogger<LayoutBase> Logger { get; set; } = default!;

        [Inject]
        protected UserService UserService { get; set; } = default!;

        [Inject]
        protected SignInManager<ApplicationUser> SignInManager { get; set; } = default!;

        protected ApplicationUser CurrentUser { get; private set; } = default!;

        /// <summary>
        /// Initializes the component and loads the current user.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await LoadCurrentUser();
        }

        /// <summary>
        /// Loads the current authenticated user.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task LoadCurrentUser()
        {
            try
            {
                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;

                if (user.Identity != null && user.Identity.IsAuthenticated)
                {
                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (userId != null)
                    {
                        CurrentUser = await UserService.FindUserByIdAsync(userId);

                        Logger.LogInformation("Current user loaded: {UserId}", userId);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred while loading the current user.");
            }
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task LogoutAsync()
        {
            try
            {
                await SignInManager.SignOutAsync();
                Logger.LogInformation("User logged out.");

                // Use JavaScript interop for redirection
                await JSRuntime.InvokeVoidAsync("blazorExtensions.redirectTo", "/");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An error occurred while logging out.");
            }
        }

        /// <summary>
        /// Redirects to the login page.
        /// </summary>
        protected void Login()
        {
            NavigationManager.NavigateTo("Account/Login", true);
        }

        protected bool DoesAvatarExist(string userProfilePicture)
        {
            string filePath = Path.Combine("wwwroot", "images", "avatars", $"{userProfilePicture}");
            return System.IO.File.Exists(filePath);
        }

        protected override async Task OnAfterRenderAsync(bool isFirstRender)
        {
            if (isFirstRender)
            {
                await JSRuntime.InvokeVoidAsync("window.initializeFlowbite");
            }
        }
    }
}

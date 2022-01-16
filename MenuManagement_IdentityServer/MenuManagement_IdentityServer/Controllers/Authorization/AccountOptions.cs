namespace MenuManagement_IdentityServer.Controllers.Authorization
{
    public class AccountOptions
    {
        public static bool AllowRememberLogin = true;

        public static bool ShowLogoutPrompt = true;

        public static bool AutomaticRedirectAfterSignOut = false;

        public static string InvalidCredentialsErrorMessage = "Invalid username or password";
    }
}

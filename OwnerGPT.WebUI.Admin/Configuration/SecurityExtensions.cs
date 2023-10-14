using Microsoft.AspNetCore.Authentication.Cookies;

namespace OwnerGPT.WebUI.Admin.Configuration
{
    public static class SecurityExtensions
    {
        public static void RegisterSecurityLayer(this WebApplicationBuilder application)
        {
            application.RegisterCookieAuthentication();
        }

        private static void RegisterCookieAuthentication(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(60); 
                options.SlidingExpiration = false;
            });
        }
    }
}

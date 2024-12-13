using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.Configurations.Cookie
{
    public static class CookieConfigiguration
    {

        public static IServiceCollection AddCookieAuthenticaton(this IServiceCollection services)
        {
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //})
            //.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            //{
            //    options.Cookie = new CookieBuilder()
            //    {
            //        Name = "default-cookie",
            //    };
            //    options.LoginPath = "/User/Account/Login";
            //    options.LogoutPath = "/User/Account/Logout";
            //    options.AccessDeniedPath = "/";
            //});
            //services.AddAuthorizationBuilder()
            //    .AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"))
            //    .AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
            //return services;
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/User/Account/Login";
                options.LogoutPath = "/User/Account/Logout";
                options.AccessDeniedPath = "/";
            });
            services.AddAuthorizationBuilder()
                .AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"))
                .AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
            return services;
        }

    }
}

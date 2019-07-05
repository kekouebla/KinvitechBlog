using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.IO;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace CloudMigration.NET462Security
{
    public class CloudConfig
    {
        /// <summary>
        /// 1 - Defines a Generic Host builder (required Microsoft.Extensions.Hosting, version 2.2.0 NuGet package)
        /// </summary>
        private static IHost builder;

        /// <summary>
        /// 2 - Gets service of type T from the DI container (required Microsoft.Extensions.DependencyInjection, version 2.2.0 NuGet package)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>()
        {
            return builder.Services.GetService<T>();
        }

        /// <summary>
        /// 3 - Sets the path to the appsettings.json file if running locally
        /// </summary>
        private static readonly string APP_CONTEXT_BASE_DIRECTORY = @"C:\AppSecrets";

        /// <summary>
        /// 4 - Returns the appsettings.json full path
        /// </summary>
        /// <returns></returns>
        private static string GetContentRoot()
        {
            var basePath = Path.GetFullPath(APP_CONTEXT_BASE_DIRECTORY) ??
               AppDomain.CurrentDomain.BaseDirectory;
            return Path.GetFullPath(basePath);
        }

        private static IConfiguration _configuration;

        public static void RegisterConfigurations()
        {
            builder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    // Add configuration providers as needed:
                    _configuration = hostingContext.Configuration;
                    // 5 - Sets file provider base path
                    config.SetBasePath(GetContentRoot());
                    // 6 - Adds JSON file provider (Microsoft.Extensions.Configuration.Json, version 2.2.0)
                    config.AddJsonFile(@"appsettings.json", optional: true, reloadOnChange: true);
                }).Build();
        }

        /// <summary>
        /// Configure application for authentication/authorization
        /// </summary>
        /// <param name="app"></param>
        public static void ConfigureAuth(IAppBuilder app)
        {
            // Enable the Cookie saver middleware to work around a bug in the OWIN implementation
            app.UseKentorOwinCookieSaver();

            // Set Cookies as default authentication type
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
                LoginPath = new PathString("/Account/Login")
            });

            // Configure Auth0 authentication
            app.UseOpenIdConnectAuthentication(CreateOpenIdConnectOptions);
        }

        /// <summary>
        /// Configure application to use identity provider (ex: Auth0)
        /// </summary>
        private static OpenIdConnectAuthenticationOptions CreateOpenIdConnectOptions
        {
            get
            {
                // Configure Auth0 parameters
                string auth0Domain = _configuration["idp:domain"];
                string auth0ClientId = _configuration["idp:clientid"];
                string auth0ClientSecret = _configuration["idp:clientsecret"];
                string auth0RedirectUri = _configuration["idp:redirecturi"];
                string auth0PostLogoutRedirectUri = _configuration["idp:postlogoutredirecturi"];
                return new OpenIdConnectAuthenticationOptions(_configuration["idp:authenticationtype"])
                {
                    Authority = $"https://{auth0Domain}",
                    ClientId = auth0ClientId,
                    ClientSecret = auth0ClientSecret,
                    AuthenticationType = _configuration["idp:authenticationtype"],
                    RedirectUri = auth0RedirectUri,
                    PostLogoutRedirectUri = auth0PostLogoutRedirectUri,
                    ResponseType = OpenIdConnectResponseType.CodeIdToken,
                    Scope = "openid profile",
                    TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name"
                    },
                    Notifications = new OpenIdConnectAuthenticationNotifications
                    {
                        RedirectToIdentityProvider = notification =>
                        {
                            if (notification.ProtocolMessage.RequestType == OpenIdConnectRequestType.Logout)
                            {
                                var logoutUri = $"https://{auth0Domain}/v2/logout?client_id={auth0ClientId}";
                                var postLogoutUri = notification.ProtocolMessage.PostLogoutRedirectUri;
                                if (!string.IsNullOrEmpty(postLogoutUri))
                                {
                                    if (postLogoutUri.StartsWith("/"))
                                    {
                                        // transform to absolute
                                        var request = notification.Request;
                                        postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                                    }
                                    logoutUri += $"&returnTo={ Uri.EscapeDataString(postLogoutUri)}";
                                }

                                notification.Response.Redirect(logoutUri);
                                notification.HandleResponse();
                            }
                            return Task.FromResult(0);
                        }
                    }
                };
            }
        }
    }
}

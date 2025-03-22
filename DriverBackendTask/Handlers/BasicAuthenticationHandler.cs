using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Text;

namespace DriverBackendTask.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }


        /// <summary>
        /// Handles the authentication
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Authorization header missing.");
            }


            try
            {
                string authHeader = Request.Headers["Authorization"].ToString();
                if (authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
                {
                    string encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
                    string[] credentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials)).Split(':');

                    if (credentials.Length == 2)
                    {
                        string username = credentials[0];
                        string password = credentials[1];

                        // Validate credentials
                        if (IsValidUser(username, password))
                        {
                            var claims = new[] { new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, username) };
                            var identity = new System.Security.Claims.ClaimsIdentity(claims, Scheme.Name);
                            var principal = new System.Security.Claims.ClaimsPrincipal(identity);
                            var ticket = new AuthenticationTicket(principal, Scheme.Name);

                            return AuthenticateResult.Success(ticket);
                        }
                    }
                }

                return AuthenticateResult.Fail("Invalid Authorization header.");
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex);
            }
        }

        /// <summary>
        /// Check if user name and password matches the user name and password that we have in the constant
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>bool</returns>
        private bool IsValidUser(string username, string password)
        {
            string basicAuthenticationUser = ConfigurationHandler.AppSetting["BasicAuthentication:UserName"];
            string basicAuthenticationPassword = ConfigurationHandler.AppSetting["BasicAuthentication:Password"];
            return username == basicAuthenticationUser && password == basicAuthenticationPassword;
        }
    }
}

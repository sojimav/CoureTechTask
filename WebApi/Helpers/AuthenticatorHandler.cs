using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace WebApi.Helpers
{
	public class AuthenticatorHandler : AuthenticationHandler<AuthenticationSchemeOptions>
	{
		public AuthenticatorHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
			ILoggerFactory logger, UrlEncoder encoder,
			ISystemClock clock,
		    IConfiguration configuration) : base(options, logger, encoder, clock)
		{
		}

		protected override Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
			{
				return Task.FromResult(AuthenticateResult.Fail("Authorization header is missing."));
			}

			var apiKey = "SkFabTZibXE1aE14ckpQUUxHc2dnQ2RzdlFRTTM2NFE2cGI4d3RQNjZmdEFITmdBQkE=";
			if (string.IsNullOrEmpty(apiKey))
			{
				return Task.FromResult(AuthenticateResult.Fail("API key is not configured."));
			}

			if (authorizationHeader != $"Bearer {apiKey}")
			{
				return Task.FromResult(AuthenticateResult.Fail("Invalid API key."));
			}

			var identity = new ClaimsIdentity("ApiKey");
			var principal = new ClaimsPrincipal(identity);
			var ticket = new AuthenticationTicket(principal, "ApiKey");

			return Task.FromResult(AuthenticateResult.Success(ticket));
		}

	}
}

using Microsoft.AspNetCore.Authentication;
using WebApi.Helpers;

namespace WebApi.Configuration
{
	public class AuthoriseConfig : IServiceInstaller
	{
		public void Install(IServiceCollection services, IConfiguration configuration)
		{
			services.AddAuthentication("ApiKey")
		   .AddScheme<AuthenticationSchemeOptions, AuthenticatorHandler>("ApiKey", options => { });
		}
		
	}
}

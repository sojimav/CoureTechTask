using WebApi.DataAccess;
using WebApi.Interfaces.IDataAccess;
using WebApi.Interfaces.IRepository;
using WebApi.Repository;

namespace WebApi.Configuration
{
	public class InterfaceRegister : IServiceInstaller
	{
		public void Install(IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped<IPlayerRepo, PlayerRepo>();
			services.AddScoped<IPlayersDbAccess, PlayersDataAccess>();
			services.AddScoped<ITeamRepo, TeamRepo>();
		}
	}
}

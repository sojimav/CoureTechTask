using WebApi.DataAccess;
using WebApi.Interfaces.IDataAccess;
using WebApi.Interfaces.IRepository;
using WebApi.Repository;

namespace WebApi.Configuration
{
	public interface IServiceInstaller
	{
		void Install(IServiceCollection services, IConfiguration configuration);
	
	}
}

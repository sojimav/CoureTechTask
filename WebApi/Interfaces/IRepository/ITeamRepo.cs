using AspNetCoreHero.Results;
using WebApi.DataTransfer;

namespace WebApi.Interfaces.IRepository
{
	public interface ITeamRepo
	{
		Task<IResult<IEnumerable<PlayerDTO>>> SelectTeamPlayersAsync(List<TeamRequirementDTO> teamRequirements);
	}
}

using AspNetCoreHero.Results;
using WebApi.DataTransfer;
using WebApi.Entities;

namespace WebApi.Interfaces.IRepository
{
	public interface IPlayerRepo
	{
		Task<IResult<Player>> CreatePlayerAsync(PlayerDTO createPlayerDTO);
		Task<IResult<PlayerDTO>> UpdatePlayerAsync(PlayerDTO updatePlayer, int playerId);
		Task<IResult<string>> DeletePlayerAsync(int playerId);
		Task<IResult<IEnumerable<Player>>> GetAllPlayersAsync();
	}
}


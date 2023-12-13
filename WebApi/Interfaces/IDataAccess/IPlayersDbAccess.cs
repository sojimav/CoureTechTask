using Microsoft.EntityFrameworkCore.Storage;
using WebApi.Entities;

namespace WebApi.Interfaces.IDataAccess
{
    public interface IPlayersDbAccess
    {
        Task<IEnumerable<Player>> GetAllPlayersAsync();
        Task AddPlayersToDataBaseAsync(Player player);
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<Player> GetPlayerAsync(int playerId);
        Task<int> SaveChangesAsync();
        Task<int> DeleteFromDBAsync(Player player);
        Task<List<Player>> GetPlayersByPositionAndSkillAsync(string position, string skill);
        Task<List<Player>> GetBestPlayerByPositionAsync(string position, int numberOfPlayers);

		Task<List<Player>> GetPlayersByPositionAsync(string position);
	}
}

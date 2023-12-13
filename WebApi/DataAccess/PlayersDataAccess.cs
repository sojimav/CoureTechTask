using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Numerics;
using WebApi.DataTransfer;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Interfaces.IDataAccess;

namespace WebApi.DataAccess
{
    public class PlayersDataAccess : IPlayersDbAccess
	{
		private readonly DataContext _dataContext;

        public PlayersDataAccess(DataContext dataContext)
        {
			_dataContext = dataContext;
        }

        public async Task<IEnumerable<Player>> GetAllPlayersAsync()
		{
			return await _dataContext.Players.Include(col => col.PlayerSkills).ToListAsync();
		}

		public async Task AddPlayersToDataBaseAsync(Player player)
		{
			await _dataContext.AddAsync(player);
			await _dataContext.SaveChangesAsync();
		}

		public async Task<IDbContextTransaction> BeginTransactionAsync()
		{
			return await _dataContext.Database.BeginTransactionAsync();
		}

		public async Task<Player> GetPlayerAsync(int playerId)
		{
			return await _dataContext.Players.Where(row => row.Id == playerId).Include(col => col.PlayerSkills).FirstOrDefaultAsync();
		}

		public async Task<int> SaveChangesAsync()
		{
			return await _dataContext.SaveChangesAsync();
		}

		public async Task<int>DeleteFromDBAsync(Player player)
		{
			_dataContext.PlayerSkills.RemoveRange(player.PlayerSkills);
			_dataContext.Players.Remove(player);

		  return	await _dataContext.SaveChangesAsync();
		}

		public async Task<List<Player>> GetPlayersByPositionAndSkillAsync(string position, string skill)
		{
			return await _dataContext.Players.Where(p => p.Position == position && p.PlayerSkills.Any(s => s.Skill == skill)).Include(col => col.PlayerSkills).ToListAsync();

		}


		public async Task<List<Player>> GetBestPlayerByPositionAsync(string position, int numberOfPlayers)
		{
			return await _dataContext.Players
				.Where(p => p.Position == position).Include(col => col.PlayerSkills)
				.OrderByDescending(p => p.PlayerSkills
					.Max(s => s.Value)).Take(numberOfPlayers).ToListAsync(); 			
		}

		public async Task<List<Player>> GetPlayersByPositionAsync(string position)
		{
			return await _dataContext.Players
				.Where(p => p.Position == position)
				.ToListAsync();	
		}

	
	}
}

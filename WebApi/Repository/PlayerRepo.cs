using AspNetCoreHero.Results;
using WebApi.DataTransfer;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Interfaces.IDataAccess;
using WebApi.Interfaces.IRepository;

namespace WebApi.Repository
{
	public class PlayerRepo : IPlayerRepo
	{
		private readonly IPlayersDbAccess _dataAccess;


		public PlayerRepo(IPlayersDbAccess dataAccess)
		{
			_dataAccess = dataAccess;
		}

		public async Task<IResult<Player>> CreatePlayerAsync(PlayerDTO createPlayerDTO)
		{
			if (createPlayerDTO == null)
			{
				return Result<Player>.Fail("Invalid player data");
			}

			if (!Validation.IsValidPosition(createPlayerDTO.Position))
			{
				return Result<Player>.Fail($"Invalid position: {createPlayerDTO.Position}");
			}

			if (!Validation.AreSkillsUnique(createPlayerDTO.PlayerSkills))
			{
				var duplicateSkills = createPlayerDTO.PlayerSkills
					.GroupBy(skill => skill.Skill)
					.Where(group => group.Count() > 1)
					.Select(group => group.Key)
					.ToList();

				
				return Result<Player>.Fail($"Duplicate skills : {string.Join(", ", duplicateSkills)} : detected for player");
			}

			foreach (var playerSkill in createPlayerDTO.PlayerSkills)
			{
				if (!Validation.IsValidSkill(playerSkill.Skill))
				{
					return Result<Player>.Fail($"Invalid skill: {playerSkill.Skill}");
				}
			}

			var player = new Player()
			{
				Name = createPlayerDTO.Name,
				Position = createPlayerDTO.Position,
				PlayerSkills = createPlayerDTO.PlayerSkills
			.Select(x => new PlayerSkill { Skill = x.Skill, Value = x.Value })
			.ToList()

			};
		
				await _dataAccess.AddPlayersToDataBaseAsync(player);	
		    	return Result<Player>.Success(player);
		}


		public async Task<IResult<PlayerDTO>> UpdatePlayerAsync(PlayerDTO updatePlayer, int playerId)
		{
			var availablePlayer = await _dataAccess.GetPlayerAsync(playerId);

			if (availablePlayer == null)
			{
				return Result<PlayerDTO>.Fail($"Player with ID {playerId} not found");

			}

			if (updatePlayer is null)
			{
				return Result<PlayerDTO>.Fail("Invalid data entry!");
			}

			if (!Validation.IsValidPosition(updatePlayer.Position))
			{
				return Result<PlayerDTO>.Fail($"Invalid position: {updatePlayer.Position}");
			}

			if (!Validation.AreSkillsUnique(updatePlayer.PlayerSkills))
			{
				var duplicateSkills = updatePlayer.PlayerSkills
					.GroupBy(skill => skill.Skill)
					.Where(group => group.Count() > 1)
					.Select(group => group.Key)
					.ToList();

				return Result<PlayerDTO>.Fail($"Duplicate skills : {string.Join(", ", duplicateSkills)} : detected for player");
			}


			foreach (var playerSkill in updatePlayer.PlayerSkills)
			{
				if (!Validation.IsValidSkill(playerSkill.Skill))
				{
					return Result<PlayerDTO>.Fail($"Invalid skill: {playerSkill.Skill}");
				}
			}

			availablePlayer.Name = updatePlayer.Name ?? availablePlayer.Name;
			availablePlayer.Position = updatePlayer.Position ?? availablePlayer.Position;
			availablePlayer.PlayerSkills = updatePlayer.PlayerSkills.Select(x => new PlayerSkill
			{
				Skill = x.Skill,
				Value = x.Value,
				PlayerId = availablePlayer.Id,
			}).ToList();

			await _dataAccess.SaveChangesAsync();

			var updatedPlayerDto = new PlayerDTO
			{
				Name = availablePlayer.Name,
				Position = availablePlayer.Position,
				PlayerSkills = availablePlayer.PlayerSkills.Select(ps => new PlayerSkillsDTO
				{
					Skill = ps.Skill,
					Value = ps.Value
				}).ToList()
			};

			return Result<PlayerDTO>.Success(updatedPlayerDto);

		}


		public async Task<IResult<string>> DeletePlayerAsync(int playerId)
		{
			var foundPlayer = await _dataAccess.GetPlayerAsync(playerId);

			if (foundPlayer == null)
			{
				return Result<string>.Fail($"Player with ID {playerId} not found");
			}

			await _dataAccess.DeleteFromDBAsync(foundPlayer);

			return Result<string>.Success($"Player with ID {playerId} deleted sucessfully");

		}


		public async Task<IResult<IEnumerable<Player>>> GetAllPlayersAsync()
		{
			var allPlayers = await _dataAccess.GetAllPlayersAsync();
			if (!allPlayers.Any())
			{
				return Result<IEnumerable<Player>>.Fail("No players exist in the database!");
			}

			return Result<IEnumerable<Player>>.Success(allPlayers);		
		}

	}
}

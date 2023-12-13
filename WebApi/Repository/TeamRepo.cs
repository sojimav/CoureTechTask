using AspNetCoreHero.Results;
using System.Collections.Generic;
using WebApi.DataTransfer;
using WebApi.Helpers;
using WebApi.Interfaces.IDataAccess;
using WebApi.Interfaces.IRepository;

namespace WebApi.Repository
{
	public class TeamRepo : ITeamRepo
	{
		private readonly IPlayersDbAccess _dataAccess;

		public TeamRepo(IPlayersDbAccess dataAccess)
		{
			_dataAccess = dataAccess;
		}


		public async Task<IResult<IEnumerable<PlayerDTO>>> SelectTeamPlayersAsync(List<TeamRequirementDTO> teamRequirements)
		{
			var selectedPlayers = new List<PlayerDTO>();

			foreach (var requirement in teamRequirements)
			{
				var existingRequirement = teamRequirements.Count(r => r.Position == requirement.Position && r.MainSkill == requirement.MainSkill);
				if (existingRequirement > 1)
				{
					return Result<IEnumerable<PlayerDTO>>.Fail("Invalid team requirement: The same position and skill combination is allowed only once.");
				}

				var availablePlayers = await _dataAccess.GetPlayersByPositionAsync(requirement.Position);
				if (availablePlayers.Count < requirement.NumberOfPlayers)
				{
					return Result<IEnumerable<PlayerDTO>>.Fail($"Insufficient number of players for position: {requirement.Position}");
				}

				var players = await _dataAccess.GetPlayersByPositionAndSkillAsync(requirement.Position, requirement.MainSkill);

				if (players.Count == 0)
				{
					var bestPlayer = await _dataAccess.GetBestPlayerByPositionAsync(requirement.Position, requirement.NumberOfPlayers);
					if (bestPlayer.Any())
					{
						bestPlayer.ForEach(row =>
						{
							var playerDTO = new PlayerDTO
							{
								Name = row.Name,
								Position = row.Position,
								PlayerSkills = row.PlayerSkills.Select(ps => new PlayerSkillsDTO
								{
									Skill = ps.Skill,
									Value = ps.Value
								}).ToList()
							};

							selectedPlayers.Add(playerDTO);
						});
						
					}
				}
				else
				{
					var selectedPlayersForRequirement = players.OrderByDescending(p => p.PlayerSkills.FirstOrDefault(s => s.Skill == requirement.MainSkill)?.Value ?? 0)
						.Take(requirement.NumberOfPlayers)
						.Select(p => new PlayerDTO
					 {
						 Name = p.Name,
						 Position = p.Position,
						 PlayerSkills = p.PlayerSkills.Select(s => new PlayerSkillsDTO
						 {
							 Skill = s.Skill,
							 Value = s.Value
						 }).ToList()
					 }).ToList();

					selectedPlayers.AddRange(selectedPlayersForRequirement);
				}
			}

			 return Result<IEnumerable<PlayerDTO>>.Success(selectedPlayers);
		}

	}
}

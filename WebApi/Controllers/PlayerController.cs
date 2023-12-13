// /////////////////////////////////////////////////////////////////////////////
// YOU CAN FREELY MODIFY THE CODE BELOW IN ORDER TO COMPLETE THE TASK
// /////////////////////////////////////////////////////////////////////////////

namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Helpers;
using WebApi.Entities;
using WebApi.DataTransfer;
using WebApi.Interfaces.IRepository;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/player")]
public class PlayerController : ControllerBase
{
  private readonly DataContext Context;
	private readonly IPlayerRepo _playerRepo;

	public PlayerController(DataContext context, IPlayerRepo playerRepo)
	{
		Context = context;
		_playerRepo = playerRepo;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<Player>>> GetAll()
	{
	    var result = await _playerRepo.GetAllPlayersAsync();
		return (!result.Succeeded) ? NotFound(result) : Ok(result);
	}

	[HttpPost]
	public async Task<ActionResult<Player>> PostPlayer([FromBody] PlayerDTO createPlayerDTO)
	{
		var result =  await _playerRepo.CreatePlayerAsync(createPlayerDTO);
		return (!result.Succeeded) ? BadRequest(result) : Ok(result);
	}

	[HttpPut("{playerId}")]
	public async Task<IActionResult> PutPlayer(int playerId,[FromBody] PlayerDTO player)
	{
		var result = await _playerRepo.UpdatePlayerAsync(player, playerId);
		return (!result.Succeeded) ? BadRequest(result) : Ok(result);
	}

	[Authorize]
	[HttpDelete("{playerId}")]
	public async Task<ActionResult<Player>> DeletePlayer(int playerId)
	{
		var result = await _playerRepo.DeletePlayerAsync(playerId);
		return (!result.Succeeded) ? NotFound(result) : Ok(result);
	}

}







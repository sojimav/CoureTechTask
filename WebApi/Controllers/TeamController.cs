// /////////////////////////////////////////////////////////////////////////////
// YOU CAN FREELY MODIFY THE CODE BELOW IN ORDER TO COMPLETE THE TASK
// /////////////////////////////////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using WebApi.DataTransfer;
using WebApi.Helpers;
using WebApi.Interfaces.IRepository;

namespace WebApi.Controllers
{
	[ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly DataContext Context;
        private readonly ITeamRepo _teamRepo;

		public TeamController(DataContext context, ITeamRepo teamRepo)
		{
			Context = context;
			_teamRepo = teamRepo;
		}

		[HttpGet("process")]
        public async Task<ActionResult<IEnumerable<PlayerDTO>>> Process([FromBody] List<TeamRequirementDTO> teamRequirements)
        {
           var result = await  _teamRepo.SelectTeamPlayersAsync(teamRequirements);
			return (!result.Succeeded) ? BadRequest(result) : Ok(result);
		}

    }
}

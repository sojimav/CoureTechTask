using WebApi.Entities;

namespace WebApi.DataTransfer
{
	public class PlayerDTO
	{
		public string Name { get; set; }
		public string Position { get; set; }
		public List<PlayerSkillsDTO> PlayerSkills { get; set; }
	}
}

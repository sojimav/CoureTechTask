using WebApi.DataTransfer;

namespace WebApi.Helpers
{
	public class Validation
	{
		public static bool IsValidPosition(string position)
		{
			// Validate against the allowed positions
			var allowedPositions = new List<string> { "defender", "midfielder", "forward" };
			return allowedPositions.Contains(position.ToLower());
		}

		public static bool IsValidSkill(string skill)
		{
			// Validate against the allowed skills
			var allowedSkills = new List<string> { "defense", "attack", "speed", "strength", "stamina" };
			return allowedSkills.Contains(skill.ToLower());
		}

		public static bool AreSkillsUnique(List<PlayerSkillsDTO> playerSkills)
		{
			// Check for duplicate entries based on the skill name
			var uniqueSkillNames = new HashSet<string>();
			foreach (var skill in playerSkills)
			{
				if (!uniqueSkillNames.Add(skill.Skill))
				{
					// Duplicate skill found
					return false;
				}
			}

			return true;
		}



	}
}

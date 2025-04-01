using Portfolio.DTOs;

namespace Portfolio.Controllers;

public class SkillsController(ApplicationDbContext dbContext)
    : BaseApiController
{

    [HttpGet]
    public async Task<ActionResult<List<SkillDto>>> GetSkills()
    {
        
        var skills = await dbContext.Skills
            .Select(s => new SkillDto { Id = s.Id, Name = s.Name, Proficiency = s.Proficiency.ToString() })
            .ToListAsync();

        return Ok(skills);
    }

    [HttpPost]
    public async Task<IActionResult> AddSkill([FromForm] CreateSkillDto skillDto)
    {
        var skill = new Skill { Name = skillDto.Name, Proficiency = skillDto.Proficiency};
        dbContext.Skills.Add(skill);
        await dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSkills), new { id = skill.Id },
            new SkillDto { Id = skill.Id, Name = skill.Name, Proficiency = skill.Proficiency.ToString() });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateSkill(int id, [FromForm] CreateSkillDto skillDto)
    {


        var skill = await dbContext.Skills.FirstOrDefaultAsync(s => s.Id == id);
        
        if (skill == null)
            return NotFound();


        skill.Name = skillDto.Name;
        skill.Proficiency = skillDto.Proficiency;

        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteSkill(int id)
    {
        
        var skill = await dbContext.Skills.FirstOrDefaultAsync(s => s.Id == id);
        if (skill == null)
            return NotFound();

        dbContext.Skills.Remove(skill);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}
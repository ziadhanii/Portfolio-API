namespace Portfolio.Controllers;

public class StacksController(ApplicationDbContext context) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<List<StackDto>>> GetStacks()
    {
        var stacks = await context.Stacks
            .Include(s => s.Skills)
            .ToListAsync();

        var stackDtos = stacks.Select(s => new StackDto
        {
            Id = s.Id,
            Title = s.Title,
            Description = s.Description,
            Skills = s.Skills.Select(skill => new SkillDto
            {
                Id = skill.Id,
                Name = skill.Name,
                Proficiency = skill.Proficiency.ToString()
            }).ToList()
        }).ToList();

        return Ok(stackDtos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StackDto>> GetStackById(int id)
    {
        var stack = await context.Stacks
            .Include(s => s.Skills)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (stack == null)
            return NotFound($"Stack with ID {id} not found");

        var stackDto = new StackDto
        {
            Id = stack.Id,
            Title = stack.Title,
            Description = stack.Description,
            Skills = stack.Skills.Select(s => new SkillDto
            {
                Id = s.Id,
                Name = s.Name,
                Proficiency = s.Proficiency.ToString()
            }).ToList()
        };

        return Ok(stackDto);
    }

    [HttpGet("by-title/{title}/skills")]
    public async Task<ActionResult<List<SkillDto>>> GetSkillsByStackTitle(string title)
    {
        var stack = await context.Stacks
            .Include(s => s.Skills)
            .FirstOrDefaultAsync(s => s.Title.ToLower() == title.ToLower());

        if (stack == null)
            return NotFound($"Stack with title '{title}' not found");

        var skills = stack.Skills.Select(s => new SkillDto
        {
            Id = s.Id,
            Name = s.Name,
            Proficiency = s.Proficiency.ToString()
        }).ToList();

        return Ok(skills);
    }

    [HttpPost]
    public async Task<ActionResult<StackDto>> CreateStack([FromBody] CreateStackDto createStackDto)
    {
        if (await context.Stacks.AnyAsync(s => s.Title.ToLower() == createStackDto.Title.ToLower()))
            return BadRequest("A stack with this title already exists");

        var skills = await context.Skills
            .Where(s => createStackDto.SkillIds.Contains(s.Id))
            .ToListAsync();

        if (skills.Count != createStackDto.SkillIds.Count)
            return BadRequest("One or more skill IDs are invalid");

        var stack = new Stack
        {
            Title = createStackDto.Title,
            Description = createStackDto.Description,
            Skills = skills
        };

        context.Stacks.Add(stack);
        await context.SaveChangesAsync();

        var stackDto = new StackDto
        {
            Id = stack.Id,
            Title = stack.Title,
            Description = stack.Description,
            Skills = stack.Skills.Select(s => new SkillDto
            {
                Id = s.Id,
                Name = s.Name,
                Proficiency = s.Proficiency.ToString()
            }).ToList()
        };

        return CreatedAtAction(nameof(GetStackById), new { id = stack.Id }, stackDto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateStack(int id, [FromBody] CreateStackDto updateStackDto)
    {
        var stack = await context.Stacks
            .Include(s => s.Skills)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (stack == null)
            return NotFound($"Stack with ID {id} not found");

        if (await context.Stacks.AnyAsync(s => s.Id != id && s.Title.ToLower() == updateStackDto.Title.ToLower()))
            return BadRequest("A stack with this title already exists");

        var skills = await context.Skills
            .Where(s => updateStackDto.SkillIds.Contains(s.Id))
            .ToListAsync();

        if (skills.Count != updateStackDto.SkillIds.Count)
            return BadRequest("One or more skill IDs are invalid");

        stack.Title = updateStackDto.Title;
        stack.Description = updateStackDto.Description;
        stack.Skills = skills;

        await context.SaveChangesAsync();
        return Ok("Stack updated successfully");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteStack(int id)
    {
        var stack = await context.Stacks.FindAsync(id);
        if (stack == null)
            return NotFound($"Stack with ID {id} not found");

        context.Stacks.Remove(stack);
        await context.SaveChangesAsync();
        return Ok("Stack deleted successfully");
    }

    [HttpPost("{stackId:int}/skills")]
    public async Task<IActionResult> AddSkillsToStack(int stackId, [FromBody] AddSkillsToStackRequest request)
    {
        var stack = await context.Stacks
            .Include(s => s.Skills)
            .FirstOrDefaultAsync(s => s.Id == stackId);

        if (stack is null)
            return NotFound(new { message = $"Stack with ID {stackId} not found." });

        if (request.SkillIds is null || request.SkillIds.Count == 0)
            return BadRequest(new { message = "SkillIds list cannot be empty." });

        var skills = await context.Skills
            .Where(s => request.SkillIds.Contains(s.Id))
            .ToListAsync();

        var invalidSkillIds = request.SkillIds.Except(skills.Select(s => s.Id)).ToList();

        if (invalidSkillIds.Count != 0)
            return BadRequest(new { message = "Some skill IDs are invalid.", invalidSkillIds });

        var addedSkills = new List<int>();

        foreach (var skill in skills.Where(skill => stack.Skills.All(s => s.Id != skill.Id)))
        {
            stack.Skills.Add(skill);
            addedSkills.Add(skill.Id);
        }

        if (addedSkills.Count == 0)
            return Ok(new { message = "No new skills were added. All skills already exist in the stack." });

        await context.SaveChangesAsync();

        return Ok(new
        {
            message = "Skills added successfully.",
            addedSkillIds = addedSkills
        });
    }

    [HttpPut("{stackId:int}/skills")]
    public async Task<ActionResult> UpdateStackSkills(int stackId, [FromBody] List<int> skillIds)
    {
        var stack = await context.Stacks
            .Include(s => s.Skills)
            .FirstOrDefaultAsync(s => s.Id == stackId);

        if (stack == null)
            return NotFound($"Stack with ID {stackId} not found");

        var skills = await context.Skills
            .Where(s => skillIds.Contains(s.Id))
            .ToListAsync();

        if (skills.Count != skillIds.Count)
            return BadRequest("One or more skill IDs are invalid");

        stack.Skills.Clear();
        stack.Skills = skills;

        await context.SaveChangesAsync();
        return Ok("Stack skills updated successfully");
    }

    [HttpDelete("{stackId:int}/skills/{skillId:int}")]
    public async Task<IActionResult> RemoveSkillFromStack(int stackId, int skillId)
    {
        var stack = await context.Stacks
            .Include(s => s.Skills)
            .FirstOrDefaultAsync(s => s.Id == stackId);

        if (stack is null)
            return NotFound(new { message = $"Stack with ID {stackId} not found." });

        var skill = stack.Skills.FirstOrDefault(s => s.Id == skillId);

        if (skill is null)
            return NotFound(new { message = $"Skill with ID {skillId} not found in the stack." });

        stack.Skills.Remove(skill);
        await context.SaveChangesAsync();

        return Ok(new { message = "Skill removed successfully.", removedSkillId = skillId });
    }
}
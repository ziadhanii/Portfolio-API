using AutoMapper;
using Portfolio.DTOs;

namespace Portfolio.Controllers;

public class TechnologiesController(ApplicationDbContext context, IMapper mapper) : BaseApiController
{
    [HttpGet("")]
    public async Task<ActionResult<List<TechnologyResponseDto>>> GetTechnologies()
    {
        var technologies = await context.Technologies
            .AsNoTracking()
            .ToListAsync();
        return Ok(mapper.Map<List<TechnologyResponseDto>>(technologies));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TechnologyResponseDto>> GetTechnology(int id)
    {
        var technology = await context.Technologies
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);
        return technology == null
            ? NotFound("Technology not found.")
            : Ok(mapper.Map<TechnologyResponseDto>(technology));
    }

    [HttpPost("")]
    public async Task<ActionResult<TechnologyResponseDto>> CreateTechnology(TechnologyCreateDto technologyDto)
    {
        try
        {
            var technology = mapper.Map<Technology>(technologyDto);
            await context.Technologies.AddAsync(technology);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTechnology), new { id = technology.Id },
                mapper.Map<TechnologyResponseDto>(technology));
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to create technology: {ex.Message}");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTechnology(int id, TechnologyCreateDto technologyDto)
    {
        var technology = await context.Technologies.FindAsync(id);
        if (technology == null) return NotFound("Technology not found.");

        technology.Name = technologyDto.Name;

        try
        {
            await context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to update technology: {ex.Message}");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTechnology(int id)
    {
        var technology = await context.Technologies.FindAsync(id);
        if (technology == null) return NotFound("Technology not found.");

        try
        {
            context.Technologies.Remove(technology);
            await context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to delete technology: {ex.Message}");
        }
    }

    [HttpPost("add-technology-to-project")]
    public async Task<IActionResult> AddTechnologyToProject(ProjectTechnologyDto projectTechnologyDto)
    {
        var project = await context.Projects.AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == projectTechnologyDto.ProjectId);
        if (project == null) return NotFound("Project not found.");

        var technology = await context.Technologies.AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == projectTechnologyDto.TechnologyId);
        if (technology == null) return NotFound("Technology not found.");

        var exists = await context.ProjectTechnologies
            .AsNoTracking()
            .AnyAsync(pt =>
                pt.ProjectId == projectTechnologyDto.ProjectId && pt.TechnologyId == projectTechnologyDto.TechnologyId);

        if (exists) return BadRequest("Technology already added to the project.");

        var projectTechnology = new ProjectTechnology
        {
            ProjectId = projectTechnologyDto.ProjectId,
            TechnologyId = projectTechnologyDto.TechnologyId
        };

        try
        {
            await context.ProjectTechnologies.AddAsync(projectTechnology);
            await context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to add technology to project: {ex.Message}");
        }
    }

    [HttpDelete("remove-technology-from-project")]
    public async Task<IActionResult> RemoveTechnologyFromProject(ProjectTechnologyDto projectTechnologyDto)
    {
        var projectTechnology = await context.ProjectTechnologies
            .FirstOrDefaultAsync(
                pt => pt.ProjectId == projectTechnologyDto.ProjectId &&
                      pt.TechnologyId == projectTechnologyDto.TechnologyId);

        if (projectTechnology == null) return NotFound("Technology is not associated with this project.");

        try
        {
            context.ProjectTechnologies.Remove(projectTechnology);
            await context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to remove technology from project: {ex.Message}");
        }
    }
}
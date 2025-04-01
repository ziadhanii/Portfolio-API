using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Portfolio.DTOs;
using Portfolio.Services;

namespace Portfolio.Controllers;

public class ProjectsController(ApplicationDbContext context, IMapper mapper, IPictureService pictureService)
    : BaseApiController
{

    [HttpGet]
    public async Task<ActionResult<List<ProjectDto>>> GetProjects()
    {
        var projects = await context.Projects
            .Include(p => p.ProjectTechnologies)
            .ThenInclude(pt => pt.Technology)
            .ToListAsync();

        return Ok(mapper.Map<List<ProjectDto>>(projects));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProjectDto>> GetProject(int id)
    {
        var project = await context.Projects
            .Include(p => p.ProjectTechnologies)
            .ThenInclude(pt => pt.Technology)
            .FirstOrDefaultAsync(p => p.Id == id);

        return project == null ? NotFound() : Ok(mapper.Map<ProjectDto>(project));
    }

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> CreateProject([FromForm] CreateProjectDto projectDto)
    {
        var project = mapper.Map<Project>(projectDto);
        project.CreatedAt = DateTime.UtcNow;
        project.ImageUrl = await pictureService.SavePicture(projectDto.Image, Request);

        // Add technologies to the project
        if (projectDto.TechnologyIds?.Any() == true)
        {
            var technologies = await context.Technologies
                .Where(t => projectDto.TechnologyIds.Contains(t.Id))
                .ToListAsync();

            project.ProjectTechnologies = technologies.Select(t => new ProjectTechnology
            {
                Project = project,
                Technology = t
            }).ToList();
        }

        context.Projects.Add(project);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProject), new { id = project.Id }, mapper.Map<ProjectDto>(project));
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProject(int id, [FromForm] UpdateProjectDto projectDto)
    {
        var project = await context.Projects.FindAsync(id);
        if (project == null) return NotFound();



        mapper.Map(projectDto, project);

        if (projectDto.Image is not null)
            project.ImageUrl = await pictureService.UpdatePictureAsync(projectDto.Image, project.ImageUrl, Request);

        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var project = await context.Projects.FindAsync(id);
        if (project == null)
            return NotFound();


        if (!string.IsNullOrEmpty(project.ImageUrl))
            await pictureService.DeletePictureAsync(project.ImageUrl);

        context.Projects.Remove(project);
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("technologies-in-project/{projectId:int}")]
    public async Task<ActionResult<List<TechnologyResponseDto>>>
        GetTechnologiesInProject(int projectId)
    {
        var project = await context.Projects
            .Include(p => p.ProjectTechnologies)
            .ThenInclude(pt => pt.Technology)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        if (project == null)
            return NotFound();

        return Ok(mapper.Map<List<TechnologyResponseDto>>(project.ProjectTechnologies.Select(pt => pt.Technology)));
    }

    [HttpGet("available-technologies")]
    public async Task<ActionResult<List<TechnologyResponseDto>>> GetAvailableTechnologies()
    {
        var technologies = await context.Technologies
            .OrderBy(t => t.Name)
            .ToListAsync();

        return Ok(mapper.Map<List<TechnologyResponseDto>>(technologies));
    }
}
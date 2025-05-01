namespace Portfolio.DTOs;

public class TechnologyResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Color { get; set; } = "#000000";
}

public class ProjectTechnologyDto
{
    public int TechnologyId { get; set; }
    public int ProjectId { get; set; }
    
}

public class TechnologyCreateDto
{
    public string Name { get; set; }
    public string Color { get; set; } = "#000000";
}
public class TechnologyUpdateDto
{
    public string? Name { get; set; }
    public string? Color { get; set; } = "#000000";
}
namespace Portfolio.DTOs;

public class TechnologyResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class ProjectTechnologyDto
{
    public int TechnologyId { get; set; }
    public int ProjectId { get; set; }
    
}

public class TechnologyCreateDto
{
    public string Name { get; set; }
}
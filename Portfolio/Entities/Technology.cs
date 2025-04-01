namespace Portfolio.Entities;

public sealed class Technology
{
    public int Id { get; set; }
    public string Name { get; set; } 
    
    public ICollection<ProjectTechnology> ProjectTechnologies { get; set; } = new List<ProjectTechnology>();
}
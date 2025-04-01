namespace Portfolio.DTOs;

public class SkillDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Proficiency { get; set; }
}

public class CreateSkillDto
{
    public string Name { get; set; }
    public SkillLevel Proficiency { get; set; }
}
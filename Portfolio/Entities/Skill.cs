namespace Portfolio.Entities;

public class Skill
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public SkillLevel Proficiency { get; set; }
    

}

public enum SkillLevel
{
    Beginner,
    Intermediate,
    Advanced,
    Expert
}
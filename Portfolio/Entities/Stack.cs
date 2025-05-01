namespace Portfolio.Entities;

public class Stack
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<Skill> Skills { get; set; } = new HashSet<Skill>();
}
namespace Portfolio.DTOs;

public class StackDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<SkillDto> Skills { get; set; }
}

public class CreateStackDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<int> SkillIds { get; set; }
}

public class AddSkillsToStackRequest
{
    public List<int> SkillIds { get; set; } = [];
}
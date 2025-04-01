namespace Portfolio.Entities;

public class ProjectAnalytics
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; }
    
    public int ViewsCount { get; set; }
    public int VisitsCount { get; set; }
    public DateTime LastUpdated { get; set; }
    
    public ICollection<ProjectVisit> Visits { get; set; } = new List<ProjectVisit>();
}

public class ProjectVisit
{
    public int Id { get; set; }
    public int ProjectAnalyticsId { get; set; }
    public ProjectAnalytics ProjectAnalytics { get; set; }
    
    public DateTime VisitDate { get; set; }
    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
    public string Referrer { get; set; }
} 
namespace Portfolio.Entities;

public class WebsiteVisit
{
    [Key] public int Id { get; set; }

    [Required] public string PagePath { get; set; }

    [Required] public string PageTitle { get; set; }

    [Required] public DateTime VisitDate { get; set; }

    [Required] public string IpAddress { get; set; }

    public string UserAgent { get; set; }

    public string Referrer { get; set; }

    public int? TimeSpent { get; set; }
}

public class WebsiteAnalytics
{
    [Key] public int Id { get; set; }

    [Required] public string PagePath { get; set; }

    [Required] public string PageTitle { get; set; }

    public int ViewsCount { get; set; }

    public int UniqueVisitsCount { get; set; }

    public DateTime LastUpdated { get; set; }

    public virtual ICollection<WebsiteVisit> Visits { get; set; }
}
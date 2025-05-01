namespace Portfolio.Persistence;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options) :
    IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<Technology> Technologies { get; set; }
    public DbSet<ProjectTechnology> ProjectTechnologies { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<Stack> Stacks { get; set; }
    public DbSet<ProjectAnalytics> ProjectAnalytics { get; set; }
    public DbSet<ProjectVisit> ProjectVisits { get; set; }
    public DbSet<WebsiteAnalytics> WebsiteAnalytics { get; set; }
    public DbSet<WebsiteVisit> WebsiteVisits { get; set; }
    public DbSet<Certificate> Certificates { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.Entity<ProjectTechnology>()
            .HasKey(pt => new { pt.ProjectId, pt.TechnologyId });

        modelBuilder.Entity<ProjectTechnology>()
            .HasOne(pt => pt.Project)
            .WithMany(p => p.ProjectTechnologies)
            .HasForeignKey(pt => pt.ProjectId);

        modelBuilder.Entity<ProjectTechnology>()
            .HasOne(pt => pt.Technology)
            .WithMany(t => t.ProjectTechnologies)
            .HasForeignKey(pt => pt.TechnologyId);
        
        
        base.OnModelCreating(modelBuilder);
    }
}
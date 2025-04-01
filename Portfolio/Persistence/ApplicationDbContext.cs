namespace Portfolio.Persistence;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options) :
    IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<Technology> Technologies { get; set; }
    public DbSet<ProjectTechnology> ProjectTechnologies { get; set; }
    public DbSet<Skill> Skills { get; set; }
    

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

        modelBuilder.Entity<Technology>().HasData(
            new Technology { Id = 1, Name = "C#" },
            new Technology { Id = 2, Name = "ASP.NET Core" },
            new Technology { Id = 3, Name = "Entity Framework Core" },
            new Technology { Id = 4, Name = "Angular" },
            new Technology { Id = 5, Name = "React" },
            new Technology { Id = 6, Name = "Docker" },
            new Technology { Id = 7, Name = "Azure" },
            new Technology { Id = 8, Name = "SQL Server" }
        );
        base.OnModelCreating(modelBuilder);
    }
}
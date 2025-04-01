namespace Portfolio.Persistence.EntitiesConfigurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.Property(p => p.Title)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(p => p.ImageUrl)
            .IsRequired();

        builder.Property(p => p.ProjectUrl)
            .IsRequired(false);

        builder.Property(p => p.GitHubRepo)
            .IsRequired(false);

        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");
        
    }
}
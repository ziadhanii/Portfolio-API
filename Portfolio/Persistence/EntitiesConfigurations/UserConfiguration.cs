namespace Portfolio.Persistence.EntitiesConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.ShortBio)
            .HasMaxLength(200);

        builder.Property(u => u.Bio)
            .HasMaxLength(700);

        builder.Property(u => u.JobTitle)
            .HasMaxLength(100);

        builder.Property(u => u.Location)
            .HasMaxLength(100);

        builder.Property(u => u.LinkedInUrl)
            .HasMaxLength(200);

        builder.Property(u => u.GitHubUrl)
            .HasMaxLength(200);

        builder.Property(u => u.ProfileImageUrl)
            .HasMaxLength(200);

        builder.Ignore(u => u.FullName);

        builder.Property(u => u.DateCreated)
            .HasDefaultValueSql("GETUTCDATE()");
    }
}
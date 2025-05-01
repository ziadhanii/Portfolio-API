namespace Portfolio.Entities;

public class Certificate
{
    [Key] public int Id { get; set; }

    [Required] [MaxLength(100)] public string Title { get; set; }

    [Required] [MaxLength(200)] public string Issuer { get; set; }

    [Required] public DateTime? IssueDate { get; set; }
    
    [MaxLength(500)] public string? Description { get; set; }

    [MaxLength(200)] public string CertificateUrl { get; set; }

    [MaxLength(200)] public string ImageUrl { get; set; }
}
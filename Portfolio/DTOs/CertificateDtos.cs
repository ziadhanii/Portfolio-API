using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace Portfolio.DTOs;

public class CertificateDto
{
    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("title")] public string Title { get; set; }

    [JsonPropertyName("issuer")] public string Issuer { get; set; } 

    [JsonPropertyName("issue_date")] public DateTime? IssueDate { get; set; }
    
    [JsonPropertyName("description")] public string Description { get; set; }

    [JsonPropertyName("certificate_url")] public string CertificateUrl { get; set; }

    [JsonPropertyName("image_url")] public string ImageUrl { get; set; }
}

public class CreateCertificateDto
{
    [JsonPropertyName("title")] public string Title { get; set; }

    [JsonPropertyName("issuer")] public string Issuer { get; set; }

    [JsonPropertyName("issue_date")] public DateTime IssueDate { get; set; }

    [JsonPropertyName("description")] public string Description { get; set; }

    [JsonPropertyName("certificate_pdf")] public IFormFile CertificatePdf { get; set; }

    [JsonPropertyName("image")] public IFormFile Image { get; set; }
}

public class UpdateCertificateDto
{
    [JsonPropertyName("title")] public string Title { get; set; }

    [JsonPropertyName("issuer")] public string Issuer { get; set; }

    [JsonPropertyName("issue_date")] public DateTime IssueDate { get; set; }
    
    [JsonPropertyName("description")] public string Description { get; set; }

    [JsonPropertyName("certificate_pdf")] public IFormFile? CertificatePdf { get; set; }

    [JsonPropertyName("image")] public IFormFile? Image { get; set; }
}
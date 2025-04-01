using System;
using System.Text.Json.Serialization;

namespace Portfolio.DTOs
{
    public class CertificateDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("issuer")]
        public string Issuer { get; set; }

        [JsonPropertyName("issue_date")]
        public DateTime IssueDate { get; set; }

        [JsonPropertyName("expiry_date")]
        public DateTime? ExpiryDate { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("certificate_url")]
        public string CertificateUrl { get; set; }

        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }
    }

    public class CreateCertificateDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("issuer")]
        public string Issuer { get; set; }

        [JsonPropertyName("issue_date")]
        public DateTime IssueDate { get; set; }

        [JsonPropertyName("expiry_date")]
        public DateTime? ExpiryDate { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("certificate_url")]
        public string CertificateUrl { get; set; }

        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }
    }

    public class UpdateCertificateDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("issuer")]
        public string Issuer { get; set; }

        [JsonPropertyName("issue_date")]
        public DateTime IssueDate { get; set; }

        [JsonPropertyName("expiry_date")]
        public DateTime? ExpiryDate { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("certificate_url")]
        public string CertificateUrl { get; set; }

        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }
    }
} 
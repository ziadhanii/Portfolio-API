using Microsoft.EntityFrameworkCore;
using Portfolio.DTOs;
using Portfolio.Entities;

namespace Portfolio.Services
{
    public interface ICertificateService
    {
        Task<List<CertificateDto>> GetAllCertificatesAsync();
        Task<CertificateDto> GetCertificateByIdAsync(int id);
        Task<CertificateDto> CreateCertificateAsync(CreateCertificateDto createDto);
        Task<CertificateDto> UpdateCertificateAsync(int id, UpdateCertificateDto updateDto);
        Task<bool> DeleteCertificateAsync(int id);
        Task<List<CertificateDto>> GetActiveCertificatesAsync();
    }

    public class CertificateService : ICertificateService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CertificateService> _logger;

        public CertificateService(ApplicationDbContext context, ILogger<CertificateService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<CertificateDto>> GetAllCertificatesAsync()
        {
            var certificates = await _context.Certificates
                .OrderByDescending(c => c.IssueDate)
                .ToListAsync();

            return certificates.Select(c => new CertificateDto
            {
                Id = c.Id,
                Title = c.Title,
                Issuer = c.Issuer,
                IssueDate = c.IssueDate,
                ExpiryDate = c.ExpiryDate,
                Description = c.Description,
                CertificateUrl = c.CertificateUrl,
                ImageUrl = c.ImageUrl,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                IsActive = c.IsActive
            }).ToList();
        }

        public async Task<CertificateDto> GetCertificateByIdAsync(int id)
        {
            var certificate = await _context.Certificates.FindAsync(id);
            if (certificate == null)
                return null;

            return new CertificateDto
            {
                Id = certificate.Id,
                Title = certificate.Title,
                Issuer = certificate.Issuer,
                IssueDate = certificate.IssueDate,
                ExpiryDate = certificate.ExpiryDate,
                Description = certificate.Description,
                CertificateUrl = certificate.CertificateUrl,
                ImageUrl = certificate.ImageUrl,
                CreatedAt = certificate.CreatedAt,
                UpdatedAt = certificate.UpdatedAt,
                IsActive = certificate.IsActive
            };
        }

        public async Task<CertificateDto> CreateCertificateAsync(CreateCertificateDto createDto)
        {
            var certificate = new Certificate
            {
                Title = createDto.Title,
                Issuer = createDto.Issuer,
                IssueDate = createDto.IssueDate,
                ExpiryDate = createDto.ExpiryDate,
                Description = createDto.Description,
                CertificateUrl = createDto.CertificateUrl,
                ImageUrl = createDto.ImageUrl,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Certificates.Add(certificate);
            await _context.SaveChangesAsync();

            return new CertificateDto
            {
                Id = certificate.Id,
                Title = certificate.Title,
                Issuer = certificate.Issuer,
                IssueDate = certificate.IssueDate,
                ExpiryDate = certificate.ExpiryDate,
                Description = certificate.Description,
                CertificateUrl = certificate.CertificateUrl,
                ImageUrl = certificate.ImageUrl,
                CreatedAt = certificate.CreatedAt,
                UpdatedAt = certificate.UpdatedAt,
                IsActive = certificate.IsActive
            };
        }

        public async Task<CertificateDto> UpdateCertificateAsync(int id, UpdateCertificateDto updateDto)
        {
            var certificate = await _context.Certificates.FindAsync(id);
            if (certificate == null)
                return null;

            certificate.Title = updateDto.Title;
            certificate.Issuer = updateDto.Issuer;
            certificate.IssueDate = updateDto.IssueDate;
            certificate.ExpiryDate = updateDto.ExpiryDate;
            certificate.Description = updateDto.Description;
            certificate.CertificateUrl = updateDto.CertificateUrl;
            certificate.ImageUrl = updateDto.ImageUrl;
            certificate.IsActive = updateDto.IsActive;
            certificate.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new CertificateDto
            {
                Id = certificate.Id,
                Title = certificate.Title,
                Issuer = certificate.Issuer,
                IssueDate = certificate.IssueDate,
                ExpiryDate = certificate.ExpiryDate,
                Description = certificate.Description,
                CertificateUrl = certificate.CertificateUrl,
                ImageUrl = certificate.ImageUrl,
                CreatedAt = certificate.CreatedAt,
                UpdatedAt = certificate.UpdatedAt,
                IsActive = certificate.IsActive
            };
        }

        public async Task<bool> DeleteCertificateAsync(int id)
        {
            var certificate = await _context.Certificates.FindAsync(id);
            if (certificate == null)
                return false;

            _context.Certificates.Remove(certificate);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CertificateDto>> GetActiveCertificatesAsync()
        {
            var certificates = await _context.Certificates
                .Where(c => c.IsActive)
                .OrderByDescending(c => c.IssueDate)
                .ToListAsync();

            return certificates.Select(c => new CertificateDto
            {
                Id = c.Id,
                Title = c.Title,
                Issuer = c.Issuer,
                IssueDate = c.IssueDate,
                ExpiryDate = c.ExpiryDate,
                Description = c.Description,
                CertificateUrl = c.CertificateUrl,
                ImageUrl = c.ImageUrl,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                IsActive = c.IsActive
            }).ToList();
        }
    }
} 
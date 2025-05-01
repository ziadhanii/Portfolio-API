namespace Portfolio.Services;

public interface ICertificateService
{
    Task<List<CertificateDto>> GetAllCertificatesAsync();
    Task<CertificateDto> GetCertificateByIdAsync(int id);
    Task<CertificateDto> CreateCertificateAsync(CreateCertificateDto createDto, HttpRequest request);
    Task<CertificateDto> UpdateCertificateAsync(int id, UpdateCertificateDto updateDto, HttpRequest request);
    Task<bool> DeleteCertificateAsync(int id);
} 
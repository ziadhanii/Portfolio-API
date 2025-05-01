namespace Portfolio.Services;

public class CertificateService(ApplicationDbContext context, ILogger<CertificateService> logger, IPictureService pictureService, IPdfService pdfService)
    : ICertificateService
{
    public async Task<List<CertificateDto>> GetAllCertificatesAsync()
    {
        var certificates = await context.Certificates
            .OrderByDescending(c => c.IssueDate)
            .ToListAsync();

        return certificates.Select(c => new CertificateDto
        {
            Id = c.Id,
            Title = c.Title,
            Issuer = c.Issuer,
            IssueDate = c.IssueDate,
            Description = c.Description,
            CertificateUrl = c.CertificateUrl,
            ImageUrl = c.ImageUrl
        }).ToList();
    }

    public async Task<CertificateDto> GetCertificateByIdAsync(int id)
    {
        var certificate = await context.Certificates.FindAsync(id);
        if (certificate == null)
            return null;

        return new CertificateDto
        {
            Id = certificate.Id,
            Title = certificate.Title,
            Issuer = certificate.Issuer,
            IssueDate = certificate.IssueDate,
            Description = certificate.Description,
            CertificateUrl = certificate.CertificateUrl,
            ImageUrl = certificate.ImageUrl
        };
    }

    public async Task<CertificateDto> CreateCertificateAsync(CreateCertificateDto createDto, HttpRequest request)
    {
        var certificate = new Certificate
        {
            Title = createDto.Title,
            Issuer = createDto.Issuer,
            IssueDate = createDto.IssueDate,
            Description = createDto.Description,
            CertificateUrl = await pdfService.SavePdf(createDto.CertificatePdf, request),
            ImageUrl = await pictureService.SavePicture(createDto.Image, request)
        };

        context.Certificates.Add(certificate);
        await context.SaveChangesAsync();

        return new CertificateDto
        {
            Id = certificate.Id,
            Title = certificate.Title,
            Issuer = certificate.Issuer,
            IssueDate = certificate.IssueDate,
            Description = certificate.Description,
            CertificateUrl = certificate.CertificateUrl,
            ImageUrl = certificate.ImageUrl
        };
    }

    public async Task<CertificateDto> UpdateCertificateAsync(int id, UpdateCertificateDto updateDto, HttpRequest request)
    {
        var certificate = await context.Certificates.FindAsync(id);
        if (certificate == null)
            return null;

        certificate.Title = updateDto.Title;
        certificate.Issuer = updateDto.Issuer;
        certificate.IssueDate = updateDto.IssueDate;
        certificate.Description = updateDto.Description;

        if (updateDto.CertificatePdf != null)
            certificate.CertificateUrl = await pdfService.UpdatePdfAsync(updateDto.CertificatePdf, certificate.CertificateUrl, request);

        if (updateDto.Image != null)
            certificate.ImageUrl = await pictureService.UpdatePictureAsync(updateDto.Image, certificate.ImageUrl, request);

        await context.SaveChangesAsync();

        return new CertificateDto
        {
            Id = certificate.Id,
            Title = certificate.Title,
            Issuer = certificate.Issuer,
            IssueDate = certificate.IssueDate,
            Description = certificate.Description,
            CertificateUrl = certificate.CertificateUrl,
            ImageUrl = certificate.ImageUrl
        };
    }

    public async Task<bool> DeleteCertificateAsync(int id)
    {
        var certificate = await context.Certificates.FindAsync(id);
        if (certificate == null)
            return false;

        if (!string.IsNullOrEmpty(certificate.CertificateUrl))
            await pdfService.DeletePdfAsync(certificate.CertificateUrl);

        if (!string.IsNullOrEmpty(certificate.ImageUrl))
            await pictureService.DeletePictureAsync(certificate.ImageUrl);

        context.Certificates.Remove(certificate);
        await context.SaveChangesAsync();
        return true;
    }
    
    

}
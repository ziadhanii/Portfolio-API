namespace Portfolio.Services;

public interface IPdfService
{
    Task<string> SavePdf(IFormFile pdf, HttpRequest request);
    Task<string?> UpdatePdfAsync(IFormFile? newPdf, string? oldPdfUrl, HttpRequest request);
    Task<bool> DeletePdfAsync(string? pdfUrl);
} 
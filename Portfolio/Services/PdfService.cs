namespace Portfolio.Services;

public class PdfService : IPdfService
{
    private readonly string _pdfsPath;

    public PdfService(IWebHostEnvironment webHostEnvironment)
    {
        _pdfsPath = Path.Combine(webHostEnvironment.WebRootPath, FileSettings.PdfsPath);

        if (!Directory.Exists(_pdfsPath))
        {
            Directory.CreateDirectory(_pdfsPath);
        }
    }

    public async Task<string> SavePdf(IFormFile pdf, HttpRequest request)
    {
        var pdfName = $"{Guid.NewGuid()}{Path.GetExtension(pdf.FileName)}";
        var path = Path.Combine(_pdfsPath, pdfName);

        await using var stream = File.Create(path);
        await pdf.CopyToAsync(stream);

        return $"{request.Scheme}://{request.Host}/pdfs/{pdfName}";
    }

    public async Task<string?> UpdatePdfAsync(IFormFile? newPdf, string? oldPdfUrl, HttpRequest request)
    {
        if (newPdf is not null)
        {
            var newPdfUrl = await SavePdf(newPdf, request);

            if (!string.IsNullOrEmpty(oldPdfUrl))
            {
                var oldPdfPath = Path.Combine(_pdfsPath, Path.GetFileName(oldPdfUrl));
                if (File.Exists(oldPdfPath))
                {
                    File.Delete(oldPdfPath);
                }
            }

            return newPdfUrl;
        }

        return oldPdfUrl;
    }

    public async Task<bool> DeletePdfAsync(string? pdfUrl)
    {
        if (string.IsNullOrEmpty(pdfUrl))
            return false;

        var pdfPath = Path.Combine(_pdfsPath, Path.GetFileName(pdfUrl));

        if (File.Exists(pdfPath))
        {
            File.Delete(pdfPath);
            return true;
        }

        return false;
    }
} 
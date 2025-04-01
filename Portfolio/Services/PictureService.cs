namespace Portfolio.Services;

public class PictureService : IPictureService
{
    private readonly string _imagesPath;

    public PictureService(IWebHostEnvironment webHostEnvironment)
    {
        _imagesPath = Path.Combine(webHostEnvironment.WebRootPath, FileSettings.ImagesPath);

        if (!Directory.Exists(_imagesPath))
        {
            Directory.CreateDirectory(_imagesPath);
        }
    }

    public async Task<string> SavePicture(IFormFile picture, HttpRequest request)
    {
        var pictureName = $"{Guid.NewGuid()}{Path.GetExtension(picture.FileName)}";
        var path = Path.Combine(_imagesPath, pictureName);

        Console.WriteLine($"Saving file to: {path}"); // ✅ اطبع المسار عشان تتأكد

        await using var stream = File.Create(path);
        await picture.CopyToAsync(stream);

        return $"{request.Scheme}://{request.Host}/images/{pictureName}";
    }

    public async Task<string?> UpdatePictureAsync(IFormFile? newPicture, string? oldPictureUrl, HttpRequest request)
    {
        if (newPicture is not null)
        {
            var newPictureUrl = await SavePicture(newPicture, request);

            if (!string.IsNullOrEmpty(oldPictureUrl))
            {
                var oldPicturePath = Path.Combine(_imagesPath, Path.GetFileName(oldPictureUrl));
                if (File.Exists(oldPicturePath))
                {
                    File.Delete(oldPicturePath);
                }
            }

            return newPictureUrl;
        }

        return oldPictureUrl;
    }

    public async Task<bool> DeletePictureAsync(string? pictureUrl)
    {
        if (string.IsNullOrEmpty(pictureUrl))
            return false;

        var picturePath = Path.Combine(_imagesPath, Path.GetFileName(pictureUrl));

        if (File.Exists(picturePath))
        {
            File.Delete(picturePath);
            return true;
        }

        return false;
    }
}
public static class FileSettings
{
    public static string ImagesPath => "Images"; // بدل "/Images"
}
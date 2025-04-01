namespace Portfolio.Services;

public interface IPictureService
{
    Task<string> SavePicture(IFormFile picture, HttpRequest request);
    Task<string?> UpdatePictureAsync(IFormFile? newPicture, string? oldPictureUrl, HttpRequest request);
    Task<bool> DeletePictureAsync(string? pictureUrl);
}
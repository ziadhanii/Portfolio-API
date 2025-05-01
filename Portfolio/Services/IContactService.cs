using Portfolio.Models;

namespace Portfolio.Services;

public interface IContactService
{
    Task<bool> SendMessageAsync(Contact contact);
} 
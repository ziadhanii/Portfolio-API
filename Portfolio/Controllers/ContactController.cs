using Portfolio.Models;
using Portfolio.Services;

namespace Portfolio.Controllers;

public class ContactController(IContactService contactService) : BaseApiController
{
    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] Contact contact)
    {
        var result = await contactService.SendMessageAsync(contact);

        return result
            ? Ok(new { message = "Message sent successfully" })
            : StatusCode(500, new { message = "Failed to send message" });
    }
}
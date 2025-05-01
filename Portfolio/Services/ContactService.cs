using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Portfolio.Models;

namespace Portfolio.Services;

public class ContactService(IOptions<EmailSettings> emailSettings) : IContactService
{
    private readonly EmailSettings _emailSettings = emailSettings.Value;

    public async Task<bool> SendMessageAsync(Contact contact)
    {
        try
        {
            var adminMessage = new MimeMessage();
            adminMessage.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
            adminMessage.To.Add(new MailboxAddress("Portfolio Admin", _emailSettings.FromEmail));
            adminMessage.Subject = $"📥 New Message from {contact.FullName}";

            var adminBodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                <div style='font-family: Arial, sans-serif; color: #333;'>
                    <h2 style='color: #007BFF;'>📬 New Contact Form Submission</h2>
                    <p><strong>👤 Name:</strong> {contact.FullName}</p>
                    <p><strong>📧 Email:</strong> <a href='mailto:{contact.Email}'>{contact.Email}</a></p>
                    <p><strong>📝 Message:</strong></p>
                    <div style='border: 1px solid #ccc; padding: 10px; background-color: #f9f9f9; white-space: pre-line;'>
                        {contact.Message}
                    </div>
                    <p style='margin-top: 30px; font-size: 0.9em; color: #777;'>This message was submitted via your portfolio contact form.</p>
                </div>
            ",
                TextBody = $@"
New Contact Message

Name: {contact.FullName}
Email: {contact.Email}
Message:
{contact.Message}
            "
            };

            adminMessage.Body = adminBodyBuilder.ToMessageBody();
            
            var confirmationMessage = new MimeMessage();
            confirmationMessage.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
            confirmationMessage.To.Add(new MailboxAddress(contact.FullName, contact.Email));
            confirmationMessage.Subject = "✅ Thank you for contacting us";

            var confirmationBodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                <div style='font-family: Arial, sans-serif; color: #333;'>
                    <h2 style='color: #28a745;'>Thank You for Your Message!</h2>
                    <p>Hi <strong>{contact.FullName}</strong>,</p>
                    <p>Thank you for reaching out. I’ve received your message and will get back to you as soon as possible.</p>
                    <p style='margin-top: 20px;'>📩 <em>Your message:</em></p>
                    <blockquote style='margin: 0; padding: 10px; border-left: 4px solid #28a745; background-color: #f1f1f1; white-space: pre-line;'>
                        {contact.Message}
                    </blockquote>
                    <p style='margin-top: 30px;'>Best regards,<br><strong>Portfolio Admin</strong></p>
                </div>
            ",
                TextBody = $@"
Hi {contact.FullName},

Thank you for contacting me. I've received your message and will respond soon.

Your message:
{contact.Message}

Best regards,
Ziad Hany
            "
            };

            confirmationMessage.Body = confirmationBodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);

            await client.SendAsync(adminMessage);
            await client.SendAsync(confirmationMessage);

            await client.DisconnectAsync(true);

            return true;
        }
        catch
        {
            return false;
        }
    }
}
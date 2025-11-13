using MailKit.Net.Smtp;
using MimeKit;
using WorkingMVC.Interfaces;


public class EmailSender(IConfiguration config) : IEmailSender
{
    public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
    {
        var smtp = config.GetSection("SmtpSettings");

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("АТБ", smtp["UserName"]));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;

        message.Body = new TextPart("html") { Text = htmlBody };

        using var client = new SmtpClient();
        await client.ConnectAsync(smtp["Host"], int.Parse(smtp["Port"]), bool.Parse(smtp["EnableSsl"]));
        await client.AuthenticateAsync(smtp["UserName"], smtp["Password"]);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}

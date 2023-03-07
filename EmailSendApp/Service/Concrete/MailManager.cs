using EmailSendApp.Service.Abstract;
using System.Net.Mail;
using System.Net;

namespace EmailSendApp.Service.Concrete
{
    public class MailManager : IMailService
    {
        private readonly IConfiguration _configuration;
        public MailManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //To send an email to a single person
        public async Task SendMessageAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendMessageAsync(new[] { to }, subject, body, isBodyHtml);
        }

        //  To send an email to a plural person
        public async Task SendMessageAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage mail = new();
            mail.IsBodyHtml = isBodyHtml;
            foreach (var to in tos)
            {
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.From = new("fatihsaridag26@gmail.com", "Fatih Sarıdağ Mail Service", System.Text.Encoding.UTF8);

                SmtpClient smtp = new();
                smtp.Credentials = new NetworkCredential(_configuration["Mail:Username"], _configuration["Mail:Password"]);
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Host = _configuration["Mail:Host"];
                await smtp.SendMailAsync(mail);
            }
        }
    }

}

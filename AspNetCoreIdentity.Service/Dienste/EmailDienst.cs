using AspNetCoreIdentity.Core.OptionModell;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace AspNetCoreIdentity.Service.Dienste
{
    public class EmailDienst : IEmailDienst
    {
        private readonly EmailEinstellungen _emailEinstellungen;

        public EmailDienst(IOptions<EmailEinstellungen> options)
        {
            _emailEinstellungen = options.Value;
        }

        public async Task SendeZurücksetzenPasswortEmail(string? zurücksetzenPasswortEmailLink, string? ZurEmail)
        {
            if (_emailEinstellungen == null || string.IsNullOrEmpty(_emailEinstellungen.Email) ||
                string.IsNullOrEmpty(_emailEinstellungen.Host)) 
            {
                throw new InvalidOperationException("Die E-Mail-Einstellungen fehlen oder sind ungültig.");
            }
            var smtpClient = new SmtpClient
            {
                Host = _emailEinstellungen.Host,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Port = 587,
                Credentials = new NetworkCredential(_emailEinstellungen.Email, _emailEinstellungen.Passwort),
                EnableSsl = true
            };

            var mailNachricht = new MailMessage
            {
                From = new MailAddress(_emailEinstellungen.Email)
            };

            if (ZurEmail != null)
            {
                mailNachricht.To.Add(ZurEmail);
            }

            mailNachricht.Subject = "Localhost | Link zum Zurücksetzen des Passworts";
            mailNachricht.Body = @$"
                       <h4>Benutzer-Login-Formular</h4>
                       <p><a href='{zurücksetzenPasswortEmailLink}'>Link zur Erneuerung des Passworts</a></p>";
            mailNachricht.IsBodyHtml= true;

            await smtpClient.SendMailAsync(mailNachricht);

        }
    }
}

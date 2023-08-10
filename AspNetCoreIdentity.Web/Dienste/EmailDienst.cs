using AspNetCoreIdentity.Web.OptionModell;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace AspNetCoreIdentity.Web.Dienste
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
            var smtpClient = new SmtpClient();

            smtpClient.Host = _emailEinstellungen.Host;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(_emailEinstellungen.Email, _emailEinstellungen.Passwort);
            smtpClient.EnableSsl = true;

            var mailNachricht = new MailMessage();
            mailNachricht.From = new MailAddress(_emailEinstellungen.Email);
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

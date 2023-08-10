namespace AspNetCoreIdentity.Web.Dienste
{
    public interface IEmailDienst
    {
        Task SendeZurücksetzenPasswortEmail(string? zurücksetzenPasswortEmailLink, string? ZurEmail);
    }
}

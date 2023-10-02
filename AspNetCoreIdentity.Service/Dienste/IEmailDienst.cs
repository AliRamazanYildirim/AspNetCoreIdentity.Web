namespace AspNetCoreIdentity.Service.Dienste
{
    public interface IEmailDienst
    {
        Task SendeZurücksetzenPasswortEmail(string? zurücksetzenPasswortEmailLink, string? ZurEmail);
    }
}

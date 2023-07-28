namespace AspNetCoreIdentity.Web.AnsichtModelle
{
    public class AnmeldenAnsichtModell
    {
        public AnmeldenAnsichtModell()
        {
                
        }
        public AnmeldenAnsichtModell(string? benutzerName, string? email, string? telefonnummer, string? passwort)
        {
            BenutzerName = benutzerName;
            Email = email;
            Telefonnummer = telefonnummer;
            Passwort = passwort;
        }
        public string? BenutzerName { get; set; }
        public string? Email { get; set; }
        public string? Telefonnummer { get; set; }
        public string? Passwort { get; set; }
        public string? PasswortBestätigen { get; set; }
    }
}

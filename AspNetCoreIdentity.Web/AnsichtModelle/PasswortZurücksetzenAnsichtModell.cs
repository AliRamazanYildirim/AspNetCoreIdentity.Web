namespace AspNetCoreIdentity.Web.AnsichtModelle
{
    public class PasswortZurücksetzenAnsichtModell
    {
        public PasswortZurücksetzenAnsichtModell()
        {
                
        }
        public PasswortZurücksetzenAnsichtModell(string? passwort, string? passwortBestätigen)
        {
            Passwort = passwort;
            PasswortBestätigen = passwortBestätigen;
        }

        public string? Passwort { get; set; }
        public string? PasswortBestätigen { get; set; }
    }
}

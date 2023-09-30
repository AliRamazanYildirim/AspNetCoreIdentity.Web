namespace AspNetCoreIdentity.Core.AnsichtModelle
{
    public class EinloggenAnsichtModell
    {
        public EinloggenAnsichtModell()
        {
                
        }
        public EinloggenAnsichtModell(string? email, string? passwort, bool errinnereMich)
        {
            Email = email;
            Passwort = passwort;
            ErrinnereMich = errinnereMich;
        }

        public string? Email { get; set; }
        public string? Passwort { get; set; }
        public bool ErrinnereMich { get; set; }

    }
}

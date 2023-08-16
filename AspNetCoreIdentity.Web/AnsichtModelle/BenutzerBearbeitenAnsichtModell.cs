namespace AspNetCoreIdentity.Web.AnsichtModelle
{
    public class BenutzerBearbeitenAnsichtModell
    {
        public string? BenutzerName { get; set; }
        public string? Email { get; set; }
        public string? Telefonnummer { get; set; }
        public string? Stadt { get; set; }
        public IFormFile? Bild { get; set; }
        public DateTime? Geburtsdatum { get; set; }
        public byte? Geschlecht { get; set; }
    }
}

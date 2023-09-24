namespace AspNetCoreIdentity.Web.BerechtigungenRoot
{
    public static class Berechtigungen
    {
        public static class Vorrat
        {
            public const string Lesen = "Berechtigung.Vorrat.Lesen";
            public const string Erstellen = "Berechtigung.Vorrat.Erstellen";
            public const string Aktualisieren = "Berechtigung.Vorrat.Aktualisieren";
            public const string Löschen = "Berechtigung.Vorrat.Löschen";
        }
        public static class Bestellung
        {
            public const string Lesen = "Berechtigung.Bestellung.Lesen";
            public const string Erstellen = "Berechtigung.Bestellung.Erstellen";
            public const string Aktualisieren = "Berechtigung.Bestellung.Aktualisieren";
            public const string Löschen = "Berechtigung.Bestellung.Löschen";
        }
        public static class Katalog
        {
            public const string Lesen = "Berechtigung.Katalog.Lesen";
            public const string Erstellen = "Berechtigung.Katalog.Erstellen";
            public const string Aktualisieren = "Berechtigung.Katalog.Aktualisieren";
            public const string Löschen = "Berechtigung.Katalog.Löschen";
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNetCoreIdentity.Web.Erweiterungen
{
    public static class ModelStateErweiterung
    {
        public static void AddModelStateFehlerListe(this ModelStateDictionary modelState, List<string> fehler)
        {
            fehler.ForEach(x =>
            {
                modelState.AddModelError(string.Empty, x);

            });
        }
        public static void AddModelStateFehlerListe(this ModelStateDictionary modelState, IEnumerable<IdentityError> fehler)
        {
            fehler.ToList().ForEach(x =>
            {
                modelState.AddModelError(string.Empty, x.Description);

            });
        }
    }
}

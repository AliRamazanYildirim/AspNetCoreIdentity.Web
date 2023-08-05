using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNetCoreIdentity.Web.Erweiterungen
{
    public static class ModelStateErweiterung
    {
        public static void AddModelStateFehler(this ModelStateDictionary modelState, List<string> fehler)
        {
            fehler.ForEach(x =>
            {
                modelState.AddModelError(string.Empty, x);

            });
        }
    }
}

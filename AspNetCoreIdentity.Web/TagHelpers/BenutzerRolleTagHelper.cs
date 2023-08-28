using AspNetCoreIdentity.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Diagnostics;
using System.Text;

namespace AspNetCoreIdentity.Web.TagHelpers
{
    public class BenutzerRolleTagHelper : TagHelper
    {
        private readonly UserManager<AppBenutzer> _userManager;

        public BenutzerRolleTagHelper(UserManager<AppBenutzer> userManager)
        {
            _userManager = userManager;
        }

        public string? BenutzerID { get; set; } = null!;
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var benutzer = await _userManager.FindByIdAsync(BenutzerID!);
            var benutzerRollen = await _userManager.GetRolesAsync(benutzer!);
            var stringBuilder = new StringBuilder();
            benutzerRollen.ToList().ForEach(x =>
            {
                stringBuilder.Append(@$"<span class='badge bg-warning mx-1'>{x.ToLower()}</span>");
            });

            output.Content.SetHtmlContent(stringBuilder.ToString());
        }
    }
}

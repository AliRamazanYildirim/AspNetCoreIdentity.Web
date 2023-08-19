using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AspNetCoreIdentity.Web.TagHelpers
{
    public class BenutzerBildTagHelper:TagHelper
    {
        public string? BildUrl { get; set; }
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";
            if(string.IsNullOrEmpty(BildUrl))
            {
                output.Attributes.SetAttribute("src", "/benutzerbilder/default_bild.png");
            }
            else
            {
                output.Attributes.SetAttribute("src", $"/benutzerbilder/{BildUrl}");
            }
            return base.ProcessAsync(context, output);

        }
    }
}

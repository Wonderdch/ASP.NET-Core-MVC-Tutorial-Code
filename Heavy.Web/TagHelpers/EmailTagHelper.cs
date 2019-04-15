using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

namespace Heavy.Web.TagHelpers
{
    public class EmailTagHelper : TagHelper
    {
        public string MailTo { get; set; }

        //public override void Process(
        //    TagHelperContext context,
        //    TagHelperOutput output)
        //{
        //    // Replaces <email> with <a> tag
        //    output.TagName = "a";
        //    output.Attributes.SetAttribute("href", $"mailto:{MailTo}");
        //    output.Content.SetContent(MailTo);
        //}

        public override async Task ProcessAsync(
            TagHelperContext context,
            TagHelperOutput output)
        {
            output.TagName = "a";
            var content = await output.GetChildContentAsync();
            var target = content.GetContent();
            output.Attributes.SetAttribute("href", $"mailto:{target}");
            output.Content.SetContent(target);
        }
    }
}
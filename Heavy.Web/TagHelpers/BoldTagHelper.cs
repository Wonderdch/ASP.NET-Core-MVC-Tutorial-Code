using Heavy.Web.TagHelpers.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Heavy.Web.TagHelpers
{
    [HtmlTargetElement("bold")]
    [HtmlTargetElement(Attributes = "bold")]
    public class BoldTagHelper : TagHelper
    {
        [HtmlAttributeName("my-style")]
        public MyStyle MyStyle { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.RemoveAll("bold");
            output.PreContent.SetHtmlContent("<strong>");
            output.PostContent.SetHtmlContent("</strong>");
            output.Attributes
                .SetAttribute("style", $"color: {MyStyle.Color}; font-size: {MyStyle.FontSize}; font-family: {MyStyle.FontFamily};");
        }
    }
}
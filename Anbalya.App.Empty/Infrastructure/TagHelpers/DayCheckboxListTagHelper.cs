using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Models.Helper;

namespace Anbalya.App.Empty.Infrastructure.TagHelpers;

[HtmlTargetElement("day-checkbox-list", Attributes = ForAttributeName + "," + OptionsAttributeName)]
public class DayCheckboxListTagHelper : TagHelper
{
    private const string ForAttributeName = "asp-for";
    private const string OptionsAttributeName = "options";
    private const string IdAttributeDotReplacement = "_";

    [HtmlAttributeName(ForAttributeName)]
    public ModelExpression For { get; set; } = default!;

    [HtmlAttributeName(OptionsAttributeName)]
    public IEnumerable<DayMaskOption> Options { get; set; } = Enumerable.Empty<DayMaskOption>();

    [HtmlAttributeName("item-class")]
    public string? ItemClass { get; set; }

    [HtmlAttributeName("input-class")]
    public string InputClass { get; set; } = "custom-control-input";

    [HtmlAttributeName("label-class")]
    public string LabelClass { get; set; } = "custom-control-label";

    [HtmlAttributeName("id-prefix")]
    public string? IdPrefix { get; set; }

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; } = default!;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var fullName = ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(For.Name);
        var selectedValues = GetSelectedValues();

        foreach (var option in Options ?? Enumerable.Empty<DayMaskOption>())
        {
            var wrapper = new TagBuilder("div");
            var wrapperClass = ItemClass ?? "custom-control custom-checkbox mr-3 mb-2";
            if (!string.IsNullOrWhiteSpace(wrapperClass))
            {
                wrapper.AddCssClass(wrapperClass);
            }

            var idBase = !string.IsNullOrWhiteSpace(IdPrefix)
                ? $"{IdPrefix}-{option.Index}"
                : $"{fullName}_{option.Index}";

            var checkboxId = TagBuilder.CreateSanitizedId(idBase, IdAttributeDotReplacement);

            var checkbox = new TagBuilder("input");
            checkbox.TagRenderMode = TagRenderMode.SelfClosing;
            checkbox.Attributes["type"] = "checkbox";
            checkbox.Attributes["id"] = checkboxId;
            checkbox.Attributes["name"] = fullName;
            checkbox.Attributes["value"] = option.Index.ToString();

            if (selectedValues.Contains(option.Index))
            {
                checkbox.Attributes["checked"] = "checked";
            }

            if (!string.IsNullOrWhiteSpace(InputClass))
            {
                checkbox.AddCssClass(InputClass);
            }

            var label = new TagBuilder("label");
            label.Attributes["for"] = checkboxId;
            if (!string.IsNullOrWhiteSpace(LabelClass))
            {
                label.AddCssClass(LabelClass);
            }
            label.InnerHtml.Append(option.DisplayName);

            wrapper.InnerHtml.AppendHtml(checkbox);
            wrapper.InnerHtml.AppendHtml(label);

            output.Content.AppendHtml(wrapper);
        }
    }

    private HashSet<int> GetSelectedValues()
    {
        if (For.Model is IEnumerable<int> enumerable)
        {
            return new HashSet<int>(enumerable);
        }

        if (For.Model is int single)
        {
            return new HashSet<int>(DayMaskHelper.ToSelectedDays(single));
        }

        return new HashSet<int>();
    }
}

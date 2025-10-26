using System;
using System.Text;

public partial record TourDto
{
    private string? _slugCache;

    public string Slug => _slugCache ??= GenerateSlug(TourName);

    private static string GenerateSlug(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "tour";
        }

        var normalized = value.ToLowerInvariant();
        var builder = new StringBuilder(normalized.Length);
        var previousIsHyphen = false;

        foreach (var ch in normalized)
        {
            if (char.IsLetterOrDigit(ch))
            {
                builder.Append(ch);
                previousIsHyphen = false;
            }
            else if (!previousIsHyphen)
            {
                builder.Append('-');
                previousIsHyphen = true;
            }
        }

        var slug = builder.ToString().Trim('-');
        return string.IsNullOrEmpty(slug) ? "tour" : slug;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.Helper
{
    public static class LanguageCatalog
    {
        private static readonly LanguageOption[] _options =
        {
            new("en", "English"),
            new("de", "Deutsch"),
            new("fa", "فارسی"),
            new("ru", "Русский"),
            new("pl", "Polski"),
            new("ar", "العربية")
        };

        public static IReadOnlyList<LanguageOption> Options => _options;

        public static string Normalize(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return "en";
            var trimmed = code.Trim();
            var hyphen = trimmed.IndexOf('-', StringComparison.Ordinal);
            if (hyphen > 0)
            {
                trimmed = trimmed[..hyphen];
            }

            trimmed = trimmed.ToLowerInvariant();
            return _options.Any(o => o.Code.Equals(trimmed, StringComparison.OrdinalIgnoreCase))
                ? trimmed
                : "en";
        }

        public static LanguageOption Find(string code)
        {
            var normalized = Normalize(code);
            return _options.First(o => o.Code.Equals(normalized, StringComparison.OrdinalIgnoreCase));
        }

        public static bool IsRtl(string code)
        {
            var normalized = Normalize(code);
            return normalized is "fa" or "ar";
        }
    }

    public record LanguageOption(string Code, string DisplayName);
}

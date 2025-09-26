using Models.Interface;

namespace Models.Helper
{
    public class LanguageResolver : ILanguageResolver
    {
        private static readonly string[] Supported = { "en", "de", "fa", "ru", "pl", "ar" };

        public string Resolve(HttpContext http)
        {
            // 1) امکان override با ?lang=de
            if (http.Request.Query.TryGetValue("lang", out var qs) && !string.IsNullOrWhiteSpace(qs))
            {
                var q = Normalize(qs!);
                return Supported.Contains(q) ? q : "en";
            }

            // 2) از Accept-Language
            var al = http.Request.Headers["Accept-Language"].ToString(); // e.g. "de-DE,de;q=0.9,en;q=0.8"
            if (!string.IsNullOrWhiteSpace(al))
            {
                var primary = al.Split(',').FirstOrDefault()?.Split(';')[0] ?? "";
                var code = Normalize(primary);
                if (Supported.Contains(code)) return code;
            }

            // 3) fallback
            return "en";
        }

        private static string Normalize(string input)
        {
            var s = input.Trim();
            var hyphen = s.IndexOf('-');
            if (hyphen > 0) s = s[..hyphen];
            return s.ToLowerInvariant(); // "de-DE" → "de"
        }
    }
}
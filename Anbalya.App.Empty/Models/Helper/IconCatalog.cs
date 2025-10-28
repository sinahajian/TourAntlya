using System.Collections.Generic;

namespace Models.Helper
{
    public static class IconCatalog
    {
        public record IconOption(string Class, string Label, string? Description = null);

        private static readonly IReadOnlyList<IconOption> _linearIcons = new List<IconOption>
        {
            new("lnr lnr-bus", "Shuttle Bus"),
            new("lnr lnr-car", "Private Transfer"),
            new("lnr lnr-flag", "Guided Tour"),
            new("lnr lnr-music-note", "Music & Entertainment"),
            new("lnr lnr-dinner", "Lunch Onboard"),
            new("lnr lnr-coffee-cup", "Drinks Included"),
            new("lnr lnr-sun", "Sun Deck"),
            new("lnr lnr-sunrise", "Sunrise Experience"),
            new("lnr lnr-rocket", "Adventure"),
            new("lnr lnr-leaf", "Eco Friendly"),
            new("lnr lnr-lifebuoy", "Safety First"),
            new("lnr lnr-users", "Family Friendly"),
            new("lnr lnr-heart", "Couples"),
            new("lnr lnr-bubble", "Live Commentary"),
            new("lnr lnr-film-play", "Show Experience"),
            new("lnr lnr-map", "City Discovery"),
            new("lnr lnr-wheelchair", "Accessible"),
            new("lnr lnr-boat", "Boat Cruise")
        };

        public static IReadOnlyList<IconOption> LinearIcons => _linearIcons;
    }
}

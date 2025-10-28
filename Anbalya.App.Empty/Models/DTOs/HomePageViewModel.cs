using System.Collections.Generic;

public class HomePageViewModel
{
    public LandingContentDto Hero { get; init; } = new LandingContentDto("en", "", "", "", null);
    public List<TourDto> Tours { get; init; } = new List<TourDto>();
    public string Language { get; init; } = "en";
    public IReadOnlyList<RoyalFacilityDto> Facilities { get; init; } = new List<RoyalFacilityDto>();
    public AboutContentDto About { get; init; } = new AboutContentDto(string.Empty, string.Empty, string.Empty, string.Empty, "#", "/image/about_bg.jpg");
}

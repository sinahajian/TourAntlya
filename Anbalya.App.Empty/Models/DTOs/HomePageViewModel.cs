using System.Collections.Generic;

public class HomePageViewModel
{
    public LandingContentDto Hero { get; init; } = new LandingContentDto("en", "", "", "", null);
    public List<TourDto> Tours { get; init; } = new List<TourDto>();
    public string Language { get; init; } = "en";
}

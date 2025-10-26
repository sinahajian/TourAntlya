using System.Collections.Generic;
using Models.Entities;

public record CategoryPageViewModel(
    Category Category,
    string DisplayName,
    IReadOnlyList<TourDto> Tours
);

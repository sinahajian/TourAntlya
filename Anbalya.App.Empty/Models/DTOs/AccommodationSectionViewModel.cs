using System.Collections.Generic;

public record AccommodationSectionViewModel(
    string Title,
    string Description,
    IReadOnlyList<global::TourDto> Tours,
    string RowClass
);

using Models.Entities;

public record ManagerTourListItemDto(
    int Id,
    string Name,
    Category Category,
    int Price,
    int KinderPrice,
    int InfantPrice,
    string? PrimaryPhoto,
    IReadOnlyList<string> Photos
);

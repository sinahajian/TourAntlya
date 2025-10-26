using Models.Entities;

public partial record TourDto(
    int Id,
    string TourName,
    int Price,
    int KinderPrice,
    int InfantPrice,
    float LocLat,
    float LocLon,
    Category Category,
    string CategoryString,
    string Description,
     string MiniDescription,
    int DurationHours,
    List<string> Services,
    string? Foto,
    IEnumerable<string> Fotos,
    int ActiveDay,
    string LanguageUsed       // ← مثلا "de" یا "en"
);

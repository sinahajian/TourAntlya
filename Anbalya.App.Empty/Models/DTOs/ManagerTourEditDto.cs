using System.Collections.Generic;
using Models.Entities;

public class ManagerTourEditDto
{
    public int Id { get; init; }
    public string TourName { get; set; } = string.Empty;
    public Category Category { get; set; }
    public int Price { get; set; }
    public int KinderPrice { get; set; }
    public int InfantPrice { get; set; }
    public int DurationHours { get; set; }
    public float LocLat { get; set; }
    public float LocLon { get; set; }
    public int ActiveDay { get; set; }
    public string DescriptionEn { get; set; } = string.Empty;
    public string DescriptionDe { get; set; } = string.Empty;
    public string DescriptionRu { get; set; } = string.Empty;
    public string DescriptionPo { get; set; } = string.Empty;
    public string DescriptionPe { get; set; } = string.Empty;
    public string DescriptionAr { get; set; } = string.Empty;
    public string MiniDescriptionEn { get; set; } = string.Empty;
    public string MiniDescriptionDe { get; set; } = string.Empty;
    public string MiniDescriptionRu { get; set; } = string.Empty;
    public string MiniDescriptionPo { get; set; } = string.Empty;
    public string MiniDescriptionPe { get; set; } = string.Empty;
    public string MiniDescriptionAr { get; set; } = string.Empty;
    public List<string> Services { get; set; } = new List<string>();
    public List<string> Photos { get; set; } = new List<string>();
    public string? PrimaryPhoto { get; set; }
}

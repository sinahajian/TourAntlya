using System.Collections.Generic;
using Models.Entities;

public class ManagerTourCreateDto
{
    public string TourName { get; set; } = string.Empty;
    public Category Category { get; set; }
    public int Price { get; set; }
    public int KinderPrice { get; set; }
    public int InfantPrice { get; set; }
    public int DurationHours { get; set; }
    public float LocLat { get; set; }
    public float LocLon { get; set; }
    public List<string> Services { get; set; } = new List<string>();
    public int ActiveDay { get; set; }
}

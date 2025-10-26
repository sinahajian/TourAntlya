using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Models.Helper;

public class ManagerTourCreateViewModel
{
    public string UserName { get; set; } = string.Empty;

    public string TourName { get; set; } = string.Empty;
    public int Category { get; set; }
    public int Price { get; set; }
    public int KinderPrice { get; set; }
    public int InfantPrice { get; set; }
    public int DurationHours { get; set; }
    public float LocLat { get; set; }
    public float LocLon { get; set; }
    public string ServicesText { get; set; } = string.Empty;
    public List<string> Services { get; set; } = new List<string>();
    public List<IFormFile> Photos { get; set; } = new List<IFormFile>();
    public List<int> SelectedDays { get; set; } = new List<int>();

    public IReadOnlyList<DayMaskOption> DayOptions => DayMaskHelper.Options;

    public void SyncServices()
    {
        var lines = ServicesText?
            .Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Trim())
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList() ?? new List<string>();

        Services = lines;
    }

    public void SyncSelectedDays(IEnumerable<int>? selected = null)
    {
        var source = selected ?? SelectedDays;
        SelectedDays = DayMaskHelper.ToSelectedDays(DayMaskHelper.FromSelectedDays(source));
    }

    public ManagerTourCreateDto ToDto()
    {
        SyncSelectedDays();
        return new ManagerTourCreateDto
        {
            TourName = TourName,
            Category = (Models.Entities.Category)Category,
            Price = Price,
            KinderPrice = KinderPrice,
            InfantPrice = InfantPrice,
            DurationHours = DurationHours,
            LocLat = LocLat,
            LocLon = LocLon,
            Services = Services ?? new List<string>(),
            ActiveDay = DayMaskHelper.FromSelectedDays(SelectedDays)
        };
    }
}

using System;
using System;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Models.Helper;

public class ManagerTourEditViewModel
{
    public ManagerTourEditViewModel()
    {
        Tour = new ManagerTourEditDto();
    }

    public ManagerTourEditViewModel(ManagerTourEditDto tour, string userName)
    {
        Tour = tour;
        UserName = userName;
        ServicesText = string.Join(Environment.NewLine, tour.Services ?? Enumerable.Empty<string>());
        ExistingPhotos = tour.Photos ?? new List<string>();
        PrimaryPhoto = tour.PrimaryPhoto;
        SelectedDays = DayMaskHelper.ToSelectedDays(tour.ActiveDay);
    }

    public string UserName { get; set; } = string.Empty;

    public ManagerTourEditDto Tour { get; set; }

    public string ServicesText { get; set; } = string.Empty;

    public List<string> ExistingPhotos { get; set; } = new List<string>();

    public string? PrimaryPhoto { get; set; }

    public List<IFormFile> NewPhotos { get; set; } = new List<IFormFile>();
    public List<int> SelectedDays { get; set; } = new List<int>();
    public IReadOnlyList<DayMaskOption> DayOptions => DayMaskHelper.Options;

    public void SyncServices()
    {
        if (Tour is null)
        {
            Tour = new ManagerTourEditDto();
        }

        var lines = ServicesText?
            .Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Trim())
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList() ?? new List<string>();

        Tour.Services = lines;
    }

    public void SyncSelectedDays(IEnumerable<int>? selected = null)
    {
        var source = selected ?? SelectedDays;
        SelectedDays = DayMaskHelper.ToSelectedDays(DayMaskHelper.FromSelectedDays(source));
    }

    public void ApplyActiveDay()
    {
        Tour.ActiveDay = DayMaskHelper.FromSelectedDays(SelectedDays);
    }
}

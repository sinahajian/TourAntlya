using System.Collections.Generic;
using System.Linq;
using Models.Entities;

public class ManagerDashboardViewModel
{
    private ManagerDashboardViewModel(
        ManagerDto manager,
        ReservationDashboardSummary summary,
        IReadOnlyList<DashboardCard> summaryCards,
        IReadOnlyList<ReservationDetailsDto> recentReservations)
    {
        Manager = manager;
        Summary = summary;
        SummaryCards = summaryCards;
        RecentReservations = recentReservations;
    }

    public ManagerDto Manager { get; }

    public ReservationDashboardSummary Summary { get; }

    public IReadOnlyList<DashboardCard> SummaryCards { get; }

    public IReadOnlyList<ReservationDetailsDto> RecentReservations { get; }

    public static ManagerDashboardViewModel Create(ManagerDto manager, IReadOnlyList<ReservationDetailsDto> reservations)
    {
        var summary = ReservationDashboardSummary.Create(reservations);
        var recentReservations = reservations
            .OrderByDescending(r => r.CreatedAt)
            .ThenByDescending(r => r.Id)
            .Take(8)
            .ToList();

        var cards = new List<DashboardCard>
        {
            new("Total reservations", "text-primary", summary.Total.ToString("N0"), "fa fa-ticket fa-2x text-gray-300", "border-left-primary"),
            new("Paid bookings", "text-success", $"{summary.Paid:N0} · €{summary.TotalRevenue:N0}", "fa fa-check-circle fa-2x text-gray-300", "border-left-success"),
            new("Pending payment", "text-warning", summary.Pending.ToString("N0"), "fa fa-clock-o fa-2x text-gray-300", "border-left-warning"),
            new("Cancelled / failed", "text-danger", (summary.Cancelled + summary.Failed).ToString("N0"), "fa fa-times-circle fa-2x text-gray-300", "border-left-danger")
        };

        return new ManagerDashboardViewModel(manager, summary, cards, recentReservations);
    }
}

public record DashboardCard(string Title, string AccentClass, string Value, string IconCss, string BorderCss);

public record ReservationDashboardSummary(
    int Total,
    int Pending,
    int Confirmed,
    int Cancelled,
    int Paid,
    int Failed,
    int TotalRevenue)
{
    public static ReservationDashboardSummary Create(IReadOnlyList<ReservationDetailsDto> reservations)
    {
        var pending = reservations.Count(r => r.PaymentStatus == PaymentStatus.Pending);
        var paid = reservations.Count(r => r.PaymentStatus == PaymentStatus.Paid);
        var failed = reservations.Count(r => r.PaymentStatus == PaymentStatus.Failed);
        var cancelled = reservations.Count(r => r.Status == ReservationStatus.Cancelled);
        var confirmed = reservations.Count(r => r.Status == ReservationStatus.Confirmed);
        var revenue = reservations.Where(r => r.PaymentStatus == PaymentStatus.Paid).Sum(r => r.TotalPrice);

        return new ReservationDashboardSummary(
            reservations.Count,
            pending,
            confirmed,
            cancelled,
            paid,
            failed,
            revenue);
    }
}

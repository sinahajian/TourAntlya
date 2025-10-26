using System.Collections.Generic;

public class ManagerDashboardViewModel
{
    private ManagerDashboardViewModel(
        ManagerDto manager,
        IReadOnlyList<DashboardCard> summaryCards,
        IReadOnlyList<ProgressItem> projects,
        IReadOnlyList<ColorCard> paletteCards)
    {
        Manager = manager;
        SummaryCards = summaryCards;
        Projects = projects;
        PaletteCards = paletteCards;
    }

    public ManagerDto Manager { get; }

    public IReadOnlyList<DashboardCard> SummaryCards { get; }

    public IReadOnlyList<ProgressItem> Projects { get; }

    public IReadOnlyList<ColorCard> PaletteCards { get; }

    public static ManagerDashboardViewModel Create(ManagerDto manager)
    {
        var cards = new List<DashboardCard>
        {
            new("Earnings (Monthly)", "text-primary", "$40,000", "fa fa-calendar fa-2x text-gray-300", "border-left-primary"),
            new("Earnings (Annual)", "text-success", "$215,000", "fa fa-money fa-2x text-gray-300", "border-left-success"),
            new("Tasks", "text-info", "50%", "fa fa-clipboard fa-2x text-gray-300", "border-left-info"),
            new("Pending Requests", "text-warning", "18", "fa fa-comments fa-2x text-gray-300", "border-left-warning")
        };

        var projects = new List<ProgressItem>
        {
            new("Server Migration", 20, "bg-danger"),
            new("Sales Tracking", 40, "bg-warning"),
            new("Customer Database", 60, "bg-primary"),
            new("Payout Details", 80, "bg-info"),
            new("Account Setup", 100, "bg-success")
        };

        var palette = new List<ColorCard>
        {
            new("Primary", "#4e73df", "bg-primary", true),
            new("Success", "#1cc88a", "bg-success", true),
            new("Info", "#36b9cc", "bg-info", true),
            new("Warning", "#f6c23e", "bg-warning", true),
            new("Danger", "#e74a3b", "bg-danger", true),
            new("Secondary", "#858796", "bg-secondary", true),
            new("Light", "#f8f9fc", "bg-light", false),
            new("Dark", "#5a5c69", "bg-dark", true)
        };

        return new ManagerDashboardViewModel(manager, cards, projects, palette);
    }
}

public record DashboardCard(string Title, string AccentClass, string Value, string IconCss, string BorderCss);

public record ProgressItem(string Title, int Percentage, string ProgressBarClass);

public record ColorCard(string Title, string HexCode, string BackgroundClass, bool UsesLightText);

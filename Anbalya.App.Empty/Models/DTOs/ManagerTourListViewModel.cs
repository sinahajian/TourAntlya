using System.Collections.Generic;

public class ManagerTourListViewModel
{
    public ManagerTourListViewModel(ManagerDto manager, IReadOnlyList<ManagerTourListItemDto> tours)
    {
        Manager = manager;
        Tours = tours;
    }

    public ManagerDto Manager { get; }

    public IReadOnlyList<ManagerTourListItemDto> Tours { get; }

    public string? FeedbackMessage { get; init; }
}

using System.Collections.Generic;
using System.Linq;
using Models.Entities;

public class ManagerReservationListViewModel
{
    public ManagerReservationListViewModel(
        ManagerDto manager,
        IReadOnlyList<ReservationDetailsDto> reservations,
        IReadOnlyList<ManagerTourListItemDto> tours,
        int? selectedTourId = null)
    {
        Manager = manager;
        Reservations = reservations;
        Tours = tours;
        SelectedTourId = selectedTourId;
    }

    public ManagerDto Manager { get; }
    public IReadOnlyList<ReservationDetailsDto> Reservations { get; }
    public IReadOnlyList<ManagerTourListItemDto> Tours { get; }
    public int? SelectedTourId { get; }
    public string? FeedbackMessage { get; set; }

    public int PendingCount => Reservations.Count(r => r.Status == ReservationStatus.Pending);
    public int ConfirmedCount => Reservations.Count(r => r.Status == ReservationStatus.Confirmed);
    public int PaidCount => Reservations.Count(r => r.PaymentStatus == PaymentStatus.Paid);
    public int CancelledCount => Reservations.Count(r => r.Status == ReservationStatus.Cancelled);
    public int FailedCount => Reservations.Count(r => r.PaymentStatus == PaymentStatus.Failed);
}

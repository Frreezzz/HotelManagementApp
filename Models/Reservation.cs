using System.ComponentModel.DataAnnotations;

namespace HotelManagementApp.Models;

public enum ReservationStatus
{
    New,
    Confirmed,
    CheckedIn,
    CheckedOut,
    Cancelled
}

public class Reservation
{
    public int Id { get; set; }
    public int GuestId { get; set; }
    public Guest? Guest { get; set; }

    public int RoomId { get; set; }
    public Room? Room { get; set; }

    public int? EmployeeId { get; set; }
    public Employee? Employee { get; set; }

    [Required]
    public DateTime CheckInDate { get; set; } = DateTime.Today;

    [Required]
    public DateTime CheckOutDate { get; set; } = DateTime.Today.AddDays(1);

    public ReservationStatus Status { get; set; } = ReservationStatus.New;
    public decimal TotalPrice { get; set; }

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<ReservationService> ReservationServices { get; set; } = new List<ReservationService>();
}

namespace HotelManagementApp.Models;

public class ReservationService
{
    public int ReservationId { get; set; }
    public Reservation? Reservation { get; set; }

    public int ServiceId { get; set; }
    public Service? Service { get; set; }

    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
}

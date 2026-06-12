using System.ComponentModel.DataAnnotations;

namespace HotelManagementApp.Models;

public enum RoomStatus
{
    Available,
    Occupied,
    Cleaning,
    Maintenance
}

public class Room
{
    public int Id { get; set; }

    [Required, MaxLength(20)]
    public string Number { get; set; } = string.Empty;

    public int Floor { get; set; }
    public RoomStatus Status { get; set; } = RoomStatus.Available;

    public int RoomTypeId { get; set; }
    public RoomType? RoomType { get; set; }

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}

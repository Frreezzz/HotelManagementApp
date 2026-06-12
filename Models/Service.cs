using System.ComponentModel.DataAnnotations;

namespace HotelManagementApp.Models;

public class Service
{
    public int Id { get; set; }

    [Required, MaxLength(80)]
    public string Name { get; set; } = string.Empty;

    [Range(0, 100000)]
    public decimal Price { get; set; }

    public ICollection<ReservationService> ReservationServices { get; set; } = new List<ReservationService>();
}

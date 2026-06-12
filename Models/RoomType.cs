using System.ComponentModel.DataAnnotations;

namespace HotelManagementApp.Models;

public class RoomType
{
    public int Id { get; set; }

    [Required, MaxLength(60)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(250)]
    public string Description { get; set; } = string.Empty;

    [Range(1, 10)]
    public int Capacity { get; set; }

    [Range(0, 100000)]
    public decimal PricePerNight { get; set; }

    public ICollection<Room> Rooms { get; set; } = new List<Room>();
}

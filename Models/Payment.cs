using System.ComponentModel.DataAnnotations;

namespace HotelManagementApp.Models;

public enum PaymentMethod
{
    Cash,
    Card,
    BankTransfer
}

public class Payment
{
    public int Id { get; set; }
    public int ReservationId { get; set; }
    public Reservation? Reservation { get; set; }

    [Range(0.01, 1000000)]
    public decimal Amount { get; set; }

    public PaymentMethod Method { get; set; } = PaymentMethod.Card;
    public DateTime PaidAt { get; set; } = DateTime.Now;
}

using HotelManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Data;

public static class DbSeeder
{
    public static void Seed(HotelDbContext db)
    {
        if (db.RoomTypes.Any()) return;

        var single = new RoomType { Name = "Pokój jednoosobowy", Capacity = 1, PricePerNight = 180, Description = "Standardowy pokój dla jednej osoby" };
        var doubleRoom = new RoomType { Name = "Pokój dwuosobowy", Capacity = 2, PricePerNight = 280, Description = "Pokój z łóżkiem podwójnym" };
        var apartment = new RoomType { Name = "Apartament", Capacity = 4, PricePerNight = 520, Description = "Apartament rodzinny o podwyższonym standardzie" };
        db.RoomTypes.AddRange(single, doubleRoom, apartment);
        db.SaveChanges();

        db.Rooms.AddRange(
            new Room { Number = "101", Floor = 1, RoomTypeId = single.Id },
            new Room { Number = "102", Floor = 1, RoomTypeId = doubleRoom.Id },
            new Room { Number = "201", Floor = 2, RoomTypeId = apartment.Id },
            new Room { Number = "202", Floor = 2, RoomTypeId = doubleRoom.Id, Status = RoomStatus.Cleaning }
        );

        db.Guests.AddRange(
            new Guest { FirstName = "Anna", LastName = "Kowalska", Email = "anna.kowalska@example.com", Phone = "500100200", DocumentNumber = "ABC123456" },
            new Guest { FirstName = "Piotr", LastName = "Nowak", Email = "piotr.nowak@example.com", Phone = "600200300", DocumentNumber = "XYZ987654" }
        );

        db.Employees.AddRange(
            new Employee { FirstName = "Maria", LastName = "Wiśniewska", Position = "Recepcjonistka" },
            new Employee { FirstName = "Tomasz", LastName = "Zieliński", Position = "Manager" }
        );

        db.Services.AddRange(
            new Service { Name = "Śniadanie", Price = 45 },
            new Service { Name = "Parking", Price = 35 },
            new Service { Name = "Spa", Price = 120 }
        );

        db.SaveChanges();

        var room = db.Rooms.Include(r => r.RoomType).First(r => r.Number == "102");
        var guest = db.Guests.First();
        var employee = db.Employees.First();
        var reservation = new Reservation
        {
            GuestId = guest.Id,
            RoomId = room.Id,
            EmployeeId = employee.Id,
            CheckInDate = DateTime.Today,
            CheckOutDate = DateTime.Today.AddDays(2),
            Status = ReservationStatus.Confirmed,
            TotalPrice = room.RoomType!.PricePerNight * 2
        };
        db.Reservations.Add(reservation);
        db.SaveChanges();

        db.Payments.Add(new Payment { ReservationId = reservation.Id, Amount = reservation.TotalPrice, Method = PaymentMethod.Card });
        db.SaveChanges();
    }
}

using HotelManagementApp.Data;
using HotelManagementApp.Models;
using HotelManagementApp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Services;

public class HotelService : IHotelService
{
    private readonly HotelDbContext _db;
    private readonly IRepository<Guest> _guests;
    private readonly IRepository<Reservation> _reservations;
    private readonly IRepository<Payment> _payments;
    private readonly IRepository<Service> _services;

    public HotelService(
        HotelDbContext db,
        IRepository<Guest> guests,
        IRepository<Reservation> reservations,
        IRepository<Payment> payments,
        IRepository<Service> services)
    {
        _db = db;
        _guests = guests;
        _reservations = reservations;
        _payments = payments;
        _services = services;
    }

    public Task<List<Guest>> GetGuestsAsync()
    {
        return _db.Guests
            .AsNoTracking()
            .OrderBy(g => g.LastName)
            .ThenBy(g => g.FirstName)
            .ToListAsync();
    }

    public Task<List<Room>> GetRoomsAsync()
    {
        return _db.Rooms
            .Include(r => r.RoomType)
            .AsNoTracking()
            .OrderBy(r => r.Number)
            .ToListAsync();
    }

    public Task<List<Reservation>> GetReservationsAsync()
    {
        return _db.Reservations
            .Include(r => r.Guest)
            .Include(r => r.Room)
                .ThenInclude(room => room!.RoomType)
            .Include(r => r.Employee)
            .AsNoTracking()
            .OrderByDescending(r => r.CheckInDate)
            .ToListAsync();
    }

    public Task<List<Payment>> GetPaymentsAsync()
    {
        return _db.Payments
            .Include(p => p.Reservation)
                .ThenInclude(r => r!.Guest)
            .AsNoTracking()
            .OrderByDescending(p => p.PaidAt)
            .ToListAsync();
    }

    public Task<List<Service>> GetServicesAsync()
    {
        return _db.Services
            .AsNoTracking()
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public Task<List<RoomType>> GetRoomTypesAsync()
    {
        return _db.RoomTypes
            .AsNoTracking()
            .OrderBy(rt => rt.Name)
            .ToListAsync();
    }

    public Task<List<Employee>> GetEmployeesAsync()
    {
        return _db.Employees
            .AsNoTracking()
            .OrderBy(e => e.LastName)
            .ThenBy(e => e.FirstName)
            .ToListAsync();
    }

    public async Task AddGuestAsync(Guest guest)
    {
        ValidateGuest(guest);

        guest.FirstName = guest.FirstName.Trim();
        guest.LastName = guest.LastName.Trim();
        guest.Email = guest.Email.Trim();
        guest.Phone = guest.Phone.Trim();
        guest.DocumentNumber = guest.DocumentNumber.Trim();

        await _guests.AddAsync(guest);
        await _guests.SaveChangesAsync();
    }

    public async Task AddReservationAsync(Reservation reservation)
    {
        if (reservation.GuestId <= 0)
            throw new InvalidOperationException("Wybierz gościa.");

        if (reservation.RoomId <= 0)
            throw new InvalidOperationException("Wybierz pokój.");

        if (reservation.CheckOutDate <= reservation.CheckInDate)
            throw new InvalidOperationException("Data wymeldowania musi być późniejsza niż data zameldowania.");

        var room = await _db.Rooms
            .Include(r => r.RoomType)
            .FirstOrDefaultAsync(r => r.Id == reservation.RoomId);

        if (room == null)
            throw new InvalidOperationException("Wybrany pokój nie istnieje.");

        var isAvailable = await IsRoomAvailableAsync(
            reservation.RoomId,
            reservation.CheckInDate,
            reservation.CheckOutDate);

        if (!isAvailable)
            throw new InvalidOperationException("Wybrany pokój nie jest dostępny w podanym terminie.");

        reservation.Status = ReservationStatus.Confirmed;
        reservation.TotalPrice = CalculateReservationPrice(room, reservation.CheckInDate, reservation.CheckOutDate);

        await _reservations.AddAsync(reservation);
        await _reservations.SaveChangesAsync();
    }

    public async Task AddPaymentAsync(Payment payment)
    {
        if (payment.ReservationId <= 0)
            throw new InvalidOperationException("Wybierz rezerwację.");

        if (payment.Amount <= 0)
            throw new InvalidOperationException("Kwota płatności musi być większa od zera.");

        payment.PaidAt = DateTime.Now;

        await _payments.AddAsync(payment);
        await _payments.SaveChangesAsync();
    }

    public async Task AddServiceAsync(Service service)
    {
        ValidateService(service);

        service.Name = service.Name.Trim();

        await _services.AddAsync(service);
        await _services.SaveChangesAsync();
    }

    public async Task UpdateServiceAsync(Service service)
    {
        ValidateService(service);

        var existingService = await _db.Services.FirstOrDefaultAsync(s => s.Id == service.Id);

        if (existingService == null)
            throw new InvalidOperationException("Wybrana usługa nie istnieje w bazie danych.");

        existingService.Name = service.Name.Trim();
        existingService.Price = service.Price;

        await _db.SaveChangesAsync();
    }

    public async Task AddEmployeeAsync(Employee employee)
    {
        if (string.IsNullOrWhiteSpace(employee.FirstName))
            throw new InvalidOperationException("Imię pracownika jest wymagane.");

        if (string.IsNullOrWhiteSpace(employee.LastName))
            throw new InvalidOperationException("Nazwisko pracownika jest wymagane.");

        if (string.IsNullOrWhiteSpace(employee.Position))
            throw new InvalidOperationException("Stanowisko pracownika jest wymagane.");

        employee.FirstName = employee.FirstName.Trim();
        employee.LastName = employee.LastName.Trim();
        employee.Position = employee.Position.Trim();

        _db.Employees.Add(employee);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteGuestAsync(int guestId)
    {
        var hasReservations = await _db.Reservations.AnyAsync(r => r.GuestId == guestId);

        if (hasReservations)
            throw new InvalidOperationException("Nie można usunąć gościa, który ma zapisane rezerwacje. Najpierw usuń jego rezerwacje.");

        var guest = await _db.Guests.FirstOrDefaultAsync(g => g.Id == guestId);

        if (guest == null)
            throw new InvalidOperationException("Wybrany gość nie istnieje w bazie danych.");

        _db.Guests.Remove(guest);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteReservationAsync(int reservationId)
    {
        var reservation = await _db.Reservations
            .Include(r => r.Payments)
            .Include(r => r.ReservationServices)
            .FirstOrDefaultAsync(r => r.Id == reservationId);

        if (reservation == null)
            throw new InvalidOperationException("Wybrana rezerwacja nie istnieje w bazie danych.");

        _db.Payments.RemoveRange(reservation.Payments);
        _db.ReservationServices.RemoveRange(reservation.ReservationServices);
        _db.Reservations.Remove(reservation);

        await _db.SaveChangesAsync();
    }

    public async Task DeletePaymentAsync(int paymentId)
    {
        var payment = await _db.Payments.FirstOrDefaultAsync(p => p.Id == paymentId);

        if (payment == null)
            throw new InvalidOperationException("Wybrana płatność nie istnieje w bazie danych.");

        _db.Payments.Remove(payment);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteEmployeeAsync(int employeeId)
    {
        var employee = await _db.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);

        if (employee == null)
            throw new InvalidOperationException("Wybrany pracownik nie istnieje w bazie danych.");

        var reservations = await _db.Reservations
            .Where(r => r.EmployeeId == employeeId)
            .ToListAsync();

        foreach (var reservation in reservations)
        {
            reservation.EmployeeId = null;
        }

        _db.Employees.Remove(employee);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteServiceAsync(int serviceId)
    {
        var service = await _db.Services
            .Include(s => s.ReservationServices)
            .FirstOrDefaultAsync(s => s.Id == serviceId);

        if (service == null)
            throw new InvalidOperationException("Wybrana usługa nie istnieje w bazie danych.");

        if (service.ReservationServices.Any())
            throw new InvalidOperationException("Nie można usunąć usługi, która jest przypisana do rezerwacji.");

        _db.Services.Remove(service);
        await _db.SaveChangesAsync();
    }

    public async Task<List<Room>> GetAvailableRoomsAsync(DateTime from, DateTime to)
    {
        if (to <= from)
            throw new InvalidOperationException("Data wymeldowania musi być późniejsza niż data zameldowania.");

        var bookedRoomIds = await _db.Reservations
            .Where(r =>
                r.Status != ReservationStatus.Cancelled &&
                r.CheckInDate < to &&
                from < r.CheckOutDate)
            .Select(r => r.RoomId)
            .Distinct()
            .ToListAsync();

        return await _db.Rooms
            .Include(r => r.RoomType)
            .Where(r => !bookedRoomIds.Contains(r.Id) && r.Status != RoomStatus.Maintenance)
            .AsNoTracking()
            .OrderBy(r => r.Number)
            .ToListAsync();
    }

    public decimal CalculateReservationPrice(Room room, DateTime from, DateTime to)
    {
        var days = Math.Max(1, (to.Date - from.Date).Days);
        return days * (room.RoomType?.PricePerNight ?? 0);
    }

    private static void ValidateGuest(Guest guest)
    {
        if (string.IsNullOrWhiteSpace(guest.FirstName) || string.IsNullOrWhiteSpace(guest.LastName))
            throw new InvalidOperationException("Imię i nazwisko są wymagane.");

        if (string.IsNullOrWhiteSpace(guest.Email) || !guest.Email.Contains('@'))
            throw new InvalidOperationException("Podaj poprawny adres e-mail.");

        if (string.IsNullOrWhiteSpace(guest.Phone))
            throw new InvalidOperationException("Numer telefonu jest wymagany.");
    }

    private static void ValidateService(Service service)
    {
        if (string.IsNullOrWhiteSpace(service.Name))
            throw new InvalidOperationException("Nazwa usługi jest wymagana.");

        if (service.Name.Length > 80)
            throw new InvalidOperationException("Nazwa usługi może mieć maksymalnie 80 znaków.");

        if (service.Price < 0)
            throw new InvalidOperationException("Cena usługi nie może być ujemna.");
    }

    private async Task<bool> IsRoomAvailableAsync(int roomId, DateTime from, DateTime to)
    {
        return !await _db.Reservations.AnyAsync(r =>
            r.RoomId == roomId &&
            r.Status != ReservationStatus.Cancelled &&
            r.CheckInDate < to &&
            from < r.CheckOutDate);
    }
}

using HotelManagementApp.Models;

namespace HotelManagementApp.Services;

public interface IHotelService
{
    Task<List<Guest>> GetGuestsAsync();
    Task<List<Room>> GetRoomsAsync();
    Task<List<Reservation>> GetReservationsAsync();
    Task<List<Payment>> GetPaymentsAsync();
    Task<List<Service>> GetServicesAsync();
    Task<List<RoomType>> GetRoomTypesAsync();
    Task<List<Employee>> GetEmployeesAsync();

    Task AddGuestAsync(Guest guest);
    Task AddReservationAsync(Reservation reservation);
    Task AddPaymentAsync(Payment payment);
    Task AddServiceAsync(Service service);
    Task UpdateServiceAsync(Service service);
    Task AddEmployeeAsync(Employee employee);

    Task DeleteGuestAsync(int guestId);
    Task DeleteReservationAsync(int reservationId);
    Task DeletePaymentAsync(int paymentId);
    Task DeleteEmployeeAsync(int employeeId);
    Task DeleteServiceAsync(int serviceId);

    Task<List<Room>> GetAvailableRoomsAsync(DateTime from, DateTime to);
    decimal CalculateReservationPrice(Room room, DateTime from, DateTime to);
}

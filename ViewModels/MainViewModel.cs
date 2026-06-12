using System.Collections.ObjectModel;
using System.Windows.Input;
using HotelManagementApp.Commands;
using HotelManagementApp.Models;
using HotelManagementApp.Services;

namespace HotelManagementApp.ViewModels;

public class MainViewModel : ObservableObject
{
    private readonly IHotelService _hotelService;
    private string _message = "Gotowy do pracy";
    private Service? _selectedService;
    private Guest? _selectedGuest;
    private Room? _selectedRoom;
    private Employee? _selectedEmployee;
    private Reservation? _selectedReservation;
    private Payment? _selectedPayment;
    private string _newServiceName = string.Empty;
    private string _newServicePrice = string.Empty;
    private string _newPaymentAmount = string.Empty;

    public ObservableCollection<Guest> Guests { get; } = new();
    public ObservableCollection<Room> Rooms { get; } = new();
    public ObservableCollection<Reservation> Reservations { get; } = new();
    public ObservableCollection<Payment> Payments { get; } = new();
    public ObservableCollection<Service> Services { get; } = new();
    public ObservableCollection<Employee> Employees { get; } = new();
    public ObservableCollection<Room> AvailableRooms { get; } = new();

    public string NewGuestFirstName { get; set; } = string.Empty;
    public string NewGuestLastName { get; set; } = string.Empty;
    public string NewGuestEmail { get; set; } = string.Empty;
    public string NewGuestPhone { get; set; } = string.Empty;
    public string NewGuestDocument { get; set; } = string.Empty;

    public string NewEmployeeFirstName { get; set; } = string.Empty;
    public string NewEmployeeLastName { get; set; } = string.Empty;
    public string NewEmployeePosition { get; set; } = string.Empty;

    public string NewServiceName
    {
        get => _newServiceName;
        set => SetProperty(ref _newServiceName, value);
    }

    public string NewServicePrice
    {
        get => _newServicePrice;
        set => SetProperty(ref _newServicePrice, value);
    }

    public string NewPaymentAmount
    {
        get => _newPaymentAmount;
        set => SetProperty(ref _newPaymentAmount, value);
    }

    public Guest? SelectedGuest
    {
        get => _selectedGuest;
        set => SetProperty(ref _selectedGuest, value);
    }

    public Room? SelectedRoom
    {
        get => _selectedRoom;
        set => SetProperty(ref _selectedRoom, value);
    }

    public Employee? SelectedEmployee
    {
        get => _selectedEmployee;
        set => SetProperty(ref _selectedEmployee, value);
    }

    public Reservation? SelectedReservation
    {
        get => _selectedReservation;
        set => SetProperty(ref _selectedReservation, value);
    }

    public Payment? SelectedPayment
    {
        get => _selectedPayment;
        set => SetProperty(ref _selectedPayment, value);
    }

    public Service? SelectedService
    {
        get => _selectedService;
        set
        {
            if (SetProperty(ref _selectedService, value) && value != null)
            {
                NewServiceName = value.Name;
                NewServicePrice = value.Price.ToString("0.00");
                Message = "Wybrano usługę do edycji.";
            }
        }
    }

    public DateTime CheckInDate { get; set; } = DateTime.Today;
    public DateTime CheckOutDate { get; set; } = DateTime.Today.AddDays(1);

    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    public int GuestsCount => Guests.Count;
    public int RoomsCount => Rooms.Count;
    public int AvailableRoomsCount => Rooms.Count(r => r.Status == RoomStatus.Available);
    public int ActiveReservationsCount => Reservations.Count(r => r.Status is ReservationStatus.Confirmed or ReservationStatus.CheckedIn);
    public int CheckedInReservationsCount => Reservations.Count(r => r.Status == ReservationStatus.CheckedIn);
    public int TodayCheckInsCount => Reservations.Count(r => r.CheckInDate.Date == DateTime.Today);
    public int ServicesCount => Services.Count;
    public int EmployeesCount => Employees.Count;
    public decimal TotalPayments => Payments.Sum(p => p.Amount);
    public decimal ReservationPotentialValue => Reservations.Sum(r => r.TotalPrice);

    public ICommand LoadCommand { get; }
    public ICommand AddGuestCommand { get; }
    public ICommand AddReservationCommand { get; }
    public ICommand AddPaymentCommand { get; }
    public ICommand SaveServiceCommand { get; }
    public ICommand ClearServiceFormCommand { get; }
    public ICommand CheckAvailabilityCommand { get; }
    public ICommand AddEmployeeCommand { get; }
    public ICommand ClearEmployeeFormCommand { get; }

    public ICommand DeleteGuestCommand { get; }
    public ICommand DeleteReservationCommand { get; }
    public ICommand DeletePaymentCommand { get; }
    public ICommand DeleteEmployeeCommand { get; }
    public ICommand DeleteServiceCommand { get; }

    public MainViewModel(IHotelService hotelService)
    {
        _hotelService = hotelService;

        LoadCommand = new RelayCommand(async _ => await LoadAsync());
        AddGuestCommand = new RelayCommand(async _ => await AddGuestAsync());
        AddReservationCommand = new RelayCommand(async _ => await AddReservationAsync());
        AddPaymentCommand = new RelayCommand(async _ => await AddPaymentAsync());
        SaveServiceCommand = new RelayCommand(async _ => await SaveServiceAsync());
        ClearServiceFormCommand = new RelayCommand(_ => ClearServiceForm());
        CheckAvailabilityCommand = new RelayCommand(async _ => await CheckAvailabilityAsync());
        AddEmployeeCommand = new RelayCommand(async _ => await AddEmployeeAsync());
        ClearEmployeeFormCommand = new RelayCommand(_ => ClearEmployeeForm());

        DeleteGuestCommand = new RelayCommand(async _ => await DeleteGuestAsync());
        DeleteReservationCommand = new RelayCommand(async _ => await DeleteReservationAsync());
        DeletePaymentCommand = new RelayCommand(async _ => await DeletePaymentAsync());
        DeleteEmployeeCommand = new RelayCommand(async _ => await DeleteEmployeeAsync());
        DeleteServiceCommand = new RelayCommand(async _ => await DeleteServiceAsync());

        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        try
        {
            Guests.Clear();
            foreach (var item in await _hotelService.GetGuestsAsync())
                Guests.Add(item);

            Rooms.Clear();
            foreach (var item in await _hotelService.GetRoomsAsync())
                Rooms.Add(item);

            Reservations.Clear();
            foreach (var item in await _hotelService.GetReservationsAsync())
                Reservations.Add(item);

            Payments.Clear();
            foreach (var item in await _hotelService.GetPaymentsAsync())
                Payments.Add(item);

            Services.Clear();
            foreach (var item in await _hotelService.GetServicesAsync())
                Services.Add(item);

            Employees.Clear();
            foreach (var item in await _hotelService.GetEmployeesAsync())
                Employees.Add(item);

            AvailableRooms.Clear();
            foreach (var item in Rooms.Where(r => r.Status == RoomStatus.Available))
                AvailableRooms.Add(item);

            RefreshDashboard();
            Message = "Dane zostały załadowane.";
        }
        catch (Exception ex)
        {
            Message = $"Błąd ładowania danych: {ex.Message}";
        }
    }

    private async Task AddGuestAsync()
    {
        try
        {
            var guest = new Guest
            {
                FirstName = NewGuestFirstName,
                LastName = NewGuestLastName,
                Email = NewGuestEmail,
                Phone = NewGuestPhone,
                DocumentNumber = NewGuestDocument
            };

            await _hotelService.AddGuestAsync(guest);

            NewGuestFirstName = string.Empty;
            NewGuestLastName = string.Empty;
            NewGuestEmail = string.Empty;
            NewGuestPhone = string.Empty;
            NewGuestDocument = string.Empty;

            OnPropertyChanged(nameof(NewGuestFirstName));
            OnPropertyChanged(nameof(NewGuestLastName));
            OnPropertyChanged(nameof(NewGuestEmail));
            OnPropertyChanged(nameof(NewGuestPhone));
            OnPropertyChanged(nameof(NewGuestDocument));

            await LoadAsync();
            Message = "Dodano nowego gościa.";
        }
        catch (Exception ex)
        {
            Message = $"Nie można dodać gościa: {ex.Message}";
        }
    }

    private async Task AddReservationAsync()
    {
        try
        {
            if (SelectedGuest == null)
                throw new InvalidOperationException("Wybierz gościa.");

            if (SelectedRoom == null)
                throw new InvalidOperationException("Wybierz pokój.");

            var reservation = new Reservation
            {
                GuestId = SelectedGuest.Id,
                RoomId = SelectedRoom.Id,
                EmployeeId = SelectedEmployee?.Id,
                CheckInDate = CheckInDate,
                CheckOutDate = CheckOutDate,
                Status = ReservationStatus.Confirmed
            };

            await _hotelService.AddReservationAsync(reservation);
            await LoadAsync();
            Message = "Dodano nową rezerwację.";
        }
        catch (Exception ex)
        {
            Message = $"Nie można dodać rezerwacji: {ex.Message}";
        }
    }

    private async Task AddPaymentAsync()
    {
        try
        {
            if (SelectedReservation == null)
                throw new InvalidOperationException("Wybierz rezerwację.");

            if (!decimal.TryParse(NewPaymentAmount, out var amount))
                throw new InvalidOperationException("Podaj poprawną kwotę płatności.");

            var payment = new Payment
            {
                ReservationId = SelectedReservation.Id,
                Amount = amount,
                Method = PaymentMethod.Card,
                PaidAt = DateTime.Now
            };

            await _hotelService.AddPaymentAsync(payment);

            NewPaymentAmount = string.Empty;
            await LoadAsync();
            Message = "Dodano płatność.";
        }
        catch (Exception ex)
        {
            Message = $"Nie można dodać płatności: {ex.Message}";
        }
    }

    private async Task SaveServiceAsync()
    {
        try
        {
            if (!decimal.TryParse(NewServicePrice, out var price))
                throw new InvalidOperationException("Podaj poprawną cenę usługi.");

            var service = new Service
            {
                Id = SelectedService?.Id ?? 0,
                Name = NewServiceName,
                Price = price
            };

            if (SelectedService == null)
            {
                await _hotelService.AddServiceAsync(service);
                Message = "Dodano nową usługę.";
            }
            else
            {
                await _hotelService.UpdateServiceAsync(service);
                Message = "Zaktualizowano usługę.";
            }

            ClearServiceForm();
            await LoadAsync();
        }
        catch (Exception ex)
        {
            Message = $"Nie można zapisać usługi: {ex.Message}";
        }
    }

    private void ClearServiceForm()
    {
        _selectedService = null;
        NewServiceName = string.Empty;
        NewServicePrice = string.Empty;

        OnPropertyChanged(nameof(SelectedService));
        OnPropertyChanged(nameof(NewServiceName));
        OnPropertyChanged(nameof(NewServicePrice));

        Message = "Formularz usługi został wyczyszczony.";
    }

    private async Task AddEmployeeAsync()
    {
        try
        {
            var employee = new Employee
            {
                FirstName = NewEmployeeFirstName,
                LastName = NewEmployeeLastName,
                Position = NewEmployeePosition
            };

            await _hotelService.AddEmployeeAsync(employee);

            ClearEmployeeForm();
            await LoadAsync();
            Message = "Dodano nowego pracownika.";
        }
        catch (Exception ex)
        {
            Message = $"Nie można dodać pracownika: {ex.Message}";
        }
    }

    private void ClearEmployeeForm()
    {
        NewEmployeeFirstName = string.Empty;
        NewEmployeeLastName = string.Empty;
        NewEmployeePosition = string.Empty;

        OnPropertyChanged(nameof(NewEmployeeFirstName));
        OnPropertyChanged(nameof(NewEmployeeLastName));
        OnPropertyChanged(nameof(NewEmployeePosition));

        Message = "Formularz pracownika został wyczyszczony.";
    }

    private async Task DeleteGuestAsync()
    {
        try
        {
            if (SelectedGuest == null)
                throw new InvalidOperationException("Wybierz gościa do usunięcia.");

            var fullName = SelectedGuest.FullName;
            await _hotelService.DeleteGuestAsync(SelectedGuest.Id);

            SelectedGuest = null;
            await LoadAsync();
            Message = $"Usunięto gościa: {fullName}.";
        }
        catch (Exception ex)
        {
            Message = $"Nie można usunąć gościa: {ex.Message}";
        }
    }

    private async Task DeleteReservationAsync()
    {
        try
        {
            if (SelectedReservation == null)
                throw new InvalidOperationException("Wybierz rezerwację do usunięcia.");

            var reservationId = SelectedReservation.Id;
            await _hotelService.DeleteReservationAsync(SelectedReservation.Id);

            SelectedReservation = null;
            await LoadAsync();
            Message = $"Usunięto rezerwację nr {reservationId}.";
        }
        catch (Exception ex)
        {
            Message = $"Nie można usunąć rezerwacji: {ex.Message}";
        }
    }

    private async Task DeletePaymentAsync()
    {
        try
        {
            if (SelectedPayment == null)
                throw new InvalidOperationException("Wybierz płatność do usunięcia.");

            var paymentId = SelectedPayment.Id;
            await _hotelService.DeletePaymentAsync(SelectedPayment.Id);

            SelectedPayment = null;
            await LoadAsync();
            Message = $"Usunięto płatność nr {paymentId}.";
        }
        catch (Exception ex)
        {
            Message = $"Nie można usunąć płatności: {ex.Message}";
        }
    }

    private async Task DeleteEmployeeAsync()
    {
        try
        {
            if (SelectedEmployee == null)
                throw new InvalidOperationException("Wybierz pracownika do usunięcia.");

            var fullName = SelectedEmployee.FullName;
            await _hotelService.DeleteEmployeeAsync(SelectedEmployee.Id);

            SelectedEmployee = null;
            await LoadAsync();
            Message = $"Usunięto pracownika: {fullName}.";
        }
        catch (Exception ex)
        {
            Message = $"Nie można usunąć pracownika: {ex.Message}";
        }
    }

    private async Task DeleteServiceAsync()
    {
        try
        {
            if (SelectedService == null)
                throw new InvalidOperationException("Wybierz usługę do usunięcia.");

            var serviceName = SelectedService.Name;
            await _hotelService.DeleteServiceAsync(SelectedService.Id);

            ClearServiceForm();
            await LoadAsync();
            Message = $"Usunięto usługę: {serviceName}.";
        }
        catch (Exception ex)
        {
            Message = $"Nie można usunąć usługi: {ex.Message}";
        }
    }

    private async Task CheckAvailabilityAsync()
    {
        try
        {
            AvailableRooms.Clear();
            foreach (var room in await _hotelService.GetAvailableRoomsAsync(CheckInDate, CheckOutDate))
                AvailableRooms.Add(room);

            OnPropertyChanged(nameof(AvailableRooms));
            Message = $"Znaleziono {AvailableRooms.Count} dostępnych pokoi w wybranym terminie.";
        }
        catch (Exception ex)
        {
            Message = $"Błąd sprawdzania dostępności: {ex.Message}";
        }
    }

    private void RefreshDashboard()
    {
        OnPropertyChanged(nameof(GuestsCount));
        OnPropertyChanged(nameof(RoomsCount));
        OnPropertyChanged(nameof(AvailableRoomsCount));
        OnPropertyChanged(nameof(ActiveReservationsCount));
        OnPropertyChanged(nameof(CheckedInReservationsCount));
        OnPropertyChanged(nameof(TodayCheckInsCount));
        OnPropertyChanged(nameof(ServicesCount));
        OnPropertyChanged(nameof(EmployeesCount));
        OnPropertyChanged(nameof(TotalPayments));
        OnPropertyChanged(nameof(ReservationPotentialValue));
    }
}

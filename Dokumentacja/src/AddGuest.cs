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
        await LoadAsync();
        Message = "Dodano nowego gościa.";
    }
    catch (Exception ex)
    {
        Message = $"Nie można dodać gościa: {ex.Message}";
    }
}

private static void ValidateGuest(Guest guest)
{
    if (string.IsNullOrWhiteSpace(guest.FirstName) ||
        string.IsNullOrWhiteSpace(guest.LastName))
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

    if (service.Price < 0)
        throw new InvalidOperationException("Cena usługi nie może być ujemna.");
}

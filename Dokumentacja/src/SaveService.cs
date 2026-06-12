private async Task SaveServiceAsync()
{
    var service = new Service
    {
        Id = SelectedService?.Id ?? 0,
        Name = NewServiceName,
        Price = NewServicePrice
    };

    if (SelectedService == null)
        await _hotelService.AddServiceAsync(service);
    else
        await _hotelService.UpdateServiceAsync(service);

    ClearServiceForm();
    await LoadAsync();
}

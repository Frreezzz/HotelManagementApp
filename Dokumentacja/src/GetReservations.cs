public Task<List<Reservation>> GetReservationsAsync() => _db.Reservations
    .Include(r => r.Guest)
    .Include(r => r.Room).ThenInclude(room => room!.RoomType)
    .Include(r => r.Employee)
    .AsNoTracking()
    .OrderByDescending(r => r.CheckInDate)
    .ToListAsync();

modelBuilder.Entity<Room>()
    .HasOne(r => r.RoomType)
    .WithMany(rt => rt.Rooms)
    .HasForeignKey(r => r.RoomTypeId)
    .OnDelete(DeleteBehavior.Restrict);

modelBuilder.Entity<Reservation>()
    .HasOne(r => r.Guest)
    .WithMany(g => g.Reservations)
    .HasForeignKey(r => r.GuestId)
    .OnDelete(DeleteBehavior.Restrict);

modelBuilder.Entity<ReservationService>()
    .HasKey(rs => new { rs.ReservationId, rs.ServiceId });

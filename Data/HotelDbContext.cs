using HotelManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Data;

public class HotelDbContext : DbContext
{
    public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) { }

    public DbSet<Guest> Guests => Set<Guest>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<RoomType> RoomTypes => Set<RoomType>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<ReservationService> ReservationServices => Set<ReservationService>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Guest>()
            .HasIndex(g => g.Email)
            .IsUnique();

        modelBuilder.Entity<Room>()
            .HasIndex(r => r.Number)
            .IsUnique();

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

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Room)
            .WithMany(room => room.Reservations)
            .HasForeignKey(r => r.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Employee)
            .WithMany(e => e.Reservations)
            .HasForeignKey(r => r.EmployeeId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Reservation)
            .WithMany(r => r.Payments)
            .HasForeignKey(p => p.ReservationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ReservationService>()
            .HasKey(rs => new { rs.ReservationId, rs.ServiceId });

        modelBuilder.Entity<ReservationService>()
            .HasOne(rs => rs.Reservation)
            .WithMany(r => r.ReservationServices)
            .HasForeignKey(rs => rs.ReservationId);

        modelBuilder.Entity<ReservationService>()
            .HasOne(rs => rs.Service)
            .WithMany(s => s.ReservationServices)
            .HasForeignKey(rs => rs.ServiceId);

        modelBuilder.Entity<RoomType>().Property(x => x.PricePerNight).HasPrecision(10, 2);
        modelBuilder.Entity<Reservation>().Property(x => x.TotalPrice).HasPrecision(10, 2);
        modelBuilder.Entity<Payment>().Property(x => x.Amount).HasPrecision(10, 2);
        modelBuilder.Entity<Service>().Property(x => x.Price).HasPrecision(10, 2);
        modelBuilder.Entity<ReservationService>().Property(x => x.UnitPrice).HasPrecision(10, 2);
    }
}

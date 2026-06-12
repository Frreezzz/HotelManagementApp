using HotelManagementApp.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManagementApp.Migrations;

[DbContext(typeof(HotelDbContext))]
[Migration("20260608000000_InitialCreate")]
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS RoomTypes (
    Id INTEGER NOT NULL CONSTRAINT PK_RoomTypes PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Description TEXT NOT NULL,
    Capacity INTEGER NOT NULL,
    PricePerNight TEXT NOT NULL
);
CREATE TABLE IF NOT EXISTS Guests (
    Id INTEGER NOT NULL CONSTRAINT PK_Guests PRIMARY KEY AUTOINCREMENT,
    FirstName TEXT NOT NULL,
    LastName TEXT NOT NULL,
    Email TEXT NOT NULL,
    Phone TEXT NOT NULL,
    DocumentNumber TEXT NOT NULL
);
CREATE TABLE IF NOT EXISTS Employees (
    Id INTEGER NOT NULL CONSTRAINT PK_Employees PRIMARY KEY AUTOINCREMENT,
    FirstName TEXT NOT NULL,
    LastName TEXT NOT NULL,
    Position TEXT NOT NULL
);
CREATE TABLE IF NOT EXISTS Services (
    Id INTEGER NOT NULL CONSTRAINT PK_Services PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Price TEXT NOT NULL
);
CREATE TABLE IF NOT EXISTS Rooms (
    Id INTEGER NOT NULL CONSTRAINT PK_Rooms PRIMARY KEY AUTOINCREMENT,
    Number TEXT NOT NULL,
    Floor INTEGER NOT NULL,
    Status INTEGER NOT NULL,
    RoomTypeId INTEGER NOT NULL,
    CONSTRAINT FK_Rooms_RoomTypes_RoomTypeId FOREIGN KEY (RoomTypeId) REFERENCES RoomTypes (Id) ON DELETE RESTRICT
);
CREATE TABLE IF NOT EXISTS Reservations (
    Id INTEGER NOT NULL CONSTRAINT PK_Reservations PRIMARY KEY AUTOINCREMENT,
    GuestId INTEGER NOT NULL,
    RoomId INTEGER NOT NULL,
    EmployeeId INTEGER NULL,
    CheckInDate TEXT NOT NULL,
    CheckOutDate TEXT NOT NULL,
    Status INTEGER NOT NULL,
    TotalPrice TEXT NOT NULL,
    CONSTRAINT FK_Reservations_Guests_GuestId FOREIGN KEY (GuestId) REFERENCES Guests (Id) ON DELETE RESTRICT,
    CONSTRAINT FK_Reservations_Rooms_RoomId FOREIGN KEY (RoomId) REFERENCES Rooms (Id) ON DELETE RESTRICT,
    CONSTRAINT FK_Reservations_Employees_EmployeeId FOREIGN KEY (EmployeeId) REFERENCES Employees (Id) ON DELETE SET NULL
);
CREATE TABLE IF NOT EXISTS Payments (
    Id INTEGER NOT NULL CONSTRAINT PK_Payments PRIMARY KEY AUTOINCREMENT,
    ReservationId INTEGER NOT NULL,
    Amount TEXT NOT NULL,
    Method INTEGER NOT NULL,
    PaidAt TEXT NOT NULL,
    CONSTRAINT FK_Payments_Reservations_ReservationId FOREIGN KEY (ReservationId) REFERENCES Reservations (Id) ON DELETE CASCADE
);
CREATE TABLE IF NOT EXISTS ReservationServices (
    ReservationId INTEGER NOT NULL,
    ServiceId INTEGER NOT NULL,
    Quantity INTEGER NOT NULL,
    UnitPrice TEXT NOT NULL,
    CONSTRAINT PK_ReservationServices PRIMARY KEY (ReservationId, ServiceId),
    CONSTRAINT FK_ReservationServices_Reservations_ReservationId FOREIGN KEY (ReservationId) REFERENCES Reservations (Id) ON DELETE CASCADE,
    CONSTRAINT FK_ReservationServices_Services_ServiceId FOREIGN KEY (ServiceId) REFERENCES Services (Id) ON DELETE CASCADE
);
CREATE UNIQUE INDEX IF NOT EXISTS IX_Guests_Email ON Guests (Email);
CREATE UNIQUE INDEX IF NOT EXISTS IX_Rooms_Number ON Rooms (Number);
CREATE INDEX IF NOT EXISTS IX_Rooms_RoomTypeId ON Rooms (RoomTypeId);
CREATE INDEX IF NOT EXISTS IX_Reservations_GuestId ON Reservations (GuestId);
CREATE INDEX IF NOT EXISTS IX_Reservations_RoomId ON Reservations (RoomId);
CREATE INDEX IF NOT EXISTS IX_Reservations_EmployeeId ON Reservations (EmployeeId);
CREATE INDEX IF NOT EXISTS IX_Payments_ReservationId ON Payments (ReservationId);
CREATE INDEX IF NOT EXISTS IX_ReservationServices_ServiceId ON ReservationServices (ServiceId);
");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
DROP TABLE IF EXISTS ReservationServices;
DROP TABLE IF EXISTS Payments;
DROP TABLE IF EXISTS Reservations;
DROP TABLE IF EXISTS Rooms;
DROP TABLE IF EXISTS Services;
DROP TABLE IF EXISTS Employees;
DROP TABLE IF EXISTS Guests;
DROP TABLE IF EXISTS RoomTypes;
");
    }
}

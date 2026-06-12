using HotelManagementApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HotelManagementApp.Migrations;

[DbContext(typeof(HotelDbContext))]
partial class HotelDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("ProductVersion", "8.0.6");
        modelBuilder.Entity("HotelManagementApp.Models.Guest", b =>
        {
            b.Property<int>("Id").ValueGeneratedOnAdd();
            b.Property<string>("FirstName").IsRequired().HasMaxLength(80);
            b.Property<string>("LastName").IsRequired().HasMaxLength(80);
            b.Property<string>("Email").IsRequired().HasMaxLength(120);
            b.Property<string>("Phone").IsRequired().HasMaxLength(30);
            b.Property<string>("DocumentNumber").IsRequired().HasMaxLength(30);
            b.HasKey("Id");
            b.HasIndex("Email").IsUnique();
            b.ToTable("Guests");
        });
    }
}

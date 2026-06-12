using System.Windows;
using HotelManagementApp.Data;
using HotelManagementApp.Repositories;
using HotelManagementApp.Services;
using HotelManagementApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HotelManagementApp;

public partial class App : Application
{
    private ServiceProvider? _serviceProvider;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();
        services.AddDbContext<HotelDbContext>(options =>
            options.UseSqlite("Data Source=hotel.db"));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IHotelService, HotelService>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<MainWindow>();

        _serviceProvider = services.BuildServiceProvider();

        using (var scope = _serviceProvider.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<HotelDbContext>();
            db.Database.Migrate();
            DbSeeder.Seed(db);
        }

        var window = _serviceProvider.GetRequiredService<MainWindow>();
        window.Show();
    }
}

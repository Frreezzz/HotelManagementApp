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

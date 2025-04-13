using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using weatherbot.Models;
using static weatherbot.Host;

namespace weatherbot;
public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public ApplicationContext() => Database.EnsureCreated();


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql($"Host=194.87.55.155;Port=5432;Database=vp25_new;Username=sane5k;Password=JeckDog093");
    }

    //optionsBuilder.UseSqlite("Data Source=MyDb.db");
    //using (ApplicationContext db = new ApplicationContext())
    //{
    //    // создаем два объекта User
    //    User tom = new User { UserId = "", City = "Kirov"};
    //    User alice = new User { UserId = "Alice", City = "Kazan"};

    //    // добавляем их в бд
    //    db.Users.Add(tom);
    //    db.Users.Add(alice);
    //    db.SaveChanges();
    //    Console.WriteLine("Объекты успешно сохранены");

    //    // получаем объекты из бд и выводим на консоль
    //    var users = db.Users.ToList();
    //    Console.WriteLine("Список объектов:");
    //    foreach (User u in users)
    //    {
    //        Console.WriteLine($"{u.UserId} - {u.City}");
    //    }
    //}
}
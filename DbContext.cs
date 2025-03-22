using Microsoft.EntityFrameworkCore;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public ApplicationContext() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=users.db");
        using (ApplicationContext db = new ApplicationContext())
        {
            // создаем два объекта User
            User tom = new User { UserId = "Tom", City = "Kirov"};
            User alice = new User { UserId = "Alice", City = "Kazan"};

            // добавляем их в бд
            db.Users.Add(tom);
            db.Users.Add(alice);
            db.SaveChanges();
            Console.WriteLine("Объекты успешно сохранены");

            // получаем объекты из бд и выводим на консоль
            var users = db.Users.ToList();
            Console.WriteLine("Список объектов:");
            foreach (User u in users)
            {
                Console.WriteLine($"{u.UserId} - {u.City}");
            }
        }
    }

    public class User
    {
        public string UserId { get; set; }
        public string City { get; set; }
    }
}
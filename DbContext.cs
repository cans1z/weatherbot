using Microsoft.EntityFrameworkCore;
using weatherbot.Models;

namespace weatherbot;

public class ApplicationContext : DbContext
{
    public ApplicationContext()
    {
        Database.EnsureCreated();
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Time> Times { get; set; } = null!;


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=194.87.55.155;Port=5432;Database=vp25_new;Username=sane5k;Password=JeckDog093");
    }
}
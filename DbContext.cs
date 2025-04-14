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
}
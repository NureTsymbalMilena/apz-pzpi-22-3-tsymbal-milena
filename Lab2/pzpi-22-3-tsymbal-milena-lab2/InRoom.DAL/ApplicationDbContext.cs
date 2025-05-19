using InRoom.DLL.Models;
using Microsoft.EntityFrameworkCore;

namespace InRoom.DAL;

public class ApplicationDbContext : DbContext
{
    // Constructor that accepts DbContextOptions to configure the context and passes it to the base class
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> context) : base(context)
    {
        AutoSchema = true;
    }

    public bool AutoSchema { get; }

    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Device> Devices { get; set; } 
    public DbSet<Hospital> Hospitals { get; set; }
    public DbSet<Movement> Movements { get; set; } 
    public DbSet<Notification> Notifications { get; set; } 
    public DbSet<Room> Rooms { get; set; } 
    public DbSet<User> Users { get; set; } 
    public DbSet<Zone> Zones { get; set; }
    public DbSet<Disease> Diseases { get; set; }
    public DbSet<RequestLog> RequestLogs { get; set; }

    // This method is used to configure the entity models (e.g., relationships, constraints) during the model creation
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>()
            .HasOne(c => c.ContactInitiator) 
            .WithMany() 
            .HasForeignKey(c => c.ContactInitiatorId) 
            .OnDelete(DeleteBehavior.NoAction); 

        modelBuilder.Entity<Contact>()
            .HasOne(c => c.ContactReceiver)
            .WithMany() 
            .HasForeignKey(c => c.ContactReceiverId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Contact>()
            .HasOne(c => c.Device)
            .WithMany() 
            .HasForeignKey(c => c.DeviceId) 
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<Movement>()
            .HasOne(m => m.User) 
            .WithMany()
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<User>()
            .HasOne(u => u.Disease)
            .WithMany()
            .HasForeignKey(u => u.DiseaseId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

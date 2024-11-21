using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Community> Communities { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Community>()
            .HasMany(c => c.UserSubscribers)
            .WithMany(u => u.SubscribedCommunities) // Ensure the property exists in the User model
            .UsingEntity(j => j.ToTable("CommunitySubscribers"));
    }
}
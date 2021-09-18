using Microsoft.EntityFrameworkCore;

namespace AsyncStreams.Entities
{
    public class SampleDbContext : DbContext
    {
        public SampleDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Person> Persons { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasData(
                new (1, "John", "Doe"),
                new (2, "Jane", "Doe")
            );
        }
    }
}

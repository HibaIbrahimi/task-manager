using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domaine.Entities;

namespace TaskManager.Infrastructure.DbContexts
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options)
            : base(options) { }

        public DbSet<TaskItem> Tasks => Set<TaskItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Must explicitly use EntityTypeBuilder<TaskItem>
            EntityTypeBuilder<TaskItem> entity = modelBuilder.Entity<TaskItem>();

            // Correct EF Core method
            entity.ToTable("Tasks");

            entity.HasKey(t => t.Id);

            entity.OwnsOne(t => t.Title, t =>
            {
                t.Property(p => p.Value)
                 .HasColumnName("Title")
                 .HasMaxLength(200)
                 .IsRequired();
            });

            entity.Property(t => t.IsCompleted).IsRequired();
        }
    }
}

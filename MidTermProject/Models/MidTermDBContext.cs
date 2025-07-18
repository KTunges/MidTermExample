using Microsoft.EntityFrameworkCore;

namespace MidTermProject.Models
{
    public partial class MidTermDBContext : DbContext
    {
        public MidTermDBContext()
        {
        }

        public MidTermDBContext(DbContextOptions<MidTermDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Authors { get; set; } = null!;
        public virtual DbSet<Book> Books { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Kết nối local hoặc Azure SQL
                optionsBuilder.UseSqlServer("Server=.;Database=MidTermDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(e => e.AuthorId);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Bio).HasMaxLength(500);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.BookId);
                entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.CoverImagePath).HasMaxLength(255);

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}

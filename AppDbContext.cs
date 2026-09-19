using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Expense> Expenses { get; set; }    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Expense>().ToTable("Expense");
}
}




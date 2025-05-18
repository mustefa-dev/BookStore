using BookStore.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DATA;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }


    public DbSet<AppUser> Users { get; set; }
    


    // here to add
public DbSet<Order> Orders { get; set; }
public DbSet<OrderItem> OrderItems { get; set; }
public DbSet<Cart> Carts { get; set; }
public DbSet<CartProduct> CartProducts { get; set; }
public DbSet<Category> Categorys { get; set; }
public DbSet<Book> Books { get; set; }


    public virtual async Task<int> SaveChangesAsync(Guid? userId = null)
    {
        // await OnBeforeSaveChanges(userId);
        var result = await base.SaveChangesAsync();
        return result;
    }
}

public class DbContextOptions<T>
{
}

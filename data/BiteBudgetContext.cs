
using BiteBudget.Models;
using Microsoft.EntityFrameworkCore;

namespace BiteBudget.Data
{
    public class BiteBudgetContext : DbContext
    {
        public BiteBudgetContext(
            DbContextOptions<BiteBudgetContext> options)
            : base(options)
        {
        }

        public DbSet<Meal> Meals { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

    }
}

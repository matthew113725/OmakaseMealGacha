using Microsoft.EntityFrameworkCore;

namespace OmakaseMealGacha.Models
{
    public class MealGachaDbContext : DbContext
    {
        public MealGachaDbContext(DbContextOptions<MealGachaDbContext> options)
            : base(options) { }

        public DbSet<Menu> Menus { get; set; }
        public DbSet<History> History { get; set; }
    }
}

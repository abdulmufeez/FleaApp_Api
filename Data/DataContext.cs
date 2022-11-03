using FleaApp_Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace fleaApi.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Market> Markets { get; set; }
        public DbSet<Shop> Shops { get; set; }

        public DbSet<GeoLocation> GeoLocations { get; set; }
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);                                        
        }
    }
}
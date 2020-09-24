
using ApiCountry.Models;
using ApiCountry.Repository.Mapping;
using Microsoft.EntityFrameworkCore;

namespace ApiCountry.Repository
{
    public class CountryContext : DbContext
    {
        public DbSet<Country> Countries { set; get; }
        public CountryContext(DbContextOptions<CountryContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CountryMapping());
            modelBuilder.ApplyConfiguration(new StateMapping());
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<ApiCountry.Models.State> State { get; set; }

    }
}

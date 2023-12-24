using Microsoft.EntityFrameworkCore;
using TravelTask.Entities;

namespace TravelTask.Persistance
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(IConfiguration config)
        {
            Config = config;
        }

        public IConfiguration Config { get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(Config["ConnectionStrings:MSSQL"]);
        }

        public DbSet<Destination> Destinations { get; set; }
    }
}

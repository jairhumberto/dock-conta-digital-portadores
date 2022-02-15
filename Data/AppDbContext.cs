using Microsoft.EntityFrameworkCore;
using PortadoresService.Models;

namespace PortadoresService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
            
        }

        public DbSet<Portador> Portadores { get; set; }
    }
}
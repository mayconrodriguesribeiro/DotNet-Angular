using Microsoft.EntityFrameworkCore;
using ProAgilApi.Models;

namespace ProAgilApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext>options):base(options){}

        public DbSet<Evento>Eventos{get;set;}
        
    }
}
using Microsoft.EntityFrameworkCore;
using OpeningExplorer.Entities;

namespace OpeningExplorer.Data;
public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) {}

    public DbSet<User> Users { get; set; }
    public DbSet<Opening> Openings { get; set; }
}
using Microsoft.EntityFrameworkCore;
using Youtube.Api.Models;

namespace Youtube.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ContentCreator> ContentCreators { get; set; }
}
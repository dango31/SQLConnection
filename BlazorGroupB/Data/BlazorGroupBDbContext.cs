using BlazorGroupB.Models;
using Microsoft.EntityFrameworkCore;


namespace BlazorGroupB.Data;

public class BlazorGroupBDbContext : DbContext
{
    public BlazorGroupBDbContext(DbContextOptions<BlazorGroupBDbContext> options)
    : base(options)
    {
    }
    public DbSet<Threads> Threads { get; set; } = default!;
    public DbSet<Messages> Messages { get; set; } = default!;
    public DbSet<Users> Users { get; set; } = default!;
    
}

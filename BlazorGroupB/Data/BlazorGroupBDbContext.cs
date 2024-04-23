using BlazorGroupB.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorGroupB.Data;

public class BlazorGroupBDbContext : DbContext
{
    public BlazorGroupBDbContext(DbContextOptions<BlazorGroupBDbContext> options)
    : base(options)
    {
    }
    //  データベースからThreadsテーブルの値を取り出す
    public DbSet<Threads> Threads { get; set; } = default!;

    //  データベースからMessagesテーブルの値を取り出す
    public DbSet<Messages> Messages { get; set; } = default!;

    //  データベースからUsersテーブルの値を取り出す
    public DbSet<Users> Users { get; set; } = default!;

    //  データベースからUranaisテーブルの値を取り出す
    public DbSet<Uranais> Uranais { get; set; } = default!;


}
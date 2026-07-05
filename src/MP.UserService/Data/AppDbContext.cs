namespace MP.UserService.Data;

using Microsoft.EntityFrameworkCore;
using MP.UserService.Models;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // 表名/列名统一转 snake_case（users, password_hash, created_at ...）
    // 由 EFCore.NamingConventions 包处理，不再手写 ToLower 循环，
    // 也避免了 PasswordHash -> passwordhash 这种单词挤在一起、不可读的问题。
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
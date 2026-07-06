using MP.UserService.Models;
using Dapper;
using Npgsql;

namespace MP.UserService.Repositories;

public class UserRepository : IUserRepository
{
    private readonly NpgsqlDataSource _db;

    public UserRepository(NpgsqlDataSource db) => _db = db;

    public async Task<User?> GetByEmailAsync(string email)
    {
        await using var conn = await _db.OpenConnectionAsync();
        // 🎯 把数据库里的下划线字段，用 AS 映射回 C# 的大驼峰属性
        return await conn.QueryFirstOrDefaultAsync<User>(
            "SELECT id AS Id, username AS Username, email AS Email, " +
            "password_hash AS PasswordHash, created_at AS CreatedAt, is_active AS IsActive " +
            "FROM users WHERE email = @Email AND is_active = true",
            new { Email = email });
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        await using var conn = await _db.OpenConnectionAsync();
        return await conn.QueryFirstOrDefaultAsync<User>(
            "SELECT id AS Id, username AS Username, email AS Email, " +
            "password_hash AS PasswordHash, created_at AS CreatedAt, is_active AS IsActive " +
            "FROM users WHERE id = @Id",
            new { Id = id });
    }

    public async Task CreateAsync(User user)
    {
        await using var conn = await _db.OpenConnectionAsync();
        // 🎯 插入时，左边是数据库实际的下划线列名，右边 @ 后面是你的 C# 属性名
        await conn.ExecuteAsync(
            "INSERT INTO users (id, username, email, password_hash, created_at, is_active) " +
            "VALUES (@Id, @Username, @Email, @PasswordHash, @CreatedAt, @IsActive)",
            user);
    }
}
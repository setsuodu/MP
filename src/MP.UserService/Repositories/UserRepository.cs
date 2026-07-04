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
        return await conn.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM Users WHERE Email = @Email AND IsActive = true",
            new { Email = email });
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        await using var conn = await _db.OpenConnectionAsync();
        return await conn.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM Users WHERE Id = @Id", new { Id = id });
    }

    public async Task CreateAsync(User user)
    {
        await using var conn = await _db.OpenConnectionAsync();
        await conn.ExecuteAsync(
            "INSERT INTO Users (Id, Username, Email, PasswordHash, CreatedAt, IsActive) VALUES (@Id, @Username, @Email, @PasswordHash, @CreatedAt, @IsActive)",
            user);
    }
}
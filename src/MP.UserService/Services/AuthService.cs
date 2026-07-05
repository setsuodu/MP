using BCrypt.Net;
using Microsoft.Extensions.Options;
using MP.UserService.Models;
using MP.UserService.Repositories;

namespace MP.UserService.Services;

public class AuthService(IUserRepository userRepo, JwtService jwtService, IOptions<JwtOptions> options)
{
    private readonly JwtOptions _jwtOptions = options.Value;

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest req)
    {
        var existing = await userRepo.GetByEmailAsync(req.Email);
        if (existing != null) return null;

        var user = new User
        {
            Username = req.Username,
            Email = req.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password)
        };

        await userRepo.CreateAsync(user);

        var token = jwtService.GenerateToken(user);
        return new AuthResponse(token, user, jwtService.GetExpiryFor(DateTime.UtcNow));
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest req)
    {
        var user = await userRepo.GetByEmailAsync(req.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            return null;

        var token = jwtService.GenerateToken(user);
        return new AuthResponse(token, user, jwtService.GetExpiryFor(DateTime.UtcNow));
    }
}
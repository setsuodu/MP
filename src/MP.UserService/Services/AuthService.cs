using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using MP.UserService.Models;
using MP.UserService.Repositories;

namespace MP.UserService.Services;

public class AuthService
{
    private readonly IUserRepository _userRepo;
    private readonly JwtOptions _jwtOptions;

    public AuthService(IUserRepository userRepo, IConfiguration config)
    {
        _userRepo = userRepo;
        _jwtOptions = config.GetSection("Jwt").Get<JwtOptions>() ?? new JwtOptions();
    }

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest req)
    {
        var existing = await _userRepo.GetByEmailAsync(req.Email);
        if (existing != null) return null;

        var user = new User
        {
            Username = req.Username,
            Email = req.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password)
        };

        await _userRepo.CreateAsync(user);

        var token = GenerateJwtToken(user);
        return new AuthResponse(token, user, DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiresInMinutes));
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest req)
    {
        var user = await _userRepo.GetByEmailAsync(req.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            return null;

        var token = GenerateJwtToken(user);
        return new AuthResponse(token, user, DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiresInMinutes));
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtOptions.ExpiresInMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
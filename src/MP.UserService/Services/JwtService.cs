using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MP.UserService.Models;

namespace MP.UserService.Services;

// 只负责一件事：把 User 变成签好名的 JWT。
// 不知道数据库、不知道密码校验——AuthService 编排业务，这里只管签发。
// 以后要升级成 RS256 非对称签名给游戏 MS 验签，或者加 refresh token，
// 改动范围收在这一个文件里，不用去动 AuthService。
public class JwtService(IOptions<JwtOptions> options)
{
    private readonly JwtOptions _options = options.Value;

    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpiresInMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public DateTime GetExpiryFor(DateTime issuedAt) =>
        issuedAt.AddMinutes(_options.ExpiresInMinutes);
}

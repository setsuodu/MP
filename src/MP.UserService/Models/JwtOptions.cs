namespace MP.UserService.Models;

public class JwtOptions
{
    public string Issuer { get; set; } = "mp-midplatform";
    public string Audience { get; set; } = "game-clients";
    public string SecretKey { get; set; } = string.Empty; // 你的超级长密钥-至少32字符-生产换成配置
    public int ExpiresInMinutes { get; set; } = 1440; // 24小时
}
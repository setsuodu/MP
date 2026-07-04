namespace MP.UserService.Models;

public record AuthResponse(string Token, User User, DateTime ExpiresAt);
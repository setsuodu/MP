using System.Security.Claims;
using MP.UserService.Models;
using MP.UserService.Repositories;
using MP.UserService.Services;

namespace MP.UserService.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/users").WithTags("Users");

        group.MapPost("/register", async (RegisterRequest req, AuthService auth) =>
        {
            var result = await auth.RegisterAsync(req);
            return result != null
                ? Results.Ok(result)
                : Results.BadRequest("用户名或邮箱已存在");
        })
        .WithName("Register");

        group.MapPost("/login", async (LoginRequest req, AuthService auth) =>
        {
            var result = await auth.LoginAsync(req);
            return result != null
                ? Results.Ok(result)
                : Results.Unauthorized();
        })
        .WithName("Login");

        // 受 JWT 保护：验证"登录拿到的 token 能不能在后续请求里认出人"
        group.MapGet("/me", async (ClaimsPrincipal claims, IUserRepository users) =>
        {
            var idStr = claims.FindFirstValue(ClaimTypes.NameIdentifier);
            if (idStr is null || !Guid.TryParse(idStr, out var id))
                return Results.Unauthorized();

            var user = await users.GetByIdAsync(id);
            return user != null
                ? Results.Ok(user)
                : Results.NotFound();
        })
        .RequireAuthorization()
        .WithName("Me");
    }
}
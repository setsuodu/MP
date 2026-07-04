using MP.UserService.Models;
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
    }
}
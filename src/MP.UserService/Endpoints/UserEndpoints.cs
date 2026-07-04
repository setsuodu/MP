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
            // TODO: 实现注册
            return Results.Ok("Register endpoint - to be implemented");
        });

        group.MapPost("/login", async (LoginRequest req, AuthService auth) =>
        {
            var result = await auth.LoginAsync(req);
            return result != null ? Results.Ok(result) : Results.Unauthorized();
        });
    }
}
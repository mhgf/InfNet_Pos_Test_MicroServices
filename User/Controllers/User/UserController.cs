using Core.Services.Interfaces;
using Shared.Core.Notification;
using User.Web.Controllers.Dtos;

namespace User.Web.Controllers;

public static class UserController
{
    public static RouteGroupBuilder ConfigureUserController(this RouteGroupBuilder app)
    {
        var group = app.MapGroup("user").WithDescription("user").WithTags("User");
        group.MapGet("/", GetUsers);
        group.MapGet("/{id}", GetUser);
        group.MapPost("/", CreateUser);
        group.MapPut("/{id}", UpdateUser);
        group.MapDelete("/{id}", DeleteUser);
        return group;
    }

    private static async Task<IResult> GetUsers(IUserServices userRepository)
    {
        var users = await userRepository.GetAllAsync();

        return TypedResults.Ok(users);
    }

    private static async Task<IResult> GetUser(Guid id, IUserServices userService,
        NotificationContext notificationContext)
    {
        if (Guid.Empty == id)
        {
            notificationContext.AddNotification("User", $"User {id} invalido");
            return TypedResults.BadRequest();
        }

        var user = await userService.GetByIdAsync(id);

        if (user is not null) return TypedResults.Ok(user);

        notificationContext.AddNotification("User", $"User {id} invalido");
        return TypedResults.BadRequest();
    }

    private static async Task<IResult> CreateUser(CreateUserDto dto, IUserServices userService)
    {
        var user = await userService.CreateAsync(dto.Name, dto.Email);
        return TypedResults.Created($"/api/user/{user?.Id}", user);
    }

    private static async Task<IResult> UpdateUser(Guid id, CreateUserDto dto, IUserServices userService)
    {
        var user = await userService.Update(id, dto.Name, dto.Email);
        return TypedResults.Ok(user);
    }
    private static async Task<IResult> DeleteUser(Guid id, IUserServices userService)
    {
        var result = await userService.Delete(id);
        return TypedResults.Ok(result);
    }
}
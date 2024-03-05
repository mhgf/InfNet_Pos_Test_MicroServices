using Core.Entities;
using Core.Repositories;
using Core.Services.Interfaces;
using Shared.Core.Notification;
using Shared.Infra;

namespace Core.Services;

public class UserServices(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    NotificationContext notificationContext) : IUserServices
{
    public async Task<IEnumerable<User>> GetAllAsync(bool withDelete = false)
    {
        return await userRepository.GetAllAsync(withDelete);
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await userRepository.GetByIdAsync(id);
    }

    public async Task<User?> CreateAsync(string name, string email)
    {
        var (user, errors) = User.Create(name, email);

        if (errors is { IsValid: false })
        {
            notificationContext.AddNotifications(errors);
            return null;
        }

        await userRepository.CreatedAsync(user);
        await unitOfWork.SaveChangesAsync();
        return user;
    }

    public async Task<User?> Update(Guid id, string name, string email)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user is null)
        {
            notificationContext.AddNotification("User", $"User {id} não encontrado");
            return null;
        }

        var errors = user.Update(name, email);
        if (errors is not null and { IsValid: false })
        {
            notificationContext.AddNotifications(errors);
            return null;
        }

        await userRepository.UpdateAsync(user);
        await unitOfWork.SaveChangesAsync();
        return user;
    }

    public async Task<bool> Delete(Guid id)
    {
        var user = await userRepository.GetByIdAsync(id);

        if (user is not null) return await userRepository.DeleteAsync(user);

        notificationContext.AddNotification("User", $"User {id} não encontrado");
        return false;
    }
}
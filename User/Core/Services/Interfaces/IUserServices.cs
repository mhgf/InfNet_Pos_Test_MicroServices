using Core.Entities;

namespace Core.Services.Interfaces;

public interface IUserServices
{
    public Task<IEnumerable<User>> GetAllAsync(bool withDelete = false);
    public Task<User?> GetByIdAsync(Guid id);

    public Task<User?> CreateAsync(string name, string email);
    public Task<User?> Update(Guid id, string name, string email);
    public Task<bool> Delete(Guid id);
}
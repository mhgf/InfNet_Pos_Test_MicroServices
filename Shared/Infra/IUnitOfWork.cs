namespace Shared.Infra;

public interface IUnitOfWork
{
    public Task<int> SaveChangesAsync();
}
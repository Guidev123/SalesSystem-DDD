namespace SalesSystem.SharedKernel.Data
{
    public interface IUnitOfWork
    {
        Task<bool> CommitAsync();
    }
}

namespace FleaApp_Api.Interfaces
{
    public interface IUnitOfWork
    {
        IMarketRepo MarketRepo {get;}

        Task<bool> Complete();
        bool HasChanges();
    }
}
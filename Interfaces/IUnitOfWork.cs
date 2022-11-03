namespace FleaApp_Api.Interfaces
{
    public interface IUnitOfWork
    {
        IMarketRepo MarketRepo { get; }
        IShopRepo ShopRepo { get; }

        Task<bool> Complete();
        bool HasChanges();
    }
}
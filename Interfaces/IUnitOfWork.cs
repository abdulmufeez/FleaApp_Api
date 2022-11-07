namespace FleaApp_Api.Interfaces
{
    public interface IUnitOfWork
    {
        IMarketRepo MarketRepo { get; }
        IShopRepo ShopRepo { get; }
        ICategoryRepo CategoryRepo { get; }
        ISubCategoryRepo SubCategoryRepo { get; }
        IProductRepo ProductRepo { get; }

        Task<bool> Complete();
        bool HasChanges();
    }
}
using AutoMapper;
using fleaApi.Data;
using FleaApp_Api.Interfaces;

namespace FleaApp_Api.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UnitOfWork(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public IMarketRepo MarketRepo => new MarketRepo(_context, _mapper);
        public IShopRepo ShopRepo => new ShopRepo(_context, _mapper);

        public ICategoryRepo CategoryRepo => new CategoryRepo(_context, _mapper);

        public ISubCategoryRepo SubCategoryRepo => new SubCategoryRepo(_context, _mapper);

        public IProductRepo ProductRepo => throw new NotImplementedException();

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}
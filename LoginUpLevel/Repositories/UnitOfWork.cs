using LoginUpLevel.Data;
using LoginUpLevel.Repositories.Interface;

namespace LoginUpLevel.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            ProductRepository = new ProductRepository(_context);
            CustomerRepository = new CustomerRepository(_context);
            EmployeeRepository = new EmployeeRepository(_context);
            OrderRepository = new OrderRepository(_context);
            OrderDetailRepository = new OrderDetailRepository(_context);
            OrderAdressRepository = new OrderAdressRepository(_context);
            StatisticsRepository = new StatisticsRepository(_context);
            CartRepository = new CartRepository(_context);
            CartItemRepository = new CartItemRepository(_context);
            ColorRepository = new ColorRepository(_context);
            ProductColorRepository = new ProductColorRepository(_context);
        }
        public IProductRepository ProductRepository { get; private set; }

        public ICustomerRepository CustomerRepository { get; private set; }

        public IEmployeeRepository EmployeeRepository { get; private set; }

        public IOrderRepository OrderRepository { get; private set; }

        public IOrderDetailRepository OrderDetailRepository { get; private set; }

        public IOrderAdressRepository OrderAdressRepository { get; private set; }
        public IStatisticsRepository StatisticsRepository { get; private set; }
        public ICartRepository CartRepository { get; private set; }
        public ICartItemRepository CartItemRepository { get; private set; }
        public IColorRepository ColorRepository { get; private set; }
        public IProductColorRepository ProductColorRepository { get; private set; }

        public void Dispose()
        {
            _context.Dispose();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
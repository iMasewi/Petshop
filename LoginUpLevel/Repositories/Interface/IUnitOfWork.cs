namespace LoginUpLevel.Repositories.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository ProductRepository { get; }
        ICustomerRepository CustomerRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IOrderRepository OrderRepository { get; }
        IOrderDetailRepository OrderDetailRepository { get; }
        IOrderAdressRepository OrderAdressRepository { get; }
        IStatisticsRepository StatisticsRepository { get; }
        ICartRepository CartRepository { get; }
        ICartItemRepository CartItemRepository { get; }
        IColorRepository ColorRepository { get; }
        IProductColorRepository ProductColorRepository { get; }
        ICommentRepository CommentRepository { get; }
        Task SaveChangesAsync();
        void Dispose();
    }
}
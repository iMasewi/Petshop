namespace LoginUpLevel.Repositories.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll(int page, int pageSize);
        Task Add(T entity);
        Task<IEnumerable<T>> GetAll();
        Task Update(T entity);
        Task Delete(T entity);
        Task<int> Count();
    }
}

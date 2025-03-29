
namespace Data.Repositories.Base
{
    public interface IBaseRepistory<T>
    {
       Task<List<T>> GetAllAsync();
       Task<T> GetAsync(int id);
        Task CreateAsync(T data);
        void Update(T data);
        void Delete(T data);
    }
}

using System.Threading.Tasks;

namespace AFI.Registration.Data
{
    public interface IRepository
    {
        Task AddAsync<T>(T entity) where T : class;
        Task<int> SaveChangesAsync();
    }
}
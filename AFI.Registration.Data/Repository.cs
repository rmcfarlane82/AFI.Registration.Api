using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AFI.Registration.Data
{
    public class Repository : IRepository
    {
        private readonly DbContext _context;

        public Repository(DbContext context)
        {
            _context = context;
        }

        public Task AddAsync<T>(T entity) where T : class
        {
            return _context.Set<T>().AddAsync(entity).AsTask();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}

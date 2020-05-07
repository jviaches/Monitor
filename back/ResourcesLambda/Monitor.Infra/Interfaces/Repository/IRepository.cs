using Monitor.Infra.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monitor.Infra.Interfaces.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> GetById(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}

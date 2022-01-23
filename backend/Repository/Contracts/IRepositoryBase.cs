using System.Collections.Generic;

namespace Repository.Contracts
{
    public interface IRepositoryBase<T, P> where T : class
    {
        IEnumerable<T> All();
        T Find(P id);
        bool Create(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        bool Exists(P id);
        bool Save();
    }
}

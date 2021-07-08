using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Repositories
{
    public interface ITasks <T1, T2> where T1 :class // T1 is the Model, T2 Primary key type (Ex: int)
    {
        IEnumerable<T1> GetAll();
        T1 GetById(T2 id);
        T1 Insert(T1 entity);
        T1 Update(T1 entity);
        Boolean Delete(T1 entity);
        Boolean Any(T2 id);
        Boolean Save();
    }
}

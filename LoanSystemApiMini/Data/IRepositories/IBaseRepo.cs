using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoanSystemApiMini.Data.IRepositories
{
    public interface IBaseRepo<T>
    {
        public Task<IEnumerable<T>> GetAll();
    }
}
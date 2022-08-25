using System.Threading.Tasks;

namespace LoanSystemApiMini.Data.IRepositories
{
    public interface IRepo<T> : IBaseRepo<T>
    {
        public Task<int> Create(T input);
        public Task Update(T input);
        public Task<T> Get(int id);
    }
}
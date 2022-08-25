using System.Threading.Tasks;

namespace LoanSystemApiMini.Data.IRepositories
{
    public interface IRegularRepo<T> : IRepo<T>
    {
        public Task<T> Get(int id);
    }
}
using System.Threading.Tasks;

namespace LoanSystemApiMini.Data.IRepositories
{
    public interface ISpecialRepo<T, IdType> : IRepo<T>
    {
        public Task<T> Get(IdType id);
    }
}
using System.Threading.Tasks;

namespace LoanSystemWPFMini.Managers.IManagers
{
    public interface IManager<T> : IBaseManager<T>
    {
        Task<T> Create(T input);
        Task Update(T input);
        Task<T> Get(int id);
    }
}
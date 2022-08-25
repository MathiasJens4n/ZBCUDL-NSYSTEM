using System.Threading.Tasks;

namespace LoanSystemWPFMini.Managers.IManagers
{
    public interface IRegularManager<T> : IManager<T>
    {
        Task<T> Get(int id);
    }
}
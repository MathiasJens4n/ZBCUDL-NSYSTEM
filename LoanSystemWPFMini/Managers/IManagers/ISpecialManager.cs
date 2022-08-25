using System.Threading.Tasks;

namespace LoanSystemWPFMini.Managers.IManagers
{
    public interface ISpecialManager<T, IdType> : IManager<T>
    {
        Task<T> Get(IdType id);
    }
}
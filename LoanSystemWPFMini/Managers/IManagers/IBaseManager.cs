using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoanSystemWPFMini.Managers.IManagers
{
    public interface IBaseManager<T>
    {
        Task<List<T>> GetAll();
    }
}
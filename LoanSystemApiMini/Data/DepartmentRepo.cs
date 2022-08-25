using Dapper;
using LoanSystemApiMini.Data.IRepositories;
using LoanSystemLibraryMini;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace LoanSystemApiMini.Data
{
    public class DepartmentRepo : IBaseRepo<Department>
    {
        private readonly DatabaseContext _context;

        public DepartmentRepo(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetAll()
        {
            using (IDbConnection conn = _context.CreateConnection())
            {
                IEnumerable<Department> departments = await conn.QueryAsync<Department>("getalldepartments");
                return departments;
            }
        }
    }
}

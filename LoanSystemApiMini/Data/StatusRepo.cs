using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using LoanSystemApiMini.Data.IRepositories;
using LoanSystemLibraryMini;

namespace LoanSystemApiMini.Data
{
    public class StatusRepo : IBaseRepo<Status>
    {
        private readonly DatabaseContext _context;

        public StatusRepo(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Status>> GetAll()
        {
            using (IDbConnection conn = _context.CreateConnection())
            {
                IEnumerable<Status> statuses = await conn.QueryAsync<Status>("getallstatuses");
                return statuses;
            }
        }
    }
}
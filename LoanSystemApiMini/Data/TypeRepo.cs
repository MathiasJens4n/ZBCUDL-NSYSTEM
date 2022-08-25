using Dapper;
using LoanSystemApiMini.Data.IRepositories;
using LoanSystemLibraryMini;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LoanSystemApiMini.Data
{
    public class TypeRepo : IRepo<Type>
    {
        private readonly DatabaseContext _context;
        public TypeRepo(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Type>> GetAll()
        {
            using (IDbConnection conn = _context.CreateConnection())
            {
                IEnumerable<Type> types = await conn.QueryAsync<Type>("getalltypes", commandType: CommandType.StoredProcedure);
                return types;
            }
        }

        public async Task<Type> Get(int id)
        {
            using (IDbConnection conn = _context.CreateConnection())
            {
                var param = new DynamicParameters();
                param.Add("@id", id);

                IEnumerable<Type> types = await conn.QueryAsync<Type>("gettype", param, commandType: CommandType.StoredProcedure);

                if (types.Any())
                {
                    return types.First();
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<int> Create(Type input)
        {
            var param = new DynamicParameters();
            param.Add("@name", input.Name);

            using (IDbConnection conn = _context.CreateConnection())
            {
                int id = await conn.QuerySingleAsync<int>("posttype", param, commandType: CommandType.StoredProcedure);
                return id;
            }
        }

        public async Task Update(Type input)
        {
            using (IDbConnection conn = _context.CreateConnection())
            {
                var param = new DynamicParameters();
                param.Add("@id", input.Id);
                param.Add("@name", input.Name);

                await conn.ExecuteAsync("puttype", param, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
using Dapper;
using LoanSystemApiMini.Data.IRepositories;
using LoanSystemLibraryMini;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LoanSystemApiMini.Data
{
    public class UserRepo : IRepo<User>
    {
        private readonly DatabaseContext _context;

        public UserRepo(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            using (IDbConnection conn = _context.CreateConnection())
            {
                IEnumerable<User> users = await conn.QueryAsync<User, LoanRule, Status, User>
                    (
                        "getallusers",
                        (User, LoanRule, Status) => { User.LoanRule = LoanRule; User.Status = Status; return User; },
                        splitOn: "loanruleid, statusid",
                        commandType: CommandType.StoredProcedure
                    );

                return users;
            }
        }

        public async Task<User> Get(int id)
        {
            using (IDbConnection conn = _context.CreateConnection())
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@id", id);

                IEnumerable<User> user = await conn.QueryAsync<User, LoanRule, Status, User>
                    (
                        "getuser",
                        (User, LoanRule, Status) => { User.LoanRule = LoanRule; User.Status = Status; return User; },
                        param,
                        commandType: CommandType.StoredProcedure
                    );

                if (user.Any())
                {
                    return user.First();
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<int> Create(User input)
        {
            using (IDbConnection conn = _context.CreateConnection())
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@unilogin", input.UniLogin);
                param.Add("@name", input.Name);
                param.Add("@moblienumber", input.MobileNumber);
                param.Add("@note", input.Note);
                param.Add("@loanruleid", input.LoanRule.Id);
                param.Add("@statusid", input.Status.Id);

                int id = await conn.QuerySingleAsync<int>("postuser", param, commandType: CommandType.StoredProcedure);
                return id;
            }
        }

        public async Task Update(User input)
        {
            using (IDbConnection conn = _context.CreateConnection())
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@id", input.Id);
                param.Add("@unilogin", input.UniLogin);
                param.Add("@name", input.Name);
                param.Add("@moblienumber", input.MobileNumber);
                param.Add("@note", input.Note);
                param.Add("@loanruleid", input.LoanRule.Id);
                param.Add("@statusid", input.Status.Id);

                await conn.ExecuteAsync("putuser", param, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
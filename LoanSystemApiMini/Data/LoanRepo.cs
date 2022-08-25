using Dapper;
using LoanSystemApiMini.Data.IRepositories;
using LoanSystemLibraryMini;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LoanSystemApiMini.Data
{
    public class LoanRepo : IRepo<Loan>
    {
        private readonly DatabaseContext _context;

        public LoanRepo(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Loan>> GetAll()
        {
            using (IDbConnection conn = _context.CreateConnection())
            {
                IEnumerable<Loan> loans = await conn.QueryAsync<Loan, User, Equipment, Status, Loan>
                    (
                        "getallloans",
                        (Loan, User, Equipment, Status) =>
                        { Loan.User = User; Loan.Equipment = Equipment; Loan.Status = Status; return Loan; },
                        commandType: CommandType.StoredProcedure
                    );

                return loans;
            }
        }

        public async Task<Loan> Get(int id)
        {
            using (IDbConnection conn = _context.CreateConnection())
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@id", id);

                IEnumerable<Loan> loan = await conn.QueryAsync<Loan, User, Equipment, Status, Loan>
                    (
                        "getloan",
                        (Loan, User, Equipment, Status) =>
                        { Loan.User = User; Loan.Equipment = Equipment; Loan.Status = Status; return Loan; },
                        param,
                        commandType: CommandType.StoredProcedure
                    );

                if (loan.Any())
                {
                    return loan.First();
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<int> Create(Loan input)
        {
            using (IDbConnection conn = _context.CreateConnection())
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@collectedtime", input.CollectedTime);
                param.Add("@returnedtime", input.ReturnedTime);
                param.Add("@returndeadline", input.ReturnDeadline);
                param.Add("@note", input.Note);
                param.Add("@userid", input.User.Id);
                param.Add("@equipmentid", input.Equipment.Id);
                param.Add("@statusid", input.Status.Id);

                int id = await conn.QueryFirstAsync<int>("postloan", param, commandType: CommandType.StoredProcedure);
                return id;
            }
        }

        public async Task Update(Loan input)
        {
            using (IDbConnection conn = _context.CreateConnection())
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@id", input.Id);
                param.Add("@collectedtime", input.CollectedTime);
                param.Add("@returnedtime", input.ReturnedTime);
                param.Add("@returndeadline", input.ReturnDeadline);
                param.Add("@note", input.Note);
                param.Add("@userid", input.User.Id);
                param.Add("@equipmentid", input.Equipment.Id);
                param.Add("@statusid", input.Status.Id);

                await conn.ExecuteAsync("putloan", param, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
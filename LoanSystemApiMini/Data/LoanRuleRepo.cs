using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using LoanSystemApiMini.Data.IRepositories;
using LoanSystemLibraryMini;

namespace LoanSystemApiMini.Data
{
    public class LoanRuleRepo : IRepo<LoanRule>
    {
        private readonly DatabaseContext _context;

        public LoanRuleRepo(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LoanRule>> GetAll()
        {
            using (IDbConnection conn = _context.CreateConnection())
            {
                IEnumerable<LoanRule> loanRules = await conn.QueryAsync<LoanRule, LoanRule, Status, LoanRule>
                    (
                        "getallloanrules",
                        (LoanRule, ReplacementRule, Status) =>
                        { LoanRule.ReplacementRule = ReplacementRule; LoanRule.Status = Status; return LoanRule; },
                        commandType: CommandType.StoredProcedure
                    );

                return loanRules;
            }
        }

        public async Task<LoanRule> Get(int id)
        {
            using (IDbConnection conn = _context.CreateConnection())
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@id", id);

                IEnumerable<LoanRule> loanRule = await conn.QueryAsync<LoanRule, LoanRule, Status, LoanRule>
                    (
                        "getloanrule",
                        (LoanRule, ReplacementRule, Status) =>
                        { LoanRule.ReplacementRule = ReplacementRule; LoanRule.Status = Status; return LoanRule; },
                        param,
                        commandType: CommandType.StoredProcedure
                    );

                if (loanRule.Any())
                {
                    return loanRule.First();
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<int> Create(LoanRule input)
        {
            using (IDbConnection conn = _context.CreateConnection())
            {
                DynamicParameters param = new DynamicParameters();

                param.Add("@name", input.Name);
                param.Add("@loanlength", input.LoanLength);

                if (input.ReplacementRule != null)
                {
                    param.Add("@expirationdate", input.ExpirationDate);
                    param.Add("@replacementruleid", input.ReplacementRule.Id);
                }
                else
                {
                    param.Add("@expirationdate", null);
                    param.Add("@replacementruleid", null);
                }

                param.Add("@statusid", input.Status.Id);
                param.Add("@note", input.Note);

                int id = await conn.QueryFirstOrDefaultAsync<int>("postloanrule", param, commandType: CommandType.StoredProcedure);
                return id;
            }
        }

        public async Task Update(LoanRule input)
        {
            using (IDbConnection conn = _context.CreateConnection())
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@id", input.Id);
                param.Add("@name", input.Name);
                param.Add("@loanlength", input.LoanLength);

                if (input.ReplacementRule != null)
                {
                    param.Add("@expirationdate", input.ExpirationDate);
                    param.Add("@replacementruleid", input.ReplacementRule.Id);
                }
                else
                {
                    param.Add("@expirationdate", null);
                    param.Add("@replacementruleid", null);
                }

                param.Add("@statusid", input.Status.Id);
                param.Add("@note", input.Note);

                await conn.ExecuteAsync("putloanrule", param, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
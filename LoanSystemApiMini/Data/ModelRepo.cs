using Dapper;
using LoanSystemApiMini.Data.IRepositories;
using LoanSystemLibraryMini;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LoanSystemApiMini.Data
{
    public class ModelRepo : IRepo<Model>
    {
        private readonly DatabaseContext _content;

        public ModelRepo(DatabaseContext content)
        {
            _content = content;
        }

        public async Task<IEnumerable<Model>> GetAll()
        {
            using (IDbConnection conn = _content.CreateConnection())
            {
                IEnumerable<Model> models = await conn.QueryAsync<Model, Type, Model>
                    (
                        "getallmodels", 
                        (Model, type) => { Model.Type = type; return Model; },
                        commandType: CommandType.StoredProcedure
                    );

                return models;
            }
        }

        public async Task<Model> Get(int id)
        {
            using (IDbConnection conn = _content.CreateConnection())
            {
                var param = new DynamicParameters();
                param.Add("@id", id);

                IEnumerable<Model> model = await conn.QueryAsync<Model, Type, Model>
                    (
                        "getmodel",
                        (Model, type) => { Model.Type = type; return Model; },
                        param, 
                        commandType: CommandType.StoredProcedure
                    );

                if(model.Any())
                {
                    return model.First();
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<int> Create(Model input)
        {
            using (IDbConnection conn = _content.CreateConnection())
            {
                var param = new DynamicParameters();
                param.Add("@typeid", input.Type.Id);
                param.Add("@name", input.Name);

                int id = await conn.QuerySingleAsync<int>("postmodel", param, commandType: CommandType.StoredProcedure);
                return id;
            }
        }

        public async Task Update(Model input)
        {
            using (IDbConnection conn = _content.CreateConnection())
            {
                var param = new DynamicParameters();
                param.Add("@id", input.Id);
                param.Add("@typeid", input.Type.Id);
                param.Add("@name", input.Name);

                await conn.ExecuteAsync("putmodel", param, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
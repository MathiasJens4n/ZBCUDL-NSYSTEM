using Dapper;
using LoanSystemApiMini.Data.IRepositories;
using LoanSystemLibraryMini;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LoanSystemApiMini.Data
{
    public class EquipmentRepo : IRepo<Equipment>
    {
        private readonly DatabaseContext _context;

        public EquipmentRepo(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Equipment>> GetAll()
        {
            using (IDbConnection conn = _context.CreateConnection())
            {
                IEnumerable<Equipment> equipments = await conn.QueryAsync<Equipment, Department, Department, Model, Type, Status, Equipment>
                    (
                        "getallequipments",
                        (Equipment, belong, current, Model, Type, Status) =>
                        {
                            Equipment.BelongingDepartment = belong;
                            Equipment.CurrentDepartment = current;
                            Equipment.Model = Model;
                            Equipment.Model.Type = Type;
                            Equipment.Status = Status;
                            return Equipment;
                        },
                        commandType: CommandType.StoredProcedure
                    );

                return equipments;
            }
        }

        public async Task<Equipment> Get(int id)
        {
            using (IDbConnection conn = _context.CreateConnection())
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@id", id);

                IEnumerable<Equipment> equipment = await conn.QueryAsync<Equipment, Department, Department, Model, Type, Status, Equipment>
                    (
                        "getequipment",
                        (Equipment, belong, current, Model, Type, Status) =>
                        {
                            Equipment.BelongingDepartment = belong;
                            Equipment.CurrentDepartment = current;
                            Equipment.Model = Model;
                            Equipment.Model.Type = Type;
                            Equipment.Status = Status;
                            return Equipment;
                        },
                        param,
                        commandType: CommandType.StoredProcedure
                    );

                if (equipment.Any())
                {
                    return equipment.First();
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<int> Create(Equipment input)
        {
            using (IDbConnection conn = _context.CreateConnection())
            {
                var param = new DynamicParameters();
                param.Add("@identification", input.Identification);
                param.Add("@lastmaintenance", input.LastMaintenance);
                param.Add("@note", input.Note);
                param.Add("@worknote", input.WorkNote);
                param.Add("@belongingdepartmentid", input.BelongingDepartment.Id);
                param.Add("@currentdepartmentid", input.CurrentDepartment.Id);
                param.Add("@modelid", input.Model.Id);
                param.Add("@statusid", input.Status.Id);

                int id = await conn.QuerySingleAsync<int>("postequipment", param, commandType: CommandType.StoredProcedure);
                return id;
            }
        }

        public async Task Update(Equipment input)
        {
            using (IDbConnection conn = _context.CreateConnection())
            {
                var param = new DynamicParameters();
                param.Add("@id", input.Id);
                param.Add("@identification", input.Identification);
                param.Add("@lastmaintenance", input.LastMaintenance);
                param.Add("@note", input.Note);
                param.Add("@worknote", input.WorkNote);
                param.Add("@belongingdepartmentid", input.BelongingDepartment.Id);
                param.Add("@currentdepartmentid", input.CurrentDepartment.Id);
                param.Add("@modelid", input.Model.Id);
                param.Add("@statusid", input.Status.Id);

                await conn.ExecuteAsync("putequipment", param, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
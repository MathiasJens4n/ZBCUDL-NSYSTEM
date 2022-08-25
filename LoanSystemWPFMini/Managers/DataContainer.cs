using LoanSystemLibraryMini;
using LoanSystemWPFMini.Managers.IManagers;
using LoanSystemWPFMini.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoanSystemWPFMini.Managers
{
    public class DataContainer
    {
        private APIHelper _apiHelper;

        private List<Status> _allStatuses = new List<Status>();
        public List<Status> AllStatuses
        {
            get { return _allStatuses; }
            set { _allStatuses = value; }
        }

        private List<Department> _allDepartments = new List<Department>();
        public List<Department> AllDepartments
        {
            get { return _allDepartments; }
            set { _allDepartments = value; }
        }

        private List<LoanRule> _allLoanRules = new List<LoanRule>();
        public List<LoanRule> AllLoanRules
        {
            get { return _allLoanRules; }
            set { _allLoanRules = value; }
        }

        private List<User> _allUsers = new List<User>();
        public List<User> AllUsers
        {
            get { return _allUsers; }
            set { _allUsers = value; }
        }

        private List<Type> _allTypes = new List<Type>();
        public List<Type> AllTypes
        {
            get { return _allTypes; }
            set { _allTypes = value; }
        }

        private List<LoanSystemLibraryMini.Model> _allModels = new List<LoanSystemLibraryMini.Model>();
        public List<LoanSystemLibraryMini.Model> AllModels
        {
            get { return _allModels; }
            set { _allModels = value; }
        }

        private List<Equipment> _allEquipments = new List<Equipment>();
        public List<Equipment> AllEquipments
        {
            get { return _allEquipments; }
            set { _allEquipments = value; }
        }

        private List<Loan> _allLoans = new List<Loan>();
        public List<Loan> AllLoans
        {
            get { return _allLoans; }
            set { _allLoans = value; }
        }

        public IBaseManager<Status> StatusManager;
        public IBaseManager<Department> DepartmentManager;
        public IManager<LoanRule> LoanRuleManager;
        public IManager<User> UserManager;
        public IManager<Type> TypeManager;
        public IManager<LoanSystemLibraryMini.Model> ModelManager;
        public IManager<Equipment> EquipmentManager;
        public IManager<Loan> LoanManager;

        public DataContainer()
        {
            _apiHelper = new APIHelper();
            _apiHelper.InitializeClient();
            StatusManager = new StatusManager(_apiHelper);
            DepartmentManager = new DepartmentManager(_apiHelper);
            LoanRuleManager = new LoanRuleManager(_apiHelper);
            UserManager = new UserManager(_apiHelper);
            TypeManager = new TypeManager(_apiHelper);
            ModelManager = new ModelManager(_apiHelper);
            EquipmentManager = new EquipmentManager(_apiHelper);
            LoanManager = new LoanManager(_apiHelper);
        }

        public async Task InitializeData()
        {
            await GetData();
        }

        private async Task GetData()
        {
            AllStatuses = await StatusManager.GetAll();
            AllDepartments = await DepartmentManager.GetAll();
            AllLoanRules = await LoanRuleManager.GetAll();
            AllUsers = await UserManager.GetAll();
            AllTypes = await TypeManager.GetAll();
            AllModels = await ModelManager.GetAll();
            AllEquipments = await EquipmentManager.GetAll();
            AllLoans = await LoanManager.GetAll();
        }

        #region LoanRules
        public async Task<LoanRule> CreateLoanrule(LoanRule loanRule)
        {
            LoanRule newLoanRule = await LoanRuleManager.Create(loanRule);
            if (newLoanRule.ReplacementRule == null)
            {
                newLoanRule.ReplacementRule = new LoanRule { Id = 0, Name = "" };
                newLoanRule.ExpirationDate = null;
            }
            AllLoanRules.Add(newLoanRule);
            return newLoanRule;
        }

        public async Task UpdateLoanRule(LoanRule loanRule)
        {
            await LoanRuleManager.Update(loanRule);

            for (int i = 0; i < AllLoanRules.Count; i++)
            {
                if (AllLoanRules[i].Id == loanRule.Id)
                {
                    AllLoanRules[i] = loanRule;
                    break;
                }
            }
        }
        #endregion

        #region Users
        public async Task<User> CreateUser(User user)
        {
            User newUser = await UserManager.Create(user);
            AllUsers.Add(newUser);
            return newUser;
        }

        public async Task UpdateUser(User user)
        {
            await UserManager.Update(user);

            for (int i = 0; i < AllUsers.Count; i++)
            {
                if (AllUsers[i].Id == user.Id)
                {
                    AllUsers[i] = user;
                    break;
                }
            }
        }
        #endregion

        #region Types
        public async Task<LoanSystemLibraryMini.Type> CreateType(LoanSystemLibraryMini.Type type)
        {
            LoanSystemLibraryMini.Type newType = await TypeManager.Create(type);
            AllTypes.Add(newType);
            return newType;
        }

        public async Task UpdateType(LoanSystemLibraryMini.Type type)
        {
            await TypeManager.Update(type);

            for (int i = 0; i < AllTypes.Count; i++)
            {
                if (AllTypes[i].Id == type.Id)
                {
                    AllTypes[i] = type;
                    break;
                }
            }
        }

        public async Task DeleteType(int id)
        {
            await ((TypeManager)TypeManager).Delete(id);

            for (int i = 0; i < AllTypes.Count; i++)
            {
                if (AllTypes[i].Id == id)
                {
                    AllTypes.RemoveAt(i);
                    break;
                }
            }
        }
        #endregion

        #region Models
        public async Task<LoanSystemLibraryMini.Model> CreateModel(LoanSystemLibraryMini.Model model)
        {
            LoanSystemLibraryMini.Model newModel = await ModelManager.Create(model);
            AllModels.Add(newModel);
            return newModel;
        }

        public async Task UpdateModel(LoanSystemLibraryMini.Model model)
        {
            await ModelManager.Update(model);

            for (int i = 0; i < AllModels.Count; i++)
            {
                if (AllModels[i].Id == model.Id)
                {
                    AllModels[i] = model;
                    break;
                }
            }
        }

        public async Task DeleteModel(int id)
        {
            await ((ModelManager)ModelManager).Delete(id);

            for (int i = 0; i < AllModels.Count; i++)
            {
                if (AllModels[i].Id == id)
                {
                    AllModels.RemoveAt(i);
                    break;
                }
            }
        }
        #endregion

        #region Equipment
        public async Task<Equipment> CreateEquipment(Equipment equipment)
        {
            Equipment newEquipment = await EquipmentManager.Create(equipment);
            AllEquipments.Add(newEquipment);
            return newEquipment;
        }

        public async Task UpdateEquipment(Equipment equipment)
        {
            await EquipmentManager.Update(equipment);

            for (int i = 0; i < AllEquipments.Count; i++)
            {
                if (AllEquipments[i].Id == equipment.Id)
                {
                    AllEquipments[i] = equipment;
                    break;
                }
            }
        }
        #endregion

        #region Loan
        public async Task<Loan> CreateLoan(Loan loan)
        {
            Loan newLoan = await LoanManager.Create(loan);

            //foreach (var user in AllUsers)
            //{
            //    if (newLoan.User.Id == user.Id)
            //    {
            //        newLoan.User = user;
            //        break;
            //    }
            //}

            //foreach (var equipment in AllEquipments)
            //{
            //    if (newLoan.Equipment.Id == equipment.Id)
            //    {
            //        newLoan.Equipment = equipment;
            //        break;
            //    }
            //}

            //foreach (var status in AllStatuses)
            //{
            //    if (newLoan.Status.Id == status.Id)
            //    {
            //        newLoan.Status = status;
            //        break;
            //    }
            //}

            AllLoans.Add(newLoan);
            return newLoan;
        }

        public async Task UpdateLoan(Loan loan)
        {
            await LoanManager.Update(loan);

            for (int i = 0; i < AllLoans.Count; i++)
            {
                if (AllLoans[i].Id == loan.Id)
                {
                    AllLoans[i] = loan;
                    break;
                }
            }
        }
        #endregion
    }
}
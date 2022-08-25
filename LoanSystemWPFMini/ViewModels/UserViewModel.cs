using LoanSystemLibraryMini;
using LoanSystemWPFMini.Managers;
using LoanSystemWPFMini.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace LoanSystemWPFMini.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        private WindowManager _windowManager;

        private DataContainer _dataContainer;
        public DataContainer DataContainer
        {
            get { return _dataContainer; }
            set { _dataContainer = value; }
        }

        #region ICommands
        public ICommand SearchButton { get; set; }
        public ICommand ClearSearchButton { get; set; }
        public ICommand PrintButton { get; set; }
        public ICommand ClearInfoButton { get; set; }
        public ICommand LogOutButton { get; set; }
        public ICommand CreateOrUpdateUserButton { get; set; }
        public ICommand DeleteUserButton { get; set; }
        #endregion

        #region General Observable Collection Variables
        private List<User> _allUsers;
        private ObservableCollection<User> _shownUsers;
        public ObservableCollection<User> ShownUsers
        {
            get { return _shownUsers; }
            set { _shownUsers = value; OnPropertyChanged("ShownUsers"); }
        }

        private ObservableCollection<LoanRule> _loanRules;
        public ObservableCollection<LoanRule> LoanRules
        {
            get { return _loanRules; }
            set { _loanRules = value; OnPropertyChanged("LoanRules"); }
        }

        private ObservableCollection<Status> _status;
        public ObservableCollection<Status> Statuses
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged("Statuses"); }
        }
        #endregion

        #region Selected Variables
        private User _selectedUser;
        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;

                if (_selectedUser != null)
                {
                    SelectedUniLogin = SelectedUser.UniLogin;
                    SelectedName = SelectedUser.Name;
                    SelectedMobileNumber = SelectedUser.MobileNumber;
                    foreach (LoanRule loanRule in _loanRules)
                    {
                        if (SelectedUser.LoanRule.Id == loanRule.Id)
                        {
                            SelectedLoanRule = loanRule;
                            break;
                        }
                    }
                    foreach (Status status in Statuses)
                    {
                        if (SelectedUser.Status.Id == status.Id)
                        {
                            SelectedStatus = status;
                            break;
                        }
                    }
                    SelectedUserNote = SelectedUser.Note;

                    CreateOrUpdateUserButtonContent = "Gem";
                }
                else
                {
                    SelectedUniLogin = "";
                    SelectedName = "";
                    SelectedMobileNumber = "";
                    SelectedLoanRule = null;
                    SelectedStatus = null;
                    SelectedUserNote = "";

                    CreateOrUpdateUserButtonContent = "Opret";
                }

                OnPropertyChanged("SelectedUser");
            }
        }

        private string _selectedUniLogin;
        public string SelectedUniLogin
        {
            get { return _selectedUniLogin; }
            set { _selectedUniLogin = value; OnPropertyChanged("SelectedUniLogin"); }
        }

        private string _selectedName;
        public string SelectedName
        {
            get { return _selectedName; }
            set { _selectedName = value; OnPropertyChanged("SelectedName"); }
        }
        // TODO: Phone number not Mobile number
        private string _selectedMobileNumber;
        public string SelectedMobileNumber
        {
            get { return _selectedMobileNumber; }
            set { _selectedMobileNumber = value; OnPropertyChanged("SelectedMobileNumber"); }
        }

        private LoanRule _selectedLoanRule;
        public LoanRule SelectedLoanRule
        {
            get { return _selectedLoanRule; }
            set { _selectedLoanRule = value; OnPropertyChanged("SelectedLoanRule"); }
        }

        private Status _selectedStatus;
        public Status SelectedStatus
        {
            get { return _selectedStatus; }
            set { _selectedStatus = value; OnPropertyChanged("SelectedStatus"); }
        }

        private string _selectedUserNote;
        public string SelectedUserNote
        {
            get { return _selectedUserNote; }
            set { _selectedUserNote = value; OnPropertyChanged("SelectedUserNote"); }
        }
        #endregion

        #region Edit Selected Variable
        private bool _canSaveOrDeleteChecked = false;
        public bool CanSaveOrDeleteChecked
        {
            get { return _canSaveOrDeleteChecked; }
            set { _canSaveOrDeleteChecked = value; OnPropertyChanged("CanSaveOrDeleteChecked"); }
        }

        private string _createOrUpdateUserButtonContent = "Opret";
        public string CreateOrUpdateUserButtonContent
        {
            get { return _createOrUpdateUserButtonContent; }
            set { _createOrUpdateUserButtonContent = value; OnPropertyChanged("CreateOrUpdateUserButtonContent"); }
        }
        #endregion

        #region Upper Search Area Variables
        private ObservableCollection<string> _searchOptions;
        public ObservableCollection<string> SearchOptions
        {
            get { return _searchOptions; }
            set { _searchOptions = value; OnPropertyChanged("SearchOptions"); }
        }

        private string _selectedSearchOption;
        public string SelectedSearchOption
        {
            get { return _selectedSearchOption; }
            set { _selectedSearchOption = value; OnPropertyChanged("SelectedSearchOption"); }
        }

        private string _searchInput;
        public string SearchInput
        {
            get { return _searchInput; }
            set { _searchInput = value; OnPropertyChanged("SearchInput"); }
        }
        #endregion

        #region Middle Search Area Variables
        private bool _uniLoginChecked = true;
        public bool UniLoginChecked
        {
            get { return _uniLoginChecked; }
            set { _uniLoginChecked = value; OnPropertyChanged("UniLoginChecked"); }
        }

        private bool _nameChecked = true;
        public bool NameChecked
        {
            get { return _nameChecked; }
            set { _nameChecked = value; OnPropertyChanged("NameChecked"); }
        }

        private bool _mobileNumberChecked = true;
        public bool MobileNumberChecked
        {
            get { return _mobileNumberChecked; }
            set { _mobileNumberChecked = value; OnPropertyChanged("MobileNumberChecked"); }
        }

        private bool _loanRuleChecked = true;
        public bool LoanRuleChecked
        {
            get { return _loanRuleChecked; }
            set { _loanRuleChecked = value; OnPropertyChanged("LoanRuleChecked"); }
        }

        private bool _statusChecked = true;
        public bool StatusChecked
        {
            get { return _statusChecked; }
            set { _statusChecked = value; OnPropertyChanged("StatusChecked"); }
        }

        private bool _userNoteChecked = true;
        public bool UserNoteChecked
        {
            get { return _userNoteChecked; }
            set { _userNoteChecked = value; OnPropertyChanged("UserNoteChecked"); }
        }
        #endregion

        #region Lower Search Area Variables
        //private DateTime _selectedfFomDate = new DateTime(2020, 1, 1);
        //public DateTime SelectedFromDate
        //{
        //    get { return _selectedfFomDate; }
        //    set { _selectedfFomDate = value; OnPropertyChanged("SelectedFromDate"); ChangeDatePeriod(); }
        //}

        //private DateTime _selectedToDate = DateTime.Now.AddYears(1);
        //public DateTime SelectedToDate
        //{
        //    get { return _selectedToDate; }
        //    set { _selectedToDate = value; OnPropertyChanged("SelectedToDate"); ChangeDatePeriod(); }
        //}

        private ObservableCollection<string> _orderDirections;
        public ObservableCollection<string> OrderDirections
        {
            get { return _orderDirections; }
            set { _orderDirections = value; OnPropertyChanged("OrderDirections"); }
        }

        private string _selectedOrderDirection = "Stigende";
        public string SelectedOrderDirection
        {
            get { return _selectedOrderDirection; }
            set { _selectedOrderDirection = value; OnPropertyChanged("SelectedOrderDirection"); ChangeOrderDirection(); }
        }
        #endregion

        public UserViewModel()
        {
            _windowManager = new WindowManager();

            SearchOptions = new ObservableCollection<string> { "Uni login", "Navn", "Mobilnummer", "Låneregel", "Status", "" };
            OrderDirections = new ObservableCollection<string> { "Stigende", "Faldende" };

            SearchButton = new Command(x => Search());
            ClearSearchButton = new Command(x => ClearSearch());
            PrintButton = new Command(() => PrintData());
            ClearInfoButton = new Command(x => SelectedUser = null);
            LogOutButton = new Command(x => _loanSystemViewModel.ShowMenuPage());
            CreateOrUpdateUserButton = new Command(x => CreateOrUpdateUser());
            DeleteUserButton = new Command(x => DeleteUser());
        }

        #region Initialize Data on Load
        public void InitializeData()
        {
            GetData();
        }

        private void GetData()
        {
            _allUsers = new List<User>(DataContainer.AllUsers);
            ShownUsers = new ObservableCollection<User>(_allUsers);
            LoanRules = new ObservableCollection<LoanRule>(DataContainer.AllLoanRules);
            Statuses = new ObservableCollection<Status>(DataContainer.AllStatuses);
        }
        #endregion

        #region Private Button Methodes
        private void Search()
        {
            if (SelectedSearchOption != null)
            {
                if (SearchInput != null)
                {
                    ShownUsers.Clear();
                    switch (SelectedSearchOption.ToLower())
                    {
                        case "uni login":
                            foreach (User user in _allUsers)
                            {
                                if (user.UniLogin.ToLower().StartsWith(SearchInput.ToLower()))
                                {
                                    ShownUsers.Add(user);
                                }
                            }
                            break;
                        case "navn":
                            foreach (User user in _allUsers)
                            {
                                if (user.Name.ToLower().StartsWith(SearchInput.ToLower()))
                                {
                                    ShownUsers.Add(user);
                                }
                            }
                            break;
                        case "mobilnummer":
                            foreach (User user in _allUsers)
                            {
                                if (user.MobileNumber.StartsWith(SearchInput.ToLower()))
                                {
                                    ShownUsers.Add(user);
                                }
                            }
                            break;
                        case "låneregel":
                            foreach (User user in _allUsers)
                            {
                                if (user.LoanRule.Name.ToLower().StartsWith(SearchInput.ToLower()))
                                {
                                    ShownUsers.Add(user);
                                }
                            }
                            break;
                        case "status":
                            foreach (User user in _allUsers)
                            {
                                if (user.Status.Name.ToLower().StartsWith(SearchInput.ToLower()))
                                {
                                    ShownUsers.Add(user);
                                }
                            }
                            break;

                            //default:
                            //    RefreshShowUsers(_allUsers);
                            //    break;
                    }
                }
            }

            ChangeOrderDirection();
            //ChangeDatePeriod();
        }

        private void ClearSearch()
        {
            SelectedSearchOption = null;
            SearchInput = "";

            //SelectedFromDate = new DateTime(2020, 1, 1);
            //SelectedToDate = DateTime.Now.AddYears(1);
            SelectedOrderDirection = OrderDirections[0];

            UniLoginChecked = true;
            NameChecked = true;
            MobileNumberChecked = true;
            LoanRuleChecked = true;
            StatusChecked = true;
            UserNoteChecked = true;

            RefreshShowUsers(_allUsers);
        }

        private void PrintData()
        {
            FolderSelectDialog folderSelectDialog = new FolderSelectDialog();
            string path = folderSelectDialog.ChooseSavePath();

            if (!string.IsNullOrEmpty(path))
            {
                string csvData = "UniLogin;Name;MobileNumber;Model;LoanRule;Status;Note\r\n";

                foreach (User u in ShownUsers)
                {
                    csvData += $"\"{u.UniLogin}\";\"{u.Name}\";{u.MobileNumber};\"{u.LoanRule.Name}\";{u.Status.Name};\"{u.Note}\"\r\n";
                }

                File.WriteAllText(path, csvData);
            }
        }

        private async void CreateOrUpdateUser()
        {
            if (ValidateInput())
            {
                if (SelectedUser == null)
                {
                    User newUser = new User
                    {
                        UniLogin = SelectedUniLogin,
                        Name = SelectedName,
                        MobileNumber = SelectedMobileNumber,
                        LoanRule = SelectedLoanRule,
                        Status = SelectedStatus,
                        Note = SelectedUserNote
                    };

                    try
                    {
                        User createdUser = await DataContainer.CreateUser(newUser);
                        _allUsers.Add(createdUser);
                    }
                    catch (Exception ex)
                    {
                        _windowManager.OpenErrorWindow(ex.Message);
                    }
                }
                else
                {
                    SelectedUser.UniLogin = SelectedUniLogin;
                    SelectedUser.Name = SelectedName;
                    SelectedUser.MobileNumber = SelectedMobileNumber;
                    SelectedUser.LoanRule = SelectedLoanRule;
                    SelectedUser.Status = SelectedStatus;
                    SelectedUser.Note = SelectedUserNote;

                    try
                    {
                        await DataContainer.UpdateUser(SelectedUser);

                        for (int i = 0; i < _allUsers.Count; i++)
                        {
                            if (_allUsers[i].Id == SelectedUser.Id)
                            {
                                _allUsers[i] = SelectedUser;
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _windowManager.OpenErrorWindow(ex.Message);
                    }
                }

                CanSaveOrDeleteChecked = false;
                SelectedUser = null;
                RefreshShowUsers(_allUsers);
            }
        }

        public async void DeleteUser()
        {
            if (SelectedUser != null)
            {
                foreach (Status status in Statuses)
                {
                    if (status.Name.ToLower() == "deleted")
                    {
                        SelectedUser.Status = status;
                        break;
                    }
                }

                try
                {
                    await DataContainer.UpdateUser(SelectedUser);

                    for (int i = 0; i < _allUsers.Count; i++)
                    {
                        if (_allUsers[i].Id == SelectedUser.Id)
                        {
                            _allUsers[i] = SelectedUser;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _windowManager.OpenErrorWindow(ex.Message);
                }

                CanSaveOrDeleteChecked = false;
                RefreshShowUsers(_allUsers);
            }
        }
        #endregion

        #region Private Methodes
        private void ChangeOrderDirection()
        {
            List<User> tempUser = new List<User>();

            if (SelectedOrderDirection == "Stigende")
            {
                tempUser.AddRange(ShownUsers.OrderBy(x => x.UniLogin));
            }
            else
            {
                tempUser.AddRange(ShownUsers.OrderByDescending(x => x.UniLogin));
            }

            RefreshShowUsers(tempUser);
        }

        //private void ChangeDatePeriod()
        //{
        //    List<User> tempUsers = new List<User>();

        //    if (ShownUsers != null)
        //    {
        //        for (int i = 0; i < ShownUsers.Count; i++)
        //        {
        //            if (ShownUsers[i].LoanRule.ExpirationDate > SelectedFromDate && ShownUsers[i].LoanRule.ExpirationDate < SelectedToDate)
        //            {
        //                tempUsers.Add(ShownUsers[i]);
        //            }
        //        }

        //        RefreshShowUsers(tempUsers);
        //    }
        //}

        private void RefreshShowUsers(List<User> users)
        {
            ShownUsers.Clear();
            foreach (var user in users)
            {
                ShownUsers.Add(user);
            }
        }

        private bool ValidateInput()
        {
            string errorMessages = "";

            if (SelectedUniLogin == null || SelectedUniLogin == "")
            {
                errorMessages += "UniLogin er ikke udfyldt.\r\n";
            }
            if (SelectedName == null || SelectedName == "")
            {
                errorMessages += "Navn er ikke udfyldt.\r\n";
            }
            if (SelectedMobileNumber == null || SelectedMobileNumber == "")
            {
                errorMessages += "Mobil Nr. er ikke udfyldt.\r\n";
            }
            if (SelectedLoanRule == null)
            {
                errorMessages += "Der er ikke blevet valgt en Låne regel.\r\n";
            }
            if (SelectedStatus == null)
            {
                errorMessages += "Der er ikke blevet valgt en Status.";
            }

            if (!string.IsNullOrEmpty(errorMessages))
            {
                _windowManager.OpenErrorWindow(errorMessages);
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
    }
}
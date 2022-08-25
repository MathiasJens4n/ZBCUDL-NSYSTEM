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
    public class LoanViewModel : BaseViewModel
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
        public ICommand UpdateLoanButton { get; set; }
        public ICommand DeleteLoanButton { get; set; }
        #endregion

        #region General Observable Collection Variables
        private List<Loan> _allLoans;
        private ObservableCollection<Loan> _shownLoans;
        public ObservableCollection<Loan> ShownLoans
        {
            get { return _shownLoans; }
            set { _shownLoans = value; OnPropertyChanged("ShownLoans"); }
        }

        private ObservableCollection<Status> _statuses;
        public ObservableCollection<Status> Statuses
        {
            get { return _statuses; }
            set { _statuses = value; OnPropertyChanged("Statuses"); }
        }
        #endregion

        #region Selected Variables
        private Loan _selectedLoan;
        public Loan SelectedLoan
        {
            get { return _selectedLoan; }
            set
            {
                _selectedLoan = value;

                if (value != null)
                {
                    SelectedCollectedTime = SelectedLoan.CollectedTime.ToString();
                    SelectedReturnedTime = SelectedLoan.ReturnedTime.ToString();
                    SelectedReturnDeadline = SelectedLoan.ReturnDeadline.ToString();
                    SelectedLoanNote = SelectedLoan.Note;
                    SelectedUser = SelectedLoan.User;
                    SelectedEquipment = SelectedLoan.Equipment;
                    foreach (Status status in Statuses)
                    {
                        if (SelectedLoan.Status.Id == status.Id)
                        {
                            SelectedStatus = status;
                            break;
                        }
                    }
                }
                else
                {
                    SelectedCollectedTime = "";
                    SelectedReturnedTime = "";
                    SelectedReturnDeadline = "";
                    SelectedLoanNote = "";
                    SelectedUser = null;
                    SelectedEquipment = null;
                    SelectedStatus = null;
                }

                OnPropertyChanged("SelectedLoan");
            }
        }

        private string _selectedCollectedTime;
        public string SelectedCollectedTime
        {
            get { return _selectedCollectedTime; }
            set { _selectedCollectedTime = value; OnPropertyChanged("SelectedCollectedTime"); }
        }

        private string _selectedReturnedTime;
        public string SelectedReturnedTime
        {
            get { return _selectedReturnedTime; }
            set { _selectedReturnedTime = value; OnPropertyChanged("SelectedReturnedTime"); }
        }

        private string _selectedReturnDeadline;
        public string SelectedReturnDeadline
        {
            get { return _selectedReturnDeadline; }
            set { _selectedReturnDeadline = value; OnPropertyChanged("SelectedReturnDeadline"); }
        }

        private string _selectedLoanNote;
        public string SelectedLoanNote
        {
            get { return _selectedLoanNote; }
            set { _selectedLoanNote = value; OnPropertyChanged("SelectedLoanNote"); }
        }

        private User _selectedUser;
        public User SelectedUser
        {
            get { return _selectedUser; }
            set { _selectedUser = value; OnPropertyChanged("SelectedUser"); }
        }

        private Equipment _selectedEquipment;
        public Equipment SelectedEquipment
        {
            get { return _selectedEquipment; }
            set { _selectedEquipment = value; OnPropertyChanged("SelectedEquipment"); }
        }

        private Status _selectedStatus;
        public Status SelectedStatus
        {
            get { return _selectedStatus; }
            set { _selectedStatus = value; OnPropertyChanged("SelectedStatus"); }
        }
        #endregion

        #region Edit Selected Variable
        private bool _canSaveOrDeleteChecked = false;
        public bool CanSaveOrDeleteChecked
        {
            get { return _canSaveOrDeleteChecked; }
            set { _canSaveOrDeleteChecked = value; OnPropertyChanged("CanSaveOrDeleteChecked"); }
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
        private bool _collectedTimeChecked = true;
        public bool CollectedTimeChecked
        {
            get { return _collectedTimeChecked; }
            set { _collectedTimeChecked = value; OnPropertyChanged("CollectedTimeChecked"); }
        }

        private bool _returnedTimeChecked = true;
        public bool ReturnedTimeChecked
        {
            get { return _returnedTimeChecked; }
            set { _returnedTimeChecked = value; OnPropertyChanged("ReturnedTimeChecked"); }
        }

        private bool _returnDeadlineChecked = true;
        public bool ReturnDeadlineChecked
        {
            get { return _returnDeadlineChecked; }
            set { _returnDeadlineChecked = value; OnPropertyChanged("ReturnDeadlineChecked"); }
        }

        private bool _loanNoteChecked = true;
        public bool LoanNoteChecked
        {
            get { return _loanNoteChecked; }
            set { _loanNoteChecked = value; OnPropertyChanged("LoanNoteChecked"); }
        }

        private bool _userChecked = true;
        public bool UserChecked
        {
            get { return _userChecked; }
            set { _userChecked = value; OnPropertyChanged("UserChecked"); }
        }

        private bool _equipmentChecked = true;
        public bool EquipmentChecked
        {
            get { return _equipmentChecked; }
            set { _equipmentChecked = value; OnPropertyChanged("EquipmentChecked"); }
        }

        private bool _statusChecked = true;
        public bool StatusChecked
        {
            get { return _statusChecked; }
            set { _statusChecked = value; OnPropertyChanged("StatusChecked"); }
        }
        #endregion

        #region Lower Search Area Variables
        private DateTime _selectedFromCollectedDate = new DateTime(2020, 1, 1);
        public DateTime SelectedFromCollectedDate
        {
            get { return _selectedFromCollectedDate; }
            set { _selectedFromCollectedDate = value; OnPropertyChanged("SelectedFromCollectedDate"); ChangeDatePeriod(); }
        }

        private DateTime _selectedToCollectedDate = DateTime.Now.AddYears(1);
        public DateTime SelectedToCollectedDate
        {
            get { return _selectedToCollectedDate; }
            set { _selectedToCollectedDate = value; OnPropertyChanged("SelectedToCollectedDate"); ChangeDatePeriod(); }
        }

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

        public LoanViewModel()
        {
            _windowManager = new WindowManager();

            SearchOptions = new ObservableCollection<string> { "Opsamlingsdato", "Returneret dato", "Afleveringsfrist", "Note", "Bruger", "Udstyr", "Status", "" };
            OrderDirections = new ObservableCollection<string> { "Stigende", "Faldende" };

            SearchButton = new Command(x => Search());
            ClearSearchButton = new Command(x => ClearSearch());
            PrintButton = new Command(() => PrintData());
            ClearInfoButton = new Command(x => SelectedLoan = null);
            LogOutButton = new Command(x => _loanSystemViewModel.ShowMenuPage());
            UpdateLoanButton = new Command(x => UpdateLoan());
            DeleteLoanButton = new Command(x => DeleteLoan());
        }

        #region Initialize Date on Load
        public void InitializeData()
        {
            GetData();
        }

        private void GetData()
        {
            _allLoans = new List<Loan>(DataContainer.AllLoans);
            ShownLoans = new ObservableCollection<Loan>(_allLoans);
            Statuses = new ObservableCollection<Status>(DataContainer.AllStatuses);
        }
        #endregion

        #region Private Button Methods
        private void Search()
        {
            if (SelectedSearchOption != null)
            {
                if (SearchInput != null)
                {
                    ShownLoans.Clear();
                    switch (SelectedSearchOption.ToLower())
                    {
                        case "opsamlings dato":
                            foreach (var loan in _allLoans)
                            {
                                if (loan.CollectedTime != null && loan.CollectedTime.ToString().StartsWith(SearchInput.ToLower()))
                                {
                                    ShownLoans.Add(loan);
                                }
                            }
                            break;
                        case "retuneret dato":
                            foreach (var loan in _allLoans)
                            {
                                if (loan.ReturnedTime != null && loan.ReturnedTime.ToString().StartsWith(SearchInput.ToLower()))
                                {
                                    ShownLoans.Add(loan);
                                }
                            }
                            break;
                        case "afleveringsfrist":
                            foreach (var loan in _allLoans)
                            {
                                if (loan.ReturnDeadline != null && loan.ReturnDeadline.ToString().StartsWith(SearchInput.ToLower()))
                                {
                                    ShownLoans.Add(loan);
                                }
                            }
                            break;
                        case "note":
                            foreach (var loan in _allLoans)
                            {
                                if (loan.Note != null && loan.Note.ToLower().StartsWith(SearchInput.ToLower()))
                                {
                                    ShownLoans.Add(loan);
                                }
                            }
                            break;
                        case "bruger":
                            foreach (var loan in _allLoans)
                            {
                                if (loan.User.Name.ToLower().StartsWith(SearchInput.ToLower()))
                                {
                                    ShownLoans.Add(loan);
                                }
                            }
                            break;
                        case "udstyr":
                            foreach (var loan in _allLoans)
                            {
                                if (loan.Equipment.Identification.ToLower().StartsWith(SearchInput.ToLower()))
                                {
                                    ShownLoans.Add(loan);
                                }
                            }
                            break;
                        case "status":
                            foreach (var loan in _allLoans)
                            {
                                if (loan.Status.Name.ToLower().StartsWith(SearchInput.ToLower()))
                                {
                                    ShownLoans.Add(loan);
                                }
                            }
                            break;
                    }
                }
            }
        }

        private void ClearSearch()
        {
            SelectedSearchOption = null;
            SearchInput = "";

            SelectedFromCollectedDate = new DateTime(2020, 1, 1);
            SelectedToCollectedDate = DateTime.Now.AddYears(1);
            SelectedOrderDirection = OrderDirections[0];

            CollectedTimeChecked = true;
            ReturnedTimeChecked = true;
            ReturnDeadlineChecked = true;
            LoanNoteChecked = true;
            UserChecked = true;
            EquipmentChecked = true;
            StatusChecked = true;

            RefreshShownLoans(_allLoans);
        }

        private void PrintData()
        {
            FolderSelectDialog folderSelectDialog = new FolderSelectDialog();
            string path = folderSelectDialog.ChooseSavePath();

            if (!string.IsNullOrEmpty(path))
            {
                string csvData = "CollectedTime;ReturnedTime;ReturnDeadline;UniLogin;Identification;Status;Note\r\n";

                foreach (Loan l in ShownLoans)
                {
                    csvData += $"{l.CollectedTime.ToString("dd-MM-yyyy HH:mm")};{l.ReturnedTime?.ToString("dd-MM-yyyy HH:mm")};{l.ReturnDeadline.ToString("dd-MM-yyyy HH:mm")};" +
                               $"\"{l.User.UniLogin}\";\"{l.Equipment.Identification}\";{l.Status.Name};\"{l.Note}\"\r\n";
                }

                File.WriteAllText(path, csvData);
            }
        }

        private async void UpdateLoan()
        {
            if (SelectedLoan != null)
            {
                SelectedLoan.Status = SelectedStatus;
                SelectedLoan.Note = SelectedLoanNote;

                try
                {
                    await DataContainer.UpdateLoan(SelectedLoan);

                    for (int i = 0; i < _allLoans.Count; i++)
                    {
                        if (_allLoans[i].Id == SelectedLoan.Id)
                        {
                            _allLoans[i] = SelectedLoan;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _windowManager.OpenErrorWindow(ex.Message);
                }

                CanSaveOrDeleteChecked = false;
                RefreshShownLoans(_allLoans);
            }
        }

        private async void DeleteLoan()
        {
            if (SelectedLoan != null)
            {
                foreach (var status in Statuses)
                {
                    if (status.Name == "Deleted")
                    {
                        SelectedLoan.Status = status;
                        break;
                    }
                }

                try
                {
                    await DataContainer.UpdateLoan(SelectedLoan);

                    for (int i = 0; i < _allLoans.Count; i++)
                    {
                        if (_allLoans[i].Id == SelectedLoan.Id)
                        {
                            _allLoans[i] = SelectedLoan;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _windowManager.OpenErrorWindow(ex.Message);
                }

                CanSaveOrDeleteChecked = false;
                RefreshShownLoans(_allLoans);
            }
        }
        #endregion

        #region Private Methodes
        private void ChangeOrderDirection()
        {
            List<Loan> tempLoanRules = new List<Loan>();

            if (SelectedOrderDirection == "Stigende")
            {
                tempLoanRules.AddRange(ShownLoans.OrderBy(x => x.CollectedTime));
            }
            else
            {
                tempLoanRules.AddRange(ShownLoans.OrderByDescending(x => x.CollectedTime));
            }

            RefreshShownLoans(tempLoanRules);
        }

        private void ChangeDatePeriod()
        {
            List<Loan> tempLoanRules = new List<Loan>();

            if (ShownLoans != null)
            {
                for (int i = 0; i < ShownLoans.Count; i++)
                {
                    if (ShownLoans[i].CollectedTime >= SelectedFromCollectedDate && ShownLoans[i].CollectedTime <= SelectedToCollectedDate)
                    {
                        tempLoanRules.Add(ShownLoans[i]);
                    }
                }

                RefreshShownLoans(tempLoanRules);
            }
        }

        private void RefreshShownLoans(List<Loan> loans)
        {
            ShownLoans = new ObservableCollection<Loan>(loans);
        }
        #endregion
    }
}
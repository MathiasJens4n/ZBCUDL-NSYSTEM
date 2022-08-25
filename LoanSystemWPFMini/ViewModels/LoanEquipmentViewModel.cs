using LoanSystemLibraryMini;
using LoanSystemWPFMini.Managers;
using LoanSystemWPFMini.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace LoanSystemWPFMini.ViewModels
{
    public class LoanEquipmentViewModel : BaseScannerViewModel
    {
        private WindowManager _windowManager;
        private PermissionManager _permissionManager;

        private DataContainer _dataContainer;
        public DataContainer DataContainer
        {
            get { return _dataContainer; }
            set { _dataContainer = value; }
        }

        #region Private Variables 
        private User _currentUser;
        private Dispatcher _disUI;
        private string _gruppe = "zbc_alle_elever";
        #endregion

        #region Icommand
        public ICommand BackButton { get; set; }
        public ICommand CreateLoanButton { get; set; }
        public ICommand RemoveSelectedEquipmentButton { get; set; }
        public ICommand LogInButton { get; set; }
        #endregion

        #region General List and Observable Collection Variables
        private List<User> _allUsers;
        private List<Equipment> _allEquipments;
        private List<Loan> _allLoans;
        private Dictionary<string, Equipment> _hashEquipments;
        private Dictionary<string, User> _hashUsers;

        private ObservableCollection<Equipment> _shownequipmentsToLoan;
        public ObservableCollection<Equipment> ShownEquipmentsToLoan
        {
            get { return _shownequipmentsToLoan; }
            set { _shownequipmentsToLoan = value; OnPropertyChanged("ShownEquipmentsToLoan"); }
        }
        #endregion

        #region Binding Variables
        private Equipment _selectedEquipment;
        public Equipment SelectedEquipment
        {
            get { return _selectedEquipment; }
            set { _selectedEquipment = value; OnPropertyChanged(); }
        }

        private string _unilogin;
        public string Unilogin
        {
            get { return _unilogin; }
            set { _unilogin = value; OnPropertyChanged("Unilogin"); }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; OnPropertyChanged("ErrorMessage"); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        private string _mobileNumber;
        public string MobileNumber
        {
            get { return _mobileNumber; }
            set { _mobileNumber = value; OnPropertyChanged("MobileNumber"); }
        }

        private DateTime _returnDate = DateTime.Now;
        public DateTime ReturnDate
        {
            get { return _returnDate; }
            set { _returnDate = value; OnPropertyChanged("ReturnDate"); }
        }
        #endregion

        public LoanEquipmentViewModel() : base()
        {
            _disUI = Dispatcher.CurrentDispatcher;
            _hashEquipments = new Dictionary<string, Equipment>();
            _hashUsers = new Dictionary<string, User>();

            _permissionManager = new PermissionManager();
            _windowManager = new WindowManager();
            ShownEquipmentsToLoan = new ObservableCollection<Equipment>();

            BackButton = new Command(() =>
            {
                UnsubscribeScan();
                _loanSystemViewModel.ShowMenuPage();
            });
            LogInButton = new Command((obj) => ValidateLogin(obj));
            CreateLoanButton = new Command(() => CreateLoan());
            RemoveSelectedEquipmentButton = new Command(() => RemoveSelectedEquipment());
        }

        #region Initialize Data on Load
        public void InitializeData()
        {
            GetData();
        }

        private void GetData()
        {
            _allUsers = new List<User>(DataContainer.AllUsers);
            _allEquipments = new List<Equipment>(DataContainer.AllEquipments);
            _allLoans = new List<Loan>(DataContainer.AllLoans);

            foreach (Equipment equipment in _allEquipments)
            {
                _hashEquipments.Add(equipment.Identification, equipment);
            }

            foreach (User user in _allUsers)
            {
                _hashUsers.Add(user.UniLogin.ToLower(), user);
            }
        }
        #endregion

        #region Private Button Methods
        private async void CreateLoan()
        {
            if (CheckLoanEequiements())
            {
                try
                {
                    string equipmentLoaned = "";

                    for (int i = 0; i < ShownEquipmentsToLoan.Count; i++)
                    {
                        Loan newLoan = new Loan
                        {
                            CollectedTime = DateTime.Now,
                            ReturnDeadline = ReturnDate,
                            User = _currentUser,
                            Equipment = ShownEquipmentsToLoan[i],
                            Status = new Status { Id = 1 } // OK
                        };

                        await DataContainer.CreateLoan(newLoan);

                        equipmentLoaned += $"{ShownEquipmentsToLoan[i].Identification}";
                    }

                    _windowManager.OpenErrorWindow($"Du har lånt: {equipmentLoaned}.");

                    ShownEquipmentsToLoan.Clear();
                    Name = "";
                    MobileNumber = "";
                    ReturnDate = DateTime.Now;
                    _currentUser = null;
                }
                catch (Exception ex)
                {
                    _windowManager.OpenErrorWindow(ex.Message);
                }
            }
        }

        private void RemoveSelectedEquipment()
        {
            if (SelectedEquipment != null)
            {
                ShownEquipmentsToLoan.Remove(SelectedEquipment);
            }
        }
        #endregion

        #region Private Methods
        private void ValidateLogin(object parameter)
        {
            UnsubscribeScan();
            ErrorMessage = "";
            Name = "";
            MobileNumber = "";

            var passwordBox = parameter as PasswordBox;

            if (!_permissionManager.CanLoan(Unilogin, passwordBox.Password))
            {
                ErrorMessage = "Forkert Zbc-login eller kodeord.";
                return;
            }

            if (!_hashUsers.ContainsKey(Unilogin.ToLower()))
            {
                ErrorMessage = $"Denne bruger kan ikke låne i øjebliket.";
                _windowManager.OpenErrorWindow("Brugeren eksistere ikke i udlånssystemet");
                return;
            }

            if (_hashUsers[Unilogin.ToLower()].Status.Id != 1)
            {
                ErrorMessage = $"Denne bruger kan ikke låne i øjebliket.";
                return;
            }

            SubscribeScan();
            _currentUser = _hashUsers[Unilogin.ToLower()];
            Name = _currentUser.Name;
            MobileNumber = _currentUser.MobileNumber;
            ReturnDate = DateTime.Today.AddDays(_currentUser.LoanRule.LoanLength);
            ErrorMessage = "";
            Unilogin = "";
            passwordBox.Clear();
        }

        private bool IsEquipmentBorrowed(string identification)
        {
            foreach (Loan loan in _allLoans)
            {
                if (loan.Equipment.Identification == identification)
                {
                    if (loan.ReturnedTime == null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CheckLoanEequiements()
        {
            if (_currentUser != null && ShownEquipmentsToLoan.Count != 0)
            {
                return true;
            }

            _windowManager.OpenErrorWindow("Du skal både have godkendt login og scannet udstyr.");
            return false;
        }

        private bool CheckForEqualType(string identification)
        {
            foreach (Equipment equipment in _shownequipmentsToLoan)
            {
                if (equipment.Model.Type.Id == _hashEquipments[identification].Model.Type.Id)
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckForExsistingLoansWithSameType(string identification)
        {
            foreach (Loan loan in _allLoans)
            {
                if (loan.User.Name == _currentUser.Name)
                {
                    if (loan.ReturnedTime == null)
                    {
                        if (_hashEquipments[loan.Equipment.Identification].Model.Type.Id == _hashEquipments[identification].Model.Type.Id)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public override void OnQRScanAsync(object source, string e)
        {
            if (IsEquipmentBorrowed(e))
            {
                _disUI.Invoke(() => _windowManager.OpenErrorWindow($"{e} er allerede udlånt."));
                return;
            }
            if (!_hashEquipments.ContainsKey(e))
            {
                _disUI.Invoke(() => _windowManager.OpenErrorWindow($"{e} er ikke registeret i systemet."));
                return;
            }
            if (ShownEquipmentsToLoan.Contains(_hashEquipments[e]))
            {
                _disUI.Invoke(() => _windowManager.OpenErrorWindow($"{e} er allerede i listen."));
                return;
            }
            if (_hashEquipments[e].Status.Id != 1)
            {
                _disUI.Invoke(() => _windowManager.OpenErrorWindow($"Du kan ikke låne dette udstyr.\r\nUdstyret har statusen '{_hashEquipments[e].Status.Name}'.\r\nSig det til U-IT"));
                return;
            }
            else
            {
                if (_permissionManager.IsTeacher(_currentUser.UniLogin))
                {
                    _disUI.Invoke(() => ShownEquipmentsToLoan.Add(_hashEquipments[e]));
                }
                else
                {
                    if (!CheckForEqualType(e))
                    {
                        if (!CheckForExsistingLoansWithSameType(e))
                        {
                            _disUI.Invoke(() => ShownEquipmentsToLoan.Add(_hashEquipments[e]));
                        }
                        else
                        {
                            _disUI.Invoke(() => _windowManager.OpenErrorWindow($"Du har allerede et lån af {_hashEquipments[e].Model.Type.Name }."));
                            return;
                        }
                    }
                    else
                    {
                        _disUI.Invoke(() => _windowManager.OpenErrorWindow("Du må ikke låne 2 stk's af samme type."));
                    }
                }
            }
        }
        #endregion
    }
}
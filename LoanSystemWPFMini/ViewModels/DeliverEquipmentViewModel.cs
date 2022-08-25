using LoanSystemWPFMini.Managers;
using LoanSystemWPFMini.Model;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using LoanSystemLibraryMini;
using System.Windows.Threading;
using System.Configuration;

namespace LoanSystemWPFMini.ViewModels
{
    public class DeliverEquipmentViewModel : BaseScannerViewModel
    {
        private WindowManager _windowManager;

        private DataContainer _dataContainer;
        public DataContainer DataContainer
        {
            get { return _dataContainer; }
            set { _dataContainer = value; }
        }

        #region Private Veriables 
        private User _currentUser;
        private Dispatcher _disUI;

        private System.Windows.Forms.Timer _justDeliveredTimer;
        private System.Windows.Forms.Timer _activeLoansTimer;
        private System.Windows.Forms.Timer _overdueLoansTimer;
        #endregion

        #region ICommand
        public ICommand BackButton { get; set; }
        public ICommand ReturnEquipmentKeyPress { get; set; }
        #endregion

        #region General Lists & dictonary
        private List<Loan> _allLoans;
        private List<Department> _allDepartments;
        private List<Status> _allStatuses;
        
        private Dictionary<string, Equipment> _hashEquipments;
        #endregion

        #region Binding Variables
        private string _justDelivered;
        public string JustDelivered
        {
            get { return _justDelivered; }
            set { _justDelivered = value; OnPropertyChanged("JustDelivered"); }
        }

        private string _justDeliveredVisibility = "Hidden";
        public string JustDeliveredVisibility
        {
            get { return _justDeliveredVisibility; }
            set { _justDeliveredVisibility = value; OnPropertyChanged("JustDeliveredVisibility"); }
        }

        private string _activeLoans;
        public string ActiveLoans
        {
            get { return _activeLoans; }
            set { _activeLoans = value; OnPropertyChanged("ActiveLoans"); }
        }

        private string _activeLoansVisibility = "Hidden";
        public string ActiveLoansVisibility
        {
            get { return _activeLoansVisibility; }
            set { _activeLoansVisibility = value; OnPropertyChanged("ActiveLoansVisibility"); }
        }

        private string _overdueLoans;
        public string OverdueLoans
        {
            get { return _overdueLoans; }
            set { _overdueLoans = value; OnPropertyChanged("OverdueLoans"); }
        }

        private string _overdueLoansVisibility = "Hidden";
        public string OverdueLoansVisibility
        {
            get { return _overdueLoansVisibility; }
            set { _overdueLoansVisibility = value; OnPropertyChanged("OverdueLoansVisibility"); }
        }
        #endregion

        public DeliverEquipmentViewModel() : base()
        {
            SubscribeScan();

            _disUI = Dispatcher.CurrentDispatcher;
            _windowManager = new WindowManager();
            _hashEquipments = new Dictionary<string, Equipment>();

            InitializeTimers();

            BackButton = new Command(() =>
            {
                UnsubscribeScan();
                _loanSystemViewModel.ShowMenuPage();
            });
            ReturnEquipmentKeyPress = new Command(() => _windowManager.OpenErrorWindow("hey"));
        }

        #region Initialize Data on Load
        public void InitializeData()
        {
            GetData();
        }

        private void GetData()
        {
            _allLoans = new List<Loan>(DataContainer.AllLoans);
            _allDepartments = new List<Department>(DataContainer.AllDepartments);
            _allStatuses = new List<Status>(DataContainer.AllStatuses);

            List<Equipment> equipments = new List<Equipment>(DataContainer.AllEquipments);

            foreach(Equipment equipment in equipments)
            {
                _hashEquipments.Add(equipment.Identification, equipment);
            }   
        }
        #endregion

        #region Private Methods
        private void InitializeTimers()
        {
            _justDeliveredTimer = new System.Windows.Forms.Timer();
            _justDeliveredTimer.Tick += new EventHandler(HaveJustDelivered);
            _justDeliveredTimer.Interval = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;

            _activeLoansTimer = new System.Windows.Forms.Timer();
            _activeLoansTimer.Tick += new EventHandler(HaveActiveLoans);
            _activeLoansTimer.Interval = 10000; // 10 sec

            _overdueLoansTimer = new System.Windows.Forms.Timer();
            _overdueLoansTimer.Tick += new EventHandler(HaveOverdueLoans);
            _overdueLoansTimer.Interval = 10000; // 10 sex 😏
        }

        private void HaveJustDelivered(object myObject, EventArgs args)
        {
            JustDelivered = "";
            JustDeliveredVisibility = "Hidden";
            _justDeliveredTimer.Enabled = false;
        }

        private void HaveActiveLoans(object myObject, EventArgs args)
        {
            ActiveLoans = "";
            ActiveLoansVisibility = "Hidden";
            _activeLoansTimer.Enabled = false;
        }

        private void HaveOverdueLoans(object myObject, EventArgs args)
        {
            OverdueLoans = "";
            OverdueLoansVisibility = "Hidden";
            _overdueLoansTimer.Enabled = false;
        }

        private void CheckUsersOtherLoans()
        {
            foreach (Loan loan in _allLoans)
            {
                if (loan.User.Id == _currentUser.Id)
                {
                    if (loan.ReturnedTime == null && loan.ReturnDeadline > DateTime.Now)
                    {
                        ActiveLoans += $"{loan.Equipment.Identification} er stadig et aktivt lån.\n";
                    }
                    else if (loan.ReturnedTime == null && loan.ReturnDeadline < DateTime.Now)
                    {
                        OverdueLoans += $"{loan.Equipment.Identification} er et overskredet lån.\n";
                    }
                }
            }

            if (!string.IsNullOrEmpty(_activeLoans))
            {
                ActiveLoansVisibility = "Visible";
                _activeLoansTimer.Enabled = true;
            }

            if (!string.IsNullOrEmpty(_overdueLoans))
            {
                OverdueLoansVisibility = "Visible";
                _overdueLoansTimer.Enabled = true;
            }
        }

        private void ClaerUI()
        {
            JustDelivered = "";
            JustDeliveredVisibility = "Hidden";
            _justDeliveredTimer.Enabled = false;

            ActiveLoans = "";
            ActiveLoansVisibility = "Hidden";
            _activeLoansTimer.Enabled = false;

            OverdueLoans = "";
            OverdueLoansVisibility = "Hidden";
            _overdueLoansTimer.Enabled = false;
        }

        private string CheckLocationConfig()
        {
            try
            {
                ConfigManager cm = new ConfigManager();
                return cm.GetAppSetting("CurrentLocation");
            }
            catch (ConfigurationErrorsException)
            {
                _windowManager.OpenErrorWindow("Lokationen kunne ikke findes.\r\nGenstart programmet.");
            }

            return null;
        }

        public override async void OnQRScanAsync(object source, string e)
        {
            ClaerUI();

            if (string.IsNullOrEmpty(CheckLocationConfig()))
            {
                return;
            }

            string location = CheckLocationConfig();

            foreach (Loan loan in _allLoans)
            {
                if (loan.Equipment.Identification == e)
                {
                    if (loan.ReturnedTime == null)
                    {
                        loan.ReturnedTime = DateTime.Now;
                        _currentUser = loan.User;

                        if (_hashEquipments[loan.Equipment.Identification].BelongingDepartment.Name != location)
                        {
                            Equipment equipment = _hashEquipments[loan.Equipment.Identification];

                            foreach (Department department in _allDepartments)
                            {
                                if(department.Name == location)
                                {
                                    equipment.CurrentDepartment = department;

                                    foreach (Status status in _allStatuses)
                                    {
                                        if (status.Id == 5) // Unavalilable
                                        {
                                            equipment.Status = status;
                                            break;
                                        }
                                    }

                                    break;
                                }
                            }

                            try
                            {
                                await DataContainer.UpdateEquipment(equipment);
                            }
                            catch (Exception ex)
                            {
                                _disUI.Invoke(() => _windowManager.OpenErrorWindow(ex.Message));
                            }
                        }

                        try
                        {
                            await DataContainer.UpdateLoan(loan);

                            _disUI.Invoke(() =>
                            {
                                JustDelivered = $"{loan.User.Name}: Afleveret - {loan.Equipment.Identification}.";
                                JustDeliveredVisibility = "Visible";
                                _justDeliveredTimer.Enabled = true;

                                CheckUsersOtherLoans();
                            });
                        }
                        catch (Exception ex)
                        {
                            _disUI.Invoke(() => _windowManager.OpenErrorWindow(ex.Message));
                        }

                        _currentUser = null;
                        return;
                    }
                }
            }

            _disUI.Invoke(() => _windowManager.OpenErrorWindow("Dette udstyr er allerede afleveret."));
        }
        #endregion
    }
}
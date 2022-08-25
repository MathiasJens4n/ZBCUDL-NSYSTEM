using LoanSystemWPFMini.Model;
using LoanSystemWPFMini.Views;
using LoanSystemWPFMini.Managers;
using System.Windows.Input;
using System.Windows.Forms;
using System.Threading;
using System.Windows.Threading;
using System.Windows;
using System.Collections.Generic;

namespace LoanSystemWPFMini.ViewModels
{
    public class LoanSystemViewModel : BaseViewModel
    {
        private WindowManager _windowManager;
        private DataContainer _dataContainer;

        public ICommand OverviewPageButton { get; set; }
        public ICommand LoanRulePageButton { get; set; }
        public ICommand UserPageButton { get; set; }
        public ICommand TypeModelButton { get; set; }
        public ICommand EquipmentButton { get; set; }
        public ICommand LoanButton { get; set; }

        private object _userControlView;
        public object UserControlView
        {
            get { return _userControlView; }
            set { _userControlView = value; OnPropertyChanged("UserControlView"); }
        }

        private object _userControlViewModel;
        public object UserControlViewModel
        {
            get { return _userControlViewModel; }
            set { _userControlViewModel = value; OnPropertyChanged("UserControlViewModel"); }
        }

        private double _width;
        public double Width
        {
            get { return _width; }
            set { _width = value; OnPropertyChanged("Width"); SizeChanged(); }
        }

        private double _height;
        public double Height
        {
            get { return _height; }
            set { _height = value; OnPropertyChanged("Height"); SizeChanged(); }
        }

        private string _windowState;
        public string WindowState
        {
            get { return _windowState; }
            set
            {
                _windowState = value; OnPropertyChanged("WindowState");
                if (_windowState == "Maximized")
                {
                    Height = SystemInformation.PrimaryMonitorSize.Height;
                    Width = SystemInformation.PrimaryMonitorSize.Width;
                    SizeChanged();
                }
            }
        }

        private Visibility _buttonVisibility = Visibility.Visible; 
        public Visibility ButtonVisibility
        {
            get { return _buttonVisibility; }
            set { _buttonVisibility = value; OnPropertyChanged("ButtonVisibility"); }
        }

        private string _overviewBackground = "#DDDDFF";
        public string OverviewBackground
        {
            get { return _overviewBackground; }
            set { _overviewBackground = value; OnPropertyChanged(); }
        }

        private string _loanBackground = "#DDDDDD";
        public string LoanBackground
        {
            get { return _loanBackground; }
            set { _loanBackground = value; OnPropertyChanged(); }
        }

        private string _equipmentBackground = "#DDDDDD";
        public string EquipmentBackground
        {
            get { return _equipmentBackground; }
            set { _equipmentBackground = value; OnPropertyChanged(); }
        }

        private string _userBackground = "#DDDDDD"; 
        public string UserBackground 
        {
            get { return _userBackground; }
            set { _userBackground = value; OnPropertyChanged(); }
        }

        private string _loanRuleBackground = "#DDDDDD";
        public string LoanRuleBackground
        {
            get { return _loanRuleBackground; }
            set { _loanRuleBackground= value; OnPropertyChanged(); }
        }

        private string _typeModelBackground = "#DDDDDD";
        public string TypeModelBackground
        {
            get { return _typeModelBackground; }
            set { _typeModelBackground = value; OnPropertyChanged(); }
        }

        public LoanSystemViewModel()
        {
            _windowManager = new WindowManager();
            _dataContainer = new DataContainer();

            TryInitializeData();

            OverviewPageButton = new Command((pra) => { ShowOverviewStatusPage(); ChangeColor(pra); });
            LoanRulePageButton = new Command((pra) => { ShowLoanRulesPage(); ChangeColor(pra); });
            UserPageButton = new Command((pra) => { ShowUserPage(); ChangeColor(pra); });
            TypeModelButton = new Command((pra) => { ShowTypeModelPage(); ChangeColor(pra); });
            EquipmentButton = new Command((pra) => { ShowEquipmentPage(); ChangeColor(pra); });
            LoanButton = new Command((pra) => { ShowLoanPage(); ChangeColor(pra); });

            ShowSelectDepartmentPage();
        }

        public void TryInitializeData()
        {
            Dispatcher disUI = Dispatcher.CurrentDispatcher;
            Thread getThread = new Thread(async () =>
            {
                try
                {
                    await _dataContainer.InitializeData();
                }
                catch (System.Exception)
                {
                    // TODO: Proper exception handling
                    disUI.Invoke(() =>
                    _windowManager.OpenErrorWindow($"Der kunne ikke hentes data fra API'en.\r\nTjek internet forbinelsen.\r\nTilkald din IT Administrator.")
                        );
                }
            }
            );
            getThread.Start();
        }

        public void SizeChanged()
        {
            Scalability = (Width + Height) / 1750;
            if (UserControlViewModel != null)
            {
                if (UserControlViewModel is BaseViewModel baseViewModel)
                {
                    baseViewModel.Scalability = Scalability;
                }
            }
        }

        private void ChangeColor(object parameter)
        {
            OverviewBackground = "#DDDDDD";
            LoanBackground = "#DDDDDD";
            EquipmentBackground = "#DDDDDD";
            UserBackground = "#DDDDDD";
            LoanRuleBackground = "#DDDDDD";
            TypeModelBackground = "#DDDDDD";

            switch (parameter.ToString())
            {
                case nameof(OverviewBackground):
                    OverviewBackground = "#DDDDFF";
                    break;
                case nameof(LoanBackground):
                    LoanBackground = "#DDDDFF";
                    break;
                case nameof(EquipmentBackground):
                    EquipmentBackground = "#DDDDFF";
                    break;
                case nameof(UserBackground):
                    UserBackground = "#DDDDFF";
                    break;
                case nameof(LoanRuleBackground):
                    LoanRuleBackground = "#DDDDFF";
                    break;
                case nameof(TypeModelBackground):
                    TypeModelBackground = "#DDDDFF";
                    break;
            }
        }

        private void UnsubscribeScanIfEquipmentViewModel()
        {
            if(UserControlViewModel.GetType().Name == "EquipmentViewModel")
            {
                ((EquipmentViewModel)UserControlViewModel).UnsubscribeScan();
            }
        }

        public void ShowMenuPage()
        {
            ButtonVisibility = Visibility.Hidden;

            UserControlView = new MenuView();
            UserControlViewModel = ((MenuView)UserControlView).DataContext;
            ((MenuViewModel)UserControlViewModel).Scalability = Scalability;
            ((MenuViewModel)UserControlViewModel).AttachLoanSystemViewModel(this);
        }

        public void ShowDeliverEquipment()
        {
            UserControlView = new DeliverEquipmentView();
            UserControlViewModel = ((DeliverEquipmentView)UserControlView).DataContext;
            ((DeliverEquipmentViewModel)UserControlViewModel).DataContainer = _dataContainer;
            ((DeliverEquipmentViewModel)UserControlViewModel).Scalability = Scalability;
            ((DeliverEquipmentViewModel)UserControlViewModel).InitializeData();
            ((DeliverEquipmentViewModel)UserControlViewModel).AttachLoanSystemViewModel(this);
        }

        public void ShowLoanEquipmentPage()
        {
            UserControlView = new LoanEquipmentView();
            UserControlViewModel = ((LoanEquipmentView)UserControlView).DataContext;
            ((LoanEquipmentViewModel)UserControlViewModel).DataContainer = _dataContainer;
            ((LoanEquipmentViewModel)UserControlViewModel).Scalability = Scalability;
            ((LoanEquipmentViewModel)UserControlViewModel).InitializeData();
            ((LoanEquipmentViewModel)UserControlViewModel).AttachLoanSystemViewModel(this);
        }

        public void ShowlogInPage()
        {
            UserControlView = new LogInView();
            UserControlViewModel = ((LogInView)UserControlView).DataContext;
            ((LogInViewModel)UserControlViewModel).Scalability = Scalability;
            ((LogInViewModel)UserControlViewModel).AttachLoanSystemViewModel(this);
        }

        public void ShowOverviewStatusPage()
        {
            UnsubscribeScanIfEquipmentViewModel();
            ButtonVisibility = Visibility.Visible;
            OverviewBackground = "#DDDDFF";
            LoanBackground = "#DDDDDD";
            EquipmentBackground = "#DDDDDD";
            UserBackground = "#DDDDDD";
            LoanRuleBackground = "#DDDDDD";
            TypeModelBackground = "#DDDDDD";

            UserControlView = new OverviewStatusView();
            UserControlViewModel = ((OverviewStatusView)UserControlView).DataContext;
            ((OverviewStatusViewModel)UserControlViewModel).DataContainer = _dataContainer;
            ((OverviewStatusViewModel)UserControlViewModel).Scalability = Scalability;
            ((OverviewStatusViewModel)UserControlViewModel).InitializeData();
            ((OverviewStatusViewModel)UserControlViewModel).AttachLoanSystemViewModel(this);
        }

        private void ShowLoanRulesPage()
        {
            UnsubscribeScanIfEquipmentViewModel();
            UserControlView = new LoanRuleView();
            UserControlViewModel = ((LoanRuleView)UserControlView).DataContext;
            ((LoanRuleViewModel)UserControlViewModel).DataContainer = _dataContainer;
            ((LoanRuleViewModel)UserControlViewModel).Scalability = Scalability;
            ((LoanRuleViewModel)UserControlViewModel).InitializeData();
            ((LoanRuleViewModel)UserControlViewModel).AttachLoanSystemViewModel(this);
        }

        private void ShowUserPage()
        {
            UnsubscribeScanIfEquipmentViewModel();
            UserControlView = new UserView();
            UserControlViewModel = ((UserView)UserControlView).DataContext;
            ((UserViewModel)UserControlViewModel).DataContainer = _dataContainer;
            ((UserViewModel)UserControlViewModel).Scalability = Scalability;
            ((UserViewModel)UserControlViewModel).InitializeData();
            ((UserViewModel)UserControlViewModel).AttachLoanSystemViewModel(this);
        }

        private void ShowTypeModelPage()
        {
            UnsubscribeScanIfEquipmentViewModel();
            UserControlView = new TypeModelView();
            UserControlViewModel = ((TypeModelView)UserControlView).DataContext;
            ((TypeModelViewModel)UserControlViewModel).DataContainer = _dataContainer;
            ((TypeModelViewModel)UserControlViewModel).Scalability = Scalability;
            ((TypeModelViewModel)UserControlViewModel).InitializeData();
        }

        private void ShowEquipmentPage()
        {
            UserControlView = new EquipmentView();
            UserControlViewModel = ((EquipmentView)UserControlView).DataContext;
            ((EquipmentViewModel)UserControlViewModel).DataContainer = _dataContainer;
            ((EquipmentViewModel)UserControlViewModel).Scalability = Scalability;
            ((EquipmentViewModel)UserControlViewModel).InitializeData();
            ((EquipmentViewModel)UserControlViewModel).AttachLoanSystemViewModel(this);
        }

        private void ShowLoanPage()
        {
            UnsubscribeScanIfEquipmentViewModel();
            UserControlView = new LoanView();
            UserControlViewModel = ((LoanView)UserControlView).DataContext;
            ((LoanViewModel)UserControlViewModel).DataContainer = _dataContainer;
            ((LoanViewModel)UserControlViewModel).Scalability = Scalability;
            ((LoanViewModel)UserControlViewModel).InitializeData();
            ((LoanViewModel)UserControlViewModel).AttachLoanSystemViewModel(this);
        }

        private void ShowSelectDepartmentPage()
        {
            ButtonVisibility = Visibility.Hidden;

            UserControlView = new SelectDepartmentView();
            UserControlViewModel = ((SelectDepartmentView)UserControlView).DataContext;
            ((SelectDepartmentViewModel)UserControlViewModel).Scalability = Scalability;
            ((SelectDepartmentViewModel)UserControlViewModel).AttachLoanSystemViewModel(this);
        }
    }
}
using LoanSystemLibraryMini;
using LoanSystemWPFMini.Managers;
using LoanSystemWPFMini.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

namespace LoanSystemWPFMini.ViewModels
{
    public class OverviewStatusViewModel : BaseViewModel
    {
        private DataContainer _dataContainer;
        public DataContainer DataContainer
        {
            get { return _dataContainer; }
            set { _dataContainer = value; }
        }

        public ICommand LogOutButton { get; set; }
        public ICommand RefreshDataButton { get; set; }
        public ICommand PrintButton { get; set; }

        #region General Observable Collection Variables
        private List<Loan> _allLoans;
        private ObservableCollection<Loan> _shownActiveLoans;
        public ObservableCollection<Loan> ShownActiveLoans
        {
            get { return _shownActiveLoans; }
            set { _shownActiveLoans = value; OnPropertyChanged("ShownActiveLoans"); }
        }

        private ObservableCollection<Loan> _shownShouldReturnTodayLoans;
        public ObservableCollection<Loan> ShownShouldReturnTodayLoans
        {
            get { return _shownShouldReturnTodayLoans; }
            set { _shownShouldReturnTodayLoans = value; OnPropertyChanged("ShownShouldReturnTodayLoans"); }
        }

        private ObservableCollection<Loan> _shownOverdueLoans;
        public ObservableCollection<Loan> ShownOverdueLoans
        {
            get { return _shownOverdueLoans; }
            set { _shownOverdueLoans = value; OnPropertyChanged("ShownOverdueLoans"); }
        }
        #endregion

        public OverviewStatusViewModel()
        {
            ShownActiveLoans = new ObservableCollection<Loan>();
            ShownShouldReturnTodayLoans = new ObservableCollection<Loan>();
            ShownOverdueLoans = new ObservableCollection<Loan>();

            LogOutButton = new Command(() => _loanSystemViewModel.ShowMenuPage());
            RefreshDataButton = new Command(() => { _loanSystemViewModel.TryInitializeData(); InitializeData(); });
            PrintButton = new Command(() => PrintData());
        }

        #region Initialize Data on Load
        public void InitializeData()
        {
            GetData();
            GetRelevantLoan();
        }

        private void GetData()
        {
            _allLoans = new List<Loan>(DataContainer.AllLoans);
        }
        #endregion

        #region Private Methods
        private void GetRelevantLoan()
        {
            foreach (var loan in _allLoans)
            {
                if (loan.ReturnedTime == null)
                {
                    if (loan.ReturnDeadline > DateTime.Now)
                    {
                        ShownActiveLoans.Add(loan);
                    }
                    else if (loan.ReturnDeadline.Date.Day == DateTime.Now.Day)
                    {
                        ShownShouldReturnTodayLoans.Add(loan);
                    }
                    else if (loan.ReturnDeadline < DateTime.Now)
                    {
                        ShownOverdueLoans.Add(loan);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
        #endregion

        private void PrintData()
        {
            FolderSelectDialog folderSelectDialog = new FolderSelectDialog();
            string path = folderSelectDialog.ChooseSavePath();

            if (!string.IsNullOrEmpty(path))
            {
                string csvData = "\"Lån\";CollectedTime;ReturnedTime;ReturnDeadline;Zbc-login;Identification;Status;Note\r\n";

                csvData += "\"Aktive lån.\"\r\n";

                foreach (Loan l in ShownActiveLoans)
                {
                    csvData += $";{l.CollectedTime.ToString("dd-MM-yyyy HH:mm")};{l.ReturnedTime?.ToString("dd-MM-yyyy HH:mm")};{l.ReturnDeadline.ToString("dd-MM-yyyy HH:mm")};" +
                               $"{l.User.UniLogin};{l.Equipment.Identification};{l.Status.Name};\"{l.Note}\"\r\n";
                }

                csvData += "\"Lån som burde returneres i dag.\"\r\n";

                foreach (Loan l in ShownShouldReturnTodayLoans)
                {
                    csvData += $";{l.CollectedTime.ToString("dd-MM-yyyy HH:mm")};{l.ReturnedTime?.ToString("dd-MM-yyyy HH:mm")};{l.ReturnDeadline.ToString("dd-MM-yyyy HH:mm")};" +
                               $"{l.User.UniLogin};{l.Equipment.Identification};{l.Status.Name};\"{l.Note}\"\r\n";
                }

                csvData += "\"Overskredet lån.\"\r\n";

                foreach (Loan l in ShownOverdueLoans)
                {
                    csvData += $";{l.CollectedTime.ToString("dd-MM-yyyy HH:mm")};{l.ReturnedTime?.ToString("dd-MM-yyyy HH:mm")};{l.ReturnDeadline.ToString("dd-MM-yyyy HH:mm")};" +
                               $"{l.User.UniLogin};{l.Equipment.Identification};{l.Status.Name};\"{l.Note}\"\r\n";
                }

               File.WriteAllText(path, csvData);
            }
        }
    }
}
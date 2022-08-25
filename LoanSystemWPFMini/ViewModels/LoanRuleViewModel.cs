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
    public class LoanRuleViewModel : BaseViewModel
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
        public ICommand ClearDateButton { get; set; }
        public ICommand CreateOrUpdateLoanRuleButton { get; set; }
        public ICommand DeleteLoanRuleButton { get; set; }
        #endregion

        #region General Observable Collection Variables
        private List<LoanRule> _allLoanRules;
        private ObservableCollection<LoanRule> _shownLoanRules;
        public ObservableCollection<LoanRule> ShownLoanRules
        {
            get { return _shownLoanRules; }
            set { _shownLoanRules = value; OnPropertyChanged("ShownLoanRules"); }
        }

        private ObservableCollection<Status> _status;
        public ObservableCollection<Status> Statuses
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged("Statuses"); }
        }

        private ObservableCollection<LoanRule> _replacementRules;
        public ObservableCollection<LoanRule> ReplacementRules
        {
            get { return _replacementRules; }
            set { _replacementRules = value; OnPropertyChanged("ReplacementRules"); }
        }
        #endregion

        #region Selected Variables
        private LoanRule _selectedLoanRule;
        public LoanRule SelectedLoanRule
        {
            get { return _selectedLoanRule; }
            set
            {
                SwitchReplacementRules(SelectedLoanRule, value);

                _selectedLoanRule = value;

                if (value != null)
                {
                    SelectedName = SelectedLoanRule.Name;
                    SelectedLoanLength = SelectedLoanRule.LoanLength.ToString();
                    SelectedExpirationDate = SelectedLoanRule.ExpirationDate;
                    if (SelectedLoanRule.ReplacementRule != null)
                    {
                        if (SelectedLoanRule.ReplacementRule.Id == 0)
                        {
                            SelectedReplacementRule = ReplacementRules.Last();
                        }
                        else
                        {
                            foreach (LoanRule replacementRule in _allLoanRules)
                            {
                                if (SelectedLoanRule.ReplacementRule.Id == replacementRule.Id)
                                {
                                    SelectedReplacementRule = replacementRule;
                                    break;
                                }
                            }
                        }
                    }
                    foreach (Status status in Statuses)
                    {
                        if (SelectedLoanRule.Status.Id == status.Id)
                        {
                            SelectedStatus = status;
                            break;
                        }
                    }
                    SelectedLoanRuleNote = SelectedLoanRule.Note;

                    CreateOrUpdateLoanRuleButtonContent = "Gem";
                }
                else
                {
                    SelectedName = "";
                    SelectedLoanLength = "";
                    SelectedExpirationDate = DateTime.Today;
                    SelectedReplacementRule = null;
                    SelectedStatus = null;
                    SelectedLoanRuleNote = "";

                    CreateOrUpdateLoanRuleButtonContent = "Opret";
                }

                OnPropertyChanged("SelectedLoanRule");
            }
        }

        private string _selectedName;
        public string SelectedName
        {
            get { return _selectedName; }
            set { _selectedName = value; OnPropertyChanged("SelectedName"); }
        }

        private string _selectedLoanLength;
        public string SelectedLoanLength
        {
            get { return _selectedLoanLength; }
            set { _selectedLoanLength = value; OnPropertyChanged("SelectedLoanLength"); }
        }

        private DateTime? _selectedExpirationDate = DateTime.Today;
        public DateTime? SelectedExpirationDate
        {
            get { return _selectedExpirationDate; }
            set { _selectedExpirationDate = value; OnPropertyChanged("SelectedExpirationDate"); }
        }

        private LoanRule _selectedReplacementRule;
        public LoanRule SelectedReplacementRule
        {
            get { return _selectedReplacementRule; }
            set { _selectedReplacementRule = value; OnPropertyChanged("SelectedReplacementRule"); }
        }

        private Status _selectedStatus;
        public Status SelectedStatus
        {
            get { return _selectedStatus; }
            set { _selectedStatus = value; OnPropertyChanged("SelectedStatus"); }
        }

        private string _selectedLoanRuleNote;
        public string SelectedLoanRuleNote
        {
            get { return _selectedLoanRuleNote; }
            set { _selectedLoanRuleNote = value; OnPropertyChanged("SelectedLoanRuleNote"); }
        }
        #endregion

        #region Edit Selected Variable
        private bool _canSaveOrDeleteChecked = false;
        public bool CanSaveOrDeleteChecked
        {
            get { return _canSaveOrDeleteChecked; }
            set { _canSaveOrDeleteChecked = value; OnPropertyChanged("CanSaveOrDeleteChecked"); }
        }

        private string _createOrUpdateLoanRuleButtonContent = "Opret";
        public string CreateOrUpdateLoanRuleButtonContent
        {
            get { return _createOrUpdateLoanRuleButtonContent; }
            set { _createOrUpdateLoanRuleButtonContent = value; OnPropertyChanged("CreateOrUpdateLoanRuleButtonContent"); }
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
        private bool _nameChecked = true;
        public bool NameChecked
        {
            get { return _nameChecked; }
            set { _nameChecked = value; OnPropertyChanged("NameChecked"); }
        }

        private bool _loanLengthChecked = true;
        public bool LoanLengthChecked
        {
            get { return _loanLengthChecked; }
            set { _loanLengthChecked = value; OnPropertyChanged("LoanLengthChecked"); }
        }

        private bool _expirationDateChecked = true;
        public bool ExpirationDateChecked
        {
            get { return _expirationDateChecked; }
            set { _expirationDateChecked = value; OnPropertyChanged("ExpirationDateChecked"); }
        }

        private bool _replacementRuleChecked = true;
        public bool ReplacementRuleChecked
        {
            get { return _replacementRuleChecked; }
            set { _replacementRuleChecked = value; OnPropertyChanged("ReplacementRuleChecked"); }
        }

        private bool _statusChecked = true;
        public bool StatusChecked
        {
            get { return _statusChecked; }
            set { _statusChecked = value; OnPropertyChanged("StatusChecked"); }
        }

        private bool _loanRuleNoteChecked = true;
        public bool LoanRuleNoteChecked
        {
            get { return _loanRuleNoteChecked; }
            set { _loanRuleNoteChecked = value; OnPropertyChanged("LoanRuleNoteChecked"); }
        }
        #endregion

        #region Lower Search Area Variables
        private DateTime _selectedFromDate = new DateTime(2020, 1, 1);
        public DateTime SelectedFromDate
        {
            get { return _selectedFromDate; }
            set { _selectedFromDate = value; OnPropertyChanged("SelectedFromDate"); ChangeDatePeriod(); }
        }

        private DateTime _selectedToDate = DateTime.Now.AddYears(1);
        public DateTime SelectedToDate
        {
            get { return _selectedToDate; }
            set { _selectedToDate = value; OnPropertyChanged("SelectedToDate"); ChangeDatePeriod(); }
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

        public LoanRuleViewModel()
        {
            _windowManager = new WindowManager();

            SearchOptions = new ObservableCollection<string> { "Navn", "Lånelængde", "Udløbsdato", "Erstatningsregel", "Status", "" };
            OrderDirections = new ObservableCollection<string> { "Stigende", "Faldende" };

            SearchButton = new Command(x => Search());
            ClearSearchButton = new Command(x => ClearSearch());
            PrintButton = new Command(() => PrintData());
            ClearInfoButton = new Command(x => SelectedLoanRule = null);
            LogOutButton = new Command(x => _loanSystemViewModel.ShowMenuPage());
            ClearDateButton = new Command(x => ClearDate());
            CreateOrUpdateLoanRuleButton = new Command(x => CreateOrUpdateLoanRule());
            DeleteLoanRuleButton = new Command(x => DeleteLoanRule());
        }

        #region Initialize Date on Load
        public void InitializeData()
        {
            GetData();
            SetupReplacementRules();
        }

        private void GetData()
        {
            _allLoanRules = new List<LoanRule>(DataContainer.AllLoanRules);
            ShownLoanRules = new ObservableCollection<LoanRule>(_allLoanRules);
            Statuses = new ObservableCollection<Status>(DataContainer.AllStatuses);
        }

        private void SetupReplacementRules()
        {
            ReplacementRules = new ObservableCollection<LoanRule>(_allLoanRules);
            ReplacementRules.Add(new LoanRule { Id = 0, Name = "" });
            SelectedReplacementRule = ReplacementRules.Last();
        }
        #endregion

        #region Private Button Methodes
        private void Search()
        {
            if (SelectedSearchOption != null)
            {
                if (SearchInput != null)
                {
                    ShownLoanRules.Clear();
                    switch (SelectedSearchOption.ToLower())
                    {
                        case "navn":
                            foreach (LoanRule loanRule in _allLoanRules)
                            {
                                if (loanRule.Name.ToLower().StartsWith(SearchInput.ToLower()))
                                {
                                    ShownLoanRules.Add(loanRule);
                                }
                            }
                            break;

                        case "låne længde":
                            foreach (LoanRule loanRule in _allLoanRules)
                            {
                                if (loanRule.LoanLength.ToString().StartsWith(SearchInput.ToLower()))
                                {
                                    ShownLoanRules.Add(loanRule);
                                }
                            }
                            break;

                        case "udløbs dato":
                            foreach (LoanRule loanRule in _allLoanRules)
                            {
                                if (loanRule.ExpirationDate != null && loanRule.ExpirationDate.ToString().StartsWith(SearchInput.ToLower()))
                                {
                                    ShownLoanRules.Add(loanRule);
                                }
                            }
                            break;

                        case "erstatningsregel":
                            foreach (LoanRule loanRule in _allLoanRules)
                            {
                                if (loanRule.ReplacementRule != null)
                                {
                                    if (loanRule.ReplacementRule != null && loanRule.ReplacementRule.Name.ToLower().StartsWith(SearchInput.ToLower()))
                                    {
                                        ShownLoanRules.Add(loanRule);
                                    }
                                }
                            }
                            break;

                        case "status":
                            foreach (LoanRule loanRule in _allLoanRules)
                            {
                                if (loanRule.Status.Name.ToLower().StartsWith(SearchInput.ToLower()))
                                {
                                    ShownLoanRules.Add(loanRule);
                                }
                            }
                            break;

                            //default:
                            //    RefreshShowLoanRules(_allLoanRules);
                            //    break;
                    }
                }
            }

            //ChangeOrderDirection();
            //ChangeDatePeriod();
        }

        private void ClearSearch()
        {
            SelectedSearchOption = null;
            SearchInput = "";

            SelectedFromDate = new DateTime(2020, 1, 1);
            SelectedToDate = DateTime.Now.AddYears(1);
            SelectedOrderDirection = OrderDirections[0];

            NameChecked = true;
            LoanLengthChecked = true;
            ExpirationDateChecked = true;
            ReplacementRuleChecked = true;
            StatusChecked = true;
            LoanRuleNoteChecked = true;

            RefreshShownLoanRules(_allLoanRules);
        }

        private void PrintData()
        {
            FolderSelectDialog folderSelectDialog = new FolderSelectDialog();
            string path = folderSelectDialog.ChooseSavePath();

            if (!string.IsNullOrEmpty(path))
            {
                string csvData = "Name;LoanLength;ExpirationDate;ReplacementRule;Status;Note\r\n";

                foreach (LoanRule lr in ShownLoanRules)
                {
                    csvData += $"\"{lr.Name}\";{lr.LoanLength};{lr.ExpirationDate?.ToString("dd-MM-yyyy HH:mm")};\"{lr.ReplacementRule?.Name}\";" +
                               $"{lr.Status.Name};\"{lr.Note}\";\r\n";
                }

                File.WriteAllText(path, csvData);
            }
        }

        private void ClearDate()
        {
            SelectedExpirationDate = null;
        }

        private async void CreateOrUpdateLoanRule()
        {
            if (ValidateInput())
            {
                if (SelectedLoanRule == null)
                {
                    LoanRule newLoanRule = new LoanRule
                    {
                        Name = SelectedName,
                        LoanLength = Convert.ToInt32(SelectedLoanLength),
                        Status = SelectedStatus,
                        Note = SelectedLoanRuleNote
                    };

                    newLoanRule = CheckReplacementRule(newLoanRule);

                    try
                    {
                        LoanRule createdLoanRule = await DataContainer.CreateLoanrule(newLoanRule);
                        _allLoanRules.Add(createdLoanRule);
                        ReplacementRules.Insert(ReplacementRules.Count - 2, createdLoanRule);
                    }
                    catch (Exception ex)
                    {
                        _windowManager.OpenErrorWindow(ex.Message);
                    }
                }
                else
                {
                    SelectedLoanRule.Name = SelectedName;
                    SelectedLoanRule.LoanLength = Convert.ToInt32(SelectedLoanLength);
                    SelectedLoanRule.ExpirationDate = SelectedExpirationDate;
                    SelectedLoanRule.ReplacementRule = SelectedReplacementRule;
                    SelectedLoanRule.Status = SelectedStatus;
                    SelectedLoanRule.Note = SelectedLoanRuleNote;

                    SelectedLoanRule = CheckReplacementRule(SelectedLoanRule);

                    try
                    {
                        await DataContainer.UpdateLoanRule(SelectedLoanRule);

                        for (int i = 0; i < _allLoanRules.Count; i++)
                        {
                            if (_allLoanRules[i].Id == SelectedLoanRule.Id)
                            {
                                _allLoanRules[i] = SelectedLoanRule;
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
                SelectedLoanRule = null;
                RefreshShownLoanRules(_allLoanRules);
            }
        }

        public async void DeleteLoanRule()
        {
            if (SelectedLoanRule != null)
            {
                SelectedLoanRule = CheckReplacementRule(SelectedLoanRule);

                foreach (Status status in Statuses)
                {
                    if (status.Name.ToLower() == "deleted")
                    {
                        SelectedLoanRule.Status = status;
                        break;
                    }
                }

                try
                {
                    await DataContainer.UpdateLoanRule(SelectedLoanRule);

                    SelectedLoanRule.ExpirationDate = null;

                    for (int i = 0; i < _allLoanRules.Count; i++)
                    {
                        if (_allLoanRules[i].Id == SelectedLoanRule.Id)
                        {
                            _allLoanRules[i] = SelectedLoanRule;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _windowManager.OpenErrorWindow(ex.Message);
                }

                CanSaveOrDeleteChecked = false;
                RefreshShownLoanRules(_allLoanRules);
            }
        }
        #endregion

        #region Private Methodes
        private void SwitchReplacementRules(LoanRule addReplacementRule, LoanRule removeReplacementRule)
        {
            if (removeReplacementRule != null)
            {
                if (ReplacementRules.Contains(removeReplacementRule) && addReplacementRule == null)
                {
                    ReplacementRules.Remove(removeReplacementRule);
                }
                else
                {
                    for (int i = 0; i < _allLoanRules.Count; i++)
                    {
                        if (addReplacementRule.Id == _allLoanRules[i].Id)
                        {
                            ReplacementRules.Insert(i, addReplacementRule);
                            break;
                        }
                    }

                    for (int i = 0; i < ReplacementRules.Count; i++)
                    {
                        if (removeReplacementRule.Id == ReplacementRules[i].Id)
                        {
                            ReplacementRules.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            else
            {
                ReplacementRules.Clear();
                foreach (var loanrule in _allLoanRules)
                {
                    ReplacementRules.Add(loanrule);
                }
                ReplacementRules.Add(new LoanRule { Id = 0, Name = "" });
            }
        }

        private void ChangeOrderDirection()
        {
            List<LoanRule> tempLoanRules = new List<LoanRule>();

            if (SelectedOrderDirection == "Stigende")
            {
                tempLoanRules.AddRange(ShownLoanRules.OrderBy(x => x.ExpirationDate));
            }
            else
            {
                tempLoanRules.AddRange(ShownLoanRules.OrderByDescending(x => x.ExpirationDate));
            }

            RefreshShownLoanRules(tempLoanRules);
        }

        private void ChangeDatePeriod()
        {
            List<LoanRule> tempLoanRules = new List<LoanRule>();

            if (ShownLoanRules != null)
            {
                for (int i = 0; i < ShownLoanRules.Count; i++)
                {
                    if (ShownLoanRules[i].ExpirationDate >= SelectedFromDate && ShownLoanRules[i].ExpirationDate <= SelectedToDate)
                    {
                        tempLoanRules.Add(ShownLoanRules[i]);
                    }
                }

                RefreshShownLoanRules(tempLoanRules);
            }
        }

        private void RefreshShownLoanRules(List<LoanRule> loanRules)
        {
            ShownLoanRules.Clear();
            foreach (var loanrule in loanRules)
            {
                ShownLoanRules.Add(loanrule);
            }
        }

        private LoanRule CheckReplacementRule(LoanRule loanRule)
        {
            if (loanRule.Id != 0 && loanRule.ExpirationDate != null)
            {
                loanRule.ExpirationDate = SelectedExpirationDate;
                loanRule.ReplacementRule = SelectedReplacementRule;
            }
            else
            {
                loanRule.ExpirationDate = Convert.ToDateTime("1753-01-01 00:00:00");
                loanRule.ReplacementRule = null;
            }

            return loanRule;
        }

        public bool ValidateInput()
        {
            string errorMessages = "";

            //Name is not filled out
            if (SelectedName == null || SelectedName == "")
            {
                errorMessages += "Navn er ikke udfyldt.\r\n";
            }
            //Loan Length is not filled out or not int
            if (SelectedLoanLength == null || SelectedLoanLength == "" || !int.TryParse(SelectedLoanLength, out int result))
            {
                errorMessages += "Der er ikke blevet valgt en Låne regel.\r\n";
            }
            //Status has not been selected
            if (SelectedStatus == null)
            {
                errorMessages += "Der er ikke blevet valgt en Status.\r\n";
            }
            //All three required fields have been filled out
            else
            {
                // TODO: Figure out why SelectedReplacementRule isn't filled out and non-null
                // Either Expiration Date or Replacement Rule has been filled out, but not both
                // TODO: Using Exclusive or instead of this long line so Expiration XOR ReplacementRule
                //if (SelectedExpirationDate != null ^ SelectedReplacementRule.Name != "")
                //{

                //}
                if ((SelectedExpirationDate == null && SelectedReplacementRule.Name != "") || (SelectedExpirationDate != null && SelectedReplacementRule.Name == ""))
                {
                    errorMessages += "Erstatningsregel og udløbsdato skal begge være udfyldt, hvis en af dem er.";
                }
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
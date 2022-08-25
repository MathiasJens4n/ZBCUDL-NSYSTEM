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
    public class EquipmentViewModel : BaseScannerViewModel
    {
        private WindowManager _windowManager;

        private DataContainer _dataContainer;
        public DataContainer DataContainer
        {
            get { return _dataContainer; }
            set { _dataContainer = value; }
        }

        #region ICommannds
        public ICommand SearchButton { get; set; }
        public ICommand ClearSearchButton { get; set; }
        public ICommand PrintButton { get; set; }
        public ICommand ClearInfoButton { get; set; }
        public ICommand LogOutButton { get; set; }
        public ICommand ClearDateButton { get; set; }
        public ICommand CreateOrUpdateEquipmentButton { get; set; }
        public ICommand DeleteEquipmentButton { get; set; }
        #endregion

        #region General Observable Collection Variables
        private List<Equipment> _allEquipments;
        private ObservableCollection<Equipment> _shownEquipments;
        public ObservableCollection<Equipment> ShownEquipments
        {
            get { return _shownEquipments; }
            set { _shownEquipments = value; OnPropertyChanged("ShownEquipments"); }
        }

        private ObservableCollection<LoanSystemLibraryMini.Model> _models;
        public ObservableCollection<LoanSystemLibraryMini.Model> Models
        {
            get { return _models; }
            set { _models = value; OnPropertyChanged("Models"); }
        }

        private ObservableCollection<Status> _status;
        public ObservableCollection<Status> Statuses
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged("Statuses"); }
        }

        private ObservableCollection<Department> _departments;
        public ObservableCollection<Department> Departments
        {
            get { return _departments; }
            set { _departments = value; OnPropertyChanged("Departments"); }
        }
        #endregion

        #region Selected Variables
        private Equipment _selectedEquipment;
        public Equipment SelectedEquipment
        {
            get { return _selectedEquipment; }
            set
            {
                _selectedEquipment = value;

                if (_selectedEquipment != null)
                {
                    SelectedIdentification = SelectedEquipment.Identification;
                    SelectedLastmaintenance = SelectedEquipment.LastMaintenance;
                    SelectedModel = SelectedEquipment.Model;
                    foreach (LoanSystemLibraryMini.Model model in Models)
                    {
                        if (SelectedEquipment.Model.Id == model.Id)
                        {
                            SelectedModel = model;
                            break;
                        }
                    }
                    foreach (Status status in Statuses)
                    {
                        if (SelectedEquipment.Status.Id == status.Id)
                        {
                            SelectedStatus = status;
                            break;
                        }
                    }
                    foreach (Department department in Departments)
                    {
                        if (SelectedEquipment.BelongingDepartment.Id == department.Id)
                        {
                            SelectedBelongingDepartment = department;
                            break;
                        }
                    }
                    foreach (Department department in Departments)
                    {
                        if (SelectedEquipment.CurrentDepartment.Id == department.Id)
                        {
                            SelectedCurrentDepartment = department;
                            break;
                        }
                    }
                    SelectedEquipmentNote = SelectedEquipment.Note;
                    SelectedEquipmentWorkNote = SelectedEquipment.WorkNote;

                    CreateOrUpdateEquipmentButtonContent = "Gem";
                }
                else
                {
                    SelectedIdentification = "";
                    SelectedLastmaintenance = DateTime.Today;
                    SelectedModel = null;
                    SelectedStatus = null;
                    SelectedBelongingDepartment = null;
                    SelectedCurrentDepartment = null;
                    SelectedEquipmentNote = "";
                    SelectedEquipmentWorkNote = "";

                    CreateOrUpdateEquipmentButtonContent = "Opret";
                }

                OnPropertyChanged("SelectedEquipment");
            }
        }

        private string _selectedIdentification;
        public string SelectedIdentification
        {
            get { return _selectedIdentification; }
            set { _selectedIdentification = value; OnPropertyChanged("SelectedIdentification"); }
        }

        private DateTime? _selectedLastmaintenance = DateTime.Today;
        public DateTime? SelectedLastmaintenance
        {
            get { return _selectedLastmaintenance; }
            set { _selectedLastmaintenance = value; OnPropertyChanged("SelectedLastmaintenance"); }
        }

        private LoanSystemLibraryMini.Model _selectedModel;
        public LoanSystemLibraryMini.Model SelectedModel
        {
            get { return _selectedModel; }
            set { _selectedModel = value; OnPropertyChanged("SelectedModel"); }
        }

        private Status _selectedStatus;
        public Status SelectedStatus
        {
            get { return _selectedStatus; }
            set { _selectedStatus = value; OnPropertyChanged("SelectedStatus"); }
        }

        private Department _selectedBelongingDepartment;
        public Department SelectedBelongingDepartment
        {
            get { return _selectedBelongingDepartment; }
            set { _selectedBelongingDepartment = value; OnPropertyChanged("SelectedBelongingDepartment"); }
        }

        private Department _selectedCurrentDepartment;
        public Department SelectedCurrentDepartment
        {
            get { return _selectedCurrentDepartment; }
            set { _selectedCurrentDepartment = value; OnPropertyChanged("SelectedCurrentDepartment"); }
        }

        private string _selectedEquipmentNote;
        public string SelectedEquipmentNote
        {
            get { return _selectedEquipmentNote; }
            set { _selectedEquipmentNote = value; OnPropertyChanged("SelectedEquipmentNote"); }
        }

        private string _selectedEquipmentWorkNote;
        public string SelectedEquipmentWorkNote
        {
            get { return _selectedEquipmentWorkNote; }
            set { _selectedEquipmentWorkNote = value; OnPropertyChanged("SelectedEquipmentWorkNote"); }
        }
        #endregion

        #region Edit Selected Variable
        private bool _canSaveOrDeleteChecked = false;
        public bool CanSaveOrDeleteChecked
        {
            get { return _canSaveOrDeleteChecked; }
            set { _canSaveOrDeleteChecked = value; OnPropertyChanged("CanSaveOrDeleteChecked"); }
        }

        private string _createOrUpdateEquipmentButtonContent = "Opret";
        public string CreateOrUpdateEquipmentButtonContent
        {
            get { return _createOrUpdateEquipmentButtonContent; }
            set { _createOrUpdateEquipmentButtonContent = value; OnPropertyChanged("CreateOrUpdateEquipmentButtonContent"); }
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
        private bool _identificationChecked = true;
        public bool IdentificationChecked
        {
            get { return _identificationChecked; }
            set { _identificationChecked = value; OnPropertyChanged("IdentificationChecked"); }
        }

        private bool _lastmaintenanceChecked = true;
        public bool LastmaintenanceChecked
        {
            get { return _lastmaintenanceChecked; }
            set { _lastmaintenanceChecked = value; OnPropertyChanged("LastmaintenanceChecked"); }
        }

        private bool _modelChecked = true;
        public bool ModelChecked
        {
            get { return _modelChecked; }
            set { _modelChecked = value; OnPropertyChanged("ModelChecked"); }
        }

        private bool _statusChecked = true;
        public bool StatusChecked
        {
            get { return _statusChecked; }
            set { _statusChecked = value; OnPropertyChanged("StatusChecked"); }
        }

        private bool _belongingDepartmentChecked = true;
        public bool BelongingDepartmentChecked
        {
            get { return _belongingDepartmentChecked; }
            set { _belongingDepartmentChecked = value; OnPropertyChanged("BelongingDepartmentChecked"); }
        }

        private bool _currentDepartmentChecked = true;
        public bool CurrentDepartmentChecked
        {
            get { return _currentDepartmentChecked; }
            set { _currentDepartmentChecked = value; OnPropertyChanged("CurrentDepartmentChecked"); }
        }

        private bool _equipmentNoteChecked = true;
        public bool EquipmentNoteChecked
        {
            get { return _equipmentNoteChecked; }
            set { _equipmentNoteChecked = value; OnPropertyChanged("EquipmentNoteChecked"); }
        }

        private bool _equipmentWorkNoteChecked = true;
        public bool EquipmentWorkNoteChecked
        {
            get { return _equipmentWorkNoteChecked; }
            set { _equipmentWorkNoteChecked = value; OnPropertyChanged("EquipmentWorkNoteChecked"); }
        }
        #endregion

        #region Lower Search Area Variables
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

        public EquipmentViewModel()
        {
            SubscribeScan();
            _windowManager = new WindowManager();

            SearchOptions = new ObservableCollection<string> { "Identifikation", "Sidste vedligeholdelse", "Model", "Status", "Tilhørende Afdeling", "Nuværende Afdeling", "Note", "WorkNote", "" };
            OrderDirections = new ObservableCollection<string> { "Stigende", "Faldende" };

            SearchButton = new Command(x => Search());
            ClearSearchButton = new Command(x => ClearSearch());
            PrintButton = new Command(() => PrintData());
            ClearInfoButton = new Command(x => SelectedEquipment = null);
            LogOutButton = new Command(x => { _loanSystemViewModel.ShowMenuPage(); UnsubscribeScan(); });
            ClearDateButton = new Command(x => SelectedLastmaintenance = null);
            CreateOrUpdateEquipmentButton = new Command(x => CreateOrUpdateEquipment());
            DeleteEquipmentButton = new Command(x => DeleteEquipment());
        }

        #region Initialize Date on Load
        public void InitializeData()
        {
            GetData();
        }

        private void GetData()
        {
            _allEquipments = new List<Equipment>(DataContainer.AllEquipments);
            ShownEquipments = new ObservableCollection<Equipment>(_allEquipments);
            Models = new ObservableCollection<LoanSystemLibraryMini.Model>(DataContainer.AllModels);
            Statuses = new ObservableCollection<Status>(DataContainer.AllStatuses);
            Departments = new ObservableCollection<Department>(DataContainer.AllDepartments);
        }
        #endregion

        #region Private Button Methods
        private void Search()
        {
            if (SelectedSearchOption != null)
            {
                if (SearchInput != null)
                {
                    ShownEquipments.Clear();
                    switch (SelectedSearchOption.ToLower())
                    {
                        case "identifikation":
                            foreach (Equipment equipment in _allEquipments)
                            {
                                if (equipment.Identification.ToLower().StartsWith(SearchInput.ToLower()))
                                {
                                    ShownEquipments.Add(equipment);
                                }
                            }
                            break;
                        case "sidste vedligeholdelse":
                            foreach (Equipment equipment in _allEquipments)
                            {
                                if (equipment.LastMaintenance != null && equipment.LastMaintenance.ToString().StartsWith(SearchInput.ToLower()))
                                {
                                    ShownEquipments.Add(equipment);
                                }
                            }
                            break;
                        case "model":
                            foreach (Equipment equipment in _allEquipments)
                            {
                                if (equipment.Model.Name.ToLower().StartsWith(SearchInput.ToLower()))
                                {
                                    ShownEquipments.Add(equipment);
                                }
                            }
                            break;
                        case "status":
                            foreach (Equipment equipment in _allEquipments)
                            {
                                if (equipment.Status.Name.ToLower().StartsWith(SearchInput.ToLower()))
                                {
                                    ShownEquipments.Add(equipment);
                                }
                            }
                            break;
                        case "tilhørende afdeling":
                            foreach (Equipment equipment in _allEquipments)
                            {
                                if (equipment.BelongingDepartment.Name.ToLower().StartsWith(SearchInput.ToLower()))
                                {
                                    ShownEquipments.Add(equipment);
                                }
                            }
                            break;
                        case "nuværende afdeling":
                            foreach (Equipment equipment in _allEquipments)
                            {
                                if (equipment.CurrentDepartment.Name.ToLower().StartsWith(SearchInput.ToLower()))
                                {
                                    ShownEquipments.Add(equipment);
                                }
                            }
                            break;
                        case "note":
                            foreach (Equipment equipment in _allEquipments)
                            {
                                if (!string.IsNullOrEmpty(equipment.Note) && equipment.Note.ToLower().StartsWith(SearchInput.ToLower()))
                                {
                                    ShownEquipments.Add(equipment);
                                }
                            }
                            break;
                        case "worknote":
                            foreach (Equipment equipment in _allEquipments)
                            {
                                if (!string.IsNullOrEmpty(equipment.WorkNote) && equipment.WorkNote.ToLower().StartsWith(SearchInput.ToLower()))
                                {
                                    ShownEquipments.Add(equipment);
                                }
                            }
                            break;
                    }
                }
            }

            ChangeOrderDirection();
        }

        private void ClearSearch()
        {
            SelectedSearchOption = null;
            SearchInput = "";

            SelectedOrderDirection = OrderDirections[0];

            IdentificationChecked = true;
            LastmaintenanceChecked = true;
            ModelChecked = true;
            StatusChecked = true;
            EquipmentNoteChecked = true;
            EquipmentWorkNoteChecked = true;

            RefreshEquipments(_allEquipments);
        }

        private void PrintData()
        {
            FolderSelectDialog folderSelectDialog = new FolderSelectDialog();
            string path = folderSelectDialog.ChooseSavePath();

            if (!string.IsNullOrEmpty(path))
            {
                string csvData = "Identification;BelongingDepartment;CurrentDepartment;Model;Status;LastMaintenance;Note;WorkNote\r\n";

                foreach (Equipment e in ShownEquipments)
                {
                    csvData += $"\"{e.Identification}\";\"{e.BelongingDepartment.Name}\";\"{e.CurrentDepartment.Name}\";\"{e.Model.Name}\";{e.Status.Name};" +
                               $"{e.LastMaintenance?.ToString("dd-MM-yyyy HH:mm")};\"{e.Note}\";\"{e.WorkNote}\"\r\n";
                }

                File.WriteAllText(path, csvData);
            }
        }

        private async void CreateOrUpdateEquipment()
        {
            if (ValidateInput())
            {
                if (SelectedEquipment == null)
                {
                    Equipment newEquipment = new Equipment
                    {
                        Identification = SelectedIdentification,
                        LastMaintenance = SelectedLastmaintenance,
                        Model = SelectedModel,
                        Status = SelectedStatus,
                        BelongingDepartment = SelectedBelongingDepartment,
                        CurrentDepartment = SelectedCurrentDepartment,
                        Note = SelectedEquipmentNote,
                        WorkNote = SelectedEquipmentWorkNote
                    };

                    try
                    {
                        Equipment createdEquipment = await DataContainer.CreateEquipment(newEquipment);
                        _allEquipments.Add(createdEquipment);
                    }
                    catch (Exception ex)
                    {
                        _windowManager.OpenErrorWindow(ex.Message);
                    }
                }
                else
                {
                    SelectedEquipment.Identification = SelectedIdentification;
                    SelectedEquipment.LastMaintenance = SelectedLastmaintenance;
                    SelectedEquipment.Model = SelectedModel;
                    SelectedEquipment.Status = SelectedStatus;
                    SelectedEquipment.BelongingDepartment = SelectedBelongingDepartment;
                    SelectedEquipment.CurrentDepartment = SelectedCurrentDepartment;
                    SelectedEquipment.Note = SelectedEquipmentNote;
                    SelectedEquipment.WorkNote = SelectedEquipmentWorkNote;

                    try
                    {
                        await DataContainer.UpdateEquipment(SelectedEquipment);

                        for (int i = 0; i < _allEquipments.Count; i++)
                        {
                            if (_allEquipments[i].Id == SelectedEquipment.Id)
                            {
                                _allEquipments[i] = SelectedEquipment;
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
                SelectedEquipment = null;
                RefreshEquipments(_allEquipments);
            }
        }

        private async void DeleteEquipment()
        {
            if (SelectedEquipment != null)
            {
                foreach (Status status in Statuses)
                {
                    if (status.Name.ToLower() == "deleted")
                    {
                        SelectedEquipment.Status = status;
                        break;
                    }
                }

                try
                {
                    await DataContainer.UpdateEquipment(SelectedEquipment);

                    for (int i = 0; i < _allEquipments.Count; i++)
                    {
                        if (_allEquipments[i].Id == SelectedEquipment.Id)
                        {
                            _allEquipments[i] = SelectedEquipment;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _windowManager.OpenErrorWindow(ex.Message);
                }

                CanSaveOrDeleteChecked = false;
                RefreshEquipments(_allEquipments);
            }
        }
        #endregion

        #region Private Methodes
        private void ChangeOrderDirection()
        {
            List<Equipment> tempEquipments = new List<Equipment>();

            if (SelectedOrderDirection == "Stigende")
            {
                tempEquipments.AddRange(ShownEquipments.OrderBy(x => x.Identification));
            }
            else
            {
                tempEquipments.AddRange(ShownEquipments.OrderByDescending(x => x.Identification));
            }

            RefreshEquipments(tempEquipments);
        }

        private void RefreshEquipments(List<Equipment> equipments)
        {
            ShownEquipments = new ObservableCollection<Equipment>(equipments);
        }

        public bool ValidateInput()
        {
            string errorMessages = "";

            if (string.IsNullOrEmpty(SelectedIdentification))
            {
                errorMessages += "Identifikation er ikke udfyldt.\r\n";
            }
            if (SelectedModel == null)
            {
                errorMessages += "Der er ikke blevet valgt en Model.\r\n";
            }
            if (SelectedStatus == null)
            {
                errorMessages += "Der er ikke blevet valgt en Status.\r\n";
            }
            if (SelectedBelongingDepartment == null)
            {
                errorMessages += "Det er ikke blevet valgt et Tilhørende Afdeling.\r\n";
            }
            if (SelectedCurrentDepartment == null)
            {
                errorMessages += "Det er ikke blevet valgt en Nuværende Afdeling.\r\n";
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

        public override void OnQRScanAsync(object source, string e)
        {
            SelectedIdentification = e;
        }
        #endregion
    }
}
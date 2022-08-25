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
    public class TypeModelViewModel : BaseViewModel
    {
        private WindowManager _windowManager;

        private DataContainer _dataContainer;
        public DataContainer DataContainer
        {
            get { return _dataContainer; }
            set { _dataContainer = value; }
        }

        #region ICommands
        public ICommand SearchTypeButton { get; set; }
        public ICommand SearchModelButton { get; set; }
        public ICommand ClearSearchTypeButton { get; set; }
        public ICommand ClearSearchModelButton { get; set; }
        public ICommand PrintTypeButton { get; set; }
        public ICommand PrintModelButton { get; set; }
        public ICommand ClearInfoTypeButton { get; set; }
        public ICommand ClearInfoModelButton { get; set; }
        public ICommand CreateOrUpdateTypeButton { get; set; }
        public ICommand CreateOrUpdateModelButton { get; set; }
        public ICommand DeleteTypeButton { get; set; }
        public ICommand DeleteModelButton { get; set; }
        #endregion

        #region General Observable Collection Variables
        private List<LoanSystemLibraryMini.Type> _allTypes = new List<LoanSystemLibraryMini.Type>();
        private List<LoanSystemLibraryMini.Model> _allModels = new List<LoanSystemLibraryMini.Model>();
        private ObservableCollection<LoanSystemLibraryMini.Type> _shownTypes;
        public ObservableCollection<LoanSystemLibraryMini.Type> ShownTypes
        {
            get { return _shownTypes; }
            set { _shownTypes = value; OnPropertyChanged("ShownTypes"); }
        }

        private ObservableCollection<LoanSystemLibraryMini.Model> _shownModels;
        public ObservableCollection<LoanSystemLibraryMini.Model> ShownModels
        {
            get { return _shownModels; }
            set { _shownModels = value; OnPropertyChanged("ShownModels"); }
        }

        private ObservableCollection<LoanSystemLibraryMini.Type> _types;
        public ObservableCollection<LoanSystemLibraryMini.Type> Types
        {
            get { return _types; }
            set { _types = value; OnPropertyChanged("Types"); }
        }
        #endregion

        #region Selected Variables
        // Type
        private LoanSystemLibraryMini.Type _selectedType;
        public LoanSystemLibraryMini.Type SelectedType
        {
            get { return _selectedType; }
            set
            {
                _selectedType = value;

                if (_selectedType != null)
                {
                    SelectedTypeName = SelectedType.Name;

                    CreateOrUpdateTypeButtonContent = "Gem";
                }
                else
                {
                    SelectedTypeName = "";

                    CreateOrUpdateTypeButtonContent = "Opret";
                }

                OnPropertyChanged("SelectedType");
            }
        }

        private string _selectedTypeName;
        public string SelectedTypeName
        {
            get { return _selectedTypeName; }
            set { _selectedTypeName = value; OnPropertyChanged("SelectedTypeName"); }
        }

        // Model
        private LoanSystemLibraryMini.Model _selectedModel;
        public LoanSystemLibraryMini.Model SelectedModel
        {
            get { return _selectedModel; }
            set
            {
                _selectedModel = value;

                if (_selectedModel != null)
                {
                    SelectedModelName = SelectedModel.Name;
                    foreach (LoanSystemLibraryMini.Type type in _allTypes)
                    {
                        if (SelectedModel.Type.Id == type.Id)
                        {
                            SelectedModelType = type;
                            break;
                        }
                    }

                    CreateOrUpdateModelButtonContent = "Gem";
                }
                else
                {
                    SelectedModelName = "";
                    SelectedModelType = null;

                    CreateOrUpdateModelButtonContent = "Opret";
                }

                OnPropertyChanged("SelectedModel");
            }
        }

        private string _selectedModelName;
        public string SelectedModelName
        {
            get { return _selectedModelName; }
            set { _selectedModelName = value; OnPropertyChanged("SelectedModelName"); }
        }

        private LoanSystemLibraryMini.Type _selectedModelType;
        public LoanSystemLibraryMini.Type SelectedModelType
        {
            get { return _selectedModelType; }
            set { _selectedModelType = value; OnPropertyChanged("SelectedModelType"); }
        }
        #endregion

        #region Edit Selected Variable
        // Type
        private bool _canSaveOrDeleteTypeChecked = false;
        public bool CanSaveOrDeleteTypeChecked
        {
            get { return _canSaveOrDeleteTypeChecked; }
            set { _canSaveOrDeleteTypeChecked = value; OnPropertyChanged("CanSaveOrDeleteTypeChecked"); }
        }

        private string _createOrUpdateTypeButtonContent = "Opret";
        public string CreateOrUpdateTypeButtonContent
        {
            get { return _createOrUpdateTypeButtonContent; }
            set { _createOrUpdateTypeButtonContent = value; OnPropertyChanged("CreateOrUpdateTypeButtonContent"); }
        }

        // Model
        private bool _canSaveOrDeleteModelChecked = false;
        public bool CanSaveOrDeleteModelChecked
        {
            get { return _canSaveOrDeleteModelChecked; }
            set { _canSaveOrDeleteModelChecked = value; OnPropertyChanged("CanSaveOrDeleteModelChecked"); }
        }

        private string _createOrUpdateModelButtonContent = "Opret";
        public string CreateOrUpdateModelButtonContent
        {
            get { return _createOrUpdateModelButtonContent; }
            set { _createOrUpdateModelButtonContent = value; OnPropertyChanged("CreateOrUpdateModelButtonContent"); }
        }
        #endregion

        #region Search Section
        private string _searchTypeInput;
        public string SearchTypeInput
        {
            get { return _searchTypeInput; }
            set { _searchTypeInput = value; OnPropertyChanged("SearchTypeInput"); }
        }

        private string _searchModelInput;
        public string SearchModelInput
        {
            get { return _searchModelInput; }
            set { _searchModelInput = value; OnPropertyChanged("SearchModelInput"); }
        }
        #endregion

        public TypeModelViewModel()
        {
            _windowManager = new WindowManager();

            SearchTypeButton = new Command(x => SearchType());
            ClearSearchTypeButton = new Command(x => ClearSearchType());
            PrintTypeButton = new Command(() => PrintTypeData());
            ClearInfoTypeButton = new Command(x => SelectedType = null);
            CreateOrUpdateTypeButton = new Command(x => CreateOrUpdateType());
            DeleteTypeButton = new Command(x => DeleteType());

            SearchModelButton = new Command(x => SearchModel());
            ClearSearchModelButton = new Command(x => ClearSearchModel());
            PrintModelButton = new Command(() => PrintModelData());
            ClearInfoModelButton = new Command(x => SelectedModel = null);
            CreateOrUpdateModelButton = new Command(x => CreateOrUpdateModel());
            DeleteModelButton = new Command(x => DeleteModel());
        }

        #region Initializa Data on Load
        public void InitializeData()
        {
            GetData();
        }

        private void GetData()
        {
            _allTypes = new List<LoanSystemLibraryMini.Type>(DataContainer.AllTypes);
            _allModels = new List<LoanSystemLibraryMini.Model>(DataContainer.AllModels);
            ShownTypes = new ObservableCollection<LoanSystemLibraryMini.Type>(_allTypes);
            ShownModels = new ObservableCollection<LoanSystemLibraryMini.Model>(_allModels);
            Types = new ObservableCollection<LoanSystemLibraryMini.Type>(_allTypes);
        }
        #endregion

        #region Private Button Methodes
        // Type
        private void SearchType()
        {
            if (SearchTypeInput != null)
            {
                ShownTypes.Clear();
                foreach (var type in _allTypes)
                {
                    if (type.Name.ToLower().StartsWith(SearchTypeInput.ToLower()))
                    {
                        ShownTypes.Add(type);
                    }
                }
            }
        }

        private void ClearSearchType()
        {
            SearchTypeInput = "";

            RefreshShownTypes(_allTypes);
        }

        private void PrintTypeData()
        {
            FolderSelectDialog folderSelectDialog = new FolderSelectDialog();
            string path = folderSelectDialog.ChooseSavePath();

            if (!string.IsNullOrEmpty(path))
            {
                string csvData = "Name\r\n";

                foreach (var t in ShownTypes)
                {
                    csvData += $"\"{t.Name}\"\r\n";
                }

                File.WriteAllText(path, csvData);
            }
        }

        private async void CreateOrUpdateType()
        {
            if (ValidateTypeInput())
            {
                if (SelectedType == null)
                {
                    LoanSystemLibraryMini.Type type = new LoanSystemLibraryMini.Type
                    {
                        Name = SelectedTypeName
                    };

                    try
                    {
                        LoanSystemLibraryMini.Type createdType = await DataContainer.CreateType(type);
                        _allTypes.Add(createdType);
                        Types.Add(createdType);
                    }
                    catch (Exception ex)
                    {
                        _windowManager.OpenErrorWindow(ex.Message);
                    }
                }
                else
                {
                    SelectedType.Name = SelectedTypeName;

                    try
                    {
                        await DataContainer.UpdateType(SelectedType);

                        for (int i = 0; i < _allTypes.Count(); i++)
                        {
                            if (_allTypes[i].Id == SelectedType.Id)
                            {
                                _allTypes[i] = SelectedType;
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _windowManager.OpenErrorWindow(ex.Message);
                    }

                }

                CanSaveOrDeleteTypeChecked = false;
                SelectedType = null;
                RefreshShownTypes(_allTypes);
            }
        }

        private async void DeleteType()
        {
            if (SelectedType != null)
            {
                try
                {
                    await DataContainer.DeleteType(SelectedType.Id);
                    _allTypes.Remove(SelectedType);
                    Types.Remove(SelectedType);
                }
                catch (Exception ex)
                {
                    _windowManager.OpenErrorWindow(ex.Message + "\nDet er nok fejl, fordi at der er en model som har denne type.");
                }

                CanSaveOrDeleteTypeChecked = false;
                RefreshShownTypes(_allTypes);
            }
        }

        // Model
        private void SearchModel()
        {
            if (SearchModelInput != null)
            {
                ShownModels.Clear();
                foreach (var model in _allModels)
                {
                    if (model.Name.ToLower().StartsWith(SearchModelInput.ToLower()))
                    {
                        ShownModels.Add(model);
                    }
                    else if (model.Type.Name.ToLower().StartsWith(SearchModelInput.ToLower()))
                    {
                        ShownModels.Add(model);
                    }
                }
            }
        }

        private void ClearSearchModel()
        {
            SearchModelInput = "";

            RefreshShownModels(_allModels);
        }

        private void PrintModelData()
        {
            FolderSelectDialog folderSelectDialog = new FolderSelectDialog();
            string path = folderSelectDialog.ChooseSavePath();

            if (!string.IsNullOrEmpty(path))
            {
                string csvData = "Name;Type\r\n";

                foreach (var m in ShownModels)
                {
                    csvData += $"\"{m.Name}\";\"{m.Type.Name}\"\r\n";
                }

                File.WriteAllText(path, csvData);
            }
        }

        private async void CreateOrUpdateModel()
        {
            if (ValidateModelInput())
            {
                if (SelectedModel == null)
                {
                    LoanSystemLibraryMini.Model model = new LoanSystemLibraryMini.Model
                    {
                        Name = SelectedModelName,
                        Type = SelectedModelType
                    };

                    try
                    {
                        LoanSystemLibraryMini.Model createdModel = await DataContainer.CreateModel(model);
                        _allModels.Add(createdModel);
                    }
                    catch (Exception ex)
                    {
                        _windowManager.OpenErrorWindow(ex.Message);
                    }
                }
                else
                {
                    SelectedModel.Name = SelectedModelName;
                    SelectedModel.Type = SelectedModelType;

                    try
                    {
                        await DataContainer.UpdateModel(SelectedModel);

                        for (int i = 0; i < _allModels.Count; i++)
                        {
                            if (_allModels[i].Id == SelectedModel.Id)
                            {
                                _allModels[i] = SelectedModel;
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _windowManager.OpenErrorWindow(ex.Message);
                    }
                }

                CanSaveOrDeleteModelChecked = false;
                SelectedModel = null;
                RefreshShownModels(_allModels);
            }
        }

        private async void DeleteModel()
        {
            if (SelectedModel != null)
            {
                try
                {
                    await DataContainer.DeleteModel(SelectedModel.Id);
                    _allModels.Remove(SelectedModel);
                }
                catch (Exception ex)
                {
                    _windowManager.OpenErrorWindow(ex.Message + "\nDet er nok fejl, fordi at der er et udstyr som har denne model.");
                }

                CanSaveOrDeleteModelChecked = false;
                RefreshShownModels(_allModels);
            }

        }
        #endregion

        #region Private Methodes
        // Type
        private void RefreshShownTypes(List<LoanSystemLibraryMini.Type> types)
        {
            ShownTypes.Clear();
            foreach (var type in types)
            {
                ShownTypes.Add(type);
            }
        }

        private bool ValidateTypeInput()
        {
            if (string.IsNullOrEmpty(SelectedTypeName))
            {
                _windowManager.OpenErrorWindow("Navn er ikke udfyldt.");
                return false;
            }
            else
            {
                return true;
            }
        }

        // Model
        private void RefreshShownModels(List<LoanSystemLibraryMini.Model> models)
        {
            ShownModels.Clear();
            foreach (var model in models)
            {
                ShownModels.Add(model);
            }
        }

        private bool ValidateModelInput()
        {
            string errorMessages = "";

            if (string.IsNullOrEmpty(SelectedModelName))
            {
                errorMessages += "Navn er ikke udfyldt.\r\n";
            }
            if (SelectedModelType == null)
            {
                errorMessages += "Det er ikke blevet valgt en Type.\r\n";
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
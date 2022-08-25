using LoanSystemWPFMini.Managers;
using LoanSystemWPFMini.Model;
using System.Configuration;
using System.Windows.Input;

namespace LoanSystemWPFMini.ViewModels
{
    public class SelectDepartmentViewModel : BaseViewModel
    {
        private WindowManager _windowManager;

        public ICommand SelectButton { get; set; }

        public SelectDepartmentViewModel()
        {
            _windowManager = new WindowManager();

            SelectButton = new Command((parameter) =>
            {
                WriteLocationChoiceToConfig(parameter.ToString());
                _loanSystemViewModel.ShowMenuPage();
            });
        }
        
        private void WriteLocationChoiceToConfig(string value)
        {
            try
            {
                ConfigManager cm = new ConfigManager();
                cm.UpdateAppSettings("appSettings", "CurrentLocation", value);
            }
            catch (ConfigurationErrorsException)
            {
                _windowManager.OpenErrorWindow("Noget gik galt.\r\nGenstart programmmet.");
            }
        }
    }
}

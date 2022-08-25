using LoanSystemWPFMini.Model;
using System.Windows;
using System.Windows.Controls;

namespace LoanSystemWPFMini.ViewModels.PopupViewModels
{
    public class ErrorViewModel : BaseViewModel
    {
        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; OnPropertyChanged("ErrorMessage"); }
        }

        public Command<UserControl> CloseWindowButton { get; set; }

        public ErrorViewModel()
        {
            CloseWindowButton = new Command<UserControl>(CloseWindow);
        }

        private void CloseWindow(UserControl userControl)
        {
            Window.GetWindow(userControl).Close();
        }
    }
}
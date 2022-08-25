using LoanSystemWPFMini.Model;
using System.Windows.Controls;
using System.Windows.Input;

namespace LoanSystemWPFMini.ViewModels
{
    public class LogInViewModel : BaseViewModel
    {
        public ICommand LogInButton { get; set; }
        public ICommand BackButton { get; set; }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged("UserName"); }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; OnPropertyChanged("ErrorMessage"); }
        }

        public LogInViewModel()
        {
            LogInButton = new Command((o) => ValidateLogin(o));
            BackButton = new Command(() => _loanSystemViewModel.ShowMenuPage());
        }

        private void ValidateLogin(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            var password = passwordBox.Password;

            if (UserName == "IT" && password == "123")
            {
                _loanSystemViewModel.ShowOverviewStatusPage();
            }
            else
            {
                ErrorMessage = "Brugernavn og, eller password er forkert.";
            }            

        }
    }
}
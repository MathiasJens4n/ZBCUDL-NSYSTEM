using LoanSystemWPFMini.Model;
using System.Windows.Input;

namespace LoanSystemWPFMini.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        public ICommand LogInButton { get; set; }
        public ICommand DeliverButton { get; set; }
        public ICommand LoanButton { get; set; }

        public MenuViewModel()
        {
            LogInButton = new Command(() => _loanSystemViewModel.ShowlogInPage());
            DeliverButton = new Command(() => _loanSystemViewModel.ShowDeliverEquipment());
            LoanButton = new Command(() => _loanSystemViewModel.ShowLoanEquipmentPage());
        }
    }
}
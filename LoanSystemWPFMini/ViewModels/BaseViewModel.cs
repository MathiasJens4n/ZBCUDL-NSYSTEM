using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LoanSystemWPFMini.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        /*
         * OnPropertyChanged exposes an event to which the UI can "listen",
         * so when a control "hears" that the property to which it is bound has changed,
         * it can "update itself".
         */
        public event PropertyChangedEventHandler PropertyChanged;

        // TODO: Should this take value in so we can make sure that the value is always set before
        // invoking the PropertyChanged event?
        //protected void OnPropertyChanged<T>(ref T orig, T val, [CallerMemberName] string propertyName = "")
        //{
        //    orig = val;
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected LoanSystemViewModel _loanSystemViewModel;
        public void AttachLoanSystemViewModel(LoanSystemViewModel loanSystemViewModel)
        {
            _loanSystemViewModel = loanSystemViewModel;
        }

        private double _scalability;
        public double Scalability
        {
            get { return _scalability; }
            set
            {
                _scalability = value;
                FontSize = Scalability;
                Margin = Scalability;
                TitleSize = Scalability;
                OnPropertyChanged("Scalability");
            }
        }

        private double _fontSize;
        public double FontSize { get { return _fontSize; } set { _fontSize = (value * 12); OnPropertyChanged("FontSize"); } }

        private double _titleSize;
        public double TitleSize { get { return _titleSize; } set { _titleSize = (value * 12 * 1.5); OnPropertyChanged("TitleSize"); } }

        private double _margin;
        public double Margin { get { return _margin; } set { _margin = (value * 5); OnPropertyChanged("Margin"); } }
    }
}
using System.Windows;

namespace LoanSystemWPFMini.Model
{
    public class BindingProxy : Freezable
    {
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy));

        public object Data
        {
            get { return GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        //overrides af freezable rte  
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }
    }
}
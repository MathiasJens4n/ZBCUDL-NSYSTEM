using LoanSystemWPFMini.ViewModels.PopupViewModels;
using System.Windows;

namespace LoanSystemWPFMini.Managers
{
    public class WindowManager
    {
        public void OpenErrorWindow(string message)
        {
            ErrorViewModel evm = new ErrorViewModel();
            evm.ErrorMessage = message;

            // TODO: Why not use ErrorView instead of having a global resource
            var win = new Window();
            win.ResizeMode = ResizeMode.NoResize;
            win.Topmost = true;
            win.Height = 300;
            win.Width = 400;
            win.MaxHeight = 300;
            win.MaxWidth = 400;
            win.MinHeight = 300;
            win.MinWidth = 400;

            ShowWindow(win, evm);
        }
        private void ShowWindow(Window win, object viewModel)
        {
            win.Content = viewModel;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ShowDialog();
        }
    }
}
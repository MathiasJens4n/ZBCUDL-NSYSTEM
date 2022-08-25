using LoanSystemWPFMini.Model;

namespace LoanSystemWPFMini.ViewModels
{
    public abstract class BaseScannerViewModel : BaseViewModel
    {
        private QRScanner _qRScanner;

        public BaseScannerViewModel()
        {
            _qRScanner = new QRScanner();
        }

        public abstract void OnQRScanAsync(object source, string e);

        public void SubscribeScan()
        {
            _qRScanner.OnScan += OnQRScanAsync;
        }
        
        public void UnsubscribeScan()
        {
            _qRScanner.OnScan -= OnQRScanAsync;
        }
    }
}
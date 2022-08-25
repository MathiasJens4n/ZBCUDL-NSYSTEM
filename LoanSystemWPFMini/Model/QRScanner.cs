using System;
using CoreScanner;
using System.Xml;

namespace LoanSystemWPFMini.Model
{
    public delegate void ScanEventHandler(object source, string e);

    public class QRScanner
    {
        private CCoreScannerClass cCoreScannerClass;

        public event ScanEventHandler OnScan;

        public QRScanner()
        {
            //Instantiate CoreScanner Class
            cCoreScannerClass = new CCoreScannerClass();
            //Call Open API
            short[] scannerTypes = new short[1];//Scanner Types you are interested in
            scannerTypes[0] = 1; // 1 for all scanner types
            short numberOfScannerTypes = 1; // Size of the scannerTypes array
            int status; // Extended API return code
            cCoreScannerClass.Open(0, scannerTypes, numberOfScannerTypes, out status);
            // Subscribe for barcode events in cCoreScannerClass
            cCoreScannerClass.BarcodeEvent += new
            _ICoreScannerEvents_BarcodeEventEventHandler(OnBarcodeEvent);
            // Let's subscribe for events
            int opcode = 1001; // Method for Subscribe events
            string outXML; // XML Output
            string inXML = "<inArgs>" +
                            "<cmdArgs>" +
                            "<arg-int>1</arg-int>" + // Number of events you want to subscribe
                            "<arg-int>1</arg-int>" + // Comma separated event IDs
                            "</cmdArgs>" +
                           "</inArgs>";
            cCoreScannerClass.ExecCommand(opcode, ref inXML, out outXML, out status);
        }

        private void OnBarcodeEvent(short eventType, ref string pscanData)
        {
            XmlDocument xmlData = new XmlDocument();
            xmlData.LoadXml(pscanData);
            string s = xmlData["outArgs"]["arg-xml"]["scandata"]["datalabel"].InnerText;

            s = s.Replace("0x", "");
            string[] hexSplit = s.Split(' ');
            string output = "";
            foreach (string hex in hexSplit)
            {
                // Convert the number expressed in base-16 to an integer.
                int value = Convert.ToInt32(hex, 16);
                // Get the character corresponding to the integral value.
                string stringValue = Char.ConvertFromUtf32(value);
                output += stringValue;
            }

            OnScan?.Invoke(this, output);
        }
    }
}
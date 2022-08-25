using Microsoft.Win32;

namespace LoanSystemWPFMini.Model
{
    public class FolderSelectDialog
    {
        public string ChooseSavePath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "Print";
            saveFileDialog.DefaultExt = ".csv";
            saveFileDialog.Filter = "(.csv)|*csv";

            bool? path = saveFileDialog.ShowDialog();

            if(path == true) 
            { 
                return saveFileDialog.FileName;
            }

            return null;
        }
    }
}

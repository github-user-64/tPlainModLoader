using System.Windows.Forms;

namespace PixelArt.Utils
{
    internal class Utils
    {
        public static string GetFileName()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                if (openFileDialog.ShowDialog() != DialogResult.OK) return null;

                return openFileDialog.FileName;
            }
            catch
            {
                return null;
            }
        }
    }
}

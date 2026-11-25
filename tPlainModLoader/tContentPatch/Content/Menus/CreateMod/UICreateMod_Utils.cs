using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using tContentPatch.Content.UI;
using Terraria;
using Terraria.GameContent;
using Terraria.UI;

namespace tContentPatch.Content.Menus.CreateMod
{
    internal partial class UICreateMod : UIMenuPanel
    {
        private readonly object _lock_Switching = new object();
        private bool Switching = false;

        public static void OpenMenu(UIState back = null)
        {
            if (Main.dedServ) return;
            if (Main.showSplash) return;

            GameUitl.OpenMenu(new UICreateMod(back));
        }

        protected void SetPath(string path)
        {
            UIPath.UITextBox.Text = path;
        }

        protected virtual void SetPathDefault()
        {
            try
            {
                SetPath(Path.GetDirectoryName(typeof(ContentPatch).Assembly.Location));
            }
            catch { }
        }

        private string GetTextLine(string text, float width)
        {
            if (text == null) return string.Empty;

            string[] ss = Terraria.Utils.WordwrapString(text, FontAssets.MouseText.Value,
                (int)width, 16, out int lineAmount);

            text = string.Empty;

            foreach (string i in ss) if (i != null) text += $"{i}\n";

            return text;
        }

        protected void TryOpenDirectory(string s)
        {
            try
            {
                Process.Start(s);
            }
            catch (Exception ex)
            {
                SetTip($"无法打开目录:{ex.Message}", Color.Red);
            }
        }

        protected void TrySwitchPath()
        {
            try
            {
                lock (_lock_Switching)
                {
                    if (Switching) return;
                    Switching = true;
                }

                string path = null;

                Thread t = new Thread(() =>
                {
                    FolderBrowserDialog f = new FolderBrowserDialog();
                    f.Description = "选择创建路径";
                    f.SelectedPath = UIPath.UITextBox.Text;
                    if (f.ShowDialog() == DialogResult.OK) path = f.SelectedPath;
                });
                t.Name = $"{nameof(UICreateMod)}:{nameof(TrySwitchPath)}";
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
                t.Join();

                if (string.IsNullOrEmpty(path)) return;

                SetPath(path);
            }
            catch (Exception ex)
            {
                SetTip($"选择失败:{ex.Message}", Color.Red);
            }
            finally
            {
                Switching = false;
            }
        }
    }
}

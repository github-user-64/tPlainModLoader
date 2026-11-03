using Microsoft.Xna.Framework;
using System;
using System.Threading.Tasks;
using tContentPatch.Content.UI;

namespace tContentPatch.Content.Menus.CreateMod
{
    internal partial class UICreateMod : UIMenuPanel
    {
        private readonly object _lock_Createing = new object();
        private bool Createing = false;

        protected async void TryCreate()
        {
            try
            {
                lock (_lock_Createing)
                {
                    if (Createing) return;
                    Createing = true;
                }

                string displayName = UIDisplayName.UITextBox.Text;
                string modName = UIModName.UITextBox.Text;
                string path = UIPath.UITextBox.Text;

                SetTip("创建中", Color.Yellow);

                string open = null;
                await Task.Run(() =>
                {
                    Content.CreateMod.Create(displayName, modName, path, out open);
                });

                SetTip("创建完成", Color.Green);
                TryOpenDirectory(open);
            }
            catch (Exception ex)
            {
                SetTip($"创建失败:{ex.Message}", Color.Red);
            }
            finally
            {
                Createing = false;
            }
        }
    }
}

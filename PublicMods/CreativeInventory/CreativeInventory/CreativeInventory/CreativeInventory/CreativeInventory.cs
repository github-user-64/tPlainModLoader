namespace CreativeInventory.CreativeInventory
{
    internal class CreativeInventory
    {
        private static string key = null;
        private static UICreativeInventory ui_ci = null;

        public static void SwitchOpenOrClose()
        {
            if (ui_ci == null)
            {
                ui_ci = new UICreativeInventory("物品浏览器", 600, 450);
            }

            if (ui_ci.IsOpen) ui_ci.Close();
            else ui_ci.Open(ModifyInterfaceLayers.ui_state);
        }

        public static void SetBind(string key)
        {
            ListenInput.DelListenInput(CreativeInventory.key, foo);

            CreativeInventory.key = key;

            ListenInput.AddListenInput(CreativeInventory.key, foo);
        }

        private static void foo(bool isOne)
        {
            if (isOne) SwitchOpenOrClose();
        }
    }
}

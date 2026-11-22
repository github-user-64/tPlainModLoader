namespace OptimizeAndTool.Content.UI.Menus
{
    public partial class UIMenuServerList
    {
        protected virtual void ControlAdd()
        {
            if (Control == null) return;

            int index = UISP.ElementsIndexOf(CheckedItem);

            //

            index = Control.Add(Control.GetDefault(), index);
            if (index == -1) return;

            AddUIItem(Control.Data[index], index);
        }

        protected virtual void ControlDel(UIServerListItem item)
        {
            if (Control == null) return;
            if (item.Info == null) return;

            Control.Del(item.Info);
            UISP.RemoveChild(item);
        }

        protected virtual void ControlUp(UIServerListItem item)
        {
            if (Control == null) return;
            if (item.Info == null) return;

            Control.OffUp(item.Info);
            UISP.UpItem(item);
        }

        protected virtual void ControlDown(UIServerListItem item)
        {
            if (Control == null) return;
            if (item.Info == null) return;

            Control.OffDown(item.Info);
            UISP.DownItem(item);
        }

        protected virtual void LoadData()
        {
            if (Control == null) return;

            Control.Load();
            UpdateUIData();
        }

        protected virtual void SaveData() => Control?.Save();
    }
}

using System;

namespace OptimizeAndTool.Content.UI.Menus
{
    public partial class UIMenuServerList
    {
        #region 设置UpdatePrefixControl
        protected virtual void SetControl(Action action)
        {
            if (Control == null) return;

            UpdatePrefixControl = action;
        }

        protected virtual void SetControlAdd() => SetControl(ControlAdd);
        protected virtual void SetControlDel(UIServerListItem item) => SetControl(() => ControlDel(item));
        protected virtual void SetControlUp(UIServerListItem item) => SetControl(() => ControlUp(item));
        protected virtual void SetControlDown(UIServerListItem item) => SetControl(() => ControlDown(item));
        protected virtual void SetControlLoad() => SetControl(ControlLoad);
        protected virtual void SetControlSave() => SetControl(ControlSave);
        #endregion

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

            UISV.UIScrollbar.ViewPosition -= item.GetOuterDimensions().Height + UISP.ItemMargin;
        }

        protected virtual void ControlDown(UIServerListItem item)
        {
            if (Control == null) return;
            if (item.Info == null) return;

            Control.OffDown(item.Info);
            UISP.DownItem(item);

            UISV.UIScrollbar.ViewPosition += item.GetOuterDimensions().Height + UISP.ItemMargin;
        }

        protected virtual void ControlLoad()
        {
            if (Control == null) return;

            Control.Load();
            UpdateUIData();
        }

        protected virtual void ControlSave() => Control?.Save();
    }
}

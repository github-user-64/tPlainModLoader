using System;
using System.Collections.Generic;
using tContentPatch;

namespace QuickSetting.QuickSetting
{
    public class KeyOrderSetting : ModSetting
    {
        public override string FilePath => "KeyOrder.json";
        public override Type DataType => typeof(List<string>);
        public override bool HasUI => false;
        private List<string> data = null;

        public override void Load(object v)
        {
            data = v as List<string>;
            if (data == null)
            {
                data = new List<string>();
            }

            QuickSetting.OnAddItem = s =>
            {
                if (data.Contains(s) == false) data.Add(s);

                QuickSetting.SetKeyOrder(data);
            };
            QuickSetting.OnSwitchItem = (s1, s2) =>
            {
                if (s1 == s2 || s1 == null || s2 == null) return;
                int index1 = data.IndexOf(s1);
                int index2 = data.IndexOf(s2);
                if (index1 ==  -1 || index2 == -1) return;
                data[index1] = s2;
                data[index2] = s1;

                QuickSetting.SetKeyOrder(data);

                NeedSave = true;
                Save();
            };

            QuickSetting.SetKeyOrder(data);
        }

        public override object GetSaveData() => data;
    }
}

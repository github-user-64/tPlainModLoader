using System;
using tContentPatch;
using tContentPatch.Content.UI;
using tContentPatch.Content.UI.ModSet;
using Terraria.UI;

namespace DeadEffect
{
    internal class Config : ModSetting
    {
        public class ConfigData
        {
            public bool DeBug = false;

            public bool EffectContent = true;
            public bool EffectSound = true;

            public bool EnableHurtEffect = true;
            public bool EnableDeadEffect = true;
        }

        public override string Name => "设置";
        public override string Title => "死亡效果: 设置";
        public override string FilePath => "setting.json";
        public override Type DataType => typeof(ConfigData);
        public static ConfigData Data { get; protected set; } = null;

        public override void Load(object v)
        {
            NeedSave = true;

            Data = v as ConfigData;

            if (v != null) return;

            SetDefault();
            Save();
        }

        public override object GetSaveData() => Data;

        public override void SetDefault() => Data = new ConfigData();

        public override void Save()
        {
            base.Save();
            NeedSave = true;
        }

        public override UIElement GetUI()
        {
            UIScrollViewer2 sv = new UIScrollViewer2();
            sv.Width.Precent = 1;
            sv.Height.Precent = 1;

            sv.AddChild(GetUI("效果内容", () => Data.EffectContent, v => Data.EffectContent = v));
            sv.AddChild(GetUI("效果声音", () => Data.EffectSound, v => Data.EffectSound = v));
            sv.AddChild(GetUI("受击效果", () => Data.EnableHurtEffect, v => Data.EnableHurtEffect = v));
            sv.AddChild(GetUI("死亡效果", () => Data.EnableDeadEffect, v => Data.EnableDeadEffect = v));
            sv.AddChild(GetUI("调式", () => Data.DeBug, v => Data.DeBug = v));

            return sv;
        }

        private UIElement GetUI(string text, Func<bool> get, Action<bool> set)
        {
            UIItemSwitch ui = new UIItemSwitch(null, text);
            ui.OnUpdate += v => ui.SetVal(get());
            ui.OnValUpdate += v => set(v);

            return ui;
        }
    }
}

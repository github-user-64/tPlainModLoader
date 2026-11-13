using Microsoft.Xna.Framework.Graphics;
using PixelArt.Utils.quickBuild;
using ReLogic.Content;
using System.Collections.Generic;
using tContentPatch;
using tContentPatch.Content.UI.Item1;
using Terraria;
using Terraria.UI;

namespace PixelArt.Content
{
    public partial class PixelArt : PatchMain
    {
        public override void OnEnterWorld()
        {
            InitPixelArt();
        }

        private class OnUnload : Mod
        {
            public override void Unload()
            {
                Close();
            }
        }

        public static List<UIElement> GetUI()
        {
            Texture2D ico1 = Main.Assets.Request<Texture2D>("Images/UI/Camera_6", AssetRequestMode.ImmediateLoad).Value;

            UIItemTextButton getFile = new UIItemTextButton(ico1, "选择文件并加载", "选择");
            getFile.OnClick += CtlSetPathAndLoadPixelInfo;

            List<UIElement> uis = new List<UIElement>
            {
                getFile,

                new UIItemSwitchATextBoxString(title: "加载图片数据")
                {
                    MouseText = "文件路径<string>",
                    OnGetIsChecked = () => PixelInfoLoading,
                    OnGetValue = () => LoadPath.val,
                    OnSetValue = v => LoadPath.val = v,
                    OnIsCheckChanged = v =>
                    {
                        if (v)CtlLoadPixelInfo();
                        else CtlCancelLoadPixelInfo();
                    }
                },

                new UIItemSwitch(title: "生成")
                {
                    OnGetIsChecked = () => SpawIng,
                    OnIsCheckChanged = v =>
                    {
                        if (v)CtlStartSpaw();
                        else CtlEndSpaw();
                    }
                },

                new UIItemSwitch(title: "生成位置")
                {
                    OnGetIsChecked = () => SpawPosSelecting,
                    OnIsCheckChanged = v =>
                    {
                        if (v)CtlSetSpawPos();
                        else CtlCancelSetSpawPos();
                    }
                },

                new UIItemTextBoxInt(title: "生成速度")
                {
                    MouseText = "<int>",
                    OnGetValue = () => SpawSpeed.val,
                    OnSetValue = v => SpawSpeed.val = v,
                },

                UIBuild.get2(SetSelectedItem, text:"设置手中物品"),

                new UIItemValueSliderInt(0, 3, title: "生成方向")
                {
                    OnGetValue = () => SpawPosSelectV.val,
                    OnSetValue = v => SpawPosSelectV.val = v,
                    ValueToString = v =>
                    {
                        if (v == 0) return "左上";
                        if (v == 1) return "右上";
                        if (v == 2) return "左下";
                        if (v == 3) return "右下";
                        return "-";
                    }
                },

                UIBuild.get2(DisplayWall, text:"显示预览"),

                new UIItemValueSliderInt(0, 1, title: "加载方式")
                {
                    OnGetValue = () => LoadType.val,
                    OnSetValue = v => LoadType.val = v,
                    ValueToString = v =>
                    {
                        if (v == 0) return "普通";
                        if (v == 1) return "快速";
                        return "-";
                    }
                },

                new UIItemSwitch(title: "加载时使用油漆")
                {
                    MouseText = "不要频繁开关\n切换后会停下其它操作且需重新加载",
                    OnGetIsChecked = () => LoadUsePaint.val,
                    OnIsCheckChanged = _ => CtlSwitchLoadUsePaint(),
                },
            };

            return uis;
        }

        public static void CtlSwitchLoadUsePaint()
        {
            if (NoControl()) return;

            LoadUsePaint.val = !LoadUsePaint.val;

            InitPixelArt();
        }

        public static void CtlSetPathAndLoadPixelInfo()
        {
            if (NoControl()) return;
            if (foo(SwitchPathing == true, "无法选择, 正在选择")) return;

            SwitchPath(() =>
            {
                CtlLoadPixelInfo();
            });
        }

        #region 加载
        public static void CtlLoadPixelInfo()
        {
            if (NoControl()) return;
            if (foo(SpawIng == true, "无法加载, 正在生成")) return;
            if (foo(PixelInfoLoading == true, "正在加载")) return;

            Main.NewText("加载像素信息");

            LoadPixelInfo();
        }

        public static void CtlCancelLoadPixelInfo()
        {
            if (NoControl()) return;
            if (PixelInfoLoading == false) return;

            CancelLoadPixelInfo();

            Main.NewText("已取消");
        }
        #endregion

        #region 生成
        public static void CtlStartSpaw()
        {
            if (NoControl()) return;

            if (foo(PixelInfoLoading == true, "无法生成, 正在加载像素信息")) return;
            if (foo(PixelInfoLoaded == false, "无法生成, 像素信息未加载")) return;
            if (foo(SpawPosSelecting == true, "无法生成, 正在设置生成位置")) return;
            if (foo(SpawIng == true, "正在生成")) return;

            Main.NewText("开始生成");

            StartSpaw();
        }

        public static void CtlEndSpaw()
        {
            if (NoControl()) return;

            Main.NewText("结束生成");

            EndSpaw();
        }
        #endregion

        #region 设置位置
        public static void CtlSetSpawPos()
        {
            if (NoControl()) return;

            if (foo(SpawIng, "无法选择, 正在生成")) return;

            SpawPosSelecting = true;
        }

        public static void CtlCancelSetSpawPos()
        {
            if (NoControl()) return;

            SpawPosSelecting = false;
        }
        #endregion

        public static bool NoControl()
        {
            return foo(Loaded == false, "未初始化");
        }

        public static bool foo(bool v, string s)
        {
            if (v) Main.NewText(s);
            return v;
        }
    }
}

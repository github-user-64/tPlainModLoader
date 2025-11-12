using PixelArt.Content.UI;
using PixelArt.Utils.quickBuild;
using System.Collections.Generic;
using tContentPatch;
using Terraria;
using Terraria.UI;

namespace PixelArt.Content
{
    public partial class PixelArt : ModMain
    {
        public override void OnEnterWorld()
        {
            initialize();
        }

        public static List<UIElement> GetUI()
        {
            List<UIElement> uis = new List<UIElement>
            {
                new UIItemTextBoxBindSwitchAction<string>(
                    () => PixelInfoLoading, CtlLoadPixelInfo, CtlCancelLoadPixelInfo,
                    LoadPath, v => v, text: "加载图片数据"){ MouseText = "文件路径<string>" },

                new UIItemSwitchAction(() => SpawIng, CtlStartSpaw, CtlEndSpaw, text: "生成"),

                new UIItemSwitchAction(() => SpawPosSelecting, CtlSetSpawPos, CtlCancelSetSpawPos, text: "生成位置"),

                new UIItemTextBoxBind<int>(SpawSpeed, int.Parse, null, "生成速度"){ MouseText = "<int>" },

                UIBuild.get1(SpawDistance, SpawDistance_val, int.Parse, "<int>", text:"在范围内生成"),

                UIBuild.get2(SetSelectedItem, text:"设置手中物品"),

                new UIItemValueSliderBind<int>(SpawPosSelectV, v => v, v => (int)v, 0, 3, text: "生成方向")
                {
                    FloatToString = v =>
                    {
                        if (v == 0) return "左上";
                        if (v == 1) return "右上";
                        if (v == 2) return "左下";
                        if (v == 3) return "右下";
                        return "-";
                    }
                },
            };

            return uis;
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

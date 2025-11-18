using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OptimizeAndTool.Content.UI;
using OptimizeAndTool.Utils;
using tContentPatch.Content.UI;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;

namespace OptimizeAndTool.Content.ServerList
{
    internal class UIServerItem : UIPanel
    {
        public ServerInfo serverInfo = null;
        public bool isLight = false;
        private GetSetReset<string> ip = new GetSetReset<string>();
        private GetSetReset<int> port = new GetSetReset<int>(func: GetSetReset.GetIntFunc(0, 65535));

        public UIServerItem(ServerInfo si)
        {
            serverInfo = si;

            Width.Precent = 1;
            float pad = 6;
            Height.Pixels = 25 + 25 + pad * 2 + 4;
            SetPadding(pad);

            ip.OnValUpdate += v => si.ip = v;
            port.OnValUpdate += v => si.port = v;

            ip.val = si.ip;
            port.val = si.port;

            UITextBoxBind<string> ui_ip = new UITextBoxBind<string>(ip, v => v, "地址");
            ui_ip.Width.Precent = 1;
            ui_ip.Height.Pixels = 25;
            ui_ip.SetTextScale(0.8f);

            UITextBoxBind<int> ui_port = new UITextBoxBind<int>(port, int.Parse, "端口");
            ui_port.Width.Precent = 0.4f;
            ui_port.Height.Pixels = 25;
            ui_port.VAlign = 1;
            ui_port.SetTextScale(0.8f);

            UIImageButton btnDel = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/ButtonDelete", ReLogic.Content.AssetRequestMode.ImmediateLoad));
            btnDel.OnUpdate += _ =>
            {
                if (btnDel.IsMouseHovering) Main.instance.MouseText("删除");
            };
            btnDel.OnLeftClick += (e, s) => ServerList.DelItem(si);

            UIImageButton btnJoin = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay", ReLogic.Content.AssetRequestMode.ImmediateLoad));
            btnJoin.OnUpdate += _ =>
            {
                if (btnJoin.IsMouseHovering) Main.instance.MouseText($"加入{si.ip}:{si.port}");
            };
            btnJoin.OnLeftClick += (e, s) => ServerList.Join(si);

            UIStackPanel ui_sp = new UIStackPanel();
            ui_sp.Height.Pixels = btnDel.Height.Pixels;
            ui_sp.HAlign = 1;
            ui_sp.VAlign = 1;
            ui_sp.ItemMargin = 8;
            ui_sp.IsAutoUpdateSize = true;
            ui_sp.Horizontal = true;

            OnLeftDoubleClick += (e, s) => ServerList.AddItem(si);

            Append(ui_ip);
            Append(ui_port);
            Append(ui_sp);
            ui_sp.Append(btnDel);
            ui_sp.Append(btnJoin);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            if (isLight)
            {
                isLight = false;
                BorderColor = Colors.FancyUIFatButtonMouseOver;
            }
            else
            {
                BorderColor = new Color(43, 60, 120) * 0.7f;
            }
        }
    }
}

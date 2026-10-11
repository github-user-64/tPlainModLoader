using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace CreativeInventory.CreativeInventory
{
    /// <summary>
    /// 椭圆轨道加圆点的开关, 圆点在右边是开, 在左边是关
    /// </summary>
    public class UISearchSwitch : UIElement
    {
        public Action<bool> OnValUpdate = null;

        protected const int LABEL_W = 40;//文字占的宽度
        protected const int TRACK_W = 40;//轨道宽度
        protected const int TRACK_H = 20;//轨道高度
        protected const int KNOB_D = 14;//圆点直径
        protected const int TEX_D = 32;//生成的圆形贴图的边长

        protected static Texture2D circle_tex = null;
        protected static Texture2D white_tex = null;

        protected bool val = false;
        protected float anim = 0;//0=关, 1=开

        public UISearchSwitch(string text)
        {
            InitTex();

            Width.Set(LABEL_W + TRACK_W, 0);
            Height.Set(TRACK_H, 0);

            UIText ui_text = new UIText(text, 0.6f);
            ui_text.VAlign = 0.5f;
            Append(ui_text);

            OnLeftClick += (e, s) => SetVal(!val);
        }

        public void SetVal(bool v)
        {
            if (v == val) return;

            val = v;
            OnValUpdate?.Invoke(val);
        }

        public bool GetVal() => val;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //每帧走0.2, 5帧滑到位
            float target = val ? 1 : 0;

            if (anim < target) anim = Math.Min(target, anim + 0.2f);
            else if (anim > target) anim = Math.Max(target, anim - 0.2f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dim = GetDimensions();
            float x = dim.X + LABEL_W;
            float y = dim.Y;

            Color color = val ? new Color(60, 140, 220) : new Color(70, 70, 90);
            if (IsMouseHovering) color = Color.Lerp(color, Color.White, 0.2f);

            //轨道: 两端各画一个圆, 中间用纯白贴图拉出矩形
            spriteBatch.Draw(circle_tex, new Rectangle((int)x, (int)y, TRACK_H, TRACK_H), color);
            spriteBatch.Draw(circle_tex, new Rectangle((int)(x + TRACK_W - TRACK_H), (int)y, TRACK_H, TRACK_H), color);
            spriteBatch.Draw(white_tex,
                new Rectangle((int)(x + TRACK_H / 2), (int)y, TRACK_W - TRACK_H, TRACK_H), color);

            //圆点: anim为1时贴右, 为0时贴左
            float left = 3 + (TRACK_W - KNOB_D - 6) * anim;
            spriteBatch.Draw(circle_tex,
                new Rectangle((int)(x + left), (int)(y + (TRACK_H - KNOB_D) / 2), KNOB_D, KNOB_D),
                Color.White);
        }

        protected static void InitTex()
        {
            if (circle_tex != null) return;

            GraphicsDevice gd = Main.instance.GraphicsDevice;

            //都是纯白, 只有alpha不同, 所以不用管RGBA的字节序
            uint white = 0xFFFFFFFF;
            uint edge = 0x80FFFFFF;

            uint[] data = new uint[TEX_D * TEX_D];
            float r = TEX_D / 2f;

            for (int y = 0; y < TEX_D; ++y)
            {
                for (int x = 0; x < TEX_D; ++x)
                {
                    float dx = x + 0.5f - r;
                    float dy = y + 0.5f - r;
                    float dist = (float)Math.Sqrt(dx * dx + dy * dy);

                    if (dist <= r - 1f) data[y * TEX_D + x] = white;
                    else if (dist <= r) data[y * TEX_D + x] = edge;
                }
            }

            circle_tex = new Texture2D(gd, TEX_D, TEX_D);
            circle_tex.SetData(data);

            white_tex = new Texture2D(gd, 1, 1);
            white_tex.SetData(new uint[] { white });
        }
    }
}

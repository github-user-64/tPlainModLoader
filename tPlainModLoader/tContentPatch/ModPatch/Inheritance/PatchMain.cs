using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;

namespace tContentPatch
{
    /// <summary/>
    public abstract class PatchMain
    {
        /// <summary>
        /// <see cref="Mod.Loaded"/>后调用
        /// </summary>
        public virtual void Initialize() { }
        /// <summary>
        /// 进入世界时, 仅在单人和客户端有效
        /// </summary>
        public virtual void OnEnterWorld() { }
        /// <summary>
        /// <see cref="Main.Update(GameTime)"/>前调用
        /// </summary>
        public virtual void UpdatePrefix(GameTime gameTime) { }
        /// <summary>
        /// <see cref="Main.Update(GameTime)"/>后调用
        /// </summary>
        public virtual void UpdatePostfix(GameTime gameTime) { }
        /// <summary>
        /// 用于修改或添加ui
        /// </summary>
        public virtual void SetupDrawInterfaceLayersPostfix(List<GameInterfaceLayer> gameInterfaceLayers) { }
        /// <summary/>
        public virtual void UpdateUIStatesPrefix(GameTime gameTime) { }
        /// <summary/>
        public virtual void UpdateUIStatesPostfix(GameTime gameTime) { }
        /// <summary/>
        public virtual void DoUpdateInWorldPrefix() { }
        /// <summary/>
        public virtual void DoUpdateInWorldPostfix() { }
        /// <summary/>
        public virtual void DrawMapPostfix(GameTime gameTime) { }
        /// <summary/>
        public virtual void DrawMenuPrefix(GameTime gameTime) { }
        /// <summary/>
        public virtual void DrawMenuPostfix(GameTime gameTime) { }
        /// <summary/>
        public virtual void MouseText_DrawItemTooltip_GetLinesInfoPostfix(Item item, ref int yoyoLogo, ref float oldKB, ref int numLines, ref string[] toolTipLine, ref Color[] lineColors) { }
        /// <summary/>
        public virtual void DoDrawPrefix(GameTime gameTime) { }
        /// <summary/>
        public virtual void DoDrawPostfix(GameTime gameTime) { }
        /// <summary>
        /// <paramref name="origin"/>是原本的值, <paramref name="modifi"/>是其它<see cref="PlayerFocusedScreenPosition(Vector2, Vector2)"/>修改过的值
        /// <para/>如果不想影响其它<see cref="PlayerFocusedScreenPosition(Vector2, Vector2)"/>修改值就直接返回<paramref name="modifi"/>
        /// </summary>
        public virtual Vector2 PlayerFocusedScreenPosition(Vector2 origin, Vector2 modifi) => modifi;
    }
}

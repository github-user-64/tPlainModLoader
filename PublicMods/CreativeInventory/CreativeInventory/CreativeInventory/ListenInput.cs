using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using tContentPatch;
using Terraria.GameInput;

namespace CreativeInventory
{
    public class ListenInput : ModMain
    {
        private static List<Action<string>> listenInputOne = new List<Action<string>>();
        private static List<string> listenInput = new List<string>();
        private static List<Action<bool>> listenInput_Dowm = new List<Action<bool>>();
        private static List<bool> listenInput_old = new List<bool>();

        public override void UpdatePrefix(GameTime gameTime)
        {
            Update_ListenInput();
            Update_ListenInputOne();
        }

        private static void Update_ListenInput()
        {
            for (int i = 0; i < listenInput.Count; ++i)
            {
                if (PlayerInput.GetPressedKeys().Exists(v => v.ToString() == listenInput[i]) ||
                    PlayerInput.MouseKeys.Exists(v => v == listenInput[i]))
                {
                    listenInput_Dowm[i]?.Invoke(listenInput_old[i] == false);
                    listenInput_old[i] = true;
                }
                else
                {
                    listenInput_old[i] = false;
                }
            }
        }

        private static void Update_ListenInputOne()
        {
            if (listenInputOne.Count < 1) return;

            string key = null;

            List<Keys> pressedKeys = PlayerInput.GetPressedKeys();
            if (pressedKeys.Count > 0) key = pressedKeys[0].ToString();

            if (key == null)
            {
                if (PlayerInput.MouseKeys.Count > 0) key = PlayerInput.MouseKeys[0];
            }

            if (key == null) return;

            foreach (Action<string> i in listenInputOne)
            {
                i.Invoke(key);
            }

            listenInputOne.Clear();
        }

        public static void AddListenInputOne(Action<string> action)
        {
            listenInputOne.Add(action);
        }

        public static void AddListenInput(string key, Action<bool> action)
        {
            if (key == null) return;

            int index = listenInput.FindIndex(i => i == key);
            if (index == -1)
            {
                listenInput.Add(key);
                listenInput_Dowm.Add(action);
                listenInput_old.Add(false);
            }
            else
            {
                listenInput_Dowm[index] += action;
            }
        }

        public static void DelListenInput(string key, Action<bool> action)
        {
            if (key == null) return;

            int index = listenInput.FindIndex(i => i == key);
            if (index == -1) return;
            listenInput_Dowm[index] -= action;
        }
    }
}

using Microsoft.Xna.Framework;
using System;

namespace PixelArt.Content
{
    public class MapHelper
    {
        private static Color[] _colorLookup = null;
        public static Color[] colorLookup
        {
            get
            {
                if (_colorLookup != null) return _colorLookup;

                Type type = typeof(Terraria.Map.MapHelper);
                if (type == null) return _colorLookup;

                System.Reflection.FieldInfo fieldInfo = type.GetField(nameof(colorLookup), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                if (fieldInfo == null) return _colorLookup;

                object v = fieldInfo.GetValue(null);
                _colorLookup = v as Color[];

                return _colorLookup;
            }
        }
    }
}

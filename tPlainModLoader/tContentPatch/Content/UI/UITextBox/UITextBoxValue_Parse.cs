namespace tContentPatch.Content.UI
{
    /// <summary/>
    public static class UITextBoxValue
    {
        /// <summary/>
        public static string StringToString(this string v) => v;

        /// <summary/>
        public static string IntToString(this int v) => v.ToString();
        /// <summary/>
        public static int StringToInt(this string v)
        {
            int.TryParse(v, out int rv);
            return rv;
        }

        /// <summary/>
        public static string FloatToString(this float v) => v.ToString();
        /// <summary/>
        public static float StringToFloat(this string v)
        {
            float.TryParse(v, out float rv);
            return rv;
        }

        /// <summary/>
        public static string DoubleToString(this double v) => v.ToString();
        /// <summary/>
        public static double StringToDouble(this string v)
        {
            double.TryParse(v, out double rv);
            return rv;
        }
    }
}

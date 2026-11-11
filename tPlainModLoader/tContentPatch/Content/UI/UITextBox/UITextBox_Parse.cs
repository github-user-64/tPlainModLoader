namespace tContentPatch.Content.UI
{
    /// <summary/>
    public partial class UITextBox
    {
        /// <summary/>
        public static string StringToString(string v) => v;

        /// <summary/>
        public static string IntToString(int v) => v.ToString();
        /// <summary/>
        public static int StringToInt(string v)
        {
            int.TryParse(v, out int rv);
            return rv;
        }

        /// <summary/>
        public static string FloatToString(float v) => v.ToString();
        /// <summary/>
        public static float StringToFloat(string v)
        {
            float.TryParse(v, out float rv);
            return rv;
        }

        /// <summary/>
        public static string DoubleToString(double v) => v.ToString();
        /// <summary/>
        public static double StringToDouble(string v)
        {
            double.TryParse(v, out double rv);
            return rv;
        }
    }
}

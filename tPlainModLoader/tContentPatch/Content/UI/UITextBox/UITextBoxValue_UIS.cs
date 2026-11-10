namespace tContentPatch.Content.UI
{
    /// <inheritdoc/>
    public class UITextBoxString : UITextBoxValue<string>
    {
        /// <inheritdoc/>
        public UITextBoxString(string defaultDisplayText = null) : base(defaultDisplayText)
        {
            ValueToText = UITextBoxValue.StringToString;
            TextToValue = UITextBoxValue.StringToString;
        }
    }

    /// <inheritdoc/>
    public class UITextBoxInt : UITextBoxValue<int>
    {
        /// <inheritdoc/>
        public UITextBoxInt(string defaultDisplayText = null) : base(defaultDisplayText)
        {
            ValueToText = UITextBoxValue.IntToString;
            TextToValue = UITextBoxValue.StringToInt;
        }
    }

    /// <inheritdoc/>
    public class UITextBoxFloat : UITextBoxValue<float>
    {
        /// <inheritdoc/>
        public UITextBoxFloat(string defaultDisplayText = null) : base(defaultDisplayText)
        {
            ValueToText = UITextBoxValue.FloatToString;
            TextToValue = UITextBoxValue.StringToFloat;
        }
    }

    /// <inheritdoc/>
    public class UITextBoxDouble : UITextBoxValue<double>
    {
        /// <inheritdoc/>
        public UITextBoxDouble(string defaultDisplayText = null) : base(defaultDisplayText)
        {
            ValueToText = UITextBoxValue.DoubleToString;
            TextToValue = UITextBoxValue.StringToDouble;
        }
    }
}

namespace tContentPatch.Content.UI
{
    /// <inheritdoc/>
    public class UITextBoxString : UITextBox<string>
    {
        /// <inheritdoc/>
        public UITextBoxString(string defaultDisplayText = null) : base(defaultDisplayText)
        {
            ValueToText = UITextBox.StringToString;
            TextToValue = UITextBox.StringToString;
        }
    }

    /// <inheritdoc/>
    public class UITextBoxInt : UITextBox<int>
    {
        /// <inheritdoc/>
        public UITextBoxInt(string defaultDisplayText = null) : base(defaultDisplayText)
        {
            ValueToText = UITextBox.IntToString;
            TextToValue = UITextBox.StringToInt;
        }
    }

    /// <inheritdoc/>
    public class UITextBoxFloat : UITextBox<float>
    {
        /// <inheritdoc/>
        public UITextBoxFloat(string defaultDisplayText = null) : base(defaultDisplayText)
        {
            ValueToText = UITextBox.FloatToString;
            TextToValue = UITextBox.StringToFloat;
        }
    }

    /// <inheritdoc/>
    public class UITextBoxDouble : UITextBox<double>
    {
        /// <inheritdoc/>
        public UITextBoxDouble(string defaultDisplayText = null) : base(defaultDisplayText)
        {
            ValueToText = UITextBox.DoubleToString;
            TextToValue = UITextBox.StringToDouble;
        }
    }
}

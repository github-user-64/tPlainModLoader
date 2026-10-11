namespace AccessoryBox.LoadItem
{
    public class ItemData
    {
        public string key;
        public int type;
        public int prefix;

        public ItemData() { }
        public ItemData(int type, int prefix) : this()
        {
            this.type = type;
            this.prefix = prefix;
        }
        public ItemData(string key, int prefix) : this()
        {
            this.key = key;
            this.prefix = prefix;
        }
    }
}

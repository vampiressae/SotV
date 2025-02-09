namespace Console
{
    [System.Serializable]
    public class ConsoleData
    {
        public string Text;
        public object Initiator;

        public ConsoleRowUI UI {  get; private set; }

        public ConsoleData(string text) => Text = text;
        public ConsoleData(string text, object initiator) : this(text) => Initiator = initiator;

        public void SetUI(ConsoleRowUI ui) => UI = ui;
    }
}
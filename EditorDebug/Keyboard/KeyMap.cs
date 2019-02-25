namespace EditorDebug
{
    using System.Windows.Input;

    public struct KeyMap
    {
        static KeyMap()
        {
            Empty = new KeyMap(Key.None, KeyFlags.None);
        }

        public static KeyMap Empty;

        public KeyMap(Key InitKey)
            : this()
        {
            Key = InitKey;
            Flags = KeyFlags.None;
            KeyText = "";
            SecondaryKey = Key.None;
        }

        public KeyMap(Key InitKey, KeyFlags InitFlags)
            : this()
        {
            Key = InitKey;
            Flags = InitFlags;
            KeyText = "";
            SecondaryKey = Key.None;
        }

        public KeyMap(Key InitKey, KeyFlags InitFlags, string InitText)
            : this()
        {
            Key = InitKey;
            Flags = InitFlags;
            KeyText = InitText;
            SecondaryKey = Key.None;
        }

        public KeyMap(Key InitKey, KeyFlags InitFlags, Key InitSecondaryKey)
            : this()
        {
            Key = InitKey;
            Flags = InitFlags;
            KeyText = "";
            SecondaryKey = InitSecondaryKey;
        }

        public Key Key { get; private set; }
        public KeyFlags Flags { get; private set; }
        public Key SecondaryKey { get; private set; }
        public string KeyText { get; private set; }

        public bool IsEmpty { get { return Key == Key.None; } }

        public bool IsEqual(KeyMap Other)
        {
            return (Key == Other.Key && Flags == Other.Flags && SecondaryKey == Other.SecondaryKey);
        }

        public KeyMap MainKey()
        {
            return new KeyMap(Key, Flags, KeyText);
        }

        public KeyMap WithSecondaryKey(Key CombinedSecondaryKey)
        {
            return new KeyMap(Key, Flags, CombinedSecondaryKey);
        }

        public override string ToString()
        {
            return (Flags.HasFlag(KeyFlags.Ctrl) ? "Ctrl-" : "") + (Flags.HasFlag(KeyFlags.Shift) ? "Shift-" : "") + Key.ToString() + (SecondaryKey != Key.None ? ", " + SecondaryKey.ToString() : "");
        }
    }
}

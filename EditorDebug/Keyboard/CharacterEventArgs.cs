using System.Windows;

namespace EditorDebug
{
    public class CharacterEventArgs : RoutedEventArgs
    {
        public CharacterEventArgs(int code)
            : base()
        {
            Code = code;
        }

        public CharacterEventArgs(int code, RoutedEvent routedEvent)
            : base(routedEvent)
        {
            Code = code;
        }

        public CharacterEventArgs(int code, RoutedEvent routedEvent, object source)
            : base(routedEvent)
        {
            Code = code;
        }

        public int Code { get; }
    }
}

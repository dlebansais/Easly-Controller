using System.Windows;

namespace EditorDebug
{
    public class MoveEventArgs : RoutedEventArgs
    {
        public MoveEventArgs(MoveDirections direction, bool isShift, bool isCtrl)
            : base()
        {
            Direction = direction;
            IsShift = isShift;
            IsCtrl = isCtrl;
        }

        public MoveEventArgs(MoveDirections direction, bool isShift, bool isCtrl, RoutedEvent routedEvent)
            : base(routedEvent)
        {
            Direction = direction;
            IsShift = isShift;
            IsCtrl = isCtrl;
        }

        public MoveEventArgs(MoveDirections direction, bool isShift, bool isCtrl, RoutedEvent routedEvent, object source)
            : base(routedEvent)
        {
            Direction = direction;
            IsShift = isShift;
            IsCtrl = isCtrl;
        }

        public MoveDirections Direction { get; }
        public bool IsShift { get; }
        public bool IsCtrl { get; }
    }
}

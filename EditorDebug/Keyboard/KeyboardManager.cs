using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace EditorDebug
{
    public class KeyboardManager
    {
        public KeyboardManager(UIElement control)
        {
            Keyboard.AddPreviewKeyDownHandler(control, OnPreviewKeyDown);
            Keyboard.AddKeyDownHandler(control, OnKeyDown);
            Keyboard.AddPreviewKeyUpHandler(control, OnPreviewKeyUp);
            Keyboard.AddKeyUpHandler(control, OnKeyUp);
        }

        public event RoutedEventHandler KeyBackEvent;

        protected bool NotifyKeyBack(RoutedEvent sourceEvent)
        {
            RoutedEventArgs Args = new RoutedEventArgs(sourceEvent);
            KeyBackEvent?.Invoke(this, Args);

            return Args.Handled;
        }

        public event RoutedEventHandler KeyInsertEvent;

        protected bool NotifyKeyInsert(RoutedEvent sourceEvent)
        {
            RoutedEventArgs Args = new RoutedEventArgs(sourceEvent);
            KeyInsertEvent?.Invoke(this, Args);

            return Args.Handled;
        }

        public event RoutedEventHandler KeyDeleteEvent;

        protected bool NotifyKeyDelete(RoutedEvent sourceEvent)
        {
            RoutedEventArgs Args = new RoutedEventArgs(sourceEvent);
            KeyDeleteEvent?.Invoke(this, Args);

            return Args.Handled;
        }

        public event RoutedEventHandler KeyTabEvent;

        protected bool NotifyKeyTab(RoutedEvent sourceEvent)
        {
            RoutedEventArgs Args = new RoutedEventArgs(sourceEvent);
            KeyTabEvent?.Invoke(this, Args);

            return Args.Handled;
        }

        public event RoutedEventHandler KeyEnterEvent;

        protected bool NotifyKeyEnter(RoutedEvent sourceEvent)
        {
            RoutedEventArgs Args = new RoutedEventArgs(sourceEvent);
            KeyEnterEvent?.Invoke(this, Args);

            return Args.Handled;
        }

        public event MoveEventHandler KeyMoveEvent;

        protected bool NotifyKeyMove(MoveDirections direction, bool isShift, bool isCtrl, RoutedEvent sourceEvent)
        {
            MoveEventArgs Args = new MoveEventArgs(direction, isShift, isCtrl, sourceEvent);
            KeyMoveEvent?.Invoke(this, Args);

            return Args.Handled;
        }

        public event CharacterEventHandler KeyCharacterEvent;

        protected bool NotifyKeyCharacter(int code, RoutedEvent sourceEvent)
        {
            CharacterEventArgs Args = new CharacterEventArgs(code, sourceEvent);
            KeyCharacterEvent?.Invoke(this, Args);

            return Args.Handled;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine($"OnPreviewKeyDown: {e.Key}, {e.ImeProcessedKey}, {e.SystemKey}, {e.DeadCharProcessedKey}, States: {e.KeyStates}");
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine($"OnKeyDown: {e.Key}");

            RoutedEvent SourceEvent = e.RoutedEvent;
            KeyMap PressedKey = GetCurrentKey(e);
            bool IsHandled = true;

            switch (PressedKey.Key)
            {
                case Key.LeftCtrl:
                case Key.RightCtrl:
                case Key.LeftShift:
                case Key.RightShift:
                case Key.LeftAlt:
                case Key.RightAlt:
                    IsHandled = false;
                    break;

                case Key.Escape:
                    break;

                case Key.Back:
                    IsHandled = NotifyKeyBack(SourceEvent);
                    break;

                case Key.Delete:
                    IsHandled = NotifyKeyDelete(SourceEvent);
                    break;

                case Key.Insert:
                    if (PressedKey.Flags == KeyFlags.None)
                        IsHandled = NotifyKeyInsert(SourceEvent);
                    else
                        IsHandled = false;
                    break;

                case Key.Tab:
                    IsHandled = NotifyKeyTab(SourceEvent);
                    break;

                case Key.Enter:
                    IsHandled = NotifyKeyEnter(SourceEvent);
                    break;

                case Key.Left:
                case Key.Right:
                case Key.Up:
                case Key.Down:
                case Key.PageUp:
                case Key.PageDown:
                case Key.Home:
                case Key.End:
                    IsHandled = HandleKeyMove(PressedKey, SourceEvent);
                    break;

                default:
                    if (!PressedKey.Flags.HasFlag(KeyFlags.Ctrl) && PreviousKey.IsEmpty && !string.IsNullOrEmpty(PressedKey.KeyText))
                    {
                        byte[] Bytes = System.Text.Encoding.UTF32.GetBytes(PressedKey.KeyText);
                        int CharacterCode = BitConverter.ToInt32(Bytes, 0);
                        IsHandled = NotifyKeyCharacter(CharacterCode, SourceEvent);
                    }
                    break;
            }

            if (IsHandled)
                e.Handled = true;
        }

        private void OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            Debug.WriteLine($"OnPreviewKeyUp: {e.Key}");
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            Debug.WriteLine($"OnKeyUp: {e.Key}, {e.ImeProcessedKey}, {e.SystemKey}, {e.DeadCharProcessedKey}, States: {e.KeyStates}");

            if (e.Key == Key.System && Keyboard.IsKeyDown(Key.LeftAlt) && NumPadText != null)
            {
                Key Key = e.SystemKey;
                if (Key >= Key.NumPad0 && Key <= Key.NumPad9)
                {
                    NumPadText += (char)((int)Key - (int)Key.NumPad0 + '0');
                    Debug.WriteLine($"Recording numpad: {NumPadText}");
                }
                else
                {
                    NumPadText = null;
                    Debug.WriteLine("Recording numpad stopped");
                }
            }

            else if (e.Key == Key.LeftAlt && !e.KeyStates.HasFlag(KeyStates.Down))
            {
                if (!string.IsNullOrEmpty(NumPadText) && int.TryParse(NumPadText, out int CharacterCode))
                    NotifyKeyCharacter(CharacterCode, e.RoutedEvent);
            }
        }

        private bool HandleKeyMove(KeyMap pressedKey, RoutedEvent sourceEvent)
        {
            MoveDirections Direction = (MoveDirections)(-1);
            bool IsValid = false;

            switch (pressedKey.Key)
            {
                case Key.Left:
                    Direction = MoveDirections.Left;
                    IsValid = true;
                    break;

                case Key.Right:
                    Direction = MoveDirections.Right;
                    IsValid = true;
                    break;

                case Key.Up:
                    Direction = MoveDirections.Up;
                    IsValid = true;
                    break;

                case Key.Down:
                    Direction = MoveDirections.Down;
                    IsValid = true;
                    break;

                case Key.PageUp:
                    Direction = MoveDirections.PageUp;
                    IsValid = true;
                    break;

                case Key.PageDown:
                    Direction = MoveDirections.PageDown;
                    IsValid = true;
                    break;

                case Key.Home:
                    Direction = MoveDirections.Home;
                    IsValid = true;
                    break;

                case Key.End:
                    Direction = MoveDirections.End;
                    IsValid = true;
                    break;
            }

            Debug.Assert(IsValid);

            return NotifyKeyMove(Direction, pressedKey.Flags.HasFlag(KeyFlags.Shift), pressedKey.Flags.HasFlag(KeyFlags.Ctrl), sourceEvent);
        }

        public KeyMap GetCurrentKey(KeyEventArgs e)
        {
            Key Key = e.Key;

            if (Key == Key.LeftCtrl || Key == Key.RightShift || Key == Key.LeftShift || Key == Key.RightShift)
                return new KeyMap(e.Key);

            if (Key == Key.System)
                Key = e.SystemKey;

            bool IsCtrlDown = (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && !(Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt));
            bool IsShiftDown = (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift));

            if (PreviousKey.IsEmpty)
            {
                if (Key == Key.LeftAlt)
                {
                    if (e.KeyStates.HasFlag(KeyStates.Down))
                    {
                        Debug.WriteLine("Recording numpad started");
                        NumPadText = "";
                        return KeyMap.Empty;
                    }
                    else if (!string.IsNullOrEmpty(NumPadText) && int.TryParse(NumPadText, out int CharacterCode))
                    {
                        System.Text.Encoding Unicode = System.Text.Encoding.Unicode;
                        return KeyMap.Empty;
                    }
                    else
                    {
                        Debug.WriteLine("Recording numpad stopped");
                        NumPadText = null;
                    }
                }

                KeyFlags Flags = KeyFlags.None;

                if (IsCtrlDown && IsShiftDown)
                    Flags = KeyFlags.Ctrl | KeyFlags.Shift;

                else if (!IsCtrlDown && IsShiftDown)
                    Flags = KeyFlags.Shift;

                else if (IsCtrlDown && !IsShiftDown)
                    Flags = KeyFlags.Ctrl;

                string KeyText;
                string Text;
                if (KeyboardHelper.TryParseKey(e, out Text))
                    KeyText = Text;
                else
                    KeyText = "";

                if (Key == Key.OemPlus)
                    Key = Key.Add;

                if (Key == Key.OemMinus)
                    Key = Key.Subtract;

                KeyMap PressedKey = new KeyMap(Key, Flags, KeyText);

                IReadOnlyCollection<KeyMap> SequenceKeyList = SequenceKeys;
                foreach (KeyMap SequenceKey in SequenceKeyList)
                    if (SequenceKey.IsEqual(PressedKey))
                    {
                        PreviousKey = PressedKey;
                        return KeyMap.Empty;
                    }

                return PressedKey;
            }
            else
            {
                KeyMap PressedKey;

                if (!IsCtrlDown && !IsShiftDown)
                    PressedKey = PreviousKey.WithSecondaryKey(Key);

                else
                    PressedKey = KeyMap.Empty;

                return PressedKey;
            }
        }

        private KeyMap PreviousKey = KeyMap.Empty;
        public List<KeyMap> SequenceKeys { get; set; } = new List<KeyMap>();
        private string NumPadText;
    }
}

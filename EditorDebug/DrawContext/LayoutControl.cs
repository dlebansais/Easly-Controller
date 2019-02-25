namespace EditorDebug
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Media;
    using EaslyController.Constants;
    using EaslyController.Layout;
    using EaslyDraw;
    using TestDebug;

    public class LayoutControl : FrameworkElement
    {
        public LayoutControl()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            KeyboardManager = new KeyboardManager(this);
            KeyboardManager.KeyCharacterEvent += OnKeyCharacter;
            KeyboardManager.KeyMoveEvent += OnKeyMove;
        }

        private void OnKeyCharacter(object sender, CharacterEventArgs e)
        {
            if (ControllerView != null)
            {
                if (ControllerView.Focus is ILayoutTextFocus AsTextFocus)
                {
                    string OldText = ControllerView.FocusedText;

                    char InsertedCharacter = Convert.ToChar(e.Code);

                    string NewText;
                    if (ControllerView.CaretMode == CaretModes.Insertion)
                    {
                        System.Text.UTF32Encoding UTF32 = System.Text.Encoding.UTF32 as System.Text.UTF32Encoding;
                        byte[] FirstBytes = UTF32.GetBytes(OldText.Substring(0, ControllerView.CaretPosition));
                        byte[] InsertedBytes = UTF32.GetBytes(InsertedCharacter.ToString());
                        byte[] LastBytes = UTF32.GetBytes(OldText.Substring(ControllerView.CaretPosition));

                        byte[] Bytes = new byte[FirstBytes.Length + InsertedBytes.Length + LastBytes.Length];
                        Array.Copy(FirstBytes, 0, Bytes, 0, FirstBytes.Length);
                        Array.Copy(InsertedBytes, 0, Bytes, FirstBytes.Length, InsertedBytes.Length);
                        Array.Copy(LastBytes, 0, Bytes, FirstBytes.Length + InsertedBytes.Length, LastBytes.Length);

                        NewText = UTF32.GetString(Bytes);
                    }
                    else
                    {
                        //NewText = OldText.Substring(0, ControllerView.CaretPosition) + InsertedText + OldText.Substring(ControllerView.CaretPosition + 1);
                        NewText = "";
                    }

                    Controller.ChangeText(AsTextFocus.CellView.StateView.State.ParentIndex, AsTextFocus.CellView.PropertyName, NewText);
                    ControllerView.SetCaretPosition(ControllerView.CaretPosition + 1, out bool IsMoved);

                    Debug.WriteLine($"Inserting: old text: '{OldText}', new text: '{NewText}'");

                    InvalidateMeasure();
                    InvalidateVisual();
                }
            }
        }

        private void OnKeyMove(object sender, MoveEventArgs e)
        {
            bool IsHandled = false;

            switch (e.Direction)
            {
                case MoveDirections.Left:
                    MoveCaretLeft();
                    IsHandled = true;
                    break;

                case MoveDirections.Right:
                    MoveCaretRight();
                    IsHandled = true;
                    break;

                case MoveDirections.Up:
                    MoveFocusVertically(-1);
                    IsHandled = true;
                    break;

                case MoveDirections.Down:
                    MoveFocusVertically(+1);
                    IsHandled = true;
                    break;

                case MoveDirections.Home:
                case MoveDirections.End:
                case MoveDirections.PageUp:
                case MoveDirections.PageDown:
                    break;
            }

            Debug.Assert(IsHandled);
            e.Handled = IsHandled;
        }

        private void MoveCaretLeft()
        {
            bool IsMoved;

            if (ControllerView.CaretPosition > 0)
                ControllerView.SetCaretPosition(ControllerView.CaretPosition - 1, out IsMoved);
            else
                ControllerView.MoveFocus(-1, out IsMoved);

            if (IsMoved)
            {
                if (ControllerView.IsInvalidated)
                    InvalidateMeasure();

                InvalidateVisual();
            }
        }

        private void MoveCaretRight()
        {
            bool IsMoved;

            if (ControllerView.CaretPosition < ControllerView.MaxCaretPosition)
                ControllerView.SetCaretPosition(ControllerView.CaretPosition + 1, out IsMoved);
            else
                ControllerView.MoveFocus(+1, out IsMoved);

            if (IsMoved)
            {
                if (ControllerView.IsInvalidated)
                    InvalidateMeasure();

                InvalidateVisual();
            }
        }

        private void MoveFocusVertically(int direction)
        {
            if (ControllerView == null)
                return;

            ControllerView.MoveFocusVertically(ControllerView.DrawContext.LineHeight * direction, out bool IsMoved);
            if (IsMoved)
                InvalidateVisual();
        }

        public void SetController(ILayoutController controller)
        {
            Controller = controller;

            DrawingVisual = new DrawingVisual();
            DrawingContext = DrawingVisual.RenderOpen();
            DrawContext = new DrawContext();

            ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, DrawContext);
            ControllerView.SetCommentDisplayMode(EaslyController.Constants.CommentDisplayModes.OnFocus);
            ControllerView.SetShowUnfocusedComments(show: true);
            ControllerView.MeasureAndArrange();
            ControllerView.ShowCaret(true, draw: false);

            DrawingContext.Close();

            InvalidateMeasure();
            InvalidateVisual();
        }

        private KeyboardManager KeyboardManager;

        public void OnActivated()
        {
            if (ControllerView != null)
            {
                ControllerView.ShowCaret(true, draw: false);
                InvalidateVisual();
            }
        }

        public void OnDeactivated()
        {
            if (ControllerView != null)
            {
                ControllerView.ShowCaret(false, draw: true);
            }
        }

        private ILayoutController Controller;
        private DrawingVisual DrawingVisual;
        private DrawingContext DrawingContext;
        private DrawContext DrawContext;

        public ILayoutControllerView ControllerView { get; private set; }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (ControllerView != null)
                return new Size(ControllerView.ViewSize.Width, ControllerView.ViewSize.Height);
            else
                return base.MeasureOverride(availableSize);
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (ControllerView != null)
            {
                DrawContext.SetWpfDrawingContext(dc);

                Brush WriteBrush = Brushes.White;
                Rect Fullrect = new Rect(0, 0, ActualWidth, ActualHeight);
                dc.DrawRectangle(WriteBrush, null, Fullrect);

                ControllerView.Draw();
            }
        }
    }
}

namespace EditorDebug
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Media;
    using EaslyController.Constants;
    using EaslyController.Focus;
    using EaslyController.Layout;
    using EaslyController.Writeable;
    using EaslyDraw;
    using KeyboardManager;
    using TestDebug;

    public class LayoutControl : FrameworkElement
    {
        public void SetController(ILayoutController controller)
        {
            Controller = controller;

            DrawingVisual = new DrawingVisual();
            DrawingContext = DrawingVisual.RenderOpen();
            DrawContext = new DrawContext();

            ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, DrawContext);
            ControllerView.SetCommentDisplayMode(CommentDisplayModes.OnFocus);
            ControllerView.SetShowUnfocusedComments(show: true);
            ControllerView.MeasureAndArrange();
            ControllerView.ShowCaret(true, draw: false);

            DrawingContext.Close();

            InvalidateMeasure();
            InvalidateVisual();

            KeyboardManager = new Manager(this);
            KeyboardManager.KeyCharacterEvent += OnKeyCharacter;
            KeyboardManager.KeyMoveEvent += OnKeyMove;
            KeyboardManager.KeyInsertEvent += ToggleCaretMode;
            KeyboardManager.KeyDeleteEvent += DeleteCharacterInPlace;
            KeyboardManager.KeyBackEvent += DeleteCharacterBackward;
            KeyboardManager.KeyTabEvent += ForceShowComment;
            KeyboardManager.KeyEnterEvent += InsertNewItem;
        }

        private void OnKeyCharacter(object sender, CharacterEventArgs e)
        {
            if (ControllerView.Focus is ILayoutTextFocus AsTextFocus)
            {
                string FocusedText = ControllerView.FocusedText;
                int CaretPosition = ControllerView.CaretPosition;

                if (ControllerView.CaretMode == CaretModes.Insertion)
                    StringHelper.InsertCharacter(e.Code, ref FocusedText, ref CaretPosition);
                else
                    StringHelper.ReplaceCharacter(e.Code, ref FocusedText, ref CaretPosition);

                if (ControllerView.Focus is ILayoutStringContentFocus AsStringContentFocus)
                    Controller.ChangeText(AsTextFocus.CellView.StateView.State.ParentIndex, AsStringContentFocus.CellView.PropertyName, FocusedText);
                else if (ControllerView.Focus is ILayoutCommentFocus AsCommentFocus)
                    Controller.ChangeComment(AsCommentFocus.CellView.StateView.State.ParentIndex, FocusedText);

                ControllerView.SetCaretPosition(CaretPosition, out bool IsMoved);

                InvalidateMeasure();
                InvalidateVisual();
            }

            e.Handled = true;
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
                    MoveFocusHorizontally(-1);
                    IsHandled = true;
                    break;

                case MoveDirections.End:
                    MoveFocusHorizontally(+1);
                    IsHandled = true;
                    break;

                case MoveDirections.PageUp:
                    MoveFocusVertically(-1 * GetVerticalPageMove());
                    IsHandled = true;
                    break;

                case MoveDirections.PageDown:
                    MoveFocusVertically(+1 * GetVerticalPageMove());
                    IsHandled = true;
                    break;
            }

            Debug.Assert(IsHandled);
            e.Handled = IsHandled;
        }

        private int GetVerticalPageMove()
        {
            if (ActualHeight >= DrawContext.LineHeight * 3)
                return (int)((ActualHeight - DrawContext.LineHeight * 2) / DrawContext.LineHeight);
            else
                return 1;
        }

        private void ToggleCaretMode(object sender, RoutedEventArgs e)
        {
            bool IsChanged;

            CaretModes OldMode = ControllerView.CaretMode;
            CaretModes NewMode = OldMode == CaretModes.Insertion ? CaretModes.Override : CaretModes.Insertion;

            ControllerView.SetCaretMode(NewMode, out IsChanged);
            Debug.Assert(IsChanged || (NewMode == CaretModes.Override && ControllerView.CaretPosition >= ControllerView.MaxCaretPosition));

            if (IsChanged)
                InvalidateVisual();

            e.Handled = true;
        }

        private void DeleteCharacterInPlace(object sender, RoutedEventArgs e)
        {
            DeleteCharacter(backward: false);

            e.Handled = true;
        }

        private void DeleteCharacterBackward(object sender, RoutedEventArgs e)
        {
            DeleteCharacter(backward: true);

            e.Handled = true;
        }

        private void DeleteCharacter(bool backward)
        {
            if (ControllerView.Focus is ILayoutTextFocus AsTextFocus)
            {
                string FocusedText = ControllerView.FocusedText;
                int CaretPosition = ControllerView.CaretPosition;
                int MaxCaretPosition = ControllerView.MaxCaretPosition;

                if (StringHelper.DeleteCharacter(backward, ref FocusedText, ref CaretPosition))
                {
                    ControllerView.SetCaretPosition(CaretPosition, out bool IsMoved);

                    if (ControllerView.Focus is ILayoutStringContentFocus AsStringContentFocus)
                        Controller.ChangeText(AsTextFocus.CellView.StateView.State.ParentIndex, AsStringContentFocus.CellView.PropertyName, FocusedText);
                    else if (ControllerView.Focus is ILayoutCommentFocus AsCommentFocus)
                        Controller.ChangeComment(AsTextFocus.CellView.StateView.State.ParentIndex, FocusedText);

                    InvalidateMeasure();
                    InvalidateVisual();
                }
            }
        }

        private void ForceShowComment(object sender, RoutedEventArgs e)
        {
            bool IsMoved;

            ControllerView.ForceShowComment(out IsMoved);

            if (IsMoved)
                InvalidateVisual();

            e.Handled = true;
        }

        private void InsertNewItem(object sender, RoutedEventArgs e)
        {
            if (!ControllerView.IsNewItemInsertable(out IFocusCollectionInner inner, out IFocusInsertionCollectionNodeIndex index))
                return;

            ControllerView.Controller.Insert(inner, index, out IWriteableBrowsingCollectionNodeIndex nodeIndex);
            InvalidateVisual();

            e.Handled = true;
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
            ControllerView.MoveFocusVertically(ControllerView.DrawContext.LineHeight * direction, out bool IsMoved);
            if (IsMoved)
                InvalidateVisual();
        }

        private void MoveFocusHorizontally(int direction)
        {
            ControllerView.MoveFocusHorizontally(direction, out bool IsMoved);
            if (IsMoved)
                InvalidateVisual();
        }

        private Manager KeyboardManager;

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

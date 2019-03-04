namespace EditorDebug
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using BaseNode;
    using EaslyController.Constants;
    using EaslyController.Focus;
    using EaslyController.Layout;
    using EaslyController.Writeable;
    using EaslyDraw;
    using KeyboardHelper;
    using TestDebug;

    public class LayoutControl : FrameworkElement
    {
        public void SetController(ILayoutController controller)
        {
            Controller = controller;

            DrawingVisual = new DrawingVisual();
            DrawingContext = DrawingVisual.RenderOpen();
            DrawContext = DrawContext.CreateDrawContext();

            ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, DrawContext);
            ControllerView.SetCommentDisplayMode(CommentDisplayModes.OnFocus);
            ControllerView.SetShowUnfocusedComments(show: true);
            ControllerView.MeasureAndArrange();
            ControllerView.ShowCaret(true, draw: false);

            DrawingContext.Close();

            InvalidateMeasure();
            InvalidateVisual();

            KeyboardManager = new KeyboardManager(this);
            KeyboardManager.CharacterKey += OnKeyCharacter;
            KeyboardManager.MoveKey += OnKeyMove;
        }

        private void OnKeyCharacter(object sender, CharacterKeyEventArgs e)
        {
            if (ControllerView.Focus is ILayoutTextFocus AsTextFocus)
            {
                if (e.Code == '.' && (e.Key == Key.Decimal || e.Key == Key.OemPeriod))
                    SplitIdentifier();
                else
                    ChangeText(AsTextFocus, e.Code);
            }
            else if (ControllerView.Focus is ILayoutDiscreteContentFocus AsDiscreteContentFocus)
                ChangeDiscreteValue(AsDiscreteContentFocus, e.Code, e.Key);

            e.Handled = true;
        }

        private void ChangeText(ILayoutTextFocus focus, int code)
        {
            string FocusedText = ControllerView.FocusedText;
            int CaretPosition = ControllerView.CaretPosition;

            if (ControllerView.ActualCaretMode == CaretModes.Insertion)
                StringHelper.InsertCharacter(code, ref FocusedText, ref CaretPosition);
            else
                StringHelper.ReplaceCharacter(code, ref FocusedText, ref CaretPosition);

            ControllerView.ChangedFocusedText(FocusedText, CaretPosition, changeCaretBeforeText: false);

            InvalidateVisual();
        }

        private void ChangeDiscreteValue(ILayoutDiscreteContentFocus focus, int code, Key key)
        {
            int Change = 0;

            if (code == '+' && (key == Key.OemPlus || key == Key.Add))
                Change = +1;
            else if (code == '-' && (key == Key.OemMinus || key == Key.Subtract))
                Change = -1;

            if (Change != 0)
            {
                ILayoutDiscreteContentFocusableCellView CellView = focus.CellView;
                ILayoutIndex Index = CellView.StateView.State.ParentIndex;
                string PropertyName = CellView.PropertyName;

                int OldValue = Controller.GetDiscreteValue(Index, PropertyName, out int Min, out int Max);

                int NewValue = OldValue + Change;
                if (NewValue < Min)
                    NewValue = Max;
                else if (NewValue > Max)
                    NewValue = Min;

                Controller.ChangeDiscreteValue(Index, PropertyName, NewValue);

                InvalidateVisual();
            }
        }

        private void SplitIdentifier()
        {
            if (ControllerView.IsIdentifierSplittable(out IFocusListInner Inner, out IFocusInsertionListNodeIndex ReplaceIndex, out IFocusInsertionListNodeIndex InsertIndex))
            {
                Controller.SplitIdentifier(Inner, ReplaceIndex, InsertIndex, out IWriteableBrowsingListNodeIndex FirstIndex, out IWriteableBrowsingListNodeIndex SecondIndex);

                InvalidateVisual();
            }
        }

        private void OnKeyMove(object sender, MoveKeyEventArgs e)
        {
            bool IsHandled = false;
            bool ResetAnchor = !e.IsShift;

            switch (e.Direction)
            {
                case MoveDirections.PageUp:
                    MoveFocusVertically(-1 * GetVerticalPageMove(), ResetAnchor);
                    IsHandled = true;
                    break;

                case MoveDirections.PageDown:
                    MoveFocusVertically(+1 * GetVerticalPageMove(), ResetAnchor);
                    IsHandled = true;
                    break;

                default:
                    if (e.IsCtrl)
                        IsHandled = OnKeyMoveCtrl(e.Direction, ResetAnchor, e.IsAlt);
                    else
                        IsHandled = OnKeyMove(e.Direction, ResetAnchor);
                    break;
            }

            Debug.Assert(IsHandled);
            e.Handled = IsHandled;
        }

        private bool OnKeyMove(MoveDirections direction, bool resetAnchor)
        {
            bool IsHandled = false;

            switch (direction)
            {
                case MoveDirections.Left:
                    MoveCaretLeft(resetAnchor);
                    IsHandled = true;
                    break;

                case MoveDirections.Right:
                    MoveCaretRight(resetAnchor);
                    IsHandled = true;
                    break;

                case MoveDirections.Up:
                    MoveFocusVertically(-1, resetAnchor);
                    IsHandled = true;
                    break;

                case MoveDirections.Down:
                    MoveFocusVertically(+1, resetAnchor);
                    IsHandled = true;
                    break;

                case MoveDirections.Home:
                    MoveFocusHorizontally(-1, resetAnchor);
                    IsHandled = true;
                    break;

                case MoveDirections.End:
                    MoveFocusHorizontally(+1, resetAnchor);
                    IsHandled = true;
                    break;
            }

            Debug.Assert(IsHandled);
            return IsHandled;
        }

        private bool OnKeyMoveCtrl(MoveDirections direction, bool resetAnchor, bool isAlt)
        {
            bool IsHandled = false;
            bool IsMoved;

            switch (direction)
            {
                case MoveDirections.Left:
                    if (!resetAnchor && ControllerView.CaretPosition > 0)
                    {
                        ControllerView.SetCaretPosition(0, resetAnchor, out IsMoved);
                        InvalidateVisual();
                    }
                    else
                        MoveFocus(-1, resetAnchor, out IsMoved);

                    IsHandled = true;
                    break;

                case MoveDirections.Right:
                    if (!resetAnchor && ControllerView.CaretPosition < ControllerView.MaxCaretPosition)
                    {
                        ControllerView.SetCaretPosition(ControllerView.MaxCaretPosition, resetAnchor, out IsMoved);
                        InvalidateVisual();
                    }
                    else
                        MoveFocus(+1, resetAnchor, out IsMoved);
                    IsHandled = true;
                    break;

                case MoveDirections.Up:
                    if (isAlt)
                        MoveExistingBlock(-1);
                    else
                        MoveExistingItem(-1);
                    IsHandled = true;
                    break;

                case MoveDirections.Down:
                    if (isAlt)
                        MoveExistingBlock(+1);
                    else
                        MoveExistingItem(+1);
                    IsHandled = true;
                    break;

                case MoveDirections.Home:
                    MoveFocus(ControllerView.MinFocusMove, resetAnchor, out IsMoved);
                    if (ControllerView.CaretPosition > 0)
                    {
                        if (IsMoved)
                            resetAnchor = true;

                        ControllerView.SetCaretPosition(0, resetAnchor, out IsMoved);
                        if (IsMoved)
                            InvalidateVisual();
                    }
                    IsHandled = true;
                    break;

                case MoveDirections.End:
                    MoveFocus(ControllerView.MaxFocusMove, resetAnchor, out IsMoved);
                    if (ControllerView.CaretPosition < ControllerView.MaxCaretPosition)
                    {
                        if (IsMoved)
                            resetAnchor = true;

                        ControllerView.SetCaretPosition(ControllerView.MaxCaretPosition, resetAnchor, out IsMoved);
                        if (IsMoved)
                            InvalidateVisual();
                    }
                    IsHandled = true;
                    break;
            }

            Debug.Assert(IsHandled);
            return IsHandled;
        }

        private int GetVerticalPageMove()
        {
            DependencyObject Control = this;
            int Result = 1;

            do
            {
                Control = VisualTreeHelper.GetParent(Control);
            }
            while (Control != null && !(Control is ScrollViewer));

            if (Control is ScrollViewer ParentViewer)
            {
                double Height = ParentViewer.ActualHeight;

                if (Height >= DrawContext.LineHeight.Draw * 3)
                    Result = (int)((Height - DrawContext.LineHeight.Draw * 2) / DrawContext.LineHeight.Draw);
            }

            return Result;
        }

        private void MoveCaretLeft(bool resetAnchor)
        {
            bool IsMoved;

            if (ControllerView.CaretPosition > 0)
                ControllerView.SetCaretPosition(ControllerView.CaretPosition - 1, resetAnchor, out IsMoved);
            else
                ControllerView.MoveFocus(-1, resetAnchor, out IsMoved);

            if (IsMoved)
            {
                if (ControllerView.IsInvalidated)
                    InvalidateMeasure();

                InvalidateVisual();
            }
        }

        private void MoveCaretRight(bool resetAnchor)
        {
            bool IsMoved;

            if (ControllerView.CaretPosition < ControllerView.MaxCaretPosition)
                ControllerView.SetCaretPosition(ControllerView.CaretPosition + 1, resetAnchor, out IsMoved);
            else
                ControllerView.MoveFocus(+1, resetAnchor, out IsMoved);

            if (IsMoved)
            {
                if (ControllerView.IsInvalidated)
                    InvalidateMeasure();

                InvalidateVisual();
            }
        }

        private void MoveFocusVertically(int direction, bool resetAnchor)
        {
            ControllerView.MoveFocusVertically(ControllerView.DrawContext.LineHeight.Draw * direction, resetAnchor, out bool IsMoved);
            if (IsMoved)
                InvalidateVisual();
        }

        private void MoveFocusHorizontally(int direction, bool resetAnchor)
        {
            ControllerView.MoveFocusHorizontally(direction, resetAnchor, out bool IsMoved);
            if (IsMoved)
                InvalidateVisual();
        }

        private void MoveFocus(int direction, bool resetAnchor, out bool isMoved)
        {
            isMoved = false;

            if (ControllerView.MinFocusMove <= direction && direction <= ControllerView.MaxFocusMove)
                ControllerView.MoveFocus(direction, resetAnchor, out isMoved);

            if (!isMoved)
            {
                if (direction < 0 && ControllerView.CaretPosition > 0)
                    ControllerView.SetCaretPosition(0, resetAnchor, out isMoved);
                else if (direction > 0 && ControllerView.CaretPosition < ControllerView.MaxCaretPosition)
                    ControllerView.SetCaretPosition(ControllerView.MaxCaretPosition, resetAnchor, out isMoved);
                else
                    isMoved = false;
            }

            if (isMoved)
                InvalidateVisual();
        }

        private void MoveExistingBlock(int direction)
        {
            if (!ControllerView.IsBlockMoveable(direction, out IFocusBlockListInner inner, out int blockIndex))
                return;

            Controller.MoveBlock(inner, blockIndex, direction);

            InvalidateVisual();
        }

        private void MoveExistingItem(int direction)
        {
            if (!ControllerView.IsItemMoveable(direction, out IFocusCollectionInner inner, out IFocusBrowsingCollectionNodeIndex index))
                return;

            Controller.Move(inner, index, direction);

            InvalidateVisual();
        }

        public void OnToggleInsert(object sender, ExecutedRoutedEventArgs e)
        {
            bool IsChanged;

            CaretModes OldMode = ControllerView.CaretMode;
            CaretModes NewMode = OldMode == CaretModes.Insertion ? CaretModes.Override : CaretModes.Insertion;

            ControllerView.SetCaretMode(NewMode, out IsChanged);
            Debug.Assert(IsChanged);

            if (IsChanged)
                InvalidateVisual();

            e.Handled = true;
        }

        public void OnDelete(object sender, ExecutedRoutedEventArgs e)
        {
            DeleteCharacter(backward: false);
            e.Handled = true;
        }

        public void OnBackspace(object sender, ExecutedRoutedEventArgs e)
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
                    ControllerView.ChangedFocusedText(FocusedText, CaretPosition, changeCaretBeforeText: true);

                    InvalidateVisual();
                }
            }
        }

        public void OnTabForward(object sender, ExecutedRoutedEventArgs e)
        {
            ControllerView.ForceShowComment(out bool IsMoved);
            if (IsMoved)
                InvalidateVisual();

            e.Handled = true;
        }

        public void InsertNewItem(object sender, ExecutedRoutedEventArgs e)
        {
            if (ControllerView.IsNewItemInsertable(out IFocusCollectionInner inner, out IFocusInsertionCollectionNodeIndex index))
            {
                Controller.Insert(inner, index, out IWriteableBrowsingCollectionNodeIndex nodeIndex);
                InvalidateVisual();
            }

            e.Handled = true;
        }

        public void RemoveExistingItem(object sender, ExecutedRoutedEventArgs e)
        {
            if (ControllerView.IsItemRemoveable(out IFocusCollectionInner inner, out IFocusBrowsingCollectionNodeIndex index))
            {
                Controller.Remove(inner, index);
                InvalidateVisual();
            }

            e.Handled = true;
        }

        public void SplitExistingItem(object sender, ExecutedRoutedEventArgs e)
        {
            if (ControllerView.IsItemSplittable(out IFocusBlockListInner inner, out IFocusBrowsingExistingBlockNodeIndex index))
            {
                Controller.SplitBlock(inner, index);
                InvalidateVisual();
            }

            e.Handled = true;
        }

        public void MergeExistingItem(object sender, ExecutedRoutedEventArgs e)
        {
            if (ControllerView.IsItemMergeable(out IFocusBlockListInner inner, out IFocusBrowsingExistingBlockNodeIndex index))
            {
                Controller.MergeBlocks(inner, index);
                InvalidateVisual();
            }

            e.Handled = true;
        }

        public void CycleThroughExistingItem(object sender, ExecutedRoutedEventArgs e)
        {
            if (ControllerView.IsItemCyclableThrough(out IFocusCyclableNodeState state, out int cyclePosition))
            {
                cyclePosition = (cyclePosition + 1) % state.CycleIndexList.Count;
                Controller.Replace(state.ParentInner, state.CycleIndexList, cyclePosition, out IFocusBrowsingChildIndex nodeIndex);
                InvalidateVisual();
            }

            e.Handled = true;
        }

        public void SimplifyExistingItem(object sender, ExecutedRoutedEventArgs e)
        {
            if (ControllerView.IsItemSimplifiable(out IFocusInner Inner, out IFocusInsertionChildIndex Index))
            {
                Controller.Replace(Inner, Index, out IWriteableBrowsingChildIndex nodeIndex);
                InvalidateVisual();
            }

            e.Handled = true;
        }

        public void ToggleReplicate(object sender, ExecutedRoutedEventArgs e)
        {
            if (ControllerView.IsReplicationModifiable(out IFocusBlockListInner Inner, out int BlockIndex, out ReplicationStatus Replication))
            {
                switch (Replication)
                {
                    case ReplicationStatus.Normal:
                        Replication = ReplicationStatus.Replicated;
                        break;
                    case ReplicationStatus.Replicated:
                        Replication = ReplicationStatus.Normal;
                        break;
                }

                Controller.ChangeReplication(Inner, BlockIndex, Replication);
                InvalidateVisual();
            }

            e.Handled = true;
        }

        public void ToggleUserVisible(object sender, ExecutedRoutedEventArgs e)
        {
            ControllerView.SetUserVisible(!ControllerView.IsUserVisible);
            InvalidateVisual();

            e.Handled = true;
        }

        public void Expand(object sender, ExecutedRoutedEventArgs e)
        {
            if (ControllerView.Focus.CellView.StateView.State.ParentIndex is ILayoutNodeIndex Index)
            {
                Controller.Expand(Index, out bool IsChanged);
                if (IsChanged)
                    InvalidateVisual();
            }

            e.Handled = true;
        }

        public void Reduce(object sender, ExecutedRoutedEventArgs e)
        {
            if (ControllerView.Focus.CellView.StateView.State.ParentIndex is ILayoutNodeIndex Index)
            {
                Controller.Reduce(Index, out bool IsChanged);
                if (IsChanged)
                    InvalidateVisual();
            }

            e.Handled = true;
        }

        public void Undo(object sender, ExecutedRoutedEventArgs e)
        {
            if (Controller.CanUndo)
            {
                Controller.Undo();
                InvalidateVisual();
            }

            e.Handled = true;
        }

        public void Redo(object sender, ExecutedRoutedEventArgs e)
        {
            if (Controller.CanRedo)
            {
                Controller.Redo();
                InvalidateVisual();
            }

            e.Handled = true;
        }

        public void OnCopy(object sender, ExecutedRoutedEventArgs e)
        {
            PrintContext PrintContext = PrintContext.CreatePrintContext();
            using (ILayoutControllerView PrintView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, PrintContext))
            {
                PrintView.Print();
                string Content = PrintContext.PrintableArea.ToString();
                string RtfContent = PrintContext.PrintableArea.ToString(PrintContext.BrushTable);

                DataObject DataObject = new DataObject();
                DataObject.SetData("Rich Text Format", RtfContent);
                DataObject.SetData(typeof(string), Content);
                DataObject.SetData("UnicodeText", Content);
                DataObject.SetData("Text", Content);

                Clipboard.Clear();
                Clipboard.SetDataObject(DataObject, true);
            }
        }

        public void OnCut(object sender, ExecutedRoutedEventArgs e)
        {
        }

        public void OnPaste(object sender, ExecutedRoutedEventArgs e)
        {
            IDataObject DataObject = Clipboard.GetDataObject();
            string[] Formats = DataObject.GetFormats();

            foreach (string Format in Formats)
            {
                object Data = DataObject.GetData(Format);
                Debug.WriteLine($"** Format: {Format}, Type: {Data?.GetType()}");

                if (Data is string AsString)
                    Debug.WriteLine(AsString);
            }
        }

        public void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Point Point = e.GetPosition(this);
            bool IsShift = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
            bool ResetAnchor = !IsShift;

            ControllerView.SetFocusToPoint(Point.X, Point.Y, ResetAnchor, out bool IsMoved);
            if (IsMoved)
                InvalidateVisual();

            e.Handled = true;
        }

        private ILayoutController Controller;
        private DrawingVisual DrawingVisual;
        private DrawingContext DrawingContext;
        private DrawContext DrawContext;

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

        public ILayoutControllerView ControllerView { get; private set; }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (ControllerView != null)
                return new Size(ControllerView.ViewSize.Width.Draw, ControllerView.ViewSize.Height.Draw);
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

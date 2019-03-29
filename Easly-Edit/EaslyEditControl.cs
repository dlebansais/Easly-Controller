namespace EaslyEdit
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using BaseNode;
    using EaslyController.Constants;
    using EaslyController.Focus;
    using EaslyController.Layout;
    using EaslyController.Writeable;
    using EaslyDraw;
    using KeyboardHelper;

    /// <summary>
    /// A control to edit Easly source code.
    /// </summary>
    public class EaslyEditControl : EaslyDisplayControl
    {
        #region Constants
        /// <summary>
        /// Represents the <see cref="ToggleUserVisibleCommand"/> command, which requests that unassigned optional nodes be displayed.
        /// </summary>
        public static readonly RoutedCommand ToggleUserVisibleCommand = new RoutedCommand();

        /// <summary>
        /// Represents the <see cref="RemoveExistingItemCommand"/> command, which requests that a node in a list be removed.
        /// </summary>
        public static readonly RoutedCommand RemoveExistingItemCommand = new RoutedCommand();

        /// <summary>
        /// Represents the <see cref="SplitExistingItemCommand"/> command, which requests that a block be split in two.
        /// </summary>
        public static readonly RoutedCommand SplitExistingItemCommand = new RoutedCommand();

        /// <summary>
        /// Represents the <see cref="MergeExistingItemCommand"/> command, which requests that two blocks be merged.
        /// </summary>
        public static readonly RoutedCommand MergeExistingItemCommand = new RoutedCommand();

        /// <summary>
        /// Represents the <see cref="CycleThroughExistingItemCommand"/> command, which requests that a node be replaced by another in a cycle.
        /// </summary>
        public static readonly RoutedCommand CycleThroughExistingItemCommand = new RoutedCommand();

        /// <summary>
        /// Represents the <see cref="SimplifyExistingItemCommand"/> command, which requests that a node be replaced by a simpler node.
        /// </summary>
        public static readonly RoutedCommand SimplifyExistingItemCommand = new RoutedCommand();

        /// <summary>
        /// Represents the <see cref="ToggleReplicateCommand"/> command, which requests that the block replication mode be toggled.
        /// </summary>
        public static readonly RoutedCommand ToggleReplicateCommand = new RoutedCommand();

        /// <summary>
        /// Represents the <see cref="ExpandCommand"/> command, which requests that a node be expanded.
        /// </summary>
        public static readonly RoutedCommand ExpandCommand = new RoutedCommand();

        /// <summary>
        /// Represents the <see cref="ReduceCommand"/> command, which requests that a node be reduced.
        /// </summary>
        public static readonly RoutedCommand ReduceCommand = new RoutedCommand();

        /// <summary>
        /// Represents the <see cref="ExtendSelectionCommand"/> command, which requests that the selection be extended.
        /// </summary>
        public static readonly RoutedCommand ExtendSelectionCommand = new RoutedCommand();

        /// <summary>
        /// Represents the <see cref="ReduceSelectionCommand"/> command, which requests that the selection be reduced.
        /// </summary>
        public static readonly RoutedCommand ReduceSelectionCommand = new RoutedCommand();

        /// <summary>
        /// Represents the <see cref="ShowBlockGeometryCommand"/> command, which requests that a geometry be shown around blocks.
        /// </summary>
        public static readonly RoutedCommand ShowBlockGeometryCommand = new RoutedCommand();
        #endregion

        #region Custom properties and events
        #region CommentDisplayMode
        /// <summary>
        /// Identifies the <see cref="CommentDisplayMode"/> dependency property.
        /// This property is reimplemented.
        /// </summary>
        public static new readonly DependencyProperty CommentDisplayModeProperty = DependencyProperty.Register("CommentDisplayMode", typeof(CommentDisplayModes), typeof(EaslyEditControl), new PropertyMetadata(CommentDisplayModes.OnFocus, CommentDisplayModePropertyChangedCallback));

        /// <summary>
        /// Gets or sets the comment display mode.
        /// This property is reimplemented.
        /// </summary>
        public new CommentDisplayModes CommentDisplayMode
        {
            get { return (CommentDisplayModes)GetValue(CommentDisplayModeProperty); }
            set { SetValue(CommentDisplayModeProperty, value); }
        }
        #endregion
        #region CaretMode
        /// <summary>
        /// Identifies the <see cref="CaretMode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CaretModeProperty = DependencyProperty.Register("CaretMode", typeof(CaretModes), typeof(EaslyEditControl), new PropertyMetadata(CaretModes.Insertion, CaretModePropertyChangedCallback));

        /// <summary>
        /// Gets or sets the caret mode.
        /// </summary>
        public CaretModes CaretMode
        {
            get { return (CaretModes)GetValue(CaretModeProperty); }
            set { SetValue(CaretModeProperty, value); }
        }

        protected private static void CaretModePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EaslyEditControl ctrl = (EaslyEditControl)d;
            if (ctrl.CaretMode != (CaretModes)e.OldValue)
                ctrl.OnCaretModePropertyChanged(e);
        }

        protected private virtual void OnCaretModePropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (ControllerView != null)
            {
                ControllerView.SetCaretMode(CaretMode, out bool IsChanged);

                if (IsChanged)
                    InvalidateVisual();
            }
        }
        #endregion
        #region AutoFormatMode
        /// <summary>
        /// Identifies the <see cref="AutoFormatMode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AutoFormatModeProperty = DependencyProperty.Register("AutoFormatMode", typeof(AutoFormatModes), typeof(EaslyEditControl), new PropertyMetadata(AutoFormatModes.None, AutoFormatModePropertyChangedCallback));

        /// <summary>
        /// Gets or sets the automatic formatting mode.
        /// </summary>
        public AutoFormatModes AutoFormatMode
        {
            get { return (AutoFormatModes)GetValue(AutoFormatModeProperty); }
            set { SetValue(AutoFormatModeProperty, value); }
        }

        protected private static void AutoFormatModePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EaslyEditControl ctrl = (EaslyEditControl)d;
            if (ctrl.AutoFormatMode != (AutoFormatModes)e.OldValue)
                ctrl.OnAutoFormatModePropertyChanged(e);
        }

        protected private virtual void OnAutoFormatModePropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (ControllerView != null)
                ControllerView.SetAutoFormatMode(AutoFormatMode);
        }
        #endregion
        #endregion

        #region Initialization
        static EaslyEditControl()
        {
            FocusableProperty.OverrideMetadata(typeof(EaslyEditControl), new FrameworkPropertyMetadata(true));
        }
        #endregion

        #region Implementation
        protected private override void Initialize()
        {
            if (IsReady)
            {
                DrawingVisual = new DrawingVisual();
                DrawingContext = DrawingVisual.RenderOpen();

                DrawContext = DrawContext.CreateDrawContext(CreateTypeface(), FontSize, hasCommentIcon: true, displayFocus: true);
                ControllerView = LayoutControllerView.Create(Controller, TemplateSet, DrawContext);
                InitializeProperties();
                ControllerView.MeasureAndArrange();
                ControllerView.ShowCaret(true, draw: false);

                DrawingContext.Close();

                KeyboardManager = new KeyboardManager(this);
                KeyboardManager.CharacterKey += OnKeyCharacter;
                KeyboardManager.MoveKey += OnKeyMove;

                InitTextReplacement(this);
            }
        }

        protected private override void InitializeProperties()
        {
            base.InitializeProperties();

            ControllerView.SetCommentDisplayMode(CommentDisplayMode);
            ControllerView.SetCaretMode(CaretMode, out bool IsChanged);
            ControllerView.SetAutoFormatMode(AutoFormatMode);
        }

        protected private override void Cleanup()
        {
            DrawContext = null;
            ControllerView = null;
            DrawingVisual = null;

            if (KeyboardManager != null)
            {
                KeyboardManager.CharacterKey -= OnKeyCharacter;
                KeyboardManager.MoveKey -= OnKeyMove;
                KeyboardManager = null;
            }
        }

        protected private DrawingVisual DrawingVisual;
        protected private DrawingContext DrawingContext;
        protected private KeyboardManager KeyboardManager;
        #endregion

        #region Events
        protected private virtual void OnKeyCharacter(object sender, CharacterKeyEventArgs e)
        {
            if (ControllerView.Focus is ILayoutTextFocus AsTextFocus)
                ChangeText(AsTextFocus, e.Code);
            else if (ControllerView.Focus is ILayoutDiscreteContentFocus AsDiscreteContentFocus)
                ChangeDiscreteValue(AsDiscreteContentFocus, e.Code, e.Key);

            e.Handled = true;
        }

        protected private virtual void ChangeText(ILayoutTextFocus focus, int code)
        {
            string FocusedText = ControllerView.FocusedText;
            int CaretPosition = ControllerView.CaretPosition;
            bool ChangeCaretBeforeText = false;

            if (ControllerView.Selection is ILayoutTextSelection AsTextSelection)
            {
                CaretPosition = AsTextSelection.Start;
                FocusedText = FocusedText.Substring(0, AsTextSelection.Start) + FocusedText.Substring(AsTextSelection.End);
                ChangeCaretBeforeText = true;
            }

            if (ControllerView.ActualCaretMode == CaretModes.Insertion)
                StringHelper.InsertCharacter(code, ref FocusedText, ref CaretPosition);
            else
                StringHelper.ReplaceCharacter(code, ref FocusedText, ref CaretPosition);

            ControllerView.ChangeFocusedText(FocusedText, CaretPosition, ChangeCaretBeforeText);

            InvalidateVisual();
            UpdateTextReplacement();
        }

        protected private virtual void ChangeDiscreteValue(ILayoutDiscreteContentFocus focus, int code, Key key)
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

        protected private virtual void SplitIdentifier(ILayoutTextFocus focus)
        {
            if (ControllerView.IsIdentifierSplittable(out IFocusListInner Inner, out IFocusInsertionListNodeIndex ReplaceIndex, out IFocusInsertionListNodeIndex InsertIndex))
            {
                Controller.SplitIdentifier(Inner, ReplaceIndex, InsertIndex, out IWriteableBrowsingListNodeIndex FirstIndex, out IWriteableBrowsingListNodeIndex SecondIndex);

                InvalidateVisual();
                UpdateTextReplacement();
            }
            else
                ChangeText(focus, '.');
        }

        protected private virtual void OnKeyMove(object sender, MoveKeyEventArgs e)
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

        protected private virtual bool OnKeyMove(MoveDirections direction, bool resetAnchor)
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
                    if (ReplacementPopup.IsOpen)
                        ReplacementPopup.SelectPreviousLine();
                    else
                        MoveFocusVertically(-1, resetAnchor);
                    IsHandled = true;
                    break;

                case MoveDirections.Down:
                    if (ReplacementPopup.IsOpen)
                        ReplacementPopup.SelectNextLine();
                    else
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

        protected private virtual bool OnKeyMoveCtrl(MoveDirections direction, bool resetAnchor, bool isAlt)
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

        protected private virtual int GetVerticalPageMove()
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

        protected private virtual void MoveCaretLeft(bool resetAnchor)
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
                UpdateTextReplacement();
            }
        }

        protected private virtual void MoveCaretRight(bool resetAnchor)
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
                UpdateTextReplacement();
            }
        }

        protected private virtual void MoveFocusVertically(int direction, bool resetAnchor)
        {
            ControllerView.MoveFocusVertically(ControllerView.DrawContext.LineHeight.Draw * direction, resetAnchor, out bool IsMoved);
            if (IsMoved)
            {
                InvalidateVisual();
                UpdateTextReplacement();
            }
        }

        protected private virtual void MoveFocusHorizontally(int direction, bool resetAnchor)
        {
            ControllerView.MoveFocusHorizontally(direction, resetAnchor, out bool IsMoved);
            if (IsMoved)
            {
                InvalidateVisual();
                UpdateTextReplacement();
            }
        }

        protected private virtual void MoveFocus(int direction, bool resetAnchor, out bool isMoved)
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
            {
                InvalidateVisual();
                UpdateTextReplacement();
            }
        }

        protected private virtual void MoveExistingBlock(int direction)
        {
            if (!ControllerView.IsBlockMoveable(direction, out IFocusBlockListInner inner, out int blockIndex))
                return;

            Controller.MoveBlock(inner, blockIndex, direction);

            InvalidateVisual();
            UpdateTextReplacement();
        }

        protected private virtual void MoveExistingItem(int direction)
        {
            if (!ControllerView.IsItemMoveable(direction, out IFocusCollectionInner inner, out IFocusBrowsingCollectionNodeIndex index))
                return;

            Controller.Move(inner, index, direction);

            InvalidateVisual();
            UpdateTextReplacement();
        }
        #endregion

        #region Commands
        protected private virtual void OnBackspace(object sender, ExecutedRoutedEventArgs e)
        {
            DeleteCharacter(backward: true);
            e.Handled = true;
        }

        protected private virtual void OnDelete(object sender, ExecutedRoutedEventArgs e)
        {
            DeleteCharacter(backward: false);
            e.Handled = true;
        }

        protected private virtual void DeleteCharacter(bool backward)
        {
            if (ControllerView.Focus is ILayoutTextFocus AsTextFocus)
            {
                string FocusedText = ControllerView.FocusedText;
                int CaretPosition = ControllerView.CaretPosition;
                int MaxCaretPosition = ControllerView.MaxCaretPosition;

                if (StringHelper.DeleteCharacter(backward, ref FocusedText, ref CaretPosition))
                {
                    ControllerView.ChangeFocusedText(FocusedText, CaretPosition, changeCaretBeforeText: true);

                    InvalidateVisual();
                    UpdateTextReplacement();
                }
            }
        }

        protected private virtual void OnEnter(object sender, ExecutedRoutedEventArgs e)
        {
            if (ReplacementPopup.IsOpen && ReplacementPopup.SelectedEntry != null)
            {
                ReplaceItem(ReplacementPopup.Inner, ReplacementPopup.SelectedEntry.Index);
                HideTextReplacement(true);
            }
            else
                InsertNewItem();

            e.Handled = true;
        }

        protected private virtual void ReplaceItem(IFocusInner inner, IFocusInsertionChildIndex replacementIndex)
        {
            Controller.Replace(inner, replacementIndex, out IWriteableBrowsingChildIndex nodeIndex);
            InvalidateVisual();
        }

        protected private virtual void InsertNewItem()
        {
            if (ControllerView.IsNewItemInsertable(out IFocusCollectionInner inner, out IFocusInsertionCollectionNodeIndex index))
            {
                Controller.Insert(inner, index, out IWriteableBrowsingCollectionNodeIndex nodeIndex);
                InvalidateVisual();
                UpdateTextReplacement();
            }
        }

        protected private virtual void OnTabForward(object sender, ExecutedRoutedEventArgs e)
        {
            ControllerView.ForceShowComment(out bool IsMoved);
            if (IsMoved)
            {
                InvalidateVisual();
                UpdateTextReplacement();
            }

            e.Handled = true;
        }

        protected private virtual void OnToggleInsert(object sender, ExecutedRoutedEventArgs e)
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

        protected private virtual void OnToggleUserVisible(object sender, ExecutedRoutedEventArgs e)
        {
            ControllerView.SetUserVisible(!ControllerView.IsUserVisible);
            InvalidateVisual();

            e.Handled = true;
        }

        protected private virtual void OnRemoveExistingItem(object sender, ExecutedRoutedEventArgs e)
        {
            if (ControllerView.IsItemRemoveable(out IFocusCollectionInner inner, out IFocusBrowsingCollectionNodeIndex index))
            {
                Controller.Remove(inner, index);
                InvalidateVisual();
                UpdateTextReplacement();
            }

            e.Handled = true;
        }

        protected private virtual void OnSplitExistingItem(object sender, ExecutedRoutedEventArgs e)
        {
            if (ControllerView.IsItemSplittable(out IFocusBlockListInner inner, out IFocusBrowsingExistingBlockNodeIndex index))
            {
                Controller.SplitBlock(inner, index);
                InvalidateVisual();
                UpdateTextReplacement();
            }

            e.Handled = true;
        }

        protected private virtual void OnMergeExistingItem(object sender, ExecutedRoutedEventArgs e)
        {
            if (ControllerView.IsItemMergeable(out IFocusBlockListInner inner, out IFocusBrowsingExistingBlockNodeIndex index))
            {
                Controller.MergeBlocks(inner, index);
                InvalidateVisual();
            }

            e.Handled = true;
        }

        protected private virtual void OnCycleThroughExistingItem(object sender, ExecutedRoutedEventArgs e)
        {
            if (ControllerView.IsItemCyclableThrough(out IFocusCyclableNodeState state, out int cyclePosition))
            {
                cyclePosition = (cyclePosition + 1) % state.CycleIndexList.Count;
                Controller.Replace(state.ParentInner, state.CycleIndexList, cyclePosition, out IFocusBrowsingChildIndex nodeIndex);
                InvalidateVisual();
            }

            e.Handled = true;
        }

        protected private virtual void OnSimplifyExistingItem(object sender, ExecutedRoutedEventArgs e)
        {
            if (ControllerView.IsItemSimplifiable(out IFocusInner Inner, out IFocusInsertionChildIndex Index))
            {
                Controller.Replace(Inner, Index, out IWriteableBrowsingChildIndex nodeIndex);
                InvalidateVisual();
            }

            e.Handled = true;
        }

        protected private virtual void OnToggleReplicate(object sender, ExecutedRoutedEventArgs e)
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

        protected private virtual void OnExpand(object sender, ExecutedRoutedEventArgs e)
        {
            if (ControllerView.Focus.CellView.StateView.State.ParentIndex is ILayoutNodeIndex Index)
            {
                Controller.Expand(Index, out bool IsChanged);
                if (IsChanged)
                    InvalidateVisual();
            }

            e.Handled = true;
        }

        protected private virtual void OnReduce(object sender, ExecutedRoutedEventArgs e)
        {
            if (ControllerView.Focus.CellView.StateView.State.ParentIndex is ILayoutNodeIndex Index)
            {
                Controller.Reduce(Index, out bool IsChanged);
                if (IsChanged)
                    InvalidateVisual();
            }

            e.Handled = true;
        }

        protected private virtual void OnExtendSelection(object sender, ExecutedRoutedEventArgs e)
        {
            ControllerView.ExtendSelection(out bool IsChanged);
            if (IsChanged)
                InvalidateVisual();

            e.Handled = true;
        }

        protected private virtual void OnReduceSelection(object sender, ExecutedRoutedEventArgs e)
        {
            ControllerView.ReduceSelection(out bool IsChanged);
            if (IsChanged)
                InvalidateVisual();

            e.Handled = true;
        }

        protected private virtual void OnShowBlockGeometry(object sender, ExecutedRoutedEventArgs e)
        {
            ShowBlockGeometry = !ShowBlockGeometry;
        }

        protected private virtual void OnRedo(object sender, ExecutedRoutedEventArgs e)
        {
            if (Controller.CanRedo)
            {
                Controller.Redo();
                InvalidateVisual();
            }

            e.Handled = true;
        }

        protected private virtual void OnUndo(object sender, ExecutedRoutedEventArgs e)
        {
            if (Controller.CanUndo)
            {
                Controller.Undo();
                InvalidateVisual();
            }

            e.Handled = true;
        }

        protected private virtual void OnCopy(object sender, ExecutedRoutedEventArgs e)
        {
            IDataObject DataObject = new DataObject();
            CopySelectionAsString(DataObject);
            ControllerView.CopySelection(DataObject);
            Clipboard.SetDataObject(DataObject);
        }

        protected private virtual void OnCut(object sender, ExecutedRoutedEventArgs e)
        {
            IDataObject DataObject = new DataObject();
            CopySelectionAsString(DataObject);
            ControllerView.CutSelection(DataObject, out bool IsDeleted);
            if (IsDeleted)
            {
                Clipboard.SetDataObject(DataObject);
                InvalidateVisual();
            }
        }

        protected private virtual void CopySelectionAsString(IDataObject dataObject)
        {
            string Content;
            string RtfContent;

            PrintContext PrintContext = PrintContext.CreatePrintContext();
            using (ILayoutControllerView PrintView = LayoutControllerView.Create(Controller, TemplateSet, PrintContext))
            {
                switch (ControllerView.Selection)
                {
                    default:
                    case ILayoutEmptySelection AsEmptySelection:
                        PrintView.ClearSelection();
                        break;

                    case ILayoutDiscreteContentSelection AsDiscreteContentSelection:
                        PrintView.SelectDiscreteContent(AsDiscreteContentSelection.StateView.State, AsDiscreteContentSelection.PropertyName);
                        break;

                    case ILayoutStringContentSelection AsStringContentSelection:
                        PrintView.SelectStringContent(AsStringContentSelection.StateView.State, AsStringContentSelection.PropertyName, AsStringContentSelection.Start, AsStringContentSelection.End);
                        break;

                    case ILayoutCommentSelection AsCommentSelection:
                        PrintView.SelectComment(AsCommentSelection.StateView.State, AsCommentSelection.Start, AsCommentSelection.End);
                        break;

                    case ILayoutNodeSelection AsNodeSelection:
                        PrintView.SelectNode(AsNodeSelection.StateView.State);
                        break;

                    case ILayoutNodeListSelection AsNodeListSelection:
                        PrintView.SelectNodeList(AsNodeListSelection.StateView.State, AsNodeListSelection.PropertyName, AsNodeListSelection.StartIndex, AsNodeListSelection.EndIndex);
                        break;

                    case ILayoutBlockNodeListSelection AsBlockNodeListSelection:
                        PrintView.SelectBlockNodeList(AsBlockNodeListSelection.StateView.State, AsBlockNodeListSelection.PropertyName, AsBlockNodeListSelection.BlockIndex, AsBlockNodeListSelection.StartIndex, AsBlockNodeListSelection.EndIndex);
                        break;

                    case ILayoutBlockListSelection AsBlockListSelection:
                        PrintView.SelectBlockList(AsBlockListSelection.StateView.State, AsBlockListSelection.PropertyName, AsBlockListSelection.StartIndex, AsBlockListSelection.EndIndex);
                        break;
                }

                PrintView.PrintSelection();
                Content = PrintContext.PrintableArea.ToString();
                RtfContent = PrintContext.PrintableArea.ToString(PrintContext.BrushTable);

                dataObject.SetData("Rich Text Format", RtfContent);
                dataObject.SetData("UnicodeText", Content);
                dataObject.SetData("Text", Content);
            }
        }

        protected private virtual void OnPaste(object sender, ExecutedRoutedEventArgs e)
        {
            IDataObject DataObject = Clipboard.GetDataObject();
            string[] Formats = DataObject.GetFormats();

            /*
            foreach (string Format in Formats)
            {
                object Data = DataObject.GetData(Format);
                Debug.WriteLine($"** Format: {Format}, Type: {Data?.GetType()}");

                if (Data is string AsString)
                    Debug.WriteLine(AsString);
            }*/

            ControllerView.PasteSelection(out bool IsChanged);
            if (IsChanged)
                InvalidateVisual();
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Raises the <see cref="FrameworkElement.Initialized"/> event. This method is invoked whenever <see cref="FrameworkElement.IsInitialized"/> is set to true internally.
        /// </summary>
        /// <param name="e">The <see cref="RoutedEventArgs"/> that contains the event data.</param>
        protected override void OnInitialized(EventArgs e)
        {
            AddDefaultCommandHandlers();
            AddDefaultBindings();

            base.OnInitialized(e);
        }

        protected private virtual void AddDefaultCommandHandlers()
        {
            List<CommandBinding> DefaultBinding = new List<CommandBinding>()
            {
                new CommandBinding(EditingCommands.Backspace, OnBackspace),
                new CommandBinding(EditingCommands.Delete, OnDelete),
                new CommandBinding(EditingCommands.EnterParagraphBreak, OnEnter),
                new CommandBinding(EditingCommands.TabForward, OnTabForward),
                new CommandBinding(EditingCommands.ToggleInsert, OnToggleInsert),
                new CommandBinding(ToggleUserVisibleCommand, OnToggleUserVisible),
                new CommandBinding(RemoveExistingItemCommand, OnRemoveExistingItem),
                new CommandBinding(SplitExistingItemCommand, OnSplitExistingItem),
                new CommandBinding(MergeExistingItemCommand, OnMergeExistingItem),
                new CommandBinding(CycleThroughExistingItemCommand, OnCycleThroughExistingItem),
                new CommandBinding(SimplifyExistingItemCommand, OnSimplifyExistingItem),
                new CommandBinding(ToggleReplicateCommand, OnToggleReplicate),
                new CommandBinding(ExpandCommand, OnExpand),
                new CommandBinding(ReduceCommand, OnReduce),
                new CommandBinding(ExtendSelectionCommand, OnExtendSelection),
                new CommandBinding(ReduceSelectionCommand, OnReduceSelection),
                new CommandBinding(ShowBlockGeometryCommand, OnShowBlockGeometry),
                new CommandBinding(ApplicationCommands.Redo, OnRedo),
                new CommandBinding(ApplicationCommands.Undo, OnUndo),
                new CommandBinding(ApplicationCommands.Copy, OnCopy),
                new CommandBinding(ApplicationCommands.Cut, OnCut),
                new CommandBinding(ApplicationCommands.Paste, OnPaste),
            };

            List<CommandBinding> BindingsToAdd = new List<CommandBinding>();

            foreach (CommandBinding Binding in DefaultBinding)
            {
                bool Found = false;
                foreach (CommandBinding Item in CommandBindings)
                    if (Item.Command == Binding.Command)
                    {
                        Found = true;
                        break;
                    }

                if (!Found)
                    BindingsToAdd.Add(Binding);
            }

            foreach (CommandBinding Binding in BindingsToAdd)
                CommandBindings.Add(Binding);
        }

        protected private virtual void AddDefaultBindings()
        {
            List<KeyBinding> DefaultBinding = new List<KeyBinding>()
            {
                new KeyBinding(EditingCommands.Backspace, new KeyGesture(Key.Back)),
                new KeyBinding(EditingCommands.Delete, new KeyGesture(Key.Delete)),
                new KeyBinding(EditingCommands.EnterParagraphBreak, new KeyGesture(Key.Enter)),
                new KeyBinding(EditingCommands.TabForward, new KeyGesture(Key.Tab)),
                new KeyBinding(EditingCommands.ToggleInsert, new KeyGesture(Key.Insert)),
                new KeyBinding(ToggleUserVisibleCommand, new KeyGesture(Key.E, ModifierKeys.Control)),
                new KeyBinding(RemoveExistingItemCommand, new KeyGesture(Key.Y, ModifierKeys.Control)),
                new KeyBinding(SplitExistingItemCommand, new KeyGesture(Key.S, ModifierKeys.Control)),
                new KeyBinding(MergeExistingItemCommand, new KeyGesture(Key.M, ModifierKeys.Control)),
                new KeyBinding(CycleThroughExistingItemCommand, new KeyGesture(Key.T, ModifierKeys.Control)),
                new KeyBinding(SimplifyExistingItemCommand, new KeyGesture(Key.I, ModifierKeys.Control)),
                new KeyBinding(ToggleReplicateCommand, new KeyGesture(Key.R, ModifierKeys.Control)),
                new KeyBinding(ExpandCommand, new KeyGesture(Key.D, ModifierKeys.Control)),
                new KeyBinding(ReduceCommand, new KeyGesture(Key.D, ModifierKeys.Control | ModifierKeys.Shift)),
                new KeyBinding(ExtendSelectionCommand, new KeyGesture(Key.A, ModifierKeys.Control)),
                new KeyBinding(ReduceSelectionCommand, new KeyGesture(Key.A, ModifierKeys.Control | ModifierKeys.Shift)),
                new KeyBinding(ShowBlockGeometryCommand, new KeyGesture(Key.U, ModifierKeys.Control)),
                new KeyBinding(ApplicationCommands.Redo, new KeyGesture(Key.Z, ModifierKeys.Control | ModifierKeys.Shift)),
                new KeyBinding(ApplicationCommands.Undo, new KeyGesture(Key.Z, ModifierKeys.Control)),
                new KeyBinding(ApplicationCommands.Copy, new KeyGesture(Key.C, ModifierKeys.Control)),
                new KeyBinding(ApplicationCommands.Copy, new KeyGesture(Key.Insert, ModifierKeys.Control)),
                new KeyBinding(ApplicationCommands.Cut, new KeyGesture(Key.X, ModifierKeys.Control)),
                new KeyBinding(ApplicationCommands.Cut, new KeyGesture(Key.Delete, ModifierKeys.Shift)),
                new KeyBinding(ApplicationCommands.Paste, new KeyGesture(Key.V, ModifierKeys.Control)),
                new KeyBinding(ApplicationCommands.Paste, new KeyGesture(Key.Insert, ModifierKeys.Shift)),
            };

            List<KeyBinding> BindingsToAdd = new List<KeyBinding>();

            foreach (KeyBinding Binding in DefaultBinding)
            {
                bool Found = false;
                foreach (InputBinding Item in InputBindings)
                    if (Item.Command == Binding.Command)
                    {
                        Found = true;
                        break;
                    }

                if (!Found)
                    BindingsToAdd.Add(Binding);
            }

            foreach (KeyBinding Binding in BindingsToAdd)
                InputBindings.Add(Binding);
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="Mouse.MouseDownEvent"/> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> that contains the event data. This event data reports details about the mouse button that was pressed and the handled state</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            Point Point = e.GetPosition(this);

            if (e.ClickCount > 1)
            {
                int Start, End;

                if (ControllerView.Focus is IFocusStringContentFocus AsStringContentFocus && WordSelected(ControllerView.FocusedText, ControllerView.CaretPosition, out Start, out End))
                {
                    IFocusStringContentFocusableCellView CellView = AsStringContentFocus.CellView;
                    ControllerView.SelectStringContent(CellView.StateView.State, CellView.PropertyName, Start, End);
                    InvalidateVisual();
                }

                else if (ControllerView.Focus is IFocusCommentFocus AsCommentFocus && WordSelected(ControllerView.FocusedText, ControllerView.CaretPosition, out Start, out End))
                {
                    IFocusCommentCellView CellView = AsCommentFocus.CellView;
                    ControllerView.SelectComment(CellView.StateView.State, Start, End);
                    InvalidateVisual();
                }
            }
            else
            {
                bool IsShift = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
                bool ResetAnchor = !IsShift;

                ControllerView.SetFocusToPoint(Point.X, Point.Y, ResetAnchor, out bool IsMoved);
                if (IsMoved)
                    InvalidateVisual();
            }

            Focus();

            e.Handled = true;
        }

        protected private virtual bool WordSelected(string text, int position, out int start, out int end)
        {
            start = position;
            end = position;

            while (start > 0 && !char.IsWhiteSpace(text[start - 1]))
                start--;

            while (end < text.Length && !char.IsWhiteSpace(text[end]))
                end++;

            return end > start;
        }

        /// <summary>
        /// Invoked whenever an unhandled <see cref="UIElement.GotFocus"/> event reaches this element in its route.
        /// </summary>
        /// <param name="e">The <see cref="RoutedEventArgs"/> that contains the event data.</param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            if (ControllerView != null)
            {
                ControllerView.ShowCaret(true, draw: false);
                InvalidateVisual();
            }
        }

        /// <summary>
        /// Raises the <see cref="UIElement.LostFocus"/> routed event by using the event data that is provided.
        /// </summary>
        /// <param name="e">A <see cref="RoutedEventArgs"/> that contains event data. This event data must contain the identifier for the <see cref="UIElement.LostFocus"/> event.</param>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            if (ControllerView != null)
                ControllerView.ShowCaret(false, draw: true);

            base.OnLostFocus(e);
        }
        #endregion

        #region Text Replacememt
        protected private virtual void InitTextReplacement(UIElement parentUi)
        {
            ReplacementPopup = new TextReplacement();
            ReplacementPopup.Placement = PlacementMode.RelativePoint;
            ReplacementPopup.PlacementTarget = parentUi;
            ReplacementState = ReplacementStates.Hidden;
        }

        protected private virtual void UpdateTextReplacement()
        {
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Render, new Action(UpdateTextReplacementAfterRender));
        }

        protected private virtual void UpdateTextReplacementAfterRender()
        {
            if (ControllerView.Focus is ILayoutTextFocus AsTextFocus)
            {
                if (ControllerView.IsItemComplexifiable(out IFocusInner inner, out List<IFocusInsertionChildNodeIndex> IndexList))
                {
                    List<ReplacementEntry> EntryList = new List<ReplacementEntry>();
                    foreach (IFocusInsertionChildNodeIndex Index in IndexList)
                        EntryList.Add(new ReplacementEntry((ILayoutInsertionChildNodeIndex)Index));

                    ReplacementPopup.SetReplacement((ILayoutInner)inner, EntryList);

                    switch (ReplacementState)
                    {
                        case ReplacementStates.Hidden:
                        case ReplacementStates.Ready:
                            ShowTextReplacement();
                            break;

                        case ReplacementStates.Shown:
                            SetReplacementPopupPosition();
                            break;

                        case ReplacementStates.Closed:
                            break;
                    }
                }
                else
                    HideTextReplacement(false);
            }
        }

        protected private virtual void SetReplacementPopupPosition()
        {
            EaslyController.Controller.Point Origin = ControllerView.Focus.CellView.CellOrigin;
            EaslyController.Controller.Size Size = ControllerView.Focus.CellView.ActualCellSize;
            double X = Origin.X.Draw;
            double Y = Origin.Y.Draw + Size.Height.Draw;
            ControllerView.DrawContext.FromRelativeLocation(ref X, ref Y);

            ReplacementPopup.HorizontalOffset = X;
            ReplacementPopup.VerticalOffset = Y;
        }

        protected private virtual void ShowTextReplacement()
        {
            SetReplacementPopupPosition();
            ReplacementPopup.IsOpen = true;

            ReplacementState = ReplacementStates.Shown;
        }

        protected private virtual void HideTextReplacement(bool untilFocusChanged)
        {
            ReplacementPopup.IsOpen = false;
            ReplacementState = untilFocusChanged ? ReplacementStates.Closed : ReplacementStates.Ready;
        }

        protected private TextReplacement ReplacementPopup;
        protected private ReplacementStates ReplacementState;
        #endregion
    }
}

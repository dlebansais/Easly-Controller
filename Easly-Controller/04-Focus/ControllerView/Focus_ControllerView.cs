﻿namespace EaslyController.Focus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.Constants;
    using EaslyController.Controller;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// View of a IxxxController.
    /// </summary>
    public interface IFocusControllerView : IFrameControllerView
    {
        /// <summary>
        /// The controller.
        /// </summary>
        new IFocusController Controller { get; }

        /// <summary>
        /// Table of views of each state in the controller.
        /// </summary>
        new IFocusStateViewDictionary StateViewTable { get; }

        /// <summary>
        /// Table of views of each block state in the controller.
        /// </summary>
        new IFocusBlockStateViewDictionary BlockStateViewTable { get; }

        /// <summary>
        /// State view of the root state.
        /// </summary>
        new IFocusNodeStateView RootStateView { get; }

        /// <summary>
        /// Template set describing the node tree.
        /// </summary>
        new IFocusTemplateSet TemplateSet { get; }

        /// <summary>
        /// Cell view with the focus.
        /// </summary>
        IFocusFocus Focus { get; }

        /// <summary>
        /// Lowest valid value for <see cref="MoveFocus"/>.
        /// </summary>
        int MinFocusMove { get; }

        /// <summary>
        /// Highest valid value for <see cref="MoveFocus"/>.
        /// </summary>
        int MaxFocusMove { get; }

        /// <summary>
        /// Position of the caret in a text with the focus, -1 otherwise.
        /// </summary>
        int CaretPosition { get; }

        /// <summary>
        /// Position of the caret anchor in a text with the focus, -1 otherwise.
        /// </summary>
        int CaretAnchorPosition { get; }

        /// <summary>
        /// Max position of the caret in a text with the focus, -1 otherwise.
        /// </summary>
        int MaxCaretPosition { get; }

        /// <summary>
        /// Current caret mode when editing text.
        /// </summary>
        CaretModes CaretMode { get; }

        /// <summary>
        /// Current text if the focus is on a string property or comment. Null otherwise.
        /// </summary>
        string FocusedText { get; }

        /// <summary>
        /// Indicates if the node with the focus has all its frames forced to visible.
        /// </summary>
        bool IsUserVisible { get; }

        /// <summary>
        /// The current selection.
        /// </summary>
        IFocusSelection Selection { get; }

        /// <summary>
        /// The anchor to use to calculate the selection.
        /// </summary>
        IFocusNodeStateView SelectionAnchor { get; }

        /// <summary>
        /// Gets how extended is the selection.
        /// </summary>
        int SelectionExtension { get; }

        /// <summary>
        /// True if the selection is empty.
        /// </summary>
        bool IsSelectionEmpty { get; }

        /// <summary>
        /// Current auto formatting mode.
        /// </summary>
        AutoFormatModes AutoFormatMode { get; }

        /// <summary>
        /// Moves the current focus in the focus chain.
        /// </summary>
        /// <param name="direction">The change in position, relative to the current position.</param>
        /// <param name="resetAnchor">If true, resets the selected text anchor.</param>
        /// <param name="isMoved">True upon return if the focus was moved.</param>
        void MoveFocus(int direction, bool resetAnchor, out bool isMoved);

        /// <summary>
        /// Changes the caret position. Does nothing if the focus isn't on a string property.
        /// </summary>
        /// <param name="position">The new position.</param>
        /// <param name="resetAnchor">If true, resets the selected text anchor.</param>
        /// <param name="isMoved">True if the position was changed. False otherwise.</param>
        void SetCaretPosition(int position, bool resetAnchor, out bool isMoved);

        /// <summary>
        /// Changes the caret mode.
        /// </summary>
        /// <param name="mode">The new mode.</param>
        /// <param name="isChanged">True if the mode was changed.</param>
        void SetCaretMode(CaretModes mode, out bool isChanged);

        /// <summary>
        /// Sets the node with the focus to have all its frames visible.
        /// If another node had this flag set, it is reset, regardless of the value of <paramref name="isUserVisible"/>.
        /// </summary>
        void SetUserVisible(bool isUserVisible);

        /// <summary>
        /// Force the comment attached to the node with the focus to show, if empty, and move the focus to this comment.
        /// </summary>
        void ForceShowComment(out bool isMoved);

        /// <summary>
        /// Checks if a new item can be inserted at the focus.
        /// </summary>
        /// <param name="inner">Inner to use to insert the new item upon return.</param>
        /// <param name="index">Index of the new item to insert upon return.</param>
        /// <returns>True if a new item can be inserted at the focus.</returns>
        bool IsNewItemInsertable(out IFocusCollectionInner inner, out IFocusInsertionCollectionNodeIndex index);

        /// <summary>
        /// Checks if an existing item can be removed at the focus.
        /// </summary>
        /// <param name="inner">Inner to use to remove the item upon return.</param>
        /// <param name="index">Index of the item to remove upon return.</param>
        /// <returns>True if an item can be removed at the focus.</returns>
        bool IsItemRemoveable(out IFocusCollectionInner inner, out IFocusBrowsingCollectionNodeIndex index);

        /// <summary>
        /// Checks if an existing block at the focus can be split in two.
        /// </summary>
        /// <param name="inner">Inner to use to split the block upon return.</param>
        /// <param name="index">Index of the block to split upon return.</param>
        /// <returns>True if a block can be split at the focus.</returns>
        bool IsItemSplittable(out IFocusBlockListInner inner, out IFocusBrowsingExistingBlockNodeIndex index);

        /// <summary>
        /// Checks if two existing blocks at the focus can be merged.
        /// </summary>
        /// <param name="inner">Inner to use to merge the blocks upon return.</param>
        /// <param name="index">Index of the last item in the block to merge upon return.</param>
        /// <returns>True if two blocks can be merged at the focus.</returns>
        bool IsItemMergeable(out IFocusBlockListInner inner, out IFocusBrowsingExistingBlockNodeIndex index);

        /// <summary>
        /// Checks if an existing item at the focus can be moved up or down.
        /// </summary>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        /// <param name="inner">Inner to use to move the item upon return.</param>
        /// <param name="index">Index of the item to move upon return.</param>
        /// <returns>True if an item can be moved at the focus.</returns>
        bool IsItemMoveable(int direction, out IFocusCollectionInner inner, out IFocusBrowsingCollectionNodeIndex index);

        /// <summary>
        /// Checks if an existing block at the focus can be moved up or down.
        /// </summary>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        /// <param name="inner">Inner to use to move the block upon return.</param>
        /// <param name="blockIndex">Index of the block to move upon return.</param>
        /// <returns>True if an item can be moved at the focus.</returns>
        bool IsBlockMoveable(int direction, out IFocusBlockListInner inner, out int blockIndex);

        /// <summary>
        /// Checks if an existing item at the focus or above that can be cycled through.
        /// Such items are features and bodies.
        /// </summary>
        /// <param name="state">State that can be replaced the item upon return.</param>
        /// <param name="cyclePosition">Position of the current node in the cycle upon return.</param>
        /// <returns>True if an item can be cycled through at the focus.</returns>
        bool IsItemCyclableThrough(out IFocusCyclableNodeState state, out int cyclePosition);

        /// <summary>
        /// Checks if a node can be simplified.
        /// </summary>
        /// <param name="inner">Inner to use to replace the node upon return.</param>
        /// <param name="index">Index of the simpler node upon return.</param>
        /// <returns>True if a node can be simplified at the focus.</returns>
        bool IsItemSimplifiable(out IFocusInner inner, out IFocusInsertionChildIndex index);

        /// <summary>
        /// Checks if a node can be complexified.
        /// </summary>
        /// <param name="indexTable">List of indexes of more complex nodes upon return.</param>
        /// <returns>True if a node can be complexified at the focus.</returns>
        bool IsItemComplexifiable(out IDictionary<IFocusInner, IList<IFocusInsertionChildNodeIndex>> indexTable);

        /// <summary>
        /// Checks if an existing identifier at the focus can be split in two.
        /// </summary>
        /// <param name="inner">Inner to use to split the identifier upon return.</param>
        /// <param name="replaceIndex">Index of the identifier to replace upon return.</param>
        /// <param name="insertIndex">Index of the identifier to insert upon return.</param>
        /// <returns>True if an identifier can be split at the focus.</returns>
        bool IsIdentifierSplittable(out IFocusListInner inner, out IFocusInsertionListNodeIndex replaceIndex, out IFocusInsertionListNodeIndex insertIndex);

        /// <summary>
        /// Checks if an existing block can have its replication status changed.
        /// </summary>
        /// <param name="inner">Inner to use to change the replication status upon return.</param>
        /// <param name="blockIndex">Index of the block that can be changed upon return.</param>
        /// <param name="replication">The current replication status upon return.</param>
        /// <returns>True if an existing block can have its replication status changed at the focus.</returns>
        bool IsReplicationModifiable(out IFocusBlockListInner inner, out int blockIndex, out ReplicationStatus replication);

        /// <summary>
        /// Changes the value of a text. The caret position is also moved for this view and other views where the caret is at the same focus and position.
        /// </summary>
        /// <param name="newText">The new text.</param>
        /// <param name="newCaretPosition">The new caret position.</param>
        /// <param name="changeCaretBeforeText">True if the caret should be changed before the text, to preserve the caret invariant.</param>
        void ChangeFocusedText(string newText, int newCaretPosition, bool changeCaretBeforeText);

        /// <summary>
        /// Extends the selection by one step, starting from the focus.
        /// </summary>
        /// <param name="isChanged">True upon return is the selection was changed.</param>
        void ExtendSelection(out bool isChanged);

        /// <summary>
        /// Reduces the selection by one step, ending at the focus.
        /// </summary>
        /// <param name="isChanged">True upon return is the selection was changed.</param>
        void ReduceSelection(out bool isChanged);

        /// <summary>
        /// Clears the selection.
        /// </summary>
        void ClearSelection();

        /// <summary>
        /// Selects the specified discrete content.
        /// </summary>
        /// <param name="state">The state with a discrete content property.</param>
        /// <param name="propertyName">The property name.</param>
        void SelectDiscreteContent(IFocusNodeState state, string propertyName);

        /// <summary>
        /// Selects the specified string content.
        /// </summary>
        /// <param name="state">The state with a string content property.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="start">Index of the first character of the selection.</param>
        /// <param name="end">Index following the last character of the selection.</param>
        void SelectStringContent(IFocusNodeState state, string propertyName, int start, int end);

        /// <summary>
        /// Selects the specified string content.
        /// </summary>
        /// <param name="state">The state with the comment to select.</param>
        /// <param name="start">Index of the first character of the selection.</param>
        /// <param name="end">Index following the last character of the selection.</param>
        void SelectComment(IFocusNodeState state, int start, int end);

        /// <summary>
        /// Selects the specified node.
        /// </summary>
        /// <param name="state">The state with the node to select.</param>
        void SelectNode(IFocusNodeState state);

        /// <summary>
        /// Selects the specified list of nodes in a node list.
        /// </summary>
        /// <param name="state">The state with a node list property.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="startIndex">Index of the first node of the selection.</param>
        /// <param name="endIndex">Index following the last node of the selection.</param>
        void SelectNodeList(IFocusNodeState state, string propertyName, int startIndex, int endIndex);

        /// <summary>
        /// Selects the specified list of nodes in a block of a block list.
        /// </summary>
        /// <param name="state">The state with a node list property.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="blockIndex">Index of the block.</param>
        /// <param name="startIndex">Index of the first node of the selection.</param>
        /// <param name="endIndex">Index of the last node of the selection.</param>
        void SelectBlockNodeList(IFocusNodeState state, string propertyName, int blockIndex, int startIndex, int endIndex);

        /// <summary>
        /// Selects the specified list of blocks in a block list.
        /// </summary>
        /// <param name="state">The state with a node list property.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="startIndex">Index of the first block of the selection.</param>
        /// <param name="endIndex">Index of the last block of the selection.</param>
        void SelectBlockList(IFocusNodeState state, string propertyName, int startIndex, int endIndex);

#if !TRAVIS
        /// <summary>
        /// Copy the selection in the clipboard.
        /// </summary>
        /// <param name="dataObject">The clipboard data object that can already contain other custom formats.</param>
        void CopySelection(IDataObject dataObject);

        /// <summary>
        /// Copy the selection in the clipboard then removes it.
        /// </summary>
        /// <param name="dataObject">The clipboard data object that can already contain other custom formats.</param>
        /// <param name="isDeleted">True if something was deleted.</param>
        void CutSelection(IDataObject dataObject, out bool isDeleted);

        /// <summary>
        /// Replaces the selection with the content of the clipboard.
        /// </summary>
        /// <param name="isChanged">True if something was replaced or added.</param>
        void PasteSelection(out bool isChanged);

        /// <summary>
        /// Deletes the selection.
        /// </summary>
        /// <param name="isDeleted">True if something was deleted.</param>
        void DeleteSelection(out bool isDeleted);
#endif

        /// <summary>
        /// Change auto formatting mode.
        /// </summary>
        void SetAutoFormatMode(AutoFormatModes mode);
    }

    /// <summary>
    /// View of a IxxxController.
    /// </summary>
    public class FocusControllerView : FrameControllerView, IFocusControllerView, IFocusInternalControllerView
    {
        #region Init
        /// <summary>
        /// Creates and initializes a new instance of a <see cref="FocusControllerView"/> object.
        /// </summary>
        /// <param name="controller">The controller on which the view is attached.</param>
        /// <param name="templateSet">The template set used to describe the view.</param>
        public static IFocusControllerView Create(IFocusController controller, IFocusTemplateSet templateSet)
        {
            FocusControllerView View = new FocusControllerView(controller, templateSet);
            View.Init();
            return View;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FocusControllerView"/> class.
        /// </summary>
        /// <param name="controller">The controller on which the view is attached.</param>
        /// <param name="templateSet">The template set used to describe the view.</param>
        private protected FocusControllerView(IFocusController controller, IFocusTemplateSet templateSet)
            : base(controller, templateSet)
        {
        }

        /// <summary>
        /// Initializes the view by attaching it to the controller.
        /// </summary>
        private protected override void Init()
        {
            base.Init();

            ForcedCommentStateView = null;
            EmptySelection = CreateEmptySelection();
            Selection = EmptySelection;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller.
        /// </summary>
        public new IFocusController Controller { get { return (IFocusController)base.Controller; } }

        /// <summary>
        /// Table of views of each state in the controller.
        /// </summary>
        public new IFocusStateViewDictionary StateViewTable { get { return (IFocusStateViewDictionary)base.StateViewTable; } }

        /// <summary>
        /// Table of views of each block state in the controller.
        /// </summary>
        public new IFocusBlockStateViewDictionary BlockStateViewTable { get { return (IFocusBlockStateViewDictionary)base.BlockStateViewTable; } }

        /// <summary>
        /// State view of the root state.
        /// </summary>
        public new IFocusNodeStateView RootStateView { get { return (IFocusNodeStateView)base.RootStateView; } }

        /// <summary>
        /// Template set describing the node tree.
        /// </summary>
        public new IFocusTemplateSet TemplateSet { get { return (IFocusTemplateSet)base.TemplateSet; } }

        /// <summary>
        /// Cell view with the focus.
        /// </summary>
        public IFocusFocus Focus { get; private set; }

        private protected IFocusFocusList FocusChain { get; private set; }

        /// <summary>
        /// Lowest valid value for <see cref="MoveFocus"/>.
        /// </summary>
        public int MinFocusMove
        {
            get
            {
                int FocusIndex = FocusChain.IndexOf(Focus);
                Debug.Assert(FocusIndex >= 0 && FocusIndex < FocusChain.Count);

                return -FocusIndex;
            }
        }

        /// <summary>
        /// Highest valid value for <see cref="MoveFocus"/>.
        /// </summary>
        public int MaxFocusMove
        {
            get
            {
                int FocusIndex = FocusChain.IndexOf(Focus);
                Debug.Assert(FocusIndex >= 0 && FocusIndex < FocusChain.Count);

                return FocusChain.Count - FocusIndex - 1;
            }
        }

        /// <summary>
        /// Position of the caret in a text with the focus, -1 otherwise.
        /// </summary>
        public int CaretPosition { get; private set; }

        /// <summary>
        /// Position of the caret anchor in a text with the focus, -1 otherwise.
        /// </summary>
        public int CaretAnchorPosition { get; private set; }

        /// <summary>
        /// Max position of the caret in a text with the focus, -1 otherwise.
        /// </summary>
        public int MaxCaretPosition { get; private set; }

        /// <summary>
        /// Current caret mode when editing text.
        /// </summary>
        public CaretModes CaretMode { get; private set; }

        /// <summary>
        /// Current text if the focus is on a string property or comment. Null otherwise.
        /// </summary>
        public string FocusedText
        {
            get
            {
                string Result = null;

                if (Focus is IFocusTextFocus AsTextFocus)
                    Result = GetFocusedText(AsTextFocus);

                return Result;
            }
        }

        /// <summary>
        /// Indicates if the node with the focus has all its frames forced to visible.
        /// </summary>
        public bool IsUserVisible { get { return Focus.CellView.StateView.IsUserVisible; } }

        /// <summary>
        /// The current selection.
        /// </summary>
        public IFocusSelection Selection { get; private set; }
        private IFocusEmptySelection EmptySelection;

        /// <summary>
        /// The anchor to use to calculate the selection.
        /// </summary>
        public IFocusNodeStateView SelectionAnchor { get; private set; }

        /// <summary>
        /// Gets how extended is the selection.
        /// </summary>
        public int SelectionExtension { get; private set; }

        /// <summary>
        /// True if the selection is empty.
        /// </summary>
        public bool IsSelectionEmpty { get { return Selection == EmptySelection; } }

        /// <summary>
        /// Current auto formatting mode.
        /// </summary>
        public AutoFormatModes AutoFormatMode { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Moves the current focus in the focus chain.
        /// </summary>
        /// <param name="direction">The change in position, relative to the current position.</param>
        /// <param name="resetAnchor">If true, resets the selected text anchor.</param>
        /// <param name="isMoved">True upon return if the focus was moved.</param>
        public virtual void MoveFocus(int direction, bool resetAnchor, out bool isMoved)
        {
            ulong OldFocusHash = FocusHash;

            int OldFocusIndex = FocusChain.IndexOf(Focus);
            Debug.Assert(OldFocusIndex >= 0 && OldFocusIndex < FocusChain.Count);

            int NewFocusIndex = OldFocusIndex + direction;
            if (NewFocusIndex < 0)
                NewFocusIndex = 0;
            else if (NewFocusIndex >= FocusChain.Count)
                NewFocusIndex = FocusChain.Count - 1;

            Debug.Assert(NewFocusIndex >= 0 && NewFocusIndex < FocusChain.Count);

            if (OldFocusIndex != NewFocusIndex)
            {
                ChangeFocus(direction, OldFocusIndex, NewFocusIndex, resetAnchor, out bool IsRefreshed);
                isMoved = true;
            }
            else
                isMoved = false;

            Debug.Assert(isMoved || OldFocusHash == FocusHash);
        }

        private protected virtual void ChangeFocus(int direction, int oldIndex, int newIndex, bool resetAnchor, out bool isRefreshed)
        {
            isRefreshed = false;

            Focus = FocusChain[newIndex];

            if (FocusChain[oldIndex] is IFocusCommentFocus AsCommentFocus)
            {
                string Text = GetFocusedCommentText(AsCommentFocus);
                if (Text == null)
                {
                    Refresh(Controller.RootState);
                    isRefreshed = true;
                }
            }

            ResetCaretPosition(direction, resetAnchor);

            if (resetAnchor)
                ResetSelection();
            else
                SetAnchoredSelection();
        }

        private protected virtual void MoveFocusToState(IFocusNodeState state)
        {
            List<IFocusNodeStateView> StateViewList = new List<IFocusNodeStateView>();
            IFocusNodeStateView MainStateView = null;
            List<IFocusFocus> SameStateFocusableList = new List<IFocusFocus>();

            // Get the state that should have the focus and all its children.
            if (GetFocusedStateAndChildren(FocusChain, state, out MainStateView, out StateViewList, out SameStateFocusableList))
            {
                Debug.Assert(SameStateFocusableList.Count > 0);

                // Use a preferred frame.
                FindPreferredFrame(MainStateView, SameStateFocusableList);

                ResetCaretPosition(0, true);
            }
        }

        private protected virtual void SetAnchoredSelection()
        {
            IFocusNodeState AnchorState = SelectionAnchor.State;
            IFocusNodeState FocusedState = Focus.CellView.StateView.State;

            if (AnchorState == FocusedState)
            {
                if (Focus is IFocusTextFocus AsTextFocus)
                    SetTextSelection(AsTextFocus);
                else
                    ResetSelection();
            }
            else
                SetAnchoredNodeSelection();
        }

        private protected virtual void SetAnchoredNodeSelection()
        {
            IFocusNodeState AnchorState = SelectionAnchor.State;
            IFocusNodeState FocusedState = Focus.CellView.StateView.State;

            IFocusNodeState State = AnchorState;
            IReadOnlyIndex FirstFocusedIndex = null;
            IReadOnlyIndex FirstAnchorIndex = null;

            while (State != null && !Controller.IsChildState(State, FocusedState, out FirstFocusedIndex))
                State = State.ParentState;

            Debug.Assert(State != null);

            bool IsAnchorChild = Controller.IsChildState(State, AnchorState, out FirstAnchorIndex);
            Debug.Assert(IsAnchorChild);

            bool IsFromPatternOrSource = (AnchorState is IFocusPatternState) || (AnchorState is IFocusSourceState) || (FocusedState is IFocusPatternState) || (FocusedState is IFocusSourceState);

            if (FirstFocusedIndex is IFocusBrowsingListNodeIndex AsFocusListNodeIndex && FirstAnchorIndex is IFocusBrowsingListNodeIndex AsAnchorListNodeIndex && AsFocusListNodeIndex.PropertyName == AsAnchorListNodeIndex.PropertyName)
                SelectNodeList(State, AsFocusListNodeIndex.PropertyName, AsFocusListNodeIndex.Index, AsAnchorListNodeIndex.Index);
            else if (FirstFocusedIndex is IFocusBrowsingExistingBlockNodeIndex AsFocusBlockListNodeIndex && FirstAnchorIndex is IFocusBrowsingExistingBlockNodeIndex AsAnchorBlockListNodeIndex && AsFocusBlockListNodeIndex.PropertyName == AsAnchorBlockListNodeIndex.PropertyName)
            {
                if (AsFocusBlockListNodeIndex.BlockIndex == AsAnchorBlockListNodeIndex.BlockIndex && !IsFromPatternOrSource)
                    SelectBlockNodeList(State, AsFocusBlockListNodeIndex.PropertyName, AsFocusBlockListNodeIndex.BlockIndex, AsFocusBlockListNodeIndex.Index, AsAnchorBlockListNodeIndex.Index);
                else
                    SelectBlockList(State, AsFocusBlockListNodeIndex.PropertyName, AsFocusBlockListNodeIndex.BlockIndex, AsAnchorBlockListNodeIndex.BlockIndex);
            }
            else
                SelectNode(State);
        }

        /// <summary>
        /// Changes the caret position. Does nothing if the focus isn't on a string property.
        /// </summary>
        /// <param name="position">The new position.</param>
        /// <param name="resetAnchor">If true, resets the selected text anchor.</param>
        /// <param name="isMoved">True if the position was changed. False otherwise.</param>
        public virtual void SetCaretPosition(int position, bool resetAnchor, out bool isMoved)
        {
            isMoved = false;
            ulong OldFocusHash = FocusHash;

            if (Focus is IFocusTextFocus AsTextFocus)
            {
                string Text = GetFocusedText(AsTextFocus);
                SetTextCaretPosition(Text, position, resetAnchor, out isMoved);
            }

            Debug.Assert(isMoved || resetAnchor || OldFocusHash == FocusHash);
        }

        private protected virtual void SetTextCaretPosition(string text, int position, bool resetAnchor, out bool isMoved)
        {
            int OldPosition = CaretPosition;

            if (position <= 0)
                CaretPosition = 0;
            else if (position >= text.Length)
                CaretPosition = text.Length;
            else
                CaretPosition = position;

            if (resetAnchor)
            {
                ResetSelection();
                CaretAnchorPosition = CaretPosition;
            }
            else if (Selection == EmptySelection)
            {
                IFocusTextFocus AsTextFocus = Focus as IFocusTextFocus;
                Debug.Assert(AsTextFocus != null);

                SetTextSelection(AsTextFocus);
            }

            else if (Selection is IFocusTextSelection AsTextSelection)
                AsTextSelection.Update(CaretAnchorPosition, CaretPosition);

            CheckCaretInvariant(text);

            isMoved = CaretPosition != OldPosition;
        }

        /// <summary>
        /// Changes the caret mode.
        /// </summary>
        /// <param name="mode">The new mode.</param>
        /// <param name="isChanged">True if the mode was changed.</param>
        public virtual void SetCaretMode(CaretModes mode, out bool isChanged)
        {
            isChanged = false;
            ulong OldFocusHash = FocusHash;

            if (CaretMode != mode)
            {
                CaretMode = mode;
                isChanged = true;

                CheckCaretInvariant();
            }

            Debug.Assert(isChanged || OldFocusHash == FocusHash);
        }

        /// <summary>
        /// Sets the node with the focus to have all its frames visible.
        /// If another node had this flag set, it is reset, regardless of the value of <paramref name="isUserVisible"/>.
        /// </summary>
        public virtual void SetUserVisible(bool isUserVisible)
        {
            if (isUserVisible)
                foreach (KeyValuePair<IFocusNodeState, IFocusNodeStateView> Entry in StateViewTable)
                    Entry.Value.SetIsUserVisible(false);

            IFocusNodeStateView StateView = Focus.CellView.StateView;
            Debug.Assert(StateView != null);

            for (;;)
            {
                StateView.SetIsUserVisible(isUserVisible);

                foreach (KeyValuePair<string, IFocusInner> Entry in StateView.State.InnerTable)
                {
                    if (Entry.Value is IFocusPlaceholderInner AsPlaceholderInner)
                    {
                        IFocusNodeStateView ChildStateView = StateViewTable[AsPlaceholderInner.ChildState];
                        if (((IFocusNodeTemplate)ChildStateView.Template).IsSimple)
                            ChildStateView.SetIsUserVisible(isUserVisible);
                    }
                }

                if (!((IFocusNodeTemplate)StateView.Template).IsSimple || StateView.State.ParentState == null)
                    break;

                StateView = StateViewTable[StateView.State.ParentState];
            }

            Refresh(StateView.State);
        }

        /// <summary>
        /// Force the comment attached to the node with the focus to show, if empty, and move the focus to this comment.
        /// </summary>
        public virtual void ForceShowComment(out bool isMoved)
        {
            IFocusNodeState State = Focus.CellView.StateView.State;
            IDocument Documentation;
            if (State is IFocusOptionalNodeState AsOptionalNodeState)
            {
                Debug.Assert(AsOptionalNodeState.ParentInner.IsAssigned);
                Documentation = AsOptionalNodeState.Node.Documentation;
            }
            else
                Documentation = State.Node.Documentation;

            isMoved = false;
            ulong OldFocusHash = FocusHash;

            if (!(Focus is IFocusCommentFocus))
            {
                string Text = CommentHelper.Get(Documentation);
                if (Text == null)
                {
                    IFocusNodeStateView StateView = Focus.CellView.StateView;
                    ForcedCommentStateView = StateView;

                    Refresh(Controller.RootState);
                    Debug.Assert(ForcedCommentStateView == null);

                    for (int i = 0; i < FocusChain.Count; i++)
                        if (FocusChain[i] is IFocusCommentFocus AsCommentFocus && AsCommentFocus.CellView.StateView == StateView)
                        {
                            int OldFocusIndex = FocusChain.IndexOf(Focus);
                            Debug.Assert(OldFocusIndex >= 0); // The old focus must have been preserved.

                            int NewFocusIndex = i;

                            ChangeFocus(NewFocusIndex - OldFocusIndex, OldFocusIndex, NewFocusIndex, true, out bool IsRefreshed);
                            Debug.Assert(!IsRefreshed); // Refresh must not be done twice.

                            isMoved = true;
                            break;
                        }

                    Debug.Assert(isMoved);
                }
            }

            if (isMoved)
                ResetSelection();

            Debug.Assert(isMoved || OldFocusHash == FocusHash);
        }

        /// <summary>
        /// Checks if a new item can be inserted at the focus.
        /// </summary>
        /// <param name="inner">Inner to use to insert the new item upon return.</param>
        /// <param name="index">Index of the new item to insert upon return.</param>
        /// <returns>True if a new item can be inserted at the focus.</returns>
        public virtual bool IsNewItemInsertable(out IFocusCollectionInner inner, out IFocusInsertionCollectionNodeIndex index)
        {
            inner = null;
            index = null;

            bool Result = false;

            IFocusNodeState State = Focus.CellView.StateView.State;
            IFocusFrame Frame = Focus.CellView.Frame;

            if (Frame is IFocusInsertFrame AsInsertFrame)
                Result = IsNewItemInsertableAtInsertFrame(State, AsInsertFrame, out inner, out index);

            else if (Focus.CellView is IFocusStringContentFocusableCellView AsStringContentFocusableCellView)
                Result = IsNewItemInsertableAtStringContentCellView(State, AsStringContentFocusableCellView, out inner, out index);

            return Result;
        }

        private protected virtual bool IsNewItemInsertableAtInsertFrame(IFocusNodeState state, IFocusInsertFrame frame, out IFocusCollectionInner inner, out IFocusInsertionCollectionNodeIndex index)
        {
            inner = null;
            index = null;

            bool Result = false;

            Type InsertType = frame.InsertType;
            Debug.Assert(InsertType != null);
            Debug.Assert(!InsertType.IsInterface);
            Debug.Assert(!InsertType.IsAbstract);

            Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(InsertType);
            INode NewItem = NodeHelper.CreateDefaultFromInterface(InterfaceType);

            IFocusCollectionInner CollectionInner = null;
            frame.CollectionNameToInner(ref state, ref CollectionInner);
            Debug.Assert(CollectionInner != null);

            if (CollectionInner is IFocusBlockListInner AsBlockListInner)
            {
                inner = AsBlockListInner;

                if (AsBlockListInner.Count == 0)
                {
                    IPattern NewPattern = NodeHelper.CreateEmptyPattern();
                    IIdentifier NewSource = NodeHelper.CreateEmptyIdentifier();
                    index = CreateNewBlockNodeIndex(state.Node, CollectionInner.PropertyName, NewItem, 0, NewPattern, NewSource);
                }
                else
                    index = CreateExistingBlockNodeIndex(state.Node, CollectionInner.PropertyName, NewItem, 0, 0);

                Result = true;
            }

            else if (CollectionInner is IFocusListInner AsListInner)
            {
                inner = AsListInner;
                index = CreateListNodeIndex(state.Node, AsListInner.PropertyName, NewItem, 0);

                Result = true;
            }

            return Result;
        }

        private protected virtual bool IsNewItemInsertableAtStringContentCellView(IFocusNodeState state, IFocusStringContentFocusableCellView cellView, out IFocusCollectionInner inner, out IFocusInsertionCollectionNodeIndex index)
        {
            inner = null;
            index = null;

            bool Result = false;

            if (CaretPosition == 0)
                Result = IsListExtremumItem(state, cellView, IsFirstFocusableCellView, InsertAbove, out inner, out index);

            else if (CaretPosition == MaxCaretPosition)
                Result = IsListExtremumItem(state, cellView, IsLastFocusableCellView, InsertBelow, out inner, out index);

            return Result;
        }

        private protected virtual bool IsListExtremumItem(IFocusNodeState state, IFocusContentFocusableCellView cellView, Func<IFocusNodeState, IFocusContentFocusableCellView, bool> isGoodFocusableCellView, Func<int, int> getInsertPosition, out IFocusCollectionInner inner, out IFocusInsertionCollectionNodeIndex index)
        {
            inner = null;
            index = null;

            IFocusInner ParentInner = state.ParentInner;
            if (ParentInner == null)
                return false;

            INode NewItem;
            int BlockPosition;
            int ItemPosition;
            bool IsHandled = false;
            bool Result = false;

            switch (ParentInner)
            {
                case IFocusPlaceholderInner AsPlaceholderInner:
                case IFocusOptionalInner AsOptionalInner:
                    Result = IsExtremumCheckParent(state, cellView, isGoodFocusableCellView, getInsertPosition, out inner, out index);
                    IsHandled = true;
                    break;

                // Check the parent state if there is a deeper list (typically, for a qualified name, there would be one).
                case IFocusListInner AsListInner:
                    if (IsDeepestList(state))
                    {
                        NewItem = NodeHelper.CreateDefaultFromInterface(AsListInner.InterfaceType);
                        ItemPosition = (state.ParentIndex as IFocusBrowsingListNodeIndex).Index;

                        inner = AsListInner;
                        index = CreateListNodeIndex(inner.Owner.Node, inner.PropertyName, NewItem, getInsertPosition(ItemPosition));

                        Result = true;
                    }
                    else
                        Result = IsExtremumCheckParent(state, cellView, isGoodFocusableCellView, getInsertPosition, out inner, out index);
                    IsHandled = true;
                    break;

                case IFocusBlockListInner AsBlockListInner:
                    NewItem = NodeHelper.CreateDefaultFromInterface(AsBlockListInner.InterfaceType);
                    BlockPosition = (state.ParentIndex as IFocusBrowsingExistingBlockNodeIndex).BlockIndex;
                    ItemPosition = (state.ParentIndex as IFocusBrowsingExistingBlockNodeIndex).Index;

                    inner = AsBlockListInner;
                    index = CreateExistingBlockNodeIndex(inner.Owner.Node, inner.PropertyName, NewItem, BlockPosition, getInsertPosition(ItemPosition));

                    Result = true;
                    IsHandled = true;
                    break;
            }

            Debug.Assert(IsHandled);

            return Result;
        }

        private protected virtual bool IsExtremumCheckParent(IFocusNodeState state, IFocusContentFocusableCellView cellView, Func<IFocusNodeState, IFocusContentFocusableCellView, bool> isGoodFocusableCellView, Func<int, int> getInsertPosition, out IFocusCollectionInner inner, out IFocusInsertionCollectionNodeIndex index)
        {
            inner = null;
            index = null;
            bool Result = false;

            IFocusNodeState ParentState = state.ParentState;

            if (ParentState != null && isGoodFocusableCellView(state, cellView))
                Result = IsListExtremumItem(ParentState, cellView, isGoodFocusableCellView, getInsertPosition, out inner, out index);

            return Result;
        }

        private protected virtual bool IsFirstFocusableCellView(IFocusNodeState state, IFocusContentFocusableCellView cellView)
        {
            Debug.Assert(StateViewTable.ContainsKey(state));

            IFocusNodeStateView StateView = StateViewTable[state];
            StateView.RootCellView.EnumerateVisibleCellViews(GetFirstFocusable, out IFrameVisibleCellView FirstCellView, reversed: false);
            return FirstCellView == cellView;
        }

        private protected virtual bool GetFirstFocusable(IFrameVisibleCellView cellView)
        {
            return cellView is IFocusFocusableCellView;
        }

        private protected virtual int InsertAbove(int position)
        {
            return position;
        }

        private protected virtual bool IsLastFocusableCellView(IFocusNodeState state, IFocusContentFocusableCellView cellView)
        {
            Debug.Assert(StateViewTable.ContainsKey(state));

            IFocusNodeStateView StateView = StateViewTable[state];
            StateView.RootCellView.EnumerateVisibleCellViews(GetLastFocusable, out IFrameVisibleCellView FirstCellView, reversed: true);
            return FirstCellView == cellView;
        }

        private protected virtual bool GetLastFocusable(IFrameVisibleCellView cellView)
        {
            return cellView is IFocusFocusableCellView;
        }

        private protected virtual int InsertBelow(int position)
        {
            return position + 1;
        }

        private protected virtual bool IsDeepestList(IFocusNodeState state)
        {
            bool Result = true;

            while (state.ParentState != null)
            {
                state = state.ParentState;
                if (state.ParentInner is IFocusCollectionInner)
                    Result = false;
            }

            return Result;
        }

        /// <summary>
        /// Checks if an existing item can be removed at the focus.
        /// </summary>
        /// <param name="inner">Inner to use to remove the item upon return.</param>
        /// <param name="index">Index of the item to remove upon return.</param>
        /// <returns>True if an item can be removed at the focus.</returns>
        public virtual bool IsItemRemoveable(out IFocusCollectionInner inner, out IFocusBrowsingCollectionNodeIndex index)
        {
            inner = null;
            index = null;

            bool IsRemoveable = false;

            IFocusNodeState State = Focus.CellView.StateView.State;

            if (Focus.CellView.Frame is IFocusInsertFrame AsInsertFrame)
                IsRemoveable = false;
            else
            {
                IsRemoveable = false;

                // Search recursively for a collection parent, up to 4 levels up.
                for (int i = 0; i < 4 && State != null; i++)
                {
                    if (State.ParentInner is IFocusCollectionInner AsCollectionInner)
                    {
                        inner = AsCollectionInner;
                        index = State.ParentIndex as IFocusBrowsingCollectionNodeIndex;
                        Debug.Assert(index != null);

                        if (Controller.IsRemoveable(inner, index))
                        {
                            IsRemoveable = true;
                            break;
                        }
                    }

                    State = State.ParentState;
                }
            }

            return IsRemoveable;
        }

        /// <summary>
        /// Checks if an existing block at the focus can be split in two.
        /// </summary>
        /// <param name="inner">Inner to use to split the block upon return.</param>
        /// <param name="index">Index of the block to split upon return.</param>
        /// <returns>True if a block can be split at the focus.</returns>
        public virtual bool IsItemSplittable(out IFocusBlockListInner inner, out IFocusBrowsingExistingBlockNodeIndex index)
        {
            inner = null;
            index = null;

            bool IsSplittable = false;

            IFocusNodeState State = Focus.CellView.StateView.State;

            if (Focus.CellView.Frame is IFocusInsertFrame AsInsertFrame)
                IsSplittable = false;
            else
            {
                IsSplittable = false;

                // Search recursively for a collection parent, up to 3 levels up.
                for (int i = 0; i < 3 && State != null; i++)
                {
                    if (State.ParentInner is IFocusBlockListInner AsBlockListInner)
                    {
                        inner = AsBlockListInner;
                        index = State.ParentIndex as IFocusBrowsingExistingBlockNodeIndex;
                        Debug.Assert(index != null);

                        if (Controller.IsSplittable(inner, index))
                            IsSplittable = true;

                        break;
                    }

                    State = State.ParentState;
                }
            }

            return IsSplittable;
        }

        /// <summary>
        /// Checks if two existing blocks at the focus can be merged.
        /// </summary>
        /// <param name="inner">Inner to use to merge the blocks upon return.</param>
        /// <param name="index">Index of the last item in the block to merge upon return.</param>
        /// <returns>True if two blocks can be merged at the focus.</returns>
        public virtual bool IsItemMergeable(out IFocusBlockListInner inner, out IFocusBrowsingExistingBlockNodeIndex index)
        {
            inner = null;
            index = null;

            bool IsMergeable = false;

            IFocusNodeState State = Focus.CellView.StateView.State;

            if (Focus.CellView.Frame is IFocusInsertFrame AsInsertFrame)
                IsMergeable = false;
            else
            {
                IsMergeable = false;

                // Search recursively for a collection parent, up to 3 levels up.
                for (int i = 0; i < 3 && State != null; i++)
                {
                    if (State.ParentInner is IFocusBlockListInner AsBlockListInner)
                    {
                        inner = AsBlockListInner;
                        index = State.ParentIndex as IFocusBrowsingExistingBlockNodeIndex;
                        Debug.Assert(index != null);

                        if (Controller.IsMergeable(inner, index))
                            IsMergeable = true;

                        break;
                    }

                    State = State.ParentState;
                }
            }

            return IsMergeable;
        }

        /// <summary>
        /// Checks if an existing item at the focus can be moved up or down.
        /// </summary>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        /// <param name="inner">Inner to use to move the item upon return.</param>
        /// <param name="index">Index of the item to move upon return.</param>
        /// <returns>True if an item can be moved at the focus.</returns>
        public virtual bool IsItemMoveable(int direction, out IFocusCollectionInner inner, out IFocusBrowsingCollectionNodeIndex index)
        {
            inner = null;
            index = null;

            bool IsMoveable = false;

            IFocusNodeState State = Focus.CellView.StateView.State;

            if (Focus.CellView.Frame is IFocusInsertFrame AsInsertFrame)
                IsMoveable = false;
            else
            {
                IsMoveable = false;

                // Search recursively for a collection parent, up to 3 levels up.
                for (int i = 0; i < 3 && State != null; i++)
                {
                    IFocusCollectionInner ListInner = null;

                    if (State.ParentInner is IFocusBlockListInner AsBlockListInner)
                        ListInner = AsBlockListInner;
                    else if (State.ParentInner is IFocusListInner AsListInner && IsDeepestList(State))
                        ListInner = AsListInner;

                    if (ListInner != null)
                    {
                        inner = ListInner;
                        index = State.ParentIndex as IFocusBrowsingCollectionNodeIndex;
                        Debug.Assert(index != null);

                        if (Controller.IsMoveable(inner, index, direction))
                            IsMoveable = true;

                        break;
                    }

                    State = State.ParentState;
                }
            }

            return IsMoveable;
        }

        /// <summary>
        /// Checks if an existing block at the focus can be moved up or down.
        /// </summary>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        /// <param name="inner">Inner to use to move the block upon return.</param>
        /// <param name="blockIndex">Index of the block to move upon return.</param>
        /// <returns>True if an item can be moved at the focus.</returns>
        public virtual bool IsBlockMoveable(int direction, out IFocusBlockListInner inner, out int blockIndex)
        {
            inner = null;
            blockIndex = -1;

            bool IsBlockMoveable = false;

            IFocusNodeState State = Focus.CellView.StateView.State;

            if (Focus.CellView.Frame is IFocusInsertFrame AsInsertFrame)
                IsBlockMoveable = false;
            else
            {
                IsBlockMoveable = false;

                // Search recursively for a collection parent, up to 3 levels up.
                for (int i = 0; i < 3 && State != null; i++)
                {
                    if (State.ParentInner is IFocusBlockListInner AsBlockListInner)
                    {
                        inner = AsBlockListInner;
                        IFocusBrowsingExistingBlockNodeIndex ParentIndex = State.ParentIndex as IFocusBrowsingExistingBlockNodeIndex;
                        Debug.Assert(ParentIndex != null);
                        blockIndex = ParentIndex.BlockIndex;

                        if (Controller.IsBlockMoveable(inner, blockIndex, direction))
                            IsBlockMoveable = true;

                        break;
                    }

                    State = State.ParentState;
                }
            }

            return IsBlockMoveable;
        }

        /// <summary>
        /// Checks if an existing item at the focus or above that can be cycled through.
        /// Such items are features and bodies.
        /// </summary>
        /// <param name="state">State that can be replaced the item upon return.</param>
        /// <param name="cyclePosition">Position of the current node in the cycle upon return.</param>
        /// <returns>True if an item can be cycled through at the focus.</returns>
        public virtual bool IsItemCyclableThrough(out IFocusCyclableNodeState state, out int cyclePosition)
        {
            state = null;
            cyclePosition = -1;

            bool IsCyclableThrough = false;

            IFocusNodeState CurrentState = Focus.CellView.StateView.State;

            // Search recursively for a collection parent.
            while (CurrentState != null)
            {
                if (CurrentState is IFocusCyclableNodeState AsCyclableNodeState && Controller.IsMemberOfCycle(AsCyclableNodeState, out IFocusCycleManager CycleManager))
                {
                    CycleManager.AddNodeToCycle(AsCyclableNodeState);

                    IFocusInsertionChildNodeIndexList CycleIndexList = AsCyclableNodeState.CycleIndexList;
                    Debug.Assert(CycleIndexList.Count >= 2);
                    int CurrentPosition = AsCyclableNodeState.CycleCurrentPosition;
                    Debug.Assert(CurrentPosition >= 0 && CurrentPosition < CycleIndexList.Count);

                    state = AsCyclableNodeState;
                    cyclePosition = CurrentPosition;

                    IsCyclableThrough = true;
                    break;
                }

                CurrentState = CurrentState.ParentState;
            }

            return IsCyclableThrough;
        }

        /// <summary>
        /// Checks if a node can be simplified.
        /// </summary>
        /// <param name="inner">Inner to use to replace the node upon return.</param>
        /// <param name="index">Index of the simpler node upon return.</param>
        /// <returns>True if a node can be simplified at the focus.</returns>
        public virtual bool IsItemSimplifiable(out IFocusInner inner, out IFocusInsertionChildIndex index)
        {
            inner = null;
            index = null;

            bool IsSimplifiable = false;

            IFocusNodeState CurrentState = Focus.CellView.StateView.State;

            // Search recursively for a simplifiable node.
            while (CurrentState != null)
            {
                if (NodeHelper.GetSimplifiedNode(CurrentState.Node, out INode SimplifiedNode))
                {
                    if (SimplifiedNode != null)
                    {
                        Type InterfaceType = CurrentState.ParentInner.InterfaceType;
                        if (InterfaceType.IsAssignableFrom(SimplifiedNode.GetType()))
                        {
                            IFocusBrowsingChildIndex ParentIndex = CurrentState.ParentIndex as IFocusBrowsingChildIndex;
                            Debug.Assert(ParentIndex != null);

                            inner = CurrentState.ParentInner;
                            index = ((IFocusBrowsingInsertableIndex)ParentIndex).ToInsertionIndex(inner.Owner.Node, SimplifiedNode) as IFocusInsertionChildIndex;
                            IsSimplifiable = true;
                        }
                    }

                    break;
                }

                CurrentState = CurrentState.ParentState;
            }

            return IsSimplifiable;
        }

        /// <summary>
        /// Checks if a node can be complexified.
        /// </summary>
        /// <param name="indexTable">List of indexes of more complex nodes upon return.</param>
        /// <returns>True if a node can be complexified at the focus.</returns>
        public virtual bool IsItemComplexifiable(out IDictionary<IFocusInner, IList<IFocusInsertionChildNodeIndex>> indexTable)
        {
            indexTable = new Dictionary<IFocusInner, IList<IFocusInsertionChildNodeIndex>>();

            bool IsComplexifiable = false;

            IFocusNodeState CurrentState = Focus.CellView.StateView.State;

            // Search recursively for a complexifiable node.
            while (CurrentState != null)
            {
                if (NodeHelper.GetComplexifiedNode(CurrentState.Node, out IList<INode> ComplexifiedNodeList))
                {
                    Debug.Assert(ComplexifiedNodeList != null && ComplexifiedNodeList.Count > 0);
                    Type InterfaceType = CurrentState.ParentInner.InterfaceType;
                    bool IsAssignable = true;

                    foreach (INode ComplexifiedNode in ComplexifiedNodeList)
                        IsAssignable &= InterfaceType.IsAssignableFrom(ComplexifiedNode.GetType());

                    if (IsAssignable)
                    {
                        IFocusBrowsingChildIndex ParentIndex = CurrentState.ParentIndex as IFocusBrowsingChildIndex;
                        Debug.Assert(ParentIndex != null);

                        IFocusInner Inner = CurrentState.ParentInner;
                        IList<IFocusInsertionChildNodeIndex> IndexList = new List<IFocusInsertionChildNodeIndex>();

                        foreach (INode ComplexifiedNode in ComplexifiedNodeList)
                        {
                            IFocusInsertionChildNodeIndex NodeIndex = ((IFocusBrowsingInsertableIndex)ParentIndex).ToInsertionIndex(Inner.Owner.Node, ComplexifiedNode) as IFocusInsertionChildNodeIndex;
                            IndexList.Add(NodeIndex);
                        }

                        indexTable.Add(Inner, IndexList);
                        IsComplexifiable = true;
                    }
                }

                CurrentState = CurrentState.ParentState;
            }

            return IsComplexifiable;
        }

        /// <summary>
        /// Checks if an existing identifier at the focus can be split in two.
        /// </summary>
        /// <param name="inner">Inner to use to split the identifier upon return.</param>
        /// <param name="replaceIndex">Index of the identifier to replace upon return.</param>
        /// <param name="insertIndex">Index of the identifier to insert upon return.</param>
        /// <returns>True if an identifier can be split at the focus.</returns>
        public virtual bool IsIdentifierSplittable(out IFocusListInner inner, out IFocusInsertionListNodeIndex replaceIndex, out IFocusInsertionListNodeIndex insertIndex)
        {
            inner = null;
            replaceIndex = null;
            insertIndex = null;

            bool IsSplittable = false;

            IFocusNodeState IdentifierState = Focus.CellView.StateView.State;

            if (IdentifierState.Node is IIdentifier AsIdentifier)
            {
                IFocusNodeState ParentState = IdentifierState.ParentState;
                if (ParentState.Node is IQualifiedName)
                {
                    string Text = AsIdentifier.Text;
                    Debug.Assert(CaretPosition >= 0 && CaretPosition <= Text.Length);

                    inner = IdentifierState.ParentInner as IFocusListInner;
                    Debug.Assert(inner != null);

                    IFocusBrowsingListNodeIndex CurrentIndex = IdentifierState.ParentIndex as IFocusBrowsingListNodeIndex;
                    Debug.Assert(CurrentIndex != null);

                    IIdentifier FirstPart = NodeHelper.CreateSimpleIdentifier(Text.Substring(0, CaretPosition));
                    IIdentifier SecondPart = NodeHelper.CreateSimpleIdentifier(Text.Substring(CaretPosition));

                    replaceIndex = CurrentIndex.ToInsertionIndex(ParentState.Node, FirstPart) as IFocusInsertionListNodeIndex;
                    Debug.Assert(replaceIndex != null);

                    insertIndex = CurrentIndex.ToInsertionIndex(ParentState.Node, SecondPart) as IFocusInsertionListNodeIndex;
                    Debug.Assert(insertIndex != null);

                    insertIndex.MoveUp();

                    IsSplittable = true;
                }
            }

            return IsSplittable;
        }

        /// <summary>
        /// Checks if an existing block can have its replication status changed.
        /// </summary>
        /// <param name="inner">Inner to use to change the replication status upon return.</param>
        /// <param name="blockIndex">Index of the block that can be changed upon return.</param>
        /// <param name="replication">The current replication status upon return.</param>
        /// <returns>True if an existing block can have its replication status changed at the focus.</returns>
        public virtual bool IsReplicationModifiable(out IFocusBlockListInner inner, out int blockIndex, out ReplicationStatus replication)
        {
            inner = null;
            blockIndex = -1;
            replication = ReplicationStatus.Normal;

            bool IsModifiable = false;

            IFocusNodeState State = Focus.CellView.StateView.State;

            // Search recursively for a collection parent, up to 3 levels up.
            for (int i = 0; i < 3 && State != null; i++)
            {
                if (State is IFocusPatternState AsPatternState)
                {
                    IFocusBlockState ParentBlock = AsPatternState.ParentBlockState;
                    IFocusBlockListInner BlockListInner = ParentBlock.ParentInner as IFocusBlockListInner;
                    Debug.Assert(BlockListInner != null);

                    inner = BlockListInner;
                    blockIndex = inner.BlockStateList.IndexOf(ParentBlock);
                    replication = ParentBlock.ChildBlock.Replication;
                    IsModifiable = true;
                    break;
                }
                else if (State is IFocusSourceState AsSourceState)
                {
                    IFocusBlockState ParentBlock = AsSourceState.ParentBlockState;
                    IFocusBlockListInner BlockListInner = ParentBlock.ParentInner as IFocusBlockListInner;
                    Debug.Assert(BlockListInner != null);

                    inner = BlockListInner;
                    blockIndex = inner.BlockStateList.IndexOf(ParentBlock);
                    replication = ParentBlock.ChildBlock.Replication;
                    IsModifiable = true;
                    break;
                }
                else if (State.ParentInner is IFocusBlockListInner AsBlockListInner)
                {
                    inner = AsBlockListInner;
                    IFocusBrowsingExistingBlockNodeIndex ParentIndex = State.ParentIndex as IFocusBrowsingExistingBlockNodeIndex;
                    Debug.Assert(ParentIndex != null);
                    blockIndex = ParentIndex.BlockIndex;
                    replication = inner.BlockStateList[blockIndex].ChildBlock.Replication;
                    IsModifiable = true;
                    break;
                }

                State = State.ParentState;
            }

            return IsModifiable;
        }

        /// <summary>
        /// Changes the value of a text. The caret position is also moved for this view and other views where the caret is at the same focus and position.
        /// </summary>
        /// <param name="newText">The new text.</param>
        /// <param name="newCaretPosition">The new caret position.</param>
        /// <param name="changeCaretBeforeText">True if the caret should be changed before the text, to preserve the caret invariant.</param>
        public virtual void ChangeFocusedText(string newText, int newCaretPosition, bool changeCaretBeforeText)
        {
            Debug.Assert(Focus is IFocusTextFocus);

            bool IsHandled = false;
            IFocusNodeState State = Focus.CellView.StateView.State;
            IFocusIndex ParentIndex = State.ParentIndex;
            int OldCaretPosition = CaretPosition;

            if (Focus is IFocusStringContentFocus AsStringContentFocus)
            {
                IFocusStringContentFocusableCellView CellView = AsStringContentFocus.CellView;
                IFocusTextValueFrame Frame = CellView.Frame as IFocusTextValueFrame;
                Debug.Assert(Frame != null);

                if (Frame.AutoFormat)
                    switch (AutoFormatMode)
                    {
                        case AutoFormatModes.None:
                            IsHandled = true;
                            break;

                        case AutoFormatModes.FirstOnly:
                            newText = StringHelper.FirstOnlyFormattedText(newText);
                            IsHandled = true;
                            break;

                        case AutoFormatModes.FirstOrAll:
                            newText = StringHelper.FirstOrAllFormattedText(newText);
                            IsHandled = true;
                            break;

                        case AutoFormatModes.AllLowercase:
                            newText = StringHelper.AllLowercaseFormattedText(newText);
                            IsHandled = true;
                            break;
                    }
                else
                    IsHandled = true;

                Controller.ChangeTextAndCaretPosition(ParentIndex, AsStringContentFocus.CellView.PropertyName, newText, OldCaretPosition, newCaretPosition, changeCaretBeforeText);
            }

            else if (Focus is IFocusCommentFocus AsCommentFocus)
            {
                Controller.ChangeCommentAndCaretPosition(ParentIndex, newText, OldCaretPosition, newCaretPosition, changeCaretBeforeText);
                IsHandled = true;
            }

            Debug.Assert(IsHandled);
        }

        /// <summary>
        /// Extends the selection by one step, starting from the focus.
        /// </summary>
        /// <param name="isChanged">True upon return is the selection was changed.</param>
        public virtual void ExtendSelection(out bool isChanged)
        {
            isChanged = false;

            if (Selection is IFocusNodeSelection AsNodeSelection && AsNodeSelection.StateView == RootStateView)
                return;

            int OldSelectionExtension = SelectionExtension;
            ResetSelection();

            SelectionExtension = OldSelectionExtension + 1;
            Debug.Assert(SelectionExtension > 0);

            isChanged = true;

            for (int i = 0; i < SelectionExtension; i++)
                ExtendSelectionOneStep();

            // Debug.WriteLine($"<{SelectionExtension}> {Selection}");
        }

        /// <summary>
        /// Reduces the selection by one step, ending at the focus.
        /// </summary>
        /// <param name="isChanged">True upon return is the selection was changed.</param>
        public virtual void ReduceSelection(out bool isChanged)
        {
            isChanged = false;

            if (SelectionExtension == 0)
                return;

            int OldSelectionExtension = SelectionExtension;
            ResetSelection();

            SelectionExtension = OldSelectionExtension - 1;
            Debug.Assert(SelectionExtension >= 0);

            isChanged = true;

            for (int i = 0; i < SelectionExtension; i++)
                ExtendSelectionOneStep();

            // Debug.WriteLine($"<{SelectionExtension}> {Selection}");
        }

        private protected virtual void ExtendSelectionOneStep()
        {
            if (Selection == EmptySelection)
            {
                if (Focus is IFocusTextFocus AsTextFocus)
                    SetTextFullSelection(AsTextFocus);
                else
                    SelectNode(Focus.CellView.StateView.State);
            }

            else
                switch (Selection)
                {
                    case IFocusStringContentSelection AsStringContentSelection:
                        ExtendSelectionStringContent(AsStringContentSelection);
                        break;

                    case IFocusCommentSelection AsCommentSelection:
                        ExtendSelectionComment(AsCommentSelection);
                        break;

                    case IFocusDiscreteContentSelection AsDiscreteContentSelection:
                        SelectNode(AsDiscreteContentSelection.StateView.State);
                        break;

                    case IFocusNodeListSelection AsNodeListSelection:
                        ExtendSelectionNodeList(AsNodeListSelection);
                        break;

                    case IFocusBlockNodeListSelection AsBlockNodeListSelection:
                        ExtendSelectionBlockNodeList(AsBlockNodeListSelection);
                        break;

                    case IFocusBlockListSelection AsBlockListSelection:
                        ExtendSelectionBlockList(AsBlockListSelection);
                        break;

                    default:
                        ExtendSelectionOther();
                        break;
                }

            Debug.Assert(Selection != EmptySelection);
        }

        private protected virtual void ExtendSelectionStringContent(IFocusStringContentSelection selection)
        {
            IFocusStringContentFocus AsStringContentFocus = Focus as IFocusStringContentFocus;
            Debug.Assert(AsStringContentFocus != null);
            string Text = GetFocusedStringContent(AsStringContentFocus);

            Debug.Assert(selection.Start <= selection.End);

            int SelectedCount = selection.End - selection.Start;
            Debug.Assert(SelectedCount == Text.Length);

            SelectNode(selection.StateView.State);
        }

        private protected virtual void ExtendSelectionComment(IFocusCommentSelection selection)
        {
            IFocusCommentFocus AsCommentFocus = Focus as IFocusCommentFocus;
            Debug.Assert(AsCommentFocus != null);
            string Text = GetFocusedCommentText(AsCommentFocus);

            Debug.Assert(selection.Start <= selection.End);

            int SelectedCount = selection.End - selection.Start;
            Debug.Assert(SelectedCount == Text.Length);

            SelectNode(selection.StateView.State);
        }

        private protected virtual void ExtendSelectionNodeList(IFocusNodeListSelection selection)
        {
            string PropertyName = selection.PropertyName;
            IFocusNodeState State = selection.StateView.State;
            IFocusListInner ListInner = State.PropertyToInner(PropertyName) as IFocusListInner;

            Debug.Assert(ListInner != null);
            Debug.Assert(selection.StartIndex <= selection.EndIndex);

            int SelectedCount = selection.EndIndex - selection.StartIndex + 1;
            if (SelectedCount < ListInner.StateList.Count)
                selection.Update(0, ListInner.StateList.Count - 1);
            else
                SelectNode(selection.StateView.State);
        }

        private protected virtual void ExtendSelectionBlockNodeList(IFocusBlockNodeListSelection selection)
        {
            string PropertyName = selection.PropertyName;
            IFocusNodeState State = selection.StateView.State;
            IFocusBlockListInner BlockListInner = State.PropertyToInner(PropertyName) as IFocusBlockListInner;

            Debug.Assert(BlockListInner != null);
            Debug.Assert(selection.BlockIndex < BlockListInner.BlockStateList.Count);
            Debug.Assert(selection.StartIndex <= selection.EndIndex);

            IFocusBlockState BlockState = BlockListInner.BlockStateList[selection.BlockIndex];
            int SelectedCount = selection.EndIndex - selection.StartIndex + 1;
            if (SelectedCount < BlockState.StateList.Count)
                selection.Update(0, BlockState.StateList.Count - 1);
            else
                SelectBlockList(selection.StateView.State, selection.PropertyName, selection.BlockIndex, selection.BlockIndex);
        }

        private protected virtual void ExtendSelectionBlockList(IFocusBlockListSelection selection)
        {
            string PropertyName = selection.PropertyName;
            IFocusNodeState State = selection.StateView.State;
            IFocusBlockListInner BlockListInner = State.PropertyToInner(PropertyName) as IFocusBlockListInner;

            Debug.Assert(BlockListInner != null);
            Debug.Assert(selection.StartIndex <= selection.EndIndex);

            int SelectedCount = selection.EndIndex - selection.StartIndex + 1;
            if (SelectedCount < BlockListInner.BlockStateList.Count)
                selection.Update(0, BlockListInner.BlockStateList.Count - 1);
            else
                SelectNode(selection.StateView.State);
        }

        private protected virtual void ExtendSelectionOther()
        {
            IFocusNodeSelection AsNodeSelection = Selection as IFocusNodeSelection;
            Debug.Assert(AsNodeSelection != null);

            IFocusNodeState State = AsNodeSelection.StateView.State;
            IFocusNodeState ParentState = AsNodeSelection.StateView.State.ParentState;
            Debug.Assert(ParentState != null);

            if (State.ParentInner is IFocusListInner AsListInner && State.ParentIndex is IFocusBrowsingListNodeIndex AsListNodeIndex)
                SelectNodeList(ParentState, AsListInner.PropertyName, AsListNodeIndex.Index, AsListNodeIndex.Index);

            else if (State.ParentInner is IFocusBlockListInner AsBlockListInner && State.ParentIndex is IFocusBrowsingExistingBlockNodeIndex AsBlockNodeIndex)
                SelectBlockNodeList(ParentState, AsBlockListInner.PropertyName, AsBlockNodeIndex.BlockIndex, AsBlockNodeIndex.Index, AsBlockNodeIndex.Index);

            else
            {
                IFocusBlockState BlockState;
                if (State is IFocusPatternState AsPatternState)
                    BlockState = AsPatternState.ParentBlockState;
                else if (State is IFocusSourceState AsSourceState)
                    BlockState = AsSourceState.ParentBlockState;
                else
                    BlockState = null;

                if (BlockState != null)
                {
                    int BlockIndex = BlockState.ParentInner.BlockStateList.IndexOf(BlockState);
                    Debug.Assert(BlockIndex >= 0);

                    SelectBlockList(ParentState, BlockState.ParentInner.PropertyName, BlockIndex, BlockIndex);
                }
                else
                    SelectNode(ParentState);
            }
        }

        /// <summary>
        /// Clears the selection.
        /// </summary>
        public virtual void ClearSelection()
        {
            Selection = EmptySelection;
        }

        /// <summary>
        /// Selects the specified discrete content.
        /// </summary>
        /// <param name="state">The state with a discrete content property.</param>
        /// <param name="propertyName">The property name.</param>
        public virtual void SelectDiscreteContent(IFocusNodeState state, string propertyName)
        {
            Debug.Assert(StateViewTable.ContainsKey(state));
            Debug.Assert(state.ValuePropertyTypeTable.ContainsKey(propertyName));
            Debug.Assert(state.ValuePropertyTypeTable[propertyName] == ValuePropertyType.Boolean || state.ValuePropertyTypeTable[propertyName] == ValuePropertyType.Enum);

            IFocusNodeStateView stateView = StateViewTable[state];
            Selection = CreateDiscreteContentSelection(stateView, propertyName);
        }

        /// <summary>
        /// Selects the specified string content.
        /// </summary>
        /// <param name="state">The state with a string content property.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="start">Index of the first character of the selection.</param>
        /// <param name="end">Index following the last character of the selection.</param>
        public virtual void SelectStringContent(IFocusNodeState state, string propertyName, int start, int end)
        {
            Debug.Assert(StateViewTable.ContainsKey(state));
            Debug.Assert(state.ValuePropertyTypeTable.ContainsKey(propertyName));
            Debug.Assert(state.ValuePropertyTypeTable[propertyName] == ValuePropertyType.String);

            IFocusNodeStateView StateView = StateViewTable[state];

            int OldFocusIndex = FocusChain.IndexOf(Focus);
            int NewFocusIndex = -1;
            for (int i = 0; i < FocusChain.Count; i++)
            {
                IFocusFocus Item = FocusChain[i];
                if (Item.CellView is IFocusStringContentFocusableCellView AsStringContentFocusableCellView)
                {
                    if (AsStringContentFocusableCellView.StateView == StateView && AsStringContentFocusableCellView.PropertyName == propertyName)
                    {
                        NewFocusIndex = i;
                        break;
                    }
                }
            }

            Debug.Assert(NewFocusIndex >= 0);

            if (OldFocusIndex != NewFocusIndex)
                ChangeFocus(NewFocusIndex - OldFocusIndex, OldFocusIndex, NewFocusIndex, true, out bool IsRefreshed);

            SelectStringContent(StateView, propertyName, start, end);

            CaretAnchorPosition = start;
            CaretPosition = end;

            CheckCaretInvariant();
        }

        /// <summary></summary>
        protected virtual void SelectStringContent(IFocusNodeStateView stateView, string propertyName, int start, int end)
        {
            Selection = CreateStringContentSelection(stateView, propertyName, start, end);
        }

        /// <summary>
        /// Selects the specified string content.
        /// </summary>
        /// <param name="state">The state with the comment to select.</param>
        /// <param name="start">Index of the first character of the selection.</param>
        /// <param name="end">Index following the last character of the selection.</param>
        public virtual void SelectComment(IFocusNodeState state, int start, int end)
        {
            Debug.Assert(StateViewTable.ContainsKey(state));

            IFocusNodeStateView StateView = StateViewTable[state];
            SelectComment(StateView, start, end);

            CaretAnchorPosition = start;
            CaretPosition = end;

            CheckCaretInvariant();
        }

        /// <summary></summary>
        protected virtual void SelectComment(IFocusNodeStateView stateView, int start, int end)
        {
            Selection = CreateCommentSelection(stateView, start, end);
        }

        /// <summary>
        /// Selects the specified node.
        /// </summary>
        /// <param name="state">The state with the node to select.</param>
        public virtual void SelectNode(IFocusNodeState state)
        {
            Debug.Assert(StateViewTable.ContainsKey(state));

            IFocusNodeStateView stateView = StateViewTable[state];
            Selection = CreateNodeSelection(stateView);
        }

        /// <summary>
        /// Selects the specified list of nodes in a node list.
        /// </summary>
        /// <param name="state">The state with a node list property.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="startIndex">Index of the first node of the selection.</param>
        /// <param name="endIndex">Index following the last node of the selection.</param>
        public virtual void SelectNodeList(IFocusNodeState state, string propertyName, int startIndex, int endIndex)
        {
            Debug.Assert(StateViewTable.ContainsKey(state));
            Debug.Assert(state.InnerTable.ContainsKey(propertyName));

            IFocusListInner ParentInner = state.InnerTable[propertyName] as IFocusListInner;
            Debug.Assert(ParentInner != null);
            Debug.Assert(startIndex >= 0 && startIndex < ParentInner.StateList.Count);
            Debug.Assert(endIndex >= 0 && endIndex <= ParentInner.StateList.Count);

            IFocusNodeStateView stateView = StateViewTable[state];
            Selection = CreateNodeListSelection(stateView, propertyName, startIndex, endIndex);
        }

        /// <summary>
        /// Selects the specified list of nodes in a block of a block list.
        /// </summary>
        /// <param name="state">The state with a node list property.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="blockIndex">Index of the block.</param>
        /// <param name="startIndex">Index of the first node of the selection.</param>
        /// <param name="endIndex">Index of the last node of the selection.</param>
        public virtual void SelectBlockNodeList(IFocusNodeState state, string propertyName, int blockIndex, int startIndex, int endIndex)
        {
            Debug.Assert(StateViewTable.ContainsKey(state));
            Debug.Assert(state.InnerTable.ContainsKey(propertyName));

            IFocusBlockListInner ParentInner = state.InnerTable[propertyName] as IFocusBlockListInner;
            Debug.Assert(ParentInner != null);
            Debug.Assert(blockIndex >= 0 && blockIndex < ParentInner.BlockStateList.Count);

            IFocusBlockState BlockState = ParentInner.BlockStateList[blockIndex];
            Debug.Assert(startIndex >= 0 && startIndex < BlockState.StateList.Count);
            Debug.Assert(endIndex >= 0 && endIndex <= BlockState.StateList.Count);

            IFocusNodeStateView stateView = StateViewTable[state];
            Selection = CreateBlockNodeListSelection(stateView, propertyName, blockIndex, startIndex, endIndex);
        }

        /// <summary>
        /// Selects the specified list of blocks in a block list.
        /// </summary>
        /// <param name="state">The state with a node list property.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="startIndex">Index of the first block of the selection.</param>
        /// <param name="endIndex">Index of the last block of the selection.</param>
        public virtual void SelectBlockList(IFocusNodeState state, string propertyName, int startIndex, int endIndex)
        {
            Debug.Assert(StateViewTable.ContainsKey(state));
            Debug.Assert(state.InnerTable.ContainsKey(propertyName));

            IFocusBlockListInner ParentInner = state.InnerTable[propertyName] as IFocusBlockListInner;
            Debug.Assert(ParentInner != null);
            Debug.Assert(startIndex >= 0 && startIndex < ParentInner.BlockStateList.Count);
            Debug.Assert(endIndex >= 0 && endIndex <= ParentInner.BlockStateList.Count);

            IFocusNodeStateView stateView = StateViewTable[state];
            Selection = CreateBlockListSelection(stateView, propertyName, startIndex, endIndex);
        }

#if !TRAVIS
        /// <summary>
        /// Copy the selection in the clipboard.
        /// </summary>
        /// <param name="dataObject">The clipboard data object that can already contain other custom formats.</param>
        public virtual void CopySelection(IDataObject dataObject)
        {
            Selection.Copy(dataObject);
        }

        /// <summary>
        /// Copy the selection in the clipboard then removes it.
        /// </summary>
        /// <param name="dataObject">The clipboard data object that can already contain other custom formats.</param>
        /// <param name="isDeleted">True if something was deleted.</param>
        public virtual void CutSelection(IDataObject dataObject, out bool isDeleted)
        {
            Selection.Cut(dataObject, out isDeleted);

            if (isDeleted)
                ResetSelection();
        }

        /// <summary>
        /// Replaces the selection with the content of the clipboard.
        /// </summary>
        /// <param name="isChanged">True if something was replaced or added.</param>
        public virtual void PasteSelection(out bool isChanged)
        {
            Selection.Paste(out isChanged);
        }

        /// <summary>
        /// Deletes the selection.
        /// </summary>
        /// <param name="isDeleted">True if something was deleted.</param>
        public virtual void DeleteSelection(out bool isDeleted)
        {
            Selection.Delete(out isDeleted);

            if (isDeleted)
                ResetSelection();
        }
#endif

        /// <summary>
        /// Change auto formatting mode.
        /// </summary>
        public virtual void SetAutoFormatMode(AutoFormatModes mode)
        {
            if (AutoFormatMode != mode)
                AutoFormatMode = mode;
        }
#endregion

#region Implementation
        /// <summary>
        /// Handler called every time a block state is inserted in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockStateInserted(IWriteableInsertBlockOperation operation)
        {
            base.OnBlockStateInserted(operation);

            IFocusBlockState BlockState = ((IFocusInsertBlockOperation)operation).BlockState;

            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));

            Debug.Assert(StateViewTable.ContainsKey(BlockState.PatternState));
            Debug.Assert(StateViewTable.ContainsKey(BlockState.SourceState));

            Debug.Assert(BlockState.StateList.Count == 1);

            IFocusPlaceholderNodeState ChildState = ((IFocusInsertBlockOperation)operation).ChildState;
            Debug.Assert(ChildState == BlockState.StateList[0]);
            Debug.Assert(ChildState.ParentIndex == ((IFocusInsertBlockOperation)operation).BrowsingIndex);
            Debug.Assert(StateViewTable.ContainsKey(ChildState));
        }

        /// <summary>
        /// Handler called every time a block state is removed from the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockStateRemoved(IWriteableRemoveBlockOperation operation)
        {
            base.OnBlockStateRemoved(operation);

            IFocusBlockState BlockState = ((IFocusRemoveBlockOperation)operation).BlockState;

            Debug.Assert(BlockState != null);
            Debug.Assert(!BlockStateViewTable.ContainsKey(BlockState));

            Debug.Assert(!StateViewTable.ContainsKey(BlockState.PatternState));
            Debug.Assert(!StateViewTable.ContainsKey(BlockState.SourceState));

            IFocusNodeState RemovedState = ((IFocusRemoveBlockOperation)operation).RemovedState;
            Debug.Assert(!StateViewTable.ContainsKey(RemovedState));

            Debug.Assert(BlockState.StateList.Count == 0);
        }

        /// <summary>
        /// Handler called every time a block view must be removed from the controller view.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockViewRemoved(IWriteableRemoveBlockViewOperation operation)
        {
            base.OnBlockViewRemoved(operation);

            IFocusBlockState BlockState = ((IFocusRemoveBlockViewOperation)operation).BlockState;

            Debug.Assert(BlockState != null);
            Debug.Assert(!BlockStateViewTable.ContainsKey(BlockState));

            Debug.Assert(!StateViewTable.ContainsKey(BlockState.PatternState));
            Debug.Assert(!StateViewTable.ContainsKey(BlockState.SourceState));

            foreach (IFocusNodeState State in BlockState.StateList)
                Debug.Assert(!StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a state is inserted in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateInserted(IWriteableInsertNodeOperation operation)
        {
            base.OnStateInserted(operation);

            IFocusNodeState ChildState = ((IFocusInsertNodeOperation)operation).ChildState;
            Debug.Assert(ChildState != null);
            Debug.Assert(StateViewTable.ContainsKey(ChildState));

            IFocusBrowsingCollectionNodeIndex BrowsingIndex = ((IFocusInsertNodeOperation)operation).BrowsingIndex;
            Debug.Assert(ChildState.ParentIndex == BrowsingIndex);

            MoveFocusToState(ChildState);
        }

        /// <summary>
        /// Handler called every time a state is removed from the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateRemoved(IWriteableRemoveNodeOperation operation)
        {
            base.OnStateRemoved(operation);

            IFocusPlaceholderNodeState RemovedState = ((IFocusRemoveNodeOperation)operation).RemovedState;
            Debug.Assert(RemovedState != null);
            Debug.Assert(!StateViewTable.ContainsKey(RemovedState));
        }

        /// <summary>
        /// Handler called every time a state is inserted in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateReplaced(IWriteableReplaceOperation operation)
        {
            base.OnStateReplaced(operation);

            IFocusNodeState NewChildState = ((IFocusReplaceOperation)operation).NewChildState;
            Debug.Assert(NewChildState != null);
            Debug.Assert(StateViewTable.ContainsKey(NewChildState));

            IFocusBrowsingChildIndex OldBrowsingIndex = ((IFocusReplaceOperation)operation).OldBrowsingIndex;
            Debug.Assert(OldBrowsingIndex != null);
            Debug.Assert(NewChildState.ParentIndex != OldBrowsingIndex);

            IFocusBrowsingChildIndex NewBrowsingIndex = ((IFocusReplaceOperation)operation).NewBrowsingIndex;
            Debug.Assert(NewBrowsingIndex != null);
            Debug.Assert(NewChildState.ParentIndex == NewBrowsingIndex);
        }

        /// <summary>
        /// Handler called every time a state is assigned in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateAssigned(IWriteableAssignmentOperation operation)
        {
            base.OnStateAssigned(operation);

            IFocusOptionalNodeState State = ((IFocusAssignmentOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a state is unassigned in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateUnassigned(IWriteableAssignmentOperation operation)
        {
            base.OnStateUnassigned(operation);

            IFocusOptionalNodeState State = ((IFocusAssignmentOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a discrete value is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnDiscreteValueChanged(IWriteableChangeDiscreteValueOperation operation)
        {
            base.OnDiscreteValueChanged(operation);

            IFocusNodeState State = ((IFocusChangeDiscreteValueOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a text is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnTextChanged(IWriteableChangeTextOperation operation)
        {
            bool IsSameFocus = IsSameChangeTextOperationFocus((IFocusChangeTextOperation)operation);
            int NewCaretPosition = ((IFocusChangeCaretOperation)operation).NewCaretPosition;
            bool ChangeCaretBeforeText = ((IFocusChangeCaretOperation)operation).ChangeCaretBeforeText;

            if (IsSameFocus && NewCaretPosition >= 0 && ChangeCaretBeforeText)
                CaretPosition = NewCaretPosition;

            base.OnTextChanged(operation);

            if (IsSameFocus && NewCaretPosition >= 0 && !ChangeCaretBeforeText)
                CaretPosition = NewCaretPosition;

            IFocusNodeState State = ((IFocusChangeTextOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));

            ResetSelection();
            CaretAnchorPosition = CaretPosition;
            CheckCaretInvariant();
        }

        private protected virtual bool IsSameChangeTextOperationFocus(IFocusChangeTextOperation operation)
        {
            INode Node = null;
            if (operation.State.ParentIndex is IFocusNodeIndex AsNodeIndex)
                Node = AsNodeIndex.Node;
            else if (operation.State is IFocusOptionalNodeState AsOptionalNodeState && AsOptionalNodeState.ParentInner.IsAssigned)
                Node = AsOptionalNodeState.Node;

            Debug.Assert(Node != null);
            string PropertyName = operation.PropertyName;

            INode FocusedNode = null;
            IFocusFrame FocusedFrame = null;
            Focus.GetLocationInSourceCode(out FocusedNode, out FocusedFrame);

            return FocusedNode == Node && FocusedFrame is IFocusTextValueFrame AsTextValueFrame && AsTextValueFrame.PropertyName == PropertyName;
        }

        /// <summary>
        /// Handler called every time a comment is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnCommentChanged(IWriteableChangeCommentOperation operation)
        {
            bool IsSameFocus = IsSameChangeCommentOperationFocus((IFocusChangeCommentOperation)operation);
            int NewCaretPosition = ((IFocusChangeCaretOperation)operation).NewCaretPosition;
            bool ChangeCaretBeforeText = ((IFocusChangeCaretOperation)operation).ChangeCaretBeforeText;

            if (IsSameFocus && NewCaretPosition >= 0 && ChangeCaretBeforeText)
                CaretPosition = NewCaretPosition;

            base.OnCommentChanged(operation);

            if (IsSameFocus && NewCaretPosition >= 0 && !ChangeCaretBeforeText)
                CaretPosition = NewCaretPosition;

            IFocusNodeState State = ((IFocusChangeCommentOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));

            ResetSelection();
            CheckCaretInvariant();
        }

        private protected virtual bool IsSameChangeCommentOperationFocus(IFocusChangeCommentOperation operation)
        {
            INode Node = null;
            if (operation.State.ParentIndex is IFocusNodeIndex AsNodeIndex)
                Node = AsNodeIndex.Node;
            else if (operation.State is IFocusOptionalNodeState AsOptionalNodeState && AsOptionalNodeState.ParentInner.IsAssigned)
                Node = AsOptionalNodeState.Node;

            Debug.Assert(Node != null);

            INode FocusedNode = null;
            IFocusFrame FocusedFrame = null;
            Focus.GetLocationInSourceCode(out FocusedNode, out FocusedFrame);

            return FocusedNode == Node && FocusedFrame is IFocusCommentFrame;
        }

        /// <summary>
        /// Handler called every time a block state is changed in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockStateChanged(IWriteableChangeBlockOperation operation)
        {
            base.OnBlockStateChanged(operation);

            IFocusBlockState BlockState = ((IFocusChangeBlockOperation)operation).BlockState;
            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));
        }

        /// <summary>
        /// Handler called every time a state is moved in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnStateMoved(IWriteableMoveNodeOperation operation)
        {
            base.OnStateMoved(operation);

            IFocusPlaceholderNodeState State = ((IFocusMoveNodeOperation)operation).State;
            Debug.Assert(State != null);
            Debug.Assert(StateViewTable.ContainsKey(State));
        }

        /// <summary>
        /// Handler called every time a block state is moved in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockStateMoved(IWriteableMoveBlockOperation operation)
        {
            base.OnBlockStateMoved(operation);

            IFocusBlockState BlockState = ((IFocusMoveBlockOperation)operation).BlockState;
            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));
        }

        /// <summary>
        /// Handler called every time a block split in the controller.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlockSplit(IWriteableSplitBlockOperation operation)
        {
            base.OnBlockSplit(operation);

            IFocusBlockState BlockState = ((IFocusSplitBlockOperation)operation).BlockState;
            Debug.Assert(BlockState != null);
            Debug.Assert(BlockStateViewTable.ContainsKey(BlockState));
        }

        /// <summary>
        /// Handler called every time two blocks are merged.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnBlocksMerged(IWriteableMergeBlocksOperation operation)
        {
            base.OnBlocksMerged(operation);

            IFocusBlockState BlockState = ((IFocusMergeBlocksOperation)operation).BlockState;
            Debug.Assert(BlockState != null);
            Debug.Assert(!BlockStateViewTable.ContainsKey(BlockState));
        }

        /// <summary>
        /// Handler called to refresh views.
        /// </summary>
        /// <param name="operation">Details of the operation performed.</param>
        private protected override void OnGenericRefresh(IWriteableGenericRefreshOperation operation)
        {
            base.OnGenericRefresh(operation);

            IFocusNodeState RefreshState = ((IFocusGenericRefreshOperation)operation).RefreshState;
            Debug.Assert(RefreshState != null);
            Debug.Assert(StateViewTable.ContainsKey(RefreshState));
        }

        private protected override IFrameCellViewTreeContext InitializedCellViewTreeContext(IFrameNodeStateView stateView)
        {
            IFocusCellViewTreeContext Context = (IFocusCellViewTreeContext)CreateCellViewTreeContext(stateView);
            ForcedCommentStateView = null;

            IList<IFocusFrameSelectorList> SelectorStack = ((IFocusNodeStateView)stateView).GetSelectorStack();
            foreach (IFocusFrameSelectorList Selectors in SelectorStack)
                Context.AddSelectors(Selectors);

            return Context;
        }

        private protected override void CloseCellViewTreeContext(IFrameCellViewTreeContext context)
        {
            Debug.Assert(((IFocusCellViewTreeContext)context).ForcedCommentStateView == null);
        }

        private protected override void Refresh(IFrameNodeState state)
        {
            INode FocusedNode = null;
            IFocusFrame FocusedFrame = null;

            if (Focus != null)
                Focus.GetLocationInSourceCode(out FocusedNode, out FocusedFrame);

            base.Refresh(state);

            UpdateFocusChain((IFocusNodeState)state, FocusedNode, FocusedFrame);
        }

        private protected virtual void UpdateFocusChain(IFocusNodeState state, INode focusedNode, IFocusFrame focusedFrame)
        {
            IFocusFocusList NewFocusChain = CreateFocusChain();
            IFocusNodeState RootState = Controller.RootState;
            IFocusNodeStateView RootStateView = StateViewTable[RootState];

            IFocusFocus MatchingFocus = null;
            RootStateView.UpdateFocusChain(NewFocusChain, focusedNode, focusedFrame, ref MatchingFocus);

            // Ensured by all templates having at least one preferred (hence focusable) frame.
            Debug.Assert(NewFocusChain.Count > 0);

            // First run, initialize the focus to the first focusable cell.
            if (Focus == null)
            {
                Debug.Assert(FocusChain == null);
                Focus = NewFocusChain[0];
                ResetCaretPosition(0, true);
            }
            else if (MatchingFocus != null)
            {
                Focus = MatchingFocus;
                UpdateMaxCaretPosition(); // The focus didn't change, but the content may have.
            }
            else
                RecoverFocus(state, NewFocusChain); // The focus has forcibly changed.

            FocusChain = NewFocusChain;
            DebugObjects.AddReference(NewFocusChain);

            Debug.Assert(Focus != null);
            Debug.Assert(FocusChain.Contains(Focus));

            SelectionAnchor = Focus.CellView.StateView;
            SelectionExtension = 0;
        }

        private protected virtual void RecoverFocus(IFocusNodeState state, IFocusFocusList newFocusChain)
        {
            IFocusNodeState CurrentState = state;
            List<IFocusNodeStateView> StateViewList = new List<IFocusNodeStateView>();
            IFocusNodeStateView MainStateView = null;
            List<IFocusFocus> SameStateFocusableList = new List<IFocusFocus>();

            // Get the state that should have the focus and all its children.
            while (CurrentState != null && !GetFocusedStateAndChildren(newFocusChain, CurrentState, out MainStateView, out StateViewList, out SameStateFocusableList))
                CurrentState = CurrentState.ParentState;

            Debug.Assert(SameStateFocusableList.Count > 0);

            // Now that we have found candidates, try to select the original frame.
            bool IsFrameChanged = true;

            IFocusFrame Frame = Focus.CellView.Frame;
            foreach (IFocusFocus CellFocus in SameStateFocusableList)
                if (CellFocus.CellView.Frame == Frame)
                {
                    IsFrameChanged = false;
                    Focus = CellFocus;
                    break;
                }

            // If the frame has changed, use a preferred frame.
            if (IsFrameChanged)
                FindPreferredFrame(MainStateView, SameStateFocusableList);

            ResetCaretPosition(0, true);
        }

        private protected virtual void FindPreferredFrame(IFocusNodeStateView mainStateView, List<IFocusFocus> sameStateFocusableList)
        {
            bool IsFrameSet = false;
            IFocusNodeTemplate Template = mainStateView.Template as IFocusNodeTemplate;
            Debug.Assert(Template != null);

            Template.GetPreferredFrame(out IFocusNodeFrame FirstPreferredFrame, out IFocusNodeFrame LastPreferredFrame);
            Debug.Assert(FirstPreferredFrame != null);
            Debug.Assert(LastPreferredFrame != null);

            foreach (IFocusFocus CellFocus in sameStateFocusableList)
            {
                IFocusFocusableCellView CellView = CellFocus.CellView;
                if (CellView.Frame == FirstPreferredFrame || CellView.Frame == LastPreferredFrame)
                {
                    IsFrameSet = true;
                    Focus = CellFocus;
                    break;
                }
            }

            // If none of the preferred frames are visible, use the first focusable cell.
            if (!IsFrameSet)
                Focus = sameStateFocusableList[0];
        }

        private protected virtual bool GetFocusedStateAndChildren(IFocusFocusList newFocusChain, IFocusNodeState state, out IFocusNodeStateView mainStateView, out List<IFocusNodeStateView> stateViewList, out List<IFocusFocus> sameStateFocusableList)
        {
            mainStateView = StateViewTable[state];

            stateViewList = new List<IFocusNodeStateView>();
            sameStateFocusableList = new List<IFocusFocus>();
            GetChildrenStateView(mainStateView, stateViewList);

            // Find all focusable cells belonging to these states.
            foreach (IFocusFocus NewFocus in newFocusChain)
            {
                IFocusFocusableCellView CellView = NewFocus.CellView;

                foreach (IFocusNodeStateView StateView in stateViewList)
                    if (CellView.StateView == StateView && !sameStateFocusableList.Contains(NewFocus))
                    {
                        sameStateFocusableList.Add(NewFocus);
                        break;
                    }
            }

            if (sameStateFocusableList.Count > 0)
                return true;

            // If it doesn't work, try the parent state, down to the root (in case of a removal or unassign).
            return false;
        }

        private protected virtual void GetChildrenStateView(IFocusNodeStateView stateView, List<IFocusNodeStateView> stateViewList)
        {
            stateViewList.Add(stateView);

            foreach (KeyValuePair<string, IFocusInner> Entry in stateView.State.InnerTable)
            {
                bool IsHandled = false;
                IFocusTemplate Template;
                IFocusAssignableCellViewReadOnlyDictionary<string> CellViewTable;
                IFocusCellViewCollection EmbeddingCellView;

                if (Entry.Value is IFocusPlaceholderInner<IFocusBrowsingPlaceholderNodeIndex> AsPlaceholderInner)
                {
                    Debug.Assert(StateViewTable.ContainsKey(AsPlaceholderInner.ChildState));
                    IFocusNodeStateView ChildStateView = StateViewTable[AsPlaceholderInner.ChildState];
                    Debug.Assert(ChildStateView != null);

                    GetChildrenStateView(ChildStateView, stateViewList);

                    Template = ChildStateView.Template;
                    CellViewTable = ChildStateView.CellViewTable;

                    IsHandled = true;
                }
                else if (Entry.Value is IFocusOptionalInner<IFocusBrowsingOptionalNodeIndex> AsOptionalInner)
                {
                    if (AsOptionalInner.IsAssigned)
                    {
                        Debug.Assert(StateViewTable.ContainsKey(AsOptionalInner.ChildState));
                        IFocusOptionalNodeStateView ChildStateView = StateViewTable[AsOptionalInner.ChildState] as IFocusOptionalNodeStateView;
                        Debug.Assert(ChildStateView != null);

                        GetChildrenStateView(ChildStateView, stateViewList);

                        Template = ChildStateView.Template;
                        CellViewTable = ChildStateView.CellViewTable;
                    }

                    IsHandled = true;
                }
                else if (Entry.Value is IFocusListInner<IFocusBrowsingListNodeIndex> AsListInner)
                {
                    foreach (IFocusNodeState ChildState in AsListInner.StateList)
                    {
                        IFocusNodeStateView ChildStateView = StateViewTable[ChildState];
                        GetChildrenStateView(ChildStateView, stateViewList);

                        Template = ChildStateView.Template;
                        CellViewTable = ChildStateView.CellViewTable;
                    }

                    IsHandled = true;
                }
                else if (Entry.Value is IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> AsBlockListInner)
                {
                    foreach (IFocusBlockState BlockState in AsBlockListInner.BlockStateList)
                    {
                        IFocusNodeStateView PatternStateView = StateViewTable[BlockState.PatternState];
                        GetChildrenStateView(PatternStateView, stateViewList);
                        Template = PatternStateView.Template;
                        CellViewTable = PatternStateView.CellViewTable;

                        IFocusNodeStateView SourceStateView = StateViewTable[BlockState.SourceState];
                        GetChildrenStateView(SourceStateView, stateViewList);
                        Template = SourceStateView.Template;
                        CellViewTable = SourceStateView.CellViewTable;

                        IFocusBlockStateView BlockStateView = BlockStateViewTable[BlockState];
                        Template = BlockStateView.Template;
                        EmbeddingCellView = BlockStateView.EmbeddingCellView;

                        foreach (IFocusNodeState ChildState in BlockState.StateList)
                        {
                            IFocusNodeStateView ChildStateView = StateViewTable[ChildState];
                            GetChildrenStateView(ChildStateView, stateViewList);

                            Template = ChildStateView.Template;
                            CellViewTable = ChildStateView.CellViewTable;
                        }
                    }

                    IsHandled = true;
                }

                Debug.Assert(IsHandled);
            }
        }

        private protected virtual void ResetCaretPosition(int direction, bool resetCaretAnchor)
        {
            if (Focus is IFocusTextFocus AsTextFocus)
            {
                string Text = GetFocusedText(AsTextFocus);

                MaxCaretPosition = Text.Length;

                if (direction >= 0)
                    CaretPosition = 0;
                else
                    CaretPosition = MaxCaretPosition;

                if (resetCaretAnchor)
                    CaretAnchorPosition = CaretPosition;

                CheckCaretInvariant(Text);
            }
            else
            {
                CaretPosition = -1;
                if (resetCaretAnchor)
                    CaretAnchorPosition = -1;
                MaxCaretPosition = -1;
            }
        }

        private protected virtual void UpdateMaxCaretPosition()
        {
            if (Focus is IFocusTextFocus AsTextFocus)
            {
                string Text = GetFocusedText(AsTextFocus);

                MaxCaretPosition = Text.Length;
                CheckCaretInvariant(Text);
            }
        }

        private protected virtual void CheckCaretInvariant()
        {
            if (Focus is IFocusTextFocus AsTextFocus)
            {
                string Text = GetFocusedText(AsTextFocus);
                CheckCaretInvariant(Text);
            }
            else
            {
                Debug.Assert(CaretPosition == -1);
                Debug.Assert(CaretAnchorPosition == -1);
                Debug.Assert(MaxCaretPosition == -1);
            }
        }

        private protected virtual void CheckCaretInvariant(string text)
        {
            Debug.Assert(text != null);
            Debug.Assert(CaretPosition >= 0);
            Debug.Assert(MaxCaretPosition == text.Length);
            Debug.Assert(CaretPosition <= MaxCaretPosition);
        }

        private protected virtual void ResetSelection()
        {
            if (Focus is IFocusDiscreteContentFocus AsDiscreteContentFocus)
            {
                IFocusDiscreteContentFocusableCellView CellView = AsDiscreteContentFocus.CellView;
                SelectDiscreteContent(CellView.StateView.State, CellView.PropertyName);
            }
            else
                ClearSelection();

            SelectionAnchor = Focus.CellView.StateView;
            SelectionExtension = 0;
        }

        private protected virtual void SetTextSelection(IFocusTextFocus textFocus)
        {
            bool IsHandled = false;

            int Start = CaretAnchorPosition >= 0 ? CaretAnchorPosition : 0;
            int End = CaretPosition;

            if (textFocus is IFocusStringContentFocus AsStringContentFocus)
            {
                IFocusStringContentFocusableCellView CellView = AsStringContentFocus.CellView;
                SelectStringContent(CellView.StateView, CellView.PropertyName, Start, End);
                IsHandled = true;
            }
            else if (textFocus is IFocusCommentFocus AsCommentFocus)
            {
                IFocusCommentCellView CellView = AsCommentFocus.CellView;
                SelectComment(CellView.StateView, Start, End);
                IsHandled = true;
            }

            Debug.Assert(IsHandled);
        }

        private protected virtual void SetTextFullSelection(IFocusTextFocus textFocus)
        {
            bool IsHandled = false;

            if (textFocus is IFocusStringContentFocus AsStringContentFocus)
            {
                string Text = GetFocusedStringContent(AsStringContentFocus);
                IFocusStringContentFocusableCellView CellView = AsStringContentFocus.CellView;
                SelectStringContent(CellView.StateView, CellView.PropertyName, 0, Text.Length);
                IsHandled = true;
            }
            else if (textFocus is IFocusCommentFocus AsCommentFocus)
            {
                string Text = GetFocusedCommentText(AsCommentFocus);
                IFocusCommentCellView CellView = AsCommentFocus.CellView;
                SelectComment(CellView.StateView, 0, Text.Length);
                IsHandled = true;
            }

            Debug.Assert(IsHandled);
        }

        private protected override IFrameFrame GetAssociatedFrame(IFrameInner<IFrameBrowsingChildIndex> inner)
        {
            IFocusNodeState Owner = ((IFocusInner<IFocusBrowsingChildIndex>)inner).Owner;
            IFocusNodeStateView StateView = StateViewTable[Owner];
            IList<IFocusFrameSelectorList> SelectorStack = StateView.GetSelectorStack();

            IFocusFrame AssociatedFrame = TemplateSet.InnerToFrame((IFocusInner<IFocusBrowsingChildIndex>)inner, SelectorStack) as IFocusFrame;

            return AssociatedFrame;
        }

        private protected override void ValidateBlockCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameBlockCellView blockCellView)
        {
            Debug.Assert(((IFocusBlockCellView)blockCellView).StateView == (IFocusNodeStateView)stateView);
            Debug.Assert(((IFocusBlockCellView)blockCellView).ParentCellView == (IFocusCellViewCollection)parentCellView);
        }

        private protected override void ValidateContainerCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView, IFrameContainerCellView containerCellView)
        {
            Debug.Assert(((IFocusContainerCellView)containerCellView).StateView == (IFocusNodeStateView)stateView);
            Debug.Assert(((IFocusContainerCellView)containerCellView).ParentCellView == (IFocusCellViewCollection)parentCellView);
            Debug.Assert(((IFocusContainerCellView)containerCellView).ChildStateView == (IFocusNodeStateView)childStateView);
        }

        private protected virtual string GetFocusedText(IFocusTextFocus textCellFocus)
        {
            string Text = null;

            if (Focus is IFocusStringContentFocus AsTextFocus)
                Text = GetFocusedStringContent(AsTextFocus);

            else if (Focus is IFocusCommentFocus AsCommentFocus)
            {
                Text = GetFocusedCommentText(AsCommentFocus);
                if (Text == null)
                    Text = string.Empty;
            }

            Debug.Assert(Text != null);
            return Text;
        }

        private protected virtual string GetFocusedStringContent(IFocusStringContentFocus textCellFocus)
        {
            IFocusStringContentFocusableCellView CellView = textCellFocus.CellView;
            INode Node = CellView.StateView.State.Node;
            string PropertyName = CellView.PropertyName;

            return NodeTreeHelper.GetString(Node, PropertyName);
        }

        private protected virtual string GetFocusedCommentText(IFocusCommentFocus commentFocus)
        {
            IFocusCommentCellView CellView = commentFocus.CellView;
            IDocument Documentation = CellView.StateView.State.Node.Documentation;

            return CommentHelper.Get(Documentation);
        }

        private protected IFocusNodeStateView ForcedCommentStateView { get; private set; }
#endregion

#region Implementation of IFocusInternalControllerView
        /// <summary>
        /// Checks if the template associated to the <paramref name="propertyName"/> property of the <paramref name="stateView"/> state is complex.
        /// </summary>
        /// <param name="stateView">The state view for the node with property <paramref name="propertyName"/>.</param>
        /// <param name="propertyName">Name of the property pointing to the template to check.</param>
        public virtual bool IsTemplateComplex(IFocusNodeStateView stateView, string propertyName)
        {
            IFocusNodeState State = stateView.State;
            Debug.Assert(State.InnerTable.ContainsKey(propertyName));

            IFocusPlaceholderInner ParentInner = State.InnerTable[propertyName] as IFocusPlaceholderInner;
            Debug.Assert(ParentInner != null);

            NodeTreeHelperChild.GetChildNode(stateView.State.Node, propertyName, out INode ChildNode);
            Debug.Assert(ChildNode != null);

            Type NodeType = ChildNode.GetType();
            Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(NodeType);
            Debug.Assert(TemplateSet.NodeTemplateTable.ContainsKey(InterfaceType));

            IFocusNodeTemplate ParentTemplate = TemplateSet.NodeTemplateTable[InterfaceType] as IFocusNodeTemplate;
            Debug.Assert(ParentTemplate != null);

            return ParentTemplate.IsComplex;
        }

        /// <summary>
        /// Checks if the collection associated to the <paramref name="propertyName"/> property of the <paramref name="stateView"/> state has more than <paramref name="count"/> item.
        /// </summary>
        /// <param name="stateView">The state view for the node with property <paramref name="propertyName"/>.</param>
        /// <param name="propertyName">Name of the property pointing to the collection to check.</param>
        /// <param name="count">The number of items.</param>
        public virtual bool CollectionHasItems(IFocusNodeStateView stateView, string propertyName, int count)
        {
            IFocusNodeState State = stateView.State;
            Debug.Assert(State.InnerTable.ContainsKey(propertyName));

            bool IsHandled = false;
            bool HasItems = false;

            switch (State.InnerTable[propertyName])
            {
                case IFocusListInner AsListInner:
                    HasItems = AsListInner.Count > count;
                    IsHandled = true;
                    break;

                case IFocusBlockListInner AsBlockListInner:
                    HasItems = AsBlockListInner.Count > count;
                    IsHandled = true;
                    break;
            }

            Debug.Assert(IsHandled);

            return HasItems;
        }

        /// <summary>
        /// Checks if the optional node associated to <paramref name="propertyName"/> is assigned.
        /// </summary>
        /// <param name="stateView">The state view for the node with property <paramref name="propertyName"/>.</param>
        /// <param name="propertyName">Name of the property pointing to the node to check.</param>
        public virtual bool IsOptionalNodeAssigned(IFocusNodeStateView stateView, string propertyName)
        {
            IFocusNodeState State = stateView.State;
            Debug.Assert(State.InnerTable.ContainsKey(propertyName));

            IFocusOptionalInner OptionalInner = State.InnerTable[propertyName] as IFocusOptionalInner;
            Debug.Assert(OptionalInner != null);

            return OptionalInner.IsAssigned;
        }

        /// <summary>
        /// Checks if the enum or boolean associated to the <paramref name="propertyName"/> property of the <paramref name="stateView"/> state has value <paramref name="defaultValue"/>.
        /// </summary>
        /// <param name="stateView">The state view for the node with property <paramref name="propertyName"/>.</param>
        /// <param name="propertyName">Name of the property pointing to the template to check.</param>
        /// <param name="defaultValue">Expected default value.</param>
        public virtual bool DiscreteHasDefaultValue(IFocusNodeStateView stateView, string propertyName, int defaultValue)
        {
            IFocusNodeState State = stateView.State;
            Debug.Assert(State.ValuePropertyTypeTable.ContainsKey(propertyName));

            bool IsHandled = false;
            bool Result = false;

            switch (State.ValuePropertyTypeTable[propertyName])
            {
                case ValuePropertyType.Boolean:
                case ValuePropertyType.Enum:
                    Result = NodeTreeHelper.GetEnumValue(State.Node, propertyName) == defaultValue;
                    IsHandled = true;
                    break;
            }

            Debug.Assert(IsHandled);

            return Result;
        }

        /// <summary>
        /// Checks if the <paramref name="stateView"/> state is the first in a collection in the parent.
        /// </summary>
        /// <param name="stateView">The state view.</param>
        public virtual bool IsFirstItem(IFocusNodeStateView stateView)
        {
            Debug.Assert(stateView != null);

            IFocusNodeState State = stateView.State;
            Debug.Assert(State != null);

            IFocusInner ParentInner = State.ParentInner;

            IFocusPlaceholderNodeState PlaceholderNodeState;
            IFocusPlaceholderNodeStateReadOnlyList StateList;
            int Index;
            bool Result;

            switch (ParentInner)
            {
                case IFocusListInner AsListInner:
                    PlaceholderNodeState = State as IFocusPlaceholderNodeState;
                    Debug.Assert(PlaceholderNodeState != null);

                    StateList = AsListInner.StateList;
                    Index = StateList.IndexOf(PlaceholderNodeState);
                    Debug.Assert(Index >= 0 && Index < StateList.Count);
                    Result = Index == 0;
                    break;

                case IFocusBlockListInner AsBlockListInner:
                    PlaceholderNodeState = State as IFocusPlaceholderNodeState;
                    Debug.Assert(PlaceholderNodeState != null);

                    Result = false;
                    for (int BlockIndex = 0; BlockIndex < AsBlockListInner.BlockStateList.Count; BlockIndex++)
                    {
                        StateList = AsBlockListInner.BlockStateList[BlockIndex].StateList;
                        Index = StateList.IndexOf(PlaceholderNodeState);
                        if (Index >= 0)
                        {
                            Debug.Assert(Index < StateList.Count);
                            Result = BlockIndex == 0 && Index == 0;
                        }
                    }
                    break;

                default:
                    Result = true;
                    break;
            }

            return Result;
        }

        /// <summary>
        /// Checks if the <paramref name="blockStateView"/> block state belongs to a replicated block.
        /// </summary>
        /// <param name="blockStateView">The block state view.</param>
        public virtual bool IsInReplicatedBlock(IFocusBlockStateView blockStateView)
        {
            IFocusBlockState BlockState = blockStateView.BlockState;
            Debug.Assert(BlockState != null);

            return BlockState.ChildBlock.Replication == BaseNode.ReplicationStatus.Replicated;
        }

        /// <summary>
        /// Checks if the string associated to the <paramref name="propertyName"/> property of the <paramref name="stateView"/> state matches the pattern in <paramref name="textPattern"/>.
        /// </summary>
        /// <param name="stateView">The state view for the node with property <paramref name="propertyName"/>.</param>
        /// <param name="propertyName">Name of the property pointing to the template to check.</param>
        /// <param name="textPattern">Expected text.</param>
        public virtual bool StringMatchTextPattern(IFocusNodeStateView stateView, string propertyName, string textPattern)
        {
            IFocusNodeState State = stateView.State;
            Debug.Assert(State.InnerTable.ContainsKey(propertyName));

            bool IsHandled = false;
            bool Result = false;

            switch (State.InnerTable[propertyName])
            {
                case IFocusPlaceholderInner AsPlaceholderInner:
                    Debug.Assert(AsPlaceholderInner.InterfaceType == typeof(IIdentifier));
                    IFocusPlaceholderNodeState ChildState = AsPlaceholderInner.ChildState as IFocusPlaceholderNodeState;
                    Debug.Assert(ChildState != null);
                    Result = NodeTreeHelper.GetString(ChildState.Node, nameof(IIdentifier.Text)) == textPattern;
                    IsHandled = true;
                    break;
            }

            Debug.Assert(IsHandled);
            return Result;
        }
#endregion

#region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusControllerView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusControllerView AsControllerView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsControllerView))
                return comparer.Failed();

            return true;
        }

        /// <summary></summary>
        protected virtual ulong FocusHash
        {
            get
            {
                ulong Hash = 0;

#if DEBUG
                foreach (IFocusFocus Item in FocusChain)
                    if (Item != Focus)
                        MergeHash(ref Hash, ((FocusFocus)Item).Hash);

                if (CaretPosition >= 0)
                {
                    Debug.Assert(CaretPosition <= MaxCaretPosition);

                    MergeHash(ref Hash, (ulong)CaretPosition);
                    MergeHash(ref Hash, (ulong)MaxCaretPosition);
                }
                else
                {
                    Debug.Assert(CaretPosition == -1);
                    Debug.Assert(MaxCaretPosition == -1);
                }

                MergeHash(ref Hash, (ulong)CaretAnchorPosition);
                MergeHash(ref Hash, (ulong)CaretMode);
#endif

                return Hash;
            }
        }

        /// <summary></summary>
        protected virtual void MergeHash(ref ulong hash, ulong value)
        {
            hash ^= CRC32.Get(value);
        }
#endregion

#region Create Methods
        /// <summary>
        /// Creates a IxxxStateViewDictionary object.
        /// </summary>
        private protected override IReadOnlyStateViewDictionary CreateStateViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusStateViewDictionary();
        }

        /// <summary>
        /// Creates a IxxxBlockStateViewDictionary object.
        /// </summary>
        private protected override IReadOnlyBlockStateViewDictionary CreateBlockStateViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusBlockStateViewDictionary();
        }

        /// <summary>
        /// Creates a IxxxAttachCallbackSet object.
        /// </summary>
        private protected override IReadOnlyAttachCallbackSet CreateCallbackSet()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusAttachCallbackSet()
            {
                NodeStateAttachedHandler = OnNodeStateCreated,
                NodeStateDetachedHandler = OnNodeStateRemoved,
                BlockListInnerAttachedHandler = OnBlockListInnerCreated,
                BlockListInnerDetachedHandler = OnBlockListInnerRemoved,
                BlockStateAttachedHandler = OnBlockStateCreated,
                BlockStateDetachedHandler = OnBlockStateRemoved,
            };
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeStateView object.
        /// </summary>
        private protected override IReadOnlyPlaceholderNodeStateView CreatePlaceholderNodeStateView(IReadOnlyPlaceholderNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusPlaceholderNodeStateView(this, (IFocusPlaceholderNodeState)state);
        }

        /// <summary>
        /// Creates a IxxxOptionalNodeStateView object.
        /// </summary>
        private protected override IReadOnlyOptionalNodeStateView CreateOptionalNodeStateView(IReadOnlyOptionalNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusOptionalNodeStateView(this, (IFocusOptionalNodeState)state);
        }

        /// <summary>
        /// Creates a IxxxPatternStateView object.
        /// </summary>
        private protected override IReadOnlyPatternStateView CreatePatternStateView(IReadOnlyPatternState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusPatternStateView(this, (IFocusPatternState)state);
        }

        /// <summary>
        /// Creates a IxxxSourceStateView object.
        /// </summary>
        private protected override IReadOnlySourceStateView CreateSourceStateView(IReadOnlySourceState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusSourceStateView(this, (IFocusSourceState)state);
        }

        /// <summary>
        /// Creates a IxxxBlockStateView object.
        /// </summary>
        private protected override IReadOnlyBlockStateView CreateBlockStateView(IReadOnlyBlockState blockState)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusBlockStateView(this, (IFocusBlockState)blockState);
        }

        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        private protected override IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView, IFrameFrame frame)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusContainerCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, (IFocusNodeStateView)childStateView, (IFocusFrame)frame);
        }

        /// <summary>
        /// Creates a IxxxBlockCellView object.
        /// </summary>
        private protected override IFrameBlockCellView CreateBlockCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameBlockStateView blockStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusBlockCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, (IFocusBlockStateView)blockStateView);
        }

        /// <summary>
        /// Creates a IxxxCellViewTreeContext object.
        /// </summary>
        private protected override IFrameCellViewTreeContext CreateCellViewTreeContext(IFrameNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusCellViewTreeContext(this, (IFocusNodeStateView)stateView, ForcedCommentStateView);
        }

        /// <summary>
        /// Creates a IxxxFocusList object.
        /// </summary>
        private protected virtual IFocusFocusList CreateFocusChain()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusFocusList();
        }

        /// <summary>
        /// Creates a IxxxInsertionNewBlockNodeIndex object.
        /// </summary>
        private protected virtual IFocusInsertionNewBlockNodeIndex CreateNewBlockNodeIndex(INode parentNode, string propertyName, INode node, int blockIndex, IPattern patternNode, IIdentifier sourceNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusInsertionNewBlockNodeIndex(parentNode, propertyName, node, 0, patternNode, sourceNode);
        }

        /// <summary>
        /// Creates a IxxxInsertionExistingBlockNodeIndex object.
        /// </summary>
        private protected virtual IFocusInsertionExistingBlockNodeIndex CreateExistingBlockNodeIndex(INode parentNode, string propertyName, INode node, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusInsertionExistingBlockNodeIndex(parentNode, propertyName, node, blockIndex, index);
        }

        /// <summary>
        /// Creates a IxxxInsertionListNodeIndex object.
        /// </summary>
        private protected virtual IFocusInsertionListNodeIndex CreateListNodeIndex(INode parentNode, string propertyName, INode node, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusInsertionListNodeIndex(parentNode, propertyName, node, index);
        }

        /// <summary>
        /// Creates a IxxxEmptySelection object.
        /// </summary>
        private protected virtual IFocusEmptySelection CreateEmptySelection()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusEmptySelection(RootStateView);
        }

        /// <summary>
        /// Creates a IxxxDiscreteContentSelection object.
        /// </summary>
        private protected virtual IFocusDiscreteContentSelection CreateDiscreteContentSelection(IFocusNodeStateView stateView, string propertyName)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusDiscreteContentSelection(stateView, propertyName);
        }

        /// <summary>
        /// Creates a IxxxStringContentSelection object.
        /// </summary>
        private protected virtual IFocusStringContentSelection CreateStringContentSelection(IFocusNodeStateView stateView, string propertyName, int start, int end)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusStringContentSelection(stateView, propertyName, start, end);
        }

        /// <summary>
        /// Creates a IxxxCommentSelection object.
        /// </summary>
        private protected virtual IFocusCommentSelection CreateCommentSelection(IFocusNodeStateView stateView, int start, int end)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusCommentSelection(stateView, start, end);
        }

        /// <summary>
        /// Creates a IxxxNodeSelection object.
        /// </summary>
        private protected virtual IFocusNodeSelection CreateNodeSelection(IFocusNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusNodeSelection(stateView);
        }

        /// <summary>
        /// Creates a IxxxListNodeSelection object.
        /// </summary>
        private protected virtual IFocusNodeListSelection CreateNodeListSelection(IFocusNodeStateView stateView, string propertyName, int startIndex, int endIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusNodeListSelection(stateView, propertyName, startIndex, endIndex);
        }

        /// <summary>
        /// Creates a IxxxBlockListNodeSelection object.
        /// </summary>
        private protected virtual IFocusBlockNodeListSelection CreateBlockNodeListSelection(IFocusNodeStateView stateView, string propertyName, int blockIndex, int startIndex, int endIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusBlockNodeListSelection(stateView, propertyName, blockIndex, startIndex, endIndex);
        }

        /// <summary>
        /// Creates a IxxxBlockSelection object.
        /// </summary>
        private protected virtual IFocusBlockListSelection CreateBlockListSelection(IFocusNodeStateView stateView, string propertyName, int startIndex, int endIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusControllerView));
            return new FocusBlockListSelection(stateView, propertyName, startIndex, endIndex);
        }
#endregion
    }
}

namespace EaslyController.Focus
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

    /// <summary>
    /// View of a IxxxController.
    /// </summary>
    public partial class FocusControllerView : FrameControllerView, IFocusControllerView, IFocusInternalControllerView
    {
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

            GetFirstFocusedChildState(out IFocusNodeState State, out IReadOnlyIndex FirstFocusedIndex);
            Debug.Assert(State != null);

            bool IsAnchorChild = Controller.IsChildState(State, AnchorState, out IReadOnlyIndex FirstAnchorIndex);
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

        private void GetFirstFocusedChildState(out IFocusNodeState state, out IReadOnlyIndex firstFocusedIndex)
        {
            state = SelectionAnchor.State;
            firstFocusedIndex = null;

            IFocusNodeState FocusedState = Focus.CellView.StateView.State;

            while (state != null && !Controller.IsChildState(state, FocusedState, out firstFocusedIndex))
                state = state.ParentState;
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

            for (; ; )
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

        /// <summary>
        /// Change auto formatting mode.
        /// </summary>
        public virtual void SetAutoFormatMode(AutoFormatModes mode)
        {
            if (AutoFormatMode != mode)
                AutoFormatMode = mode;
        }
    }
}

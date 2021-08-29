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
            Node NewItem = NodeHelper.CreateDefaultFromInterface(InterfaceType);

            IFocusCollectionInner CollectionInner = null;
            frame.CollectionNameToInner(ref state, ref CollectionInner);
            Debug.Assert(CollectionInner != null);

            if (CollectionInner is IFocusBlockListInner AsBlockListInner)
            {
                inner = AsBlockListInner;

                if (AsBlockListInner.Count == 0)
                {
                    Pattern NewPattern = NodeHelper.CreateEmptyPattern();
                    Identifier NewSource = NodeHelper.CreateEmptyIdentifier();
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

            Node NewItem;
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
                if (NodeHelper.GetSimplifiedNode(CurrentState.Node, out Node SimplifiedNode))
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
                if (NodeHelper.GetComplexifiedNode(CurrentState.Node, out IList<Node> ComplexifiedNodeList))
                {
                    Debug.Assert(ComplexifiedNodeList != null && ComplexifiedNodeList.Count > 0);
                    Type InterfaceType = CurrentState.ParentInner.InterfaceType;
                    bool IsAssignable = true;

                    foreach (Node ComplexifiedNode in ComplexifiedNodeList)
                        IsAssignable &= InterfaceType.IsAssignableFrom(ComplexifiedNode.GetType());

                    if (IsAssignable)
                    {
                        IFocusBrowsingChildIndex ParentIndex = CurrentState.ParentIndex as IFocusBrowsingChildIndex;
                        Debug.Assert(ParentIndex != null);

                        IFocusInner Inner = CurrentState.ParentInner;
                        IList<IFocusInsertionChildNodeIndex> IndexList = new List<IFocusInsertionChildNodeIndex>();

                        foreach (Node ComplexifiedNode in ComplexifiedNodeList)
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

            if (IdentifierState.Node is Identifier AsIdentifier)
            {
                IFocusNodeState ParentState = IdentifierState.ParentState;
                if (ParentState.Node is QualifiedName)
                {
                    string Text = AsIdentifier.Text;
                    Debug.Assert(CaretPosition >= 0 && CaretPosition <= Text.Length);

                    inner = IdentifierState.ParentInner as IFocusListInner;
                    Debug.Assert(inner != null);

                    IFocusBrowsingListNodeIndex CurrentIndex = IdentifierState.ParentIndex as IFocusBrowsingListNodeIndex;
                    Debug.Assert(CurrentIndex != null);

                    Identifier FirstPart = NodeHelper.CreateSimpleIdentifier(Text.Substring(0, CaretPosition));
                    Identifier SecondPart = NodeHelper.CreateSimpleIdentifier(Text.Substring(CaretPosition));

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
    }
}

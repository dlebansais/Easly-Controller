namespace EaslyController.Writeable
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Controller for a node tree.
    /// This controller supports operations to modify the tree.
    /// </summary>
    public partial class WriteableController : ReadOnlyController, IWriteableController
    {
        /// <summary>
        /// Expands an existing node. In the node:
        /// * All optional children are assigned if they aren't
        /// * If the node is a feature call, with no arguments, an empty argument is inserted.
        /// </summary>
        /// <param name="expandedIndex">Index of the expanded node.</param>
        /// <param name="isChanged">True upon return if the node was changed. False if the node was already expanded.</param>
        public virtual void Expand(IWriteableNodeIndex expandedIndex, out bool isChanged)
        {
            Debug.Assert(expandedIndex != null);
            Debug.Assert(StateTable.ContainsKey(expandedIndex));
            Debug.Assert(StateTable[expandedIndex] is IWriteablePlaceholderNodeState);

            IWriteableOperationList OperationList = CreateOperationList();

            DebugObjects.AddReference(OperationList);

            Expand(expandedIndex, OperationList);

            if (OperationList.Count > 0)
            {
                IWriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                IWriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, null);

                SetLastOperation(OperationGroup);
                CheckInvariant();

                isChanged = true;
            }
            else
                isChanged = false;
        }

        private protected virtual void Expand(IWriteableNodeIndex expandedIndex, IWriteableOperationList operationList)
        {
            IWriteablePlaceholderNodeState State = StateTable[expandedIndex] as IWriteablePlaceholderNodeState;
            State = FindBestExpandReduceState(State);
            Debug.Assert(State != null);

            IWriteableInnerReadOnlyDictionary<string> InnerTable = State.InnerTable;

            foreach (KeyValuePair<string, IWriteableInner> Entry in InnerTable)
            {
                if (Entry.Value is IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> AsOptionalInner)
                    ExpandOptional(AsOptionalInner, operationList);
                else if (Entry.Value is IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> AsBlockListInner)
                    ExpandBlockList(AsBlockListInner, operationList);
            }
        }

        private protected virtual IWriteablePlaceholderNodeState FindBestExpandReduceState(IWriteablePlaceholderNodeState state)
        {
            Debug.Assert(state != null);

            while (state.InnerTable.Count == 0 && state.ParentState is IWriteablePlaceholderNodeState AsPlaceholderNodeState)
                state = AsPlaceholderNodeState;

            return state;
        }

        /// <summary>
        /// Expands the optional node.
        /// * If assigned, does nothing.
        /// * If it has an item, assign it.
        /// * Otherwise, assign the item to a default node.
        /// </summary>
        private protected virtual void ExpandOptional(IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> optionalInner, IWriteableOperationList operationList)
        {
            if (optionalInner.IsAssigned)
                return;

            IWriteableBrowsingOptionalNodeIndex ParentIndex = optionalInner.ChildState.ParentIndex;
            if (ParentIndex.Optional.HasItem)
            {
                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoAssign(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoAssign(operation);
                IWriteableAssignmentOperation Operation = CreateAssignmentOperation(optionalInner.Owner.Node, optionalInner.PropertyName, HandlerRedo, HandlerUndo, isNested: false);

                Operation.Redo();

                operationList.Add(Operation);
            }
            else
            {
                INode NewNode = NodeHelper.CreateDefaultFromInterface(optionalInner.InterfaceType);
                Debug.Assert(NewNode != null);

                IWriteableInsertionOptionalNodeIndex NewOptionalNodeIndex = CreateNewOptionalNodeIndex(optionalInner.Owner.Node, optionalInner.PropertyName, NewNode);

                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoReplace(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoReplace(operation);
                IWriteableReplaceOperation Operation = CreateReplaceOperation(optionalInner.Owner.Node, optionalInner.PropertyName, -1, -1, NewNode, HandlerRedo, HandlerUndo, isNested: false);

                Operation.Redo();

                operationList.Add(Operation);
            }
        }

        /// <summary>
        /// Expands the block list.
        /// * Only expand block list of arguments
        /// * Only expand if the list is empty. In that case, add a single default argument.
        /// </summary>
        private protected virtual void ExpandBlockList(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> blockListInner, IWriteableOperationList operationList)
        {
            if (!blockListInner.IsEmpty)
                return;

            if (!NodeHelper.IsCollectionWithExpand(blockListInner.Owner.Node, blockListInner.PropertyName))
                return;

            INode NewItem = NodeHelper.CreateDefaultFromInterface(blockListInner.InterfaceType);
            IPattern NewPattern = NodeHelper.CreateEmptyPattern();
            IIdentifier NewSource = NodeHelper.CreateEmptyIdentifier();
            IBlock NewBlock = NodeTreeHelperBlockList.CreateBlock(blockListInner.Owner.Node, blockListInner.PropertyName, ReplicationStatus.Normal, NewPattern, NewSource);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoExpandBlockList(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoExpandBlockList(operation);
            IWriteableExpandArgumentOperation Operation = CreateExpandArgumentOperation(blockListInner.Owner.Node, blockListInner.PropertyName, NewBlock, NewItem, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();

            operationList.Add(Operation);
        }

        private protected virtual void RedoExpandBlockList(IWriteableOperation operation)
        {
            IWriteableExpandArgumentOperation ExpandArgumentOperation = (IWriteableExpandArgumentOperation)operation;
            ExecuteInsertNewBlock(ExpandArgumentOperation);
        }

        private protected virtual void UndoExpandBlockList(IWriteableOperation operation)
        {
            IWriteableExpandArgumentOperation ExpandArgumentOperation = (IWriteableExpandArgumentOperation)operation;
            IWriteableRemoveBlockOperation RemoveBlockOperation = ExpandArgumentOperation.ToRemoveBlockOperation();

            ExecuteRemoveBlock(RemoveBlockOperation);
        }

        /// <summary>
        /// Reduces an existing node. Opposite of Expand.
        /// </summary>
        /// <param name="reducedIndex">Index of the reduced node.</param>
        /// <param name="isChanged">True upon return if the node was changed. False if the node was already reduced.</param>
        public virtual void Reduce(IWriteableNodeIndex reducedIndex, out bool isChanged)
        {
            Debug.Assert(reducedIndex != null);
            Debug.Assert(StateTable.ContainsKey(reducedIndex));
            Debug.Assert(StateTable[reducedIndex] is IWriteablePlaceholderNodeState);

            IWriteableOperationList OperationList = CreateOperationList();

            Reduce(reducedIndex, OperationList, isNested: false);

            if (OperationList.Count > 0)
            {
                IWriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                IWriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, null);

                SetLastOperation(OperationGroup);
                CheckInvariant();

                isChanged = true;
            }
            else
                isChanged = false;
        }

        private protected virtual void Reduce(IWriteableNodeIndex reducedIndex, IWriteableOperationList operationList, bool isNested)
        {
            IWriteablePlaceholderNodeState State = StateTable[reducedIndex] as IWriteablePlaceholderNodeState;
            State = FindBestExpandReduceState(State);
            Debug.Assert(State != null);

            IWriteableInnerReadOnlyDictionary<string> InnerTable = State.InnerTable;

            foreach (KeyValuePair<string, IWriteableInner> Entry in InnerTable)
            {
                if (Entry.Value is IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> AsOptionalInner)
                    ReduceOptional(AsOptionalInner, operationList, isNested);
                else if (Entry.Value is IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> AsBlockListInner)
                    ReduceBlockList(AsBlockListInner, operationList, isNested);
            }
        }

        /// <summary>
        /// Reduces the optional node.
        /// </summary>
        private protected virtual void ReduceOptional(IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> optionalInner, IWriteableOperationList operationList, bool isNested)
        {
            if (optionalInner.IsAssigned && NodeHelper.IsOptionalAssignedToDefault(optionalInner.ChildState.Optional))
            {
                IWriteableBrowsingOptionalNodeIndex ParentIndex = optionalInner.ChildState.ParentIndex;

                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoUnassign(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoUnassign(operation);
                IWriteableAssignmentOperation Operation = CreateAssignmentOperation(optionalInner.Owner.Node, optionalInner.PropertyName, HandlerRedo, HandlerUndo, isNested);

                Operation.Redo();

                operationList.Add(Operation);
            }
        }

        /// <summary>
        /// Reduces the block list.
        /// </summary>
        private protected virtual void ReduceBlockList(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> blockListInner, IWriteableOperationList operationList, bool isNested)
        {
            if (!blockListInner.IsSingle)
                return;

            if (NodeHelper.IsCollectionNeverEmpty(blockListInner.Owner.Node, blockListInner.PropertyName))
                return;

            if (!NodeHelper.IsCollectionWithExpand(blockListInner.Owner.Node, blockListInner.PropertyName))
                return;

            Debug.Assert(blockListInner.BlockStateList.Count == 1);
            Debug.Assert(blockListInner.BlockStateList[0].StateList.Count == 1);
            IWriteableNodeState FirstState = blockListInner.BlockStateList[0].StateList[0];

            if (!NodeHelper.IsDefaultNode(FirstState.Node))
                return;

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRemoveBlock(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoRemoveBlock(operation);
            IWriteableRemoveBlockOperation Operation = CreateRemoveBlockOperation(blockListInner.Owner.Node, blockListInner.PropertyName, 0, HandlerRedo, HandlerUndo, isNested);

            Operation.Redo();

            operationList.Add(Operation);
        }
        /// <summary>
        /// Reduces all expanded nodes, and clear all unassigned optional nodes.
        /// </summary>
        /// <param name="isChanged">True upon return if the node was changed. False if the node was already canonic.</param>
        public virtual void Canonicalize(out bool isChanged)
        {
            IWriteableOperationList OperationList = CreateOperationList();
            Canonicalize(RootState, OperationList);

            if (OperationList.Count > 0)
            {
                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRefresh(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
                IWriteableGenericRefreshOperation RefreshOperation = CreateGenericRefreshOperation(RootState, HandlerRedo, HandlerUndo, isNested: false);

                RefreshOperation.Redo();

                IWriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                IWriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, RefreshOperation);

                SetLastOperation(OperationGroup);
                CheckInvariant();

                isChanged = true;
            }
            else
                isChanged = false;
        }

        private protected virtual void Canonicalize(IWriteableNodeState state, IWriteableOperationList operationList)
        {
            IWriteableNodeIndex NodeIndex = state.ParentIndex as IWriteableNodeIndex;
            Debug.Assert(NodeIndex != null);

            CanonicalizeChildren(state, operationList);

            Reduce(NodeIndex, operationList, isNested: state != RootState);
        }

        private protected virtual void CanonicalizeChildren(IWriteableNodeState state, IWriteableOperationList operationList)
        {
            List<IWriteableNodeState> ChildStateList = new List<IWriteableNodeState>();
            foreach (KeyValuePair<string, IWriteableInner> Entry in state.InnerTable)
            {
                switch (Entry.Value)
                {
                    case IWriteablePlaceholderInner<IWriteableBrowsingPlaceholderNodeIndex> AsPlaceholderInner:
                        ChildStateList.Add(AsPlaceholderInner.ChildState);
                        break;

                    case IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> AsOptionalInner:
                        if (AsOptionalInner.IsAssigned)
                            CanonicalizeChildren(AsOptionalInner.ChildState, operationList);
                        break;

                    case IWriteableListInner<IWriteableBrowsingListNodeIndex> AsListlInner:
                        foreach (IWriteablePlaceholderNodeState ChildState in AsListlInner.StateList)
                            ChildStateList.Add(ChildState);
                        break;

                    case IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> AsBlockListlInner:
                        foreach (IWriteableBlockState BlockState in AsBlockListlInner.BlockStateList)
                            foreach (IWriteablePlaceholderNodeState ChildState in BlockState.StateList)
                                ChildStateList.Add(ChildState);
                        break;
                }
            }

            foreach (IWriteableNodeState ChildState in ChildStateList)
                Canonicalize(ChildState, operationList);
        }
    }
}

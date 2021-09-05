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
    public partial class WriteableController : ReadOnlyController
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

            WriteableOperationList OperationList = CreateOperationList();

            DebugObjects.AddReference(OperationList);

            Expand(expandedIndex, OperationList);

            if (OperationList.Count > 0)
            {
                WriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                WriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, null);

                SetLastOperation(OperationGroup);
                CheckInvariant();

                isChanged = true;
            }
            else
                isChanged = false;
        }

        private protected virtual void Expand(IWriteableNodeIndex expandedIndex, WriteableOperationList operationList)
        {
            IWriteablePlaceholderNodeState State = StateTable[expandedIndex] as IWriteablePlaceholderNodeState;
            State = FindBestExpandReduceState(State);
            Debug.Assert(State != null);

            WriteableInnerReadOnlyDictionary<string> InnerTable = State.InnerTable;

            foreach (string Key in InnerTable.Keys)
            {
                IWriteableInner Value = (IWriteableInner)InnerTable[Key];

                if (Value is WriteableOptionalInner<WriteableBrowsingOptionalNodeIndex> AsOptionalInner)
                    ExpandOptional(AsOptionalInner, operationList);
                else if (Value is WriteableBlockListInner<WriteableBrowsingBlockNodeIndex> AsBlockListInner)
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
        private protected virtual void ExpandOptional(WriteableOptionalInner<WriteableBrowsingOptionalNodeIndex> optionalInner, WriteableOperationList operationList)
        {
            if (optionalInner.IsAssigned)
                return;

            WriteableBrowsingOptionalNodeIndex ParentIndex = optionalInner.ChildState.ParentIndex;
            if (ParentIndex.Optional.HasItem)
            {
                Action<WriteableOperation> HandlerRedo = (WriteableOperation operation) => RedoAssign(operation);
                Action<WriteableOperation> HandlerUndo = (WriteableOperation operation) => UndoAssign(operation);
                WriteableAssignmentOperation Operation = CreateAssignmentOperation(optionalInner.Owner.Node, optionalInner.PropertyName, HandlerRedo, HandlerUndo, isNested: false);

                Operation.Redo();

                operationList.Add(Operation);
            }
            else
            {
                Node NewNode = NodeHelper.CreateDefaultFromInterface(optionalInner.InterfaceType);
                Debug.Assert(NewNode != null);

                WriteableInsertionOptionalNodeIndex NewOptionalNodeIndex = CreateNewOptionalNodeIndex(optionalInner.Owner.Node, optionalInner.PropertyName, NewNode);

                Action<WriteableOperation> HandlerRedo = (WriteableOperation operation) => RedoReplace(operation);
                Action<WriteableOperation> HandlerUndo = (WriteableOperation operation) => UndoReplace(operation);
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
        private protected virtual void ExpandBlockList(WriteableBlockListInner<WriteableBrowsingBlockNodeIndex> blockListInner, WriteableOperationList operationList)
        {
            if (!blockListInner.IsEmpty)
                return;

            if (!NodeHelper.IsCollectionWithExpand(blockListInner.Owner.Node, blockListInner.PropertyName))
                return;

            Node NewItem = NodeHelper.CreateDefaultFromInterface(blockListInner.InterfaceType);
            Pattern NewPattern = NodeHelper.CreateEmptyPattern();
            Identifier NewSource = NodeHelper.CreateEmptyIdentifier();
            IBlock NewBlock = NodeTreeHelperBlockList.CreateBlock(blockListInner.Owner.Node, blockListInner.PropertyName, ReplicationStatus.Normal, NewPattern, NewSource);

            Action<WriteableOperation> HandlerRedo = (WriteableOperation operation) => RedoExpandBlockList(operation);
            Action<WriteableOperation> HandlerUndo = (WriteableOperation operation) => UndoExpandBlockList(operation);
            WriteableExpandArgumentOperation Operation = CreateExpandArgumentOperation(blockListInner.Owner.Node, blockListInner.PropertyName, NewBlock, NewItem, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();

            operationList.Add(Operation);
        }

        private protected virtual void RedoExpandBlockList(WriteableOperation operation)
        {
            WriteableExpandArgumentOperation ExpandArgumentOperation = (WriteableExpandArgumentOperation)operation;
            ExecuteInsertNewBlock(ExpandArgumentOperation);
        }

        private protected virtual void UndoExpandBlockList(WriteableOperation operation)
        {
            WriteableExpandArgumentOperation ExpandArgumentOperation = (WriteableExpandArgumentOperation)operation;
            WriteableRemoveBlockOperation RemoveBlockOperation = ExpandArgumentOperation.ToRemoveBlockOperation();

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

            WriteableOperationList OperationList = CreateOperationList();

            Reduce(reducedIndex, OperationList, isNested: false);

            if (OperationList.Count > 0)
            {
                WriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                WriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, null);

                SetLastOperation(OperationGroup);
                CheckInvariant();

                isChanged = true;
            }
            else
                isChanged = false;
        }

        private protected virtual void Reduce(IWriteableNodeIndex reducedIndex, WriteableOperationList operationList, bool isNested)
        {
            IWriteablePlaceholderNodeState State = StateTable[reducedIndex] as IWriteablePlaceholderNodeState;
            State = FindBestExpandReduceState(State);
            Debug.Assert(State != null);

            WriteableInnerReadOnlyDictionary<string> InnerTable = State.InnerTable;

            foreach (string Key in InnerTable.Keys)
            {
                IWriteableInner Value = (IWriteableInner)InnerTable[Key];

                if (Value is WriteableOptionalInner<WriteableBrowsingOptionalNodeIndex> AsOptionalInner)
                    ReduceOptional(AsOptionalInner, operationList, isNested);
                else if (Value is WriteableBlockListInner<WriteableBrowsingBlockNodeIndex> AsBlockListInner)
                    ReduceBlockList(AsBlockListInner, operationList, isNested);
            }
        }

        /// <summary>
        /// Reduces the optional node.
        /// </summary>
        private protected virtual void ReduceOptional(WriteableOptionalInner<WriteableBrowsingOptionalNodeIndex> optionalInner, WriteableOperationList operationList, bool isNested)
        {
            if (optionalInner.IsAssigned && NodeHelper.IsOptionalAssignedToDefault(optionalInner.ChildState.Optional))
            {
                WriteableBrowsingOptionalNodeIndex ParentIndex = optionalInner.ChildState.ParentIndex;

                Action<WriteableOperation> HandlerRedo = (WriteableOperation operation) => RedoUnassign(operation);
                Action<WriteableOperation> HandlerUndo = (WriteableOperation operation) => UndoUnassign(operation);
                WriteableAssignmentOperation Operation = CreateAssignmentOperation(optionalInner.Owner.Node, optionalInner.PropertyName, HandlerRedo, HandlerUndo, isNested);

                Operation.Redo();

                operationList.Add(Operation);
            }
        }

        /// <summary>
        /// Reduces the block list.
        /// </summary>
        private protected virtual void ReduceBlockList(WriteableBlockListInner<WriteableBrowsingBlockNodeIndex> blockListInner, WriteableOperationList operationList, bool isNested)
        {
            if (!blockListInner.IsSingle)
                return;

            if (NodeHelper.IsCollectionNeverEmpty(blockListInner.Owner.Node, blockListInner.PropertyName))
                return;

            if (!NodeHelper.IsCollectionWithExpand(blockListInner.Owner.Node, blockListInner.PropertyName))
                return;

            Debug.Assert(blockListInner.BlockStateList.Count == 1);
            Debug.Assert(blockListInner.BlockStateList[0].StateList.Count == 1);
            IWriteableNodeState FirstState = (IWriteableNodeState)blockListInner.BlockStateList[0].StateList[0];

            if (!NodeHelper.IsDefaultNode(FirstState.Node))
                return;

            Action<WriteableOperation> HandlerRedo = (WriteableOperation operation) => RedoRemoveBlock(operation);
            Action<WriteableOperation> HandlerUndo = (WriteableOperation operation) => UndoRemoveBlock(operation);
            WriteableRemoveBlockOperation Operation = CreateRemoveBlockOperation(blockListInner.Owner.Node, blockListInner.PropertyName, 0, HandlerRedo, HandlerUndo, isNested);

            Operation.Redo();

            operationList.Add(Operation);
        }

        /// <summary>
        /// Reduces all expanded nodes, and clear all unassigned optional nodes.
        /// </summary>
        /// <param name="isChanged">True upon return if the node was changed. False if the node was already canonic.</param>
        public virtual void Canonicalize(out bool isChanged)
        {
            WriteableOperationList OperationList = CreateOperationList();
            Canonicalize(RootState, OperationList);

            if (OperationList.Count > 0)
            {
                Action<WriteableOperation> HandlerRedo = (WriteableOperation operation) => RedoRefresh(operation);
                Action<WriteableOperation> HandlerUndo = (WriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
                WriteableGenericRefreshOperation RefreshOperation = CreateGenericRefreshOperation(RootState, HandlerRedo, HandlerUndo, isNested: false);

                RefreshOperation.Redo();

                WriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                WriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, RefreshOperation);

                SetLastOperation(OperationGroup);
                CheckInvariant();

                isChanged = true;
            }
            else
                isChanged = false;
        }

        private protected virtual void Canonicalize(IWriteableNodeState state, WriteableOperationList operationList)
        {
            IWriteableNodeIndex NodeIndex = state.ParentIndex as IWriteableNodeIndex;
            Debug.Assert(NodeIndex != null);

            CanonicalizeChildren(state, operationList);

            Reduce(NodeIndex, operationList, isNested: state != RootState);
        }

        private protected virtual void CanonicalizeChildren(IWriteableNodeState state, WriteableOperationList operationList)
        {
            List<IWriteableNodeState> ChildStateList = new List<IWriteableNodeState>();
            foreach (string Key in state.InnerTable.Keys)
            {
                IWriteableInner Value = (IWriteableInner)state.InnerTable[Key];

                switch (Value)
                {
                    case IWriteablePlaceholderInner<WriteableBrowsingPlaceholderNodeIndex> AsPlaceholderInner:
                        ChildStateList.Add(AsPlaceholderInner.ChildState);
                        break;

                    case IWriteableOptionalInner<WriteableBrowsingOptionalNodeIndex> AsOptionalInner:
                        if (AsOptionalInner.IsAssigned)
                            CanonicalizeChildren(AsOptionalInner.ChildState, operationList);
                        break;

                    case IWriteableListInner<WriteableBrowsingListNodeIndex> AsListlInner:
                        foreach (IWriteablePlaceholderNodeState ChildState in AsListlInner.StateList)
                            ChildStateList.Add(ChildState);
                        break;

                    case IWriteableBlockListInner<WriteableBrowsingBlockNodeIndex> AsBlockListlInner:
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

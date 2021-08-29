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
    public interface IWriteableController : IReadOnlyController
    {
        /// <summary>
        /// Index of the root node.
        /// </summary>
        new IWriteableRootNodeIndex RootIndex { get; }

        /// <summary>
        /// State of the root node.
        /// </summary>
        new IWriteablePlaceholderNodeState RootState { get; }

        /// <summary>
        /// State table.
        /// </summary>
        new IWriteableIndexNodeStateReadOnlyDictionary StateTable { get; }

        /// <summary>
        /// List of operations that have been performed, and can be undone or redone.
        /// </summary>
        IWriteableOperationGroupReadOnlyList OperationStack { get; }

        /// <summary>
        /// Index of the next operation that can be redone in <see cref="OperationStack"/>.
        /// </summary>
        int RedoIndex { get; }

        /// <summary>
        /// Checks if there is an operation that can be undone.
        /// </summary>
        bool CanUndo { get; }

        /// <summary>
        /// Checks if there is an operation that can be redone.
        /// </summary>
        bool CanRedo { get; }

        /// <summary>
        /// Called when a block state is inserted.
        /// </summary>
        event Action<IWriteableInsertBlockOperation> BlockStateInserted;

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        event Action<IWriteableRemoveBlockOperation> BlockStateRemoved;

        /// <summary>
        /// Called when a block view must be removed.
        /// </summary>
        event Action<IWriteableRemoveBlockViewOperation> BlockViewRemoved;

        /// <summary>
        /// Called when a state is inserted.
        /// </summary>
        event Action<IWriteableInsertNodeOperation> StateInserted;

        /// <summary>
        /// Called when a state is removed.
        /// </summary>
        event Action<IWriteableRemoveNodeOperation> StateRemoved;

        /// <summary>
        /// Called when a state is replaced.
        /// </summary>
        event Action<IWriteableReplaceOperation> StateReplaced;

        /// <summary>
        /// Called when a state is assigned.
        /// </summary>
        event Action<IWriteableAssignmentOperation> StateAssigned;

        /// <summary>
        /// Called when a state is unassigned.
        /// </summary>
        event Action<IWriteableAssignmentOperation> StateUnassigned;

        /// <summary>
        /// Called when a discrete value is changed.
        /// </summary>
        event Action<IWriteableChangeDiscreteValueOperation> DiscreteValueChanged;

        /// <summary>
        /// Called when text is changed.
        /// </summary>
        event Action<IWriteableChangeTextOperation> TextChanged;

        /// <summary>
        /// Called when comment is changed.
        /// </summary>
        event Action<IWriteableChangeCommentOperation> CommentChanged;

        /// <summary>
        /// Called when a block state is changed.
        /// </summary>
        event Action<IWriteableChangeBlockOperation> BlockStateChanged;

        /// <summary>
        /// Called when a state is moved.
        /// </summary>
        event Action<IWriteableMoveNodeOperation> StateMoved;

        /// <summary>
        /// Called when a block state is moved.
        /// </summary>
        event Action<IWriteableMoveBlockOperation> BlockStateMoved;

        /// <summary>
        /// Called when a block is split.
        /// </summary>
        event Action<IWriteableSplitBlockOperation> BlockSplit;

        /// <summary>
        /// Called when two blocks are merged.
        /// </summary>
        event Action<IWriteableMergeBlocksOperation> BlocksMerged;

        /// <summary>
        /// Called to refresh views.
        /// </summary>
        event Action<IWriteableGenericRefreshOperation> GenericRefresh;

        /// <summary>
        /// Inserts a new node in a list or block list.
        /// </summary>
        /// <param name="inner">The inner for the list or block list where the node is inserted.</param>
        /// <param name="insertedIndex">Index for the insertion operation.</param>
        /// <param name="nodeIndex">Index of the inserted node upon return.</param>
        void Insert(IWriteableCollectionInner inner, IWriteableInsertionCollectionNodeIndex insertedIndex, out IWriteableBrowsingCollectionNodeIndex nodeIndex);

        /// <summary>
        /// Inserts a range of blocks in a block list.
        /// </summary>
        /// <param name="inner">The inner for the block list in which blocks are inserted.</param>
        /// <param name="insertedIndex">Index where to insert the first block.</param>
        /// <param name="indexList">List of nodes in blocks to insert.</param>
        void InsertBlockRange(IWriteableBlockListInner inner, int insertedIndex, IList<IWriteableInsertionBlockNodeIndex> indexList);

        /// <summary>
        /// Inserts a range of nodes in a list or block list.
        /// </summary>
        /// <param name="inner">The inner for the list or block list in which nodes are inserted.</param>
        /// <param name="blockIndex">Index of the block where to insert nodes, for a block list. -1 for a list.</param>
        /// <param name="insertedIndex">Index of the first node to insert.</param>
        /// <param name="indexList">List of nodes to insert.</param>
        void InsertNodeRange(IWriteableCollectionInner inner, int blockIndex, int insertedIndex, IList<IWriteableInsertionCollectionNodeIndex> indexList);

        /// <summary>
        /// Checks whether a node can be removed from a list.
        /// </summary>
        /// <param name="inner">The inner where the node is.</param>
        /// <param name="nodeIndex">Index of the node that would be removed.</param>
        bool IsRemoveable(IWriteableCollectionInner inner, IWriteableBrowsingCollectionNodeIndex nodeIndex);

        /// <summary>
        /// Removes a node from a list or block list.
        /// </summary>
        /// <param name="inner">The inner for the list or block list from which the node is removed.</param>
        /// <param name="nodeIndex">Index for the removed node.</param>
        void Remove(IWriteableCollectionInner inner, IWriteableBrowsingCollectionNodeIndex nodeIndex);

        /// <summary>
        /// Replace an existing node with a new one.
        /// </summary>
        /// <param name="inner">The inner where the node is replaced.</param>
        /// <param name="replaceIndex">Index for the replace operation.</param>
        /// <param name="nodeIndex">Index of the replacing node upon return.</param>
        void Replace(IWriteableInner inner, IWriteableInsertionChildIndex replaceIndex, out IWriteableBrowsingChildIndex nodeIndex);

        /// <summary>
        /// Checks whether a range of blocks can be removed from a block list.
        /// </summary>
        /// <param name="inner">The inner with blocks to remove.</param>
        /// <param name="firstBlockIndex">Index of the first block to remove.</param>
        /// <param name="lastBlockIndex">Index following the last block to remove.</param>
        bool IsBlockRangeRemoveable(IWriteableBlockListInner inner, int firstBlockIndex, int lastBlockIndex);

        /// <summary>
        /// Removes a range of blocks from a block list.
        /// </summary>
        /// <param name="inner">The inner for the block list from which blocks are removed.</param>
        /// <param name="firstBlockIndex">Index of the first block to remove.</param>
        /// <param name="lastBlockIndex">Index following the last block to remove.</param>
        void RemoveBlockRange(IWriteableBlockListInner inner, int firstBlockIndex, int lastBlockIndex);

        /// <summary>
        /// Removes a range of blocks from a block list and replace them with other blocks.
        /// </summary>
        /// <param name="inner">The inner for the block list from which blocks are replaced.</param>
        /// <param name="firstBlockIndex">Index of the first block to remove.</param>
        /// <param name="lastBlockIndex">Index following the last block to remove.</param>
        /// <param name="indexList">List of nodes in blocks to insert.</param>
        void ReplaceBlockRange(IWriteableBlockListInner inner, int firstBlockIndex, int lastBlockIndex, IList<IWriteableInsertionBlockNodeIndex> indexList);

        /// <summary>
        /// Checks whether a range of nodes can be removed from a list or block list.
        /// </summary>
        /// <param name="inner">The inner with nodes to remove.</param>
        /// <param name="blockIndex">Index of the block where to remove nodes, for a block list. -1 for a list.</param>
        /// <param name="firstNodeIndex">Index of the first node to remove.</param>
        /// <param name="lastNodeIndex">Index following the last node to remove.</param>
        bool IsNodeRangeRemoveable(IWriteableCollectionInner inner, int blockIndex, int firstNodeIndex, int lastNodeIndex);

        /// <summary>
        /// Removes a range of nodes from a list or block list.
        /// </summary>
        /// <param name="inner">The inner with nodes to remove.</param>
        /// <param name="blockIndex">Index of the block where to remove nodes, for a block list. -1 for a list.</param>
        /// <param name="firstNodeIndex">Index of the first node to remove.</param>
        /// <param name="lastNodeIndex">Index following the last node to remove.</param>
        void RemoveNodeRange(IWriteableCollectionInner inner, int blockIndex, int firstNodeIndex, int lastNodeIndex);

        /// <summary>
        /// Removes a range of nodes from a list or block list and replace them with other nodes.
        /// </summary>
        /// <param name="inner">The inner for the list or block list from which nodes are replaced.</param>
        /// <param name="blockIndex">Index of the block where to remove nodes, for a block list. -1 for a list.</param>
        /// <param name="firstNodeIndex">Index of the first node to remove.</param>
        /// <param name="lastNodeIndex">Index following the last node to remove.</param>
        /// <param name="indexList">List of nodes to insert.</param>
        void ReplaceNodeRange(IWriteableCollectionInner inner, int blockIndex, int firstNodeIndex, int lastNodeIndex, IList<IWriteableInsertionCollectionNodeIndex> indexList);

        /// <summary>
        /// Assign the optional node.
        /// </summary>
        /// <param name="nodeIndex">Index of the optional node.</param>
        /// <param name="isChanged">True upon return if the node was changed. False if the node was already assigned.</param>
        void Assign(IWriteableBrowsingOptionalNodeIndex nodeIndex, out bool isChanged);

        /// <summary>
        /// Unassign the optional node.
        /// </summary>
        /// <param name="nodeIndex">Index of the optional node.</param>
        /// <param name="isChanged">True upon return if the node was changed. False if the node was already not assigned.</param>
        void Unassign(IWriteableBrowsingOptionalNodeIndex nodeIndex, out bool isChanged);

        /// <summary>
        /// Changes the replication state of a block.
        /// </summary>
        /// <param name="inner">The inner where the blok is changed.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="replication">New replication value.</param>
        void ChangeReplication(IWriteableBlockListInner inner, int blockIndex, ReplicationStatus replication);

        /// <summary>
        /// Changes the value of an enum or boolean.
        /// If the value exceeds allowed values, it is rounded to fit.
        /// </summary>
        /// <param name="nodeIndex">Index of the state with the enum to change.</param>
        /// <param name="propertyName">Name of the property to change.</param>
        /// <param name="value">The new value.</param>
        void ChangeDiscreteValue(IWriteableIndex nodeIndex, string propertyName, int value);

        /// <summary>
        /// Changes the value of a text.
        /// </summary>
        /// <param name="nodeIndex">Index of the state with the string to change.</param>
        /// <param name="propertyName">Name of the property to change.</param>
        /// <param name="text">The new text.</param>
        void ChangeText(IWriteableIndex nodeIndex, string propertyName, string text);

        /// <summary>
        /// Changes the value of a comment.
        /// </summary>
        /// <param name="nodeIndex">Index of the state with the comment to change.</param>
        /// <param name="text">The new text.</param>
        void ChangeComment(IWriteableIndex nodeIndex, string text);

        /// <summary>
        /// Checks whether a block can be split at the given index.
        /// </summary>
        /// <param name="inner">The inner where the block would be split.</param>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        bool IsSplittable(IWriteableBlockListInner inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex);

        /// <summary>
        /// Splits a block in two at the given index.
        /// </summary>
        /// <param name="inner">The inner where the block is split.</param>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        void SplitBlock(IWriteableBlockListInner inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex);

        /// <summary>
        /// Checks whether a block can be merged at the given index.
        /// </summary>
        /// <param name="inner">The inner where the block would be split.</param>
        /// <param name="nodeIndex">Index of the first node in the block to merge.</param>
        bool IsMergeable(IWriteableBlockListInner inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex);

        /// <summary>
        /// Merges two blocks at the given index.
        /// </summary>
        /// <param name="inner">The inner where blocks are merged.</param>
        /// <param name="nodeIndex">Index of the first node in the block to merge.</param>
        void MergeBlocks(IWriteableBlockListInner inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex);

        /// <summary>
        /// Checks whether a node can be moved in a list.
        /// </summary>
        /// <param name="inner">The inner where the node is.</param>
        /// <param name="nodeIndex">Index of the node that would be moved.</param>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        bool IsMoveable(IWriteableCollectionInner inner, IWriteableBrowsingCollectionNodeIndex nodeIndex, int direction);

        /// <summary>
        /// Moves a node around in a list or block list. In a block list, the node stays in same block.
        /// </summary>
        /// <param name="inner">The inner for the list or block list in which the node is moved.</param>
        /// <param name="nodeIndex">Index for the moved node.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        void Move(IWriteableCollectionInner inner, IWriteableBrowsingCollectionNodeIndex nodeIndex, int direction);

        /// <summary>
        /// Checks whether a block can be moved in a block list.
        /// </summary>
        /// <param name="inner">The inner where the block is.</param>
        /// <param name="blockIndex">Index of the block that would be moved.</param>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        bool IsBlockMoveable(IWriteableBlockListInner inner, int blockIndex, int direction);

        /// <summary>
        /// Moves a block around in a block list.
        /// </summary>
        /// <param name="inner">The inner where the block is moved.</param>
        /// <param name="blockIndex">Index of the block to move.</param>
        /// <param name="direction">The change in position, relative to the current block position.</param>
        void MoveBlock(IWriteableBlockListInner inner, int blockIndex, int direction);

        /// <summary>
        /// Expands an existing node. In the node:
        /// * All optional children are assigned if they aren't
        /// * If the node is a feature call, with no arguments, an empty argument is inserted.
        /// </summary>
        /// <param name="expandedIndex">Index of the expanded node.</param>
        /// <param name="isChanged">True upon return if the node was changed. False if the node was already expanded.</param>
        void Expand(IWriteableNodeIndex expandedIndex, out bool isChanged);

        /// <summary>
        /// Reduces an existing node. Opposite of <see cref="Expand"/>.
        /// </summary>
        /// <param name="reducedIndex">Index of the reduced node.</param>
        /// <param name="isChanged">True upon return if the node was changed. False if the node was already reduced.</param>
        void Reduce(IWriteableNodeIndex reducedIndex, out bool isChanged);

        /// <summary>
        /// Reduces all expanded nodes, and clear all unassigned optional nodes.
        /// </summary>
        /// <param name="isChanged">True upon return if the node was changed. False if the node was already canonic.</param>
        void Canonicalize(out bool isChanged);

        /// <summary>
        /// Undo the last operation.
        /// </summary>
        void Undo();

        /// <summary>
        /// Redo the last operation undone.
        /// </summary>
        void Redo();

        /// <summary>
        /// Split an identifier with replace and insert indexes.
        /// </summary>
        /// <param name="inner">The inner where the node is replaced.</param>
        /// <param name="replaceIndex">Index for the replace operation.</param>
        /// <param name="insertIndex">Index for the insert operation.</param>
        /// <param name="firstIndex">Index of the replacing node upon return.</param>
        /// <param name="secondIndex">Index of the inserted node upon return.</param>
        void SplitIdentifier(IWriteableListInner inner, IWriteableInsertionListNodeIndex replaceIndex, IWriteableInsertionListNodeIndex insertIndex, out IWriteableBrowsingListNodeIndex firstIndex, out IWriteableBrowsingListNodeIndex secondIndex);
    }

    /// <summary>
    /// Controller for a node tree.
    /// This controller supports operations to modify the tree.
    /// </summary>
    public partial class WriteableController : ReadOnlyController, IWriteableController
    {
        private protected override void CheckInvariant()
        {
            base.CheckInvariant();

            bool IsValid = IsNodeTreeValid(RootState.Node);
            Debug.Assert(IsValid);
        }

        private protected virtual bool IsNodeTreeValid(Node node)
        {
            Type ChildNodeType;
            IList<string> PropertyNames = NodeTreeHelper.EnumChildNodeProperties(node);
            bool IsValid = true;

            foreach (string PropertyName in PropertyNames)
            {
                if (NodeTreeHelperChild.IsChildNodeProperty(node, PropertyName, out ChildNodeType))
                {
                    IsValid &= InvariantFailed(IsNodeTreeChildNodeValid(node, PropertyName));
                }
                else if (NodeTreeHelperOptional.IsOptionalChildNodeProperty(node, PropertyName, out ChildNodeType))
                {
                    IsValid &= InvariantFailed(IsNodeTreeOptionalNodeValid(node, PropertyName));
                }
                else if (NodeTreeHelperList.IsNodeListProperty(node, PropertyName, out ChildNodeType))
                {
                    IsValid &= InvariantFailed(IsNodeTreeListValid(node, PropertyName));
                }
                else if (NodeTreeHelperBlockList.IsBlockListProperty(node, PropertyName, out Type ChildInterfaceType, out ChildNodeType))
                {
                    IsValid &= InvariantFailed(IsNodeTreeBlockListValid(node, PropertyName));
                }
            }

            return IsValid;
        }

        private protected virtual bool IsNodeTreeChildNodeValid(Node node, string propertyName)
        {
            NodeTreeHelperChild.GetChildNode(node, propertyName, out Node ChildNode);
            Debug.Assert(ChildNode != null);

            bool IsValid = InvariantFailed(IsNodeTreeValid(ChildNode));

            return IsValid;
        }

        private protected virtual bool IsNodeTreeOptionalNodeValid(Node node, string propertyName)
        {
            bool IsValid = true;

            NodeTreeHelperOptional.GetChildNode(node, propertyName, out bool IsAssigned, out Node ChildNode);
            if (IsAssigned)
            {
                IsValid &= InvariantFailed(IsNodeTreeValid(ChildNode));
            }

            return IsValid;
        }

        private protected virtual bool IsNodeTreeListValid(Node node, string propertyName)
        {
            NodeTreeHelperList.GetChildNodeList(node, propertyName, out IReadOnlyList<Node> ChildNodeList);
            Debug.Assert(ChildNodeList != null);

            bool IsValid = true;

            if (ChildNodeList.Count == 0)
            {
                IsValid &= InvariantFailed(IsEmptyListValid(node, propertyName));
            }

            for (int Index = 0; Index < ChildNodeList.Count; Index++)
            {
                Node ChildNode = ChildNodeList[Index];
                Debug.Assert(ChildNode != null);

                IsValid &= InvariantFailed(IsNodeTreeValid(ChildNode));
            }

            return IsValid;
        }

        private protected virtual bool IsNodeTreeBlockListValid(Node node, string propertyName)
        {
            NodeTreeHelperBlockList.GetChildBlockList(node, propertyName, out IReadOnlyList<NodeTreeBlock> ChildBlockList);
            Debug.Assert(ChildBlockList != null);

            bool IsValid = true;

            if (ChildBlockList.Count == 0)
            {
                IsValid &= InvariantFailed(IsEmptyBlockListValid(node, propertyName));
            }

            for (int BlockIndex = 0; BlockIndex < ChildBlockList.Count; BlockIndex++)
            {
                NodeTreeBlock Block = ChildBlockList[BlockIndex];
                Debug.Assert(Block.NodeList.Count > 0);

                for (int Index = 0; Index < Block.NodeList.Count; Index++)
                {
                    Node ChildNode = Block.NodeList[Index];
                    Debug.Assert(ChildNode != null);

                    IsValid &= InvariantFailed(IsNodeTreeValid(ChildNode));
                }
            }

            return IsValid;
        }

        private protected virtual bool IsEmptyListValid(Node node, string propertyName)
        {
            Type NodeType = node.GetType();
            Debug.Assert(NodeTreeHelperList.IsNodeListProperty(NodeType, propertyName, out Type ChildNodeType));

            bool IsValid = true;

            Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(NodeType);
            IReadOnlyDictionary<Type, string[]> NeverEmptyCollectionTable = NodeHelper.NeverEmptyCollectionTable;
            if (NeverEmptyCollectionTable.ContainsKey(InterfaceType))
            {
                foreach (string Item in NeverEmptyCollectionTable[InterfaceType])
                {
                    IsValid &= InvariantFailed(Item != propertyName);
                }
            }

            return IsValid;
        }

        private protected virtual bool IsEmptyBlockListValid(Node node, string propertyName)
        {
            Type NodeType = node.GetType();
            Debug.Assert(NodeTreeHelperBlockList.IsBlockListProperty(NodeType, propertyName, out Type ChildInterfaceType, out Type ChildNodeType));

            bool IsValid = true;

            Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(NodeType);
            IReadOnlyDictionary<Type, string[]> NeverEmptyCollectionTable = NodeHelper.NeverEmptyCollectionTable;
            if (NeverEmptyCollectionTable.ContainsKey(InterfaceType))
            {
                foreach (string Item in NeverEmptyCollectionTable[InterfaceType])
                {
                    IsValid &= InvariantFailed(Item != propertyName);
                }
            }

            return IsValid;
        }

        private protected bool InvariantFailed(bool condition)
        {
            Debug.Assert(condition, "Invariant");
            return condition;
        }
    }
}

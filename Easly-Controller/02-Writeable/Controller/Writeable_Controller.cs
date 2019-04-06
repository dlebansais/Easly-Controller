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
    public class WriteableController : ReadOnlyController, IWriteableController
    {
        #region Init
        /// <summary>
        /// Creates and initializes a new instance of a <see cref="WriteableController"/> object.
        /// </summary>
        /// <param name="nodeIndex">Index of the root of the node tree.</param>
        public static IWriteableController Create(IWriteableRootNodeIndex nodeIndex)
        {
            WriteableController Controller = new WriteableController();
            Controller.SetRoot(nodeIndex);
            Controller.SetInitialized();
            return Controller;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableController"/> class.
        /// </summary>
        private protected WriteableController()
        {
            _OperationStack = CreateOperationGroupStack();
            DebugObjects.AddReference(_OperationStack);
            OperationStack = _OperationStack.ToReadOnly();
            RedoIndex = 0;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Index of the root node.
        /// </summary>
        public new IWriteableRootNodeIndex RootIndex { get { return (IWriteableRootNodeIndex)base.RootIndex; } }

        /// <summary>
        /// State of the root node.
        /// </summary>
        public new IWriteablePlaceholderNodeState RootState { get { return (IWriteablePlaceholderNodeState)base.RootState; } }

        /// <summary>
        /// State table.
        /// </summary>
        public new IWriteableIndexNodeStateReadOnlyDictionary StateTable { get { return (IWriteableIndexNodeStateReadOnlyDictionary)base.StateTable; } }

        /// <summary>
        /// List of operations that have been performed, and can be undone or redone.
        /// </summary>
        public IWriteableOperationGroupReadOnlyList OperationStack { get; }
        private IWriteableOperationGroupList _OperationStack;

        /// <summary>
        /// Index of the next operation that can be redone in <see cref="OperationStack"/>.
        /// </summary>
        public int RedoIndex { get; private set; }

        /// <summary>
        /// Checks if there is an operation that can be undone.
        /// </summary>
        public bool CanUndo { get { return RedoIndex > 0; } }

        /// <summary>
        /// Checks if there is an operation that can be redone.
        /// </summary>
        public bool CanRedo { get { return RedoIndex < OperationStack.Count; } }

        /// <summary>
        /// Called when a block state is inserted.
        /// </summary>
        public event Action<IWriteableInsertBlockOperation> BlockStateInserted
        {
            add { AddBlockStateInsertedDelegate(value); }
            remove { RemoveBlockStateInsertedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableInsertBlockOperation> BlockStateInsertedHandler;
        private protected virtual void AddBlockStateInsertedDelegate(Action<IWriteableInsertBlockOperation> handler) { BlockStateInsertedHandler += handler; }
        private protected virtual void RemoveBlockStateInsertedDelegate(Action<IWriteableInsertBlockOperation> handler) { BlockStateInsertedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        public event Action<IWriteableRemoveBlockOperation> BlockStateRemoved
        {
            add { AddBlockStateRemovedDelegate(value); }
            remove { RemoveBlockStateRemovedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableRemoveBlockOperation> BlockStateRemovedHandler;
        private protected virtual void AddBlockStateRemovedDelegate(Action<IWriteableRemoveBlockOperation> handler) { BlockStateRemovedHandler += handler; }
        private protected virtual void RemoveBlockStateRemovedDelegate(Action<IWriteableRemoveBlockOperation> handler) { BlockStateRemovedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a block view must be removed.
        /// </summary>
        public event Action<IWriteableRemoveBlockViewOperation> BlockViewRemoved
        {
            add { AddBlockViewRemovedDelegate(value); }
            remove { RemoveBlockViewRemovedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableRemoveBlockViewOperation> BlockViewRemovedHandler;
        private protected virtual void AddBlockViewRemovedDelegate(Action<IWriteableRemoveBlockViewOperation> handler) { BlockViewRemovedHandler += handler; }
        private protected virtual void RemoveBlockViewRemovedDelegate(Action<IWriteableRemoveBlockViewOperation> handler) { BlockViewRemovedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a state is inserted.
        /// </summary>
        public event Action<IWriteableInsertNodeOperation> StateInserted
        {
            add { AddStateInsertedDelegate(value); }
            remove { RemoveStateInsertedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableInsertNodeOperation> StateInsertedHandler;
        private protected virtual void AddStateInsertedDelegate(Action<IWriteableInsertNodeOperation> handler) { StateInsertedHandler += handler; }
        private protected virtual void RemoveStateInsertedDelegate(Action<IWriteableInsertNodeOperation> handler) { StateInsertedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a state is removed.
        /// </summary>
        public event Action<IWriteableRemoveNodeOperation> StateRemoved
        {
            add { AddStateRemovedDelegate(value); }
            remove { RemoveStateRemovedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableRemoveNodeOperation> StateRemovedHandler;
        private protected virtual void AddStateRemovedDelegate(Action<IWriteableRemoveNodeOperation> handler) { StateRemovedHandler += handler; }
        private protected virtual void RemoveStateRemovedDelegate(Action<IWriteableRemoveNodeOperation> handler) { StateRemovedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a state is replaced.
        /// </summary>
        public event Action<IWriteableReplaceOperation> StateReplaced
        {
            add { AddStateReplacedDelegate(value); }
            remove { RemoveStateReplacedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableReplaceOperation> StateReplacedHandler;
        private protected virtual void AddStateReplacedDelegate(Action<IWriteableReplaceOperation> handler) { StateReplacedHandler += handler; }
        private protected virtual void RemoveStateReplacedDelegate(Action<IWriteableReplaceOperation> handler) { StateReplacedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a state is assigned.
        /// </summary>
        public event Action<IWriteableAssignmentOperation> StateAssigned
        {
            add { AddStateAssignedDelegate(value); }
            remove { RemoveStateAssignedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableAssignmentOperation> StateAssignedHandler;
        private protected virtual void AddStateAssignedDelegate(Action<IWriteableAssignmentOperation> handler) { StateAssignedHandler += handler; }
        private protected virtual void RemoveStateAssignedDelegate(Action<IWriteableAssignmentOperation> handler) { StateAssignedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a state is unassigned.
        /// </summary>
        public event Action<IWriteableAssignmentOperation> StateUnassigned
        {
            add { AddStateUnassignedDelegate(value); }
            remove { RemoveStateUnassignedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableAssignmentOperation> StateUnassignedHandler;
        private protected virtual void AddStateUnassignedDelegate(Action<IWriteableAssignmentOperation> handler) { StateUnassignedHandler += handler; }
        private protected virtual void RemoveStateUnassignedDelegate(Action<IWriteableAssignmentOperation> handler) { StateUnassignedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a discrete value is changed.
        /// </summary>
        public event Action<IWriteableChangeDiscreteValueOperation> DiscreteValueChanged
        {
            add { AddDiscreteValueChangedDelegate(value); }
            remove { RemoveDiscreteValueChangedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableChangeDiscreteValueOperation> DiscreteValueChangedHandler;
        private protected virtual void AddDiscreteValueChangedDelegate(Action<IWriteableChangeDiscreteValueOperation> handler) { DiscreteValueChangedHandler += handler; }
        private protected virtual void RemoveDiscreteValueChangedDelegate(Action<IWriteableChangeDiscreteValueOperation> handler) { DiscreteValueChangedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when text is changed.
        /// </summary>
        public event Action<IWriteableChangeTextOperation> TextChanged
        {
            add { AddTextChangedDelegate(value); }
            remove { RemoveTextChangedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableChangeTextOperation> TextChangedHandler;
        private protected virtual void AddTextChangedDelegate(Action<IWriteableChangeTextOperation> handler) { TextChangedHandler += handler; }
        private protected virtual void RemoveTextChangedDelegate(Action<IWriteableChangeTextOperation> handler) { TextChangedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a comment is changed.
        /// </summary>
        public event Action<IWriteableChangeCommentOperation> CommentChanged
        {
            add { AddCommentChangedDelegate(value); }
            remove { RemoveCommentChangedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableChangeCommentOperation> CommentChangedHandler;
        private protected virtual void AddCommentChangedDelegate(Action<IWriteableChangeCommentOperation> handler) { CommentChangedHandler += handler; }
        private protected virtual void RemoveCommentChangedDelegate(Action<IWriteableChangeCommentOperation> handler) { CommentChangedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a block state is changed.
        /// </summary>
        public event Action<IWriteableChangeBlockOperation> BlockStateChanged
        {
            add { AddBlockStateChangedDelegate(value); }
            remove { RemoveBlockStateChangedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableChangeBlockOperation> BlockStateChangedHandler;
        private protected virtual void AddBlockStateChangedDelegate(Action<IWriteableChangeBlockOperation> handler) { BlockStateChangedHandler += handler; }
        private protected virtual void RemoveBlockStateChangedDelegate(Action<IWriteableChangeBlockOperation> handler) { BlockStateChangedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a state is moved.
        /// </summary>
        public event Action<IWriteableMoveNodeOperation> StateMoved
        {
            add { AddStateMovedDelegate(value); }
            remove { RemoveStateMovedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableMoveNodeOperation> StateMovedHandler;
        private protected virtual void AddStateMovedDelegate(Action<IWriteableMoveNodeOperation> handler) { StateMovedHandler += handler; }
        private protected virtual void RemoveStateMovedDelegate(Action<IWriteableMoveNodeOperation> handler) { StateMovedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a block state is moved.
        /// </summary>
        public event Action<IWriteableMoveBlockOperation> BlockStateMoved
        {
            add { AddBlockStateMovedDelegate(value); }
            remove { RemoveBlockStateMovedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableMoveBlockOperation> BlockStateMovedHandler;
        private protected virtual void AddBlockStateMovedDelegate(Action<IWriteableMoveBlockOperation> handler) { BlockStateMovedHandler += handler; }
        private protected virtual void RemoveBlockStateMovedDelegate(Action<IWriteableMoveBlockOperation> handler) { BlockStateMovedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a block is split.
        /// </summary>
        public event Action<IWriteableSplitBlockOperation> BlockSplit
        {
            add { AddBlockSplitDelegate(value); }
            remove { RemoveBlockSplitDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableSplitBlockOperation> BlockSplitHandler;
        private protected virtual void AddBlockSplitDelegate(Action<IWriteableSplitBlockOperation> handler) { BlockSplitHandler += handler; }
        private protected virtual void RemoveBlockSplitDelegate(Action<IWriteableSplitBlockOperation> handler) { BlockSplitHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when two blocks are merged.
        /// </summary>
        public event Action<IWriteableMergeBlocksOperation> BlocksMerged
        {
            add { AddBlocksMergedDelegate(value); }
            remove { RemoveBlocksMergedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableMergeBlocksOperation> BlocksMergedHandler;
        private protected virtual void AddBlocksMergedDelegate(Action<IWriteableMergeBlocksOperation> handler) { BlocksMergedHandler += handler; }
        private protected virtual void RemoveBlocksMergedDelegate(Action<IWriteableMergeBlocksOperation> handler) { BlocksMergedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called to refresh views.
        /// </summary>
        public event Action<IWriteableGenericRefreshOperation> GenericRefresh
        {
            add { AddGenericRefreshDelegate(value); }
            remove { RemoveGenericRefreshDelegate(value); }
        }
#pragma warning disable 1591
        private Action<IWriteableGenericRefreshOperation> GenericRefreshHandler;
        private protected virtual void AddGenericRefreshDelegate(Action<IWriteableGenericRefreshOperation> handler) { GenericRefreshHandler += handler; }
        private protected virtual void RemoveGenericRefreshDelegate(Action<IWriteableGenericRefreshOperation> handler) { GenericRefreshHandler -= handler; }
#pragma warning restore 1591
        #endregion

        #region Client Interface
        /// <summary>
        /// Inserts a new node in a list or block list.
        /// </summary>
        /// <param name="inner">The inner for the list or block list where the node is inserted.</param>
        /// <param name="insertedIndex">Index for the insertion operation.</param>
        /// <param name="nodeIndex">Index of the inserted node upon return.</param>
        public virtual void Insert(IWriteableCollectionInner inner, IWriteableInsertionCollectionNodeIndex insertedIndex, out IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(insertedIndex != null);
            IWriteableNodeState Owner = inner.Owner;
            IWriteableIndex ParentIndex = Owner.ParentIndex;
            Debug.Assert(Contains(ParentIndex));
            Debug.Assert(IndexToState(ParentIndex) == Owner);
            IWriteableInnerReadOnlyDictionary<string> InnerTable = Owner.InnerTable;
            Debug.Assert(InnerTable.ContainsKey(inner.PropertyName));
            Debug.Assert(InnerTable[inner.PropertyName] == inner);

            bool IsHandled = false;
            nodeIndex = null;

            if (inner is IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> AsBlockListInner && insertedIndex is IWriteableInsertionNewBlockNodeIndex AsNewBlockIndex)
            {
                InsertNewBlock(AsBlockListInner, AsNewBlockIndex, out nodeIndex);
                IsHandled = true;
            }
            else if (inner is IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> AsCollectionInner && insertedIndex is IWriteableInsertionCollectionNodeIndex AsCollectionIndex)
            {
                InsertNewNode(AsCollectionInner, AsCollectionIndex, out nodeIndex);
                IsHandled = true;
            }

            Debug.Assert(IsHandled);
        }

        /// <summary></summary>
        private protected virtual void InsertNewBlock(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> blockListInner, IWriteableInsertionNewBlockNodeIndex newBlockIndex, out IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            IBlock NewBlock = NodeTreeHelperBlockList.CreateBlock(blockListInner.Owner.Node, blockListInner.PropertyName, ReplicationStatus.Normal, newBlockIndex.PatternNode, newBlockIndex.SourceNode);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoInsertNewBlock(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoInsertNewBlock(operation);
            IWriteableInsertBlockOperation Operation = CreateInsertBlockOperation(blockListInner.Owner.Node, blockListInner.PropertyName, newBlockIndex.BlockIndex, NewBlock, newBlockIndex.Node, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();

            nodeIndex = Operation.BrowsingIndex;
        }

        /// <summary></summary>
        private protected virtual void RedoInsertNewBlock(IWriteableOperation operation)
        {
            IWriteableInsertBlockOperation InsertBlockOperation = (IWriteableInsertBlockOperation)operation;
            ExecuteInsertNewBlock(InsertBlockOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteInsertNewBlock(IWriteableInsertBlockOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>;

            Inner.InsertNewBlock(operation);

            IWriteableBrowsingExistingBlockNodeIndex BrowsingIndex = operation.BrowsingIndex;
            IWriteableBlockState BlockState = operation.BlockState;
            IWriteablePlaceholderNodeState ChildState = operation.ChildState;

            Debug.Assert(BlockState.StateList.Count == 1);
            Debug.Assert(BlockState.StateList[0] == ChildState);
            ((IWriteableBlockState<IWriteableInner<IWriteableBrowsingChildIndex>>)BlockState).InitBlockState();
            Stats.BlockCount++;

            IWriteableBrowsingPatternIndex PatternIndex = BlockState.PatternIndex;
            IWriteablePatternState PatternState = BlockState.PatternState;
            AddState(PatternIndex, PatternState);
            Stats.PlaceholderNodeCount++;

            IWriteableBrowsingSourceIndex SourceIndex = BlockState.SourceIndex;
            IWriteableSourceState SourceState = BlockState.SourceState;
            AddState(SourceIndex, SourceState);
            Stats.PlaceholderNodeCount++;

            AddState(BrowsingIndex, ChildState);
            Stats.PlaceholderNodeCount++;
            BuildStateTable(Inner, null, BrowsingIndex, ChildState);

            Debug.Assert(Contains(BrowsingIndex));

            NotifyBlockStateInserted(operation);
        }

        /// <summary></summary>
        private protected virtual void UndoInsertNewBlock(IWriteableOperation operation)
        {
            IWriteableInsertBlockOperation InsertBlockOperation = (IWriteableInsertBlockOperation)operation;
            IWriteableRemoveBlockOperation RemoveBlockOperation = InsertBlockOperation.ToRemoveBlockOperation();

            ExecuteRemoveBlock(RemoveBlockOperation);
        }

        /// <summary></summary>
        private protected virtual void InsertNewNode(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, IWriteableInsertionCollectionNodeIndex insertedIndex, out IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            IndexToPositionAndNode(insertedIndex, out int BlockIndex, out int Index, out INode Node);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoInsertNewNode(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoInsertNewNode(operation);
            IWriteableInsertNodeOperation Operation = CreateInsertNodeOperation(inner.Owner.Node, inner.PropertyName, BlockIndex, Index, Node, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();

            nodeIndex = Operation.BrowsingIndex;
        }

        /// <summary></summary>
        private protected virtual void RedoInsertNewNode(IWriteableOperation operation)
        {
            IWriteableInsertNodeOperation InsertNodeOperation = (IWriteableInsertNodeOperation)operation;
            ExecuteInsertNewNode(InsertNodeOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteInsertNewNode(IWriteableInsertNodeOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex>;

            Inner.Insert(operation);

            IWriteableBrowsingCollectionNodeIndex BrowsingIndex = operation.BrowsingIndex;
            IWriteablePlaceholderNodeState ChildState = operation.ChildState;

            AddState(BrowsingIndex, ChildState);
            Stats.PlaceholderNodeCount++;
            BuildStateTable(Inner, null, BrowsingIndex, ChildState);

            Debug.Assert(Contains(BrowsingIndex));

            NotifyStateInserted(operation);
        }

        /// <summary></summary>
        private protected virtual void UndoInsertNewNode(IWriteableOperation operation)
        {
            IWriteableInsertNodeOperation InsertNodeOperation = (IWriteableInsertNodeOperation)operation;
            IWriteableRemoveNodeOperation RemoveNodeOperation = InsertNodeOperation.ToRemoveNodeOperation();

            ExecuteRemoveNode(RemoveNodeOperation);
        }

        /// <summary>
        /// Inserts a range of blocks in a block list.
        /// </summary>
        /// <param name="inner">The inner for the block list in which blocks are inserted.</param>
        /// <param name="insertedIndex">Index where to insert the first block.</param>
        /// <param name="indexList">List of nodes in blocks to insert.</param>
        public virtual void InsertBlockRange(IWriteableBlockListInner inner, int insertedIndex, IList<IWriteableInsertionBlockNodeIndex> indexList)
        {
            Debug.Assert(inner != null);
            Debug.Assert(insertedIndex >= 0 && insertedIndex <= inner.BlockStateList.Count);
            Debug.Assert(indexList != null);

            int BlockIndex = insertedIndex - 1;
            int BlockNodeIndex = 0;

            foreach (IWriteableInsertionBlockNodeIndex NodeIndex in indexList)
            {
                bool IsHandled = false;

                if (NodeIndex is IWriteableInsertionNewBlockNodeIndex AsNewBlockNodeIndex)
                {
                    BlockIndex++;
                    BlockNodeIndex = 0;

                    Debug.Assert(AsNewBlockNodeIndex.BlockIndex == BlockIndex);

                    IsHandled = true;
                }
                else if (NodeIndex is IWriteableInsertionExistingBlockNodeIndex AsExistingBlockNodeIndex)
                {
                    BlockNodeIndex++;

                    Debug.Assert(AsExistingBlockNodeIndex.BlockIndex == BlockIndex);
                    Debug.Assert(AsExistingBlockNodeIndex.Index == BlockNodeIndex);

                    IsHandled = true;
                }

                Debug.Assert(IsHandled);
            }

            int FinalBlockIndex = BlockIndex + 1;

            Action<IWriteableOperation> HandlerRedoInsertNode = (IWriteableOperation operation) => RedoInsertNewNode(operation);
            Action<IWriteableOperation> HandlerUndoInsertNode = (IWriteableOperation operation) => UndoInsertNewNode(operation);
            Action<IWriteableOperation> HandlerRedoInsertBlock = (IWriteableOperation operation) => RedoInsertNewBlock(operation);
            Action<IWriteableOperation> HandlerUndoInsertBlock = (IWriteableOperation operation) => UndoInsertNewBlock(operation);

            IWriteableOperationList OperationList = CreateOperationList();

            foreach (IWriteableInsertionBlockNodeIndex NodeIndex in indexList)
                if (NodeIndex is IWriteableInsertionNewBlockNodeIndex AsNewBlockNodeIndex)
                {
                    IBlock NewBlock = NodeTreeHelperBlockList.CreateBlock(inner.Owner.Node, inner.PropertyName, ReplicationStatus.Normal, AsNewBlockNodeIndex.PatternNode, AsNewBlockNodeIndex.SourceNode);
                    IWriteableInsertBlockOperation OperationInsertBlock = CreateInsertBlockOperation(inner.Owner.Node, inner.PropertyName, AsNewBlockNodeIndex.BlockIndex, NewBlock, AsNewBlockNodeIndex.Node, HandlerRedoInsertBlock, HandlerUndoInsertBlock, isNested: true);
                    OperationList.Add(OperationInsertBlock);
                }
                else if (NodeIndex is IWriteableInsertionExistingBlockNodeIndex AsExistingBlockNodeIndex)
                {
                    IndexToPositionAndNode(AsExistingBlockNodeIndex, out BlockIndex, out int Index, out INode Node);
                    IWriteableInsertNodeOperation OperationInsertNode = CreateInsertNodeOperation(inner.Owner.Node, inner.PropertyName, BlockIndex, Index, Node, HandlerRedoInsertNode, HandlerUndoInsertNode, isNested: true);
                    OperationList.Add(OperationInsertNode);
                }

            Debug.Assert(BlockIndex + 1 == FinalBlockIndex);

            if (OperationList.Count > 0)
            {
                IWriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRefresh(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
                IWriteableGenericRefreshOperation RefreshOperation = CreateGenericRefreshOperation(RootState, HandlerRedo, HandlerUndo, isNested: false);
                IWriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, RefreshOperation);

                OperationGroup.Redo();
                SetLastOperation(OperationGroup);
                CheckInvariant();
            }
        }

        /// <summary>
        /// Inserts a range of nodes in a list or block list.
        /// </summary>
        /// <param name="inner">The inner for the list or block list in which nodes are inserted.</param>
        /// <param name="blockIndex">Index of the block where to insert nodes, for a block list. -1 for a list.</param>
        /// <param name="insertedIndex">Index of the first node to insert.</param>
        /// <param name="indexList">List of nodes to insert.</param>
        public virtual void InsertNodeRange(IWriteableCollectionInner inner, int blockIndex, int insertedIndex, IList<IWriteableInsertionCollectionNodeIndex> indexList)
        {
            Debug.Assert(inner != null);

            bool IsHandled = false;

            if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                InsertNodeRange(AsBlockListInner, blockIndex, insertedIndex, indexList);
                IsHandled = true;
            }

            else if (inner is IWriteableListInner AsListInner)
            {
                Debug.Assert(blockIndex == -1);
                InsertNodeRange(AsListInner, insertedIndex, indexList);
                IsHandled = true;
            }

            Debug.Assert(IsHandled);
        }

        /// <summary></summary>
        public virtual void InsertNodeRange(IWriteableBlockListInner inner, int blockIndex, int insertedIndex, IList<IWriteableInsertionCollectionNodeIndex> indexList)
        {
            Debug.Assert(inner != null);
            Debug.Assert(blockIndex >= 0 && blockIndex < inner.BlockStateList.Count);

            IWriteableBlockState BlockState = inner.BlockStateList[blockIndex];
            Debug.Assert(insertedIndex >= 0 && insertedIndex <= BlockState.StateList.Count);

            int BlockNodeIndex = insertedIndex;

            foreach (IWriteableInsertionCollectionNodeIndex NodeIndex in indexList)
            {
                bool IsHandled = false;

                if (NodeIndex is IWriteableInsertionExistingBlockNodeIndex AsExistingBlockNodeIndex)
                {
                    Debug.Assert(AsExistingBlockNodeIndex.BlockIndex == blockIndex);
                    Debug.Assert(AsExistingBlockNodeIndex.Index == BlockNodeIndex);

                    BlockNodeIndex++;
                    IsHandled = true;
                }

                Debug.Assert(IsHandled);
            }

            int FinalNodeIndex = BlockNodeIndex;

            Action<IWriteableOperation> HandlerRedoInsertNode = (IWriteableOperation operation) => RedoInsertNewNode(operation);
            Action<IWriteableOperation> HandlerUndoInsertNode = (IWriteableOperation operation) => UndoInsertNewNode(operation);

            IWriteableOperationList OperationList = CreateOperationList();

            foreach (IWriteableInsertionCollectionNodeIndex NodeIndex in indexList)
                if (NodeIndex is IWriteableInsertionExistingBlockNodeIndex AsExistingBlockNodeIndex)
                {
                    IndexToPositionAndNode(AsExistingBlockNodeIndex, out blockIndex, out int Index, out INode Node);
                    IWriteableInsertNodeOperation OperationInsertNode = CreateInsertNodeOperation(inner.Owner.Node, inner.PropertyName, blockIndex, Index, Node, HandlerRedoInsertNode, HandlerUndoInsertNode, isNested: true);
                    OperationList.Add(OperationInsertNode);
                }

            if (OperationList.Count > 0)
            {
                IWriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRefresh(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
                IWriteableGenericRefreshOperation RefreshOperation = CreateGenericRefreshOperation(RootState, HandlerRedo, HandlerUndo, isNested: false);
                IWriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, RefreshOperation);

                OperationGroup.Redo();
                SetLastOperation(OperationGroup);
                CheckInvariant();
            }
        }

        /// <summary></summary>
        public virtual void InsertNodeRange(IWriteableListInner inner, int insertedIndex, IList<IWriteableInsertionCollectionNodeIndex> indexList)
        {
            Debug.Assert(inner != null);
            Debug.Assert(insertedIndex >= 0 && insertedIndex <= inner.StateList.Count);

            int BlockNodeIndex = insertedIndex;

            foreach (IWriteableInsertionCollectionNodeIndex NodeIndex in indexList)
            {
                bool IsHandled = false;

                if (NodeIndex is IWriteableInsertionListNodeIndex AsListNodeIndex)
                {
                    Debug.Assert(AsListNodeIndex.Index == BlockNodeIndex);

                    BlockNodeIndex++;
                    IsHandled = true;
                }

                Debug.Assert(IsHandled);
            }

            int FinalNodeIndex = BlockNodeIndex;

            Action<IWriteableOperation> HandlerRedoInsertNode = (IWriteableOperation operation) => RedoInsertNewNode(operation);
            Action<IWriteableOperation> HandlerUndoInsertNode = (IWriteableOperation operation) => UndoInsertNewNode(operation);

            IWriteableOperationList OperationList = CreateOperationList();

            foreach (IWriteableInsertionCollectionNodeIndex NodeIndex in indexList)
                if (NodeIndex is IWriteableInsertionListNodeIndex AsListNodeIndex)
                {
                    IndexToPositionAndNode(AsListNodeIndex, out int BlockIndex, out int Index, out INode Node);
                    IWriteableInsertNodeOperation OperationInsertNode = CreateInsertNodeOperation(inner.Owner.Node, inner.PropertyName, BlockIndex, Index, Node, HandlerRedoInsertNode, HandlerUndoInsertNode, isNested: true);
                    OperationList.Add(OperationInsertNode);
                }

            if (OperationList.Count > 0)
            {
                IWriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRefresh(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
                IWriteableGenericRefreshOperation RefreshOperation = CreateGenericRefreshOperation(RootState, HandlerRedo, HandlerUndo, isNested: false);
                IWriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, RefreshOperation);

                OperationGroup.Redo();
                SetLastOperation(OperationGroup);
                CheckInvariant();
            }
        }

        /// <summary>
        /// Checks whether a node can be removed from a list.
        /// </summary>
        /// <param name="inner">The inner where the node is.</param>
        /// <param name="nodeIndex">Index of the node that would be removed.</param>
        public bool IsRemoveable(IWriteableCollectionInner inner, IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            if (inner.Count > 1)
                return true;

            Debug.Assert(inner.Count == 1);
            Debug.Assert(inner.Owner != null);

            INode Node = inner.Owner.Node;
            string PropertyName = inner.PropertyName;
            Debug.Assert(Node != null);

            bool Result = true;

            Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(inner.Owner.Node.GetType());
            IReadOnlyDictionary<Type, string[]> NeverEmptyCollectionTable = NodeHelper.NeverEmptyCollectionTable;
            if (NeverEmptyCollectionTable.ContainsKey(InterfaceType))
            {
                foreach (string Item in NeverEmptyCollectionTable[InterfaceType])
                    if (Item == PropertyName)
                        Result = false;
            }

            return Result;
        }

        /// <summary>
        /// Removes a node from a list or block list.
        /// </summary>
        /// <param name="inner">The inner for the list or block list from which the node is removed.</param>
        /// <param name="nodeIndex">Index for the removed node.</param>
        public virtual void Remove(IWriteableCollectionInner inner, IWriteableBrowsingCollectionNodeIndex nodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);
            IWriteableNodeState Owner = inner.Owner;
            IWriteableIndex ParentIndex = Owner.ParentIndex;
            Debug.Assert(Contains(ParentIndex));
            Debug.Assert(IndexToState(ParentIndex) == Owner);
            IWriteableInnerReadOnlyDictionary<string> InnerTable = Owner.InnerTable;
            Debug.Assert(InnerTable.ContainsKey(inner.PropertyName));
            Debug.Assert(InnerTable[inner.PropertyName] == inner);

            IndexToPositionAndNode(nodeIndex, out int BlockIndex, out int Index, out INode Node);

            bool IsHandled = false;

            if (inner is IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> AsBlockListInner && nodeIndex is IWriteableBrowsingExistingBlockNodeIndex ExistingBlockIndex)
            {
                if (AsBlockListInner.BlockStateList[ExistingBlockIndex.BlockIndex].StateList.Count == 1)
                    RemoveBlock(AsBlockListInner, BlockIndex);
                else
                    RemoveNode(AsBlockListInner, BlockIndex, Index);

                IsHandled = true;
            }
            else if (inner is IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> AsCollectionInner)
            {
                RemoveNode(AsCollectionInner, BlockIndex, Index);
                IsHandled = true;
            }

            Debug.Assert(IsHandled);
        }

        /// <summary></summary>
        private protected virtual void RemoveBlock(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> blockListInner, int blockIndex)
        {
            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRemoveBlock(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoRemoveBlock(operation);
            IWriteableRemoveBlockOperation Operation = CreateRemoveBlockOperation(blockListInner.Owner.Node, blockListInner.PropertyName, blockIndex, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        /// <summary></summary>
        private protected virtual void RedoRemoveBlock(IWriteableOperation operation)
        {
            IWriteableRemoveBlockOperation RemoveBlockOperation = (IWriteableRemoveBlockOperation)operation;
            ExecuteRemoveBlock(RemoveBlockOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteRemoveBlock(IWriteableRemoveBlockOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>;

            Inner.RemoveWithBlock(operation);

            IWriteableBlockState RemovedBlockState = operation.BlockState;
            Debug.Assert(RemovedBlockState != null);

            IWriteableBrowsingPatternIndex PatternIndex = RemovedBlockState.PatternIndex;
            IWriteableBrowsingSourceIndex SourceIndex = RemovedBlockState.SourceIndex;

            Debug.Assert(PatternIndex != null);
            Debug.Assert(StateTable.ContainsKey(PatternIndex));
            Debug.Assert(SourceIndex != null);
            Debug.Assert(StateTable.ContainsKey(SourceIndex));

            Stats.BlockCount--;

            RemoveState(PatternIndex);
            Stats.PlaceholderNodeCount--;

            RemoveState(SourceIndex);
            Stats.PlaceholderNodeCount--;

            IWriteableNodeState RemovedState = operation.RemovedState;
            Debug.Assert(RemovedState != null);

            PruneState(RemovedState);
            Stats.PlaceholderNodeCount--;

            NotifyBlockStateRemoved(operation);
        }

        /// <summary></summary>
        private protected virtual void UndoRemoveBlock(IWriteableOperation operation)
        {
            IWriteableRemoveBlockOperation RemoveBlockOperation = (IWriteableRemoveBlockOperation)operation;
            IWriteableInsertBlockOperation InsertBlockOperation = RemoveBlockOperation.ToInsertBlockOperation();

            ExecuteInsertNewBlock(InsertBlockOperation);
        }

        /// <summary></summary>
        private protected virtual void RemoveNode(IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> inner, int blockIndex, int index)
        {
            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRemoveNode(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoRemoveNode(operation);
            IWriteableRemoveNodeOperation Operation = CreateRemoveNodeOperation(inner.Owner.Node, inner.PropertyName, blockIndex, index, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        /// <summary></summary>
        private protected virtual void RedoRemoveNode(IWriteableOperation operation)
        {
            IWriteableRemoveNodeOperation RemoveNodeOperation = (IWriteableRemoveNodeOperation)operation;
            ExecuteRemoveNode(RemoveNodeOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteRemoveNode(IWriteableRemoveNodeOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex>;

            Inner.Remove(operation);

            IWriteableNodeState RemovedState = operation.RemovedState;
            PruneState(RemovedState);
            Stats.PlaceholderNodeCount--;

            NotifyStateRemoved(operation);
        }

        /// <summary></summary>
        private protected virtual void UndoRemoveNode(IWriteableOperation operation)
        {
            IWriteableRemoveNodeOperation RemoveNodeOperation = (IWriteableRemoveNodeOperation)operation;
            IWriteableInsertNodeOperation InsertNodeOperation = RemoveNodeOperation.ToInsertNodeOperation();

            ExecuteInsertNewNode(InsertNodeOperation);
        }

        /// <summary>
        /// Replace an existing node with a new one.
        /// </summary>
        /// <param name="inner">The inner where the node is replaced.</param>
        /// <param name="replaceIndex">Index for the replace operation.</param>
        /// <param name="nodeIndex">Index of the replacing node upon return.</param>
        public void Replace(IWriteableInner inner, IWriteableInsertionChildIndex replaceIndex, out IWriteableBrowsingChildIndex nodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(replaceIndex != null);
            IWriteableNodeState Owner = inner.Owner;
            IWriteableIndex ParentIndex = Owner.ParentIndex;
            Debug.Assert(Contains(ParentIndex));
            Debug.Assert(IndexToState(ParentIndex) == Owner);
            IWriteableInnerReadOnlyDictionary<string> InnerTable = Owner.InnerTable;
            Debug.Assert(InnerTable.ContainsKey(inner.PropertyName));
            Debug.Assert(InnerTable[inner.PropertyName] == inner);

            IndexToPositionAndNode(replaceIndex, out int BlockIndex, out int Index, out INode NewNode);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoReplace(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoReplace(operation);
            IWriteableReplaceOperation Operation = CreateReplaceOperation(inner.Owner.Node, inner.PropertyName, BlockIndex, Index, NewNode, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();

            nodeIndex = Operation.NewBrowsingIndex;
        }

        /// <summary></summary>
        private protected virtual void RedoReplace(IWriteableOperation operation)
        {
            IWriteableReplaceOperation ReplaceOperation = (IWriteableReplaceOperation)operation;
            ExecuteReplace(ReplaceOperation);
        }

        /// <summary></summary>
        private protected virtual void UndoReplace(IWriteableOperation operation)
        {
            IWriteableReplaceOperation ReplaceOperation = (IWriteableReplaceOperation)operation;
            ReplaceOperation = ReplaceOperation.ToInverseReplace();

            ExecuteReplace(ReplaceOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteReplace(IWriteableReplaceOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableInner<IWriteableBrowsingChildIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableInner<IWriteableBrowsingChildIndex>;

            ReplaceState(operation, Inner);
            Debug.Assert(Contains(operation.NewBrowsingIndex));

            NotifyStateReplaced(operation);
        }

        /// <summary></summary>
        private protected virtual void ReplaceState(IWriteableReplaceOperation operation, IWriteableInner<IWriteableBrowsingChildIndex> inner)
        {
            Debug.Assert(inner != null);
            IWriteableNodeState Owner = inner.Owner;
            IWriteableIndex ParentIndex = Owner.ParentIndex;
            Debug.Assert(Contains(ParentIndex));
            Debug.Assert(IndexToState(ParentIndex) == Owner);
            IWriteableInnerReadOnlyDictionary<string> InnerTable = Owner.InnerTable;
            Debug.Assert(InnerTable.ContainsKey(inner.PropertyName));
            Debug.Assert(InnerTable[inner.PropertyName] == inner);

            IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> AsOptionalInner = inner as IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex>;
            if (AsOptionalInner != null)
            {
                IWriteableNodeState OldState = AsOptionalInner.ChildState;
                PruneStateChildren(OldState);

                if (AsOptionalInner.IsAssigned)
                    Stats.AssignedOptionalNodeCount--;
            }

            inner.Replace(operation);

            IWriteableBrowsingChildIndex OldBrowsingIndex = operation.OldBrowsingIndex;
            IWriteableBrowsingChildIndex NewBrowsingIndex = operation.NewBrowsingIndex;
            IWriteableNodeState ChildState = operation.NewChildState;

            if (AsOptionalInner != null)
            {
                if (AsOptionalInner.IsAssigned)
                    Stats.AssignedOptionalNodeCount++;
            }
            else
            {
                Debug.Assert(Contains(OldBrowsingIndex));
                IWriteableNodeState OldState = StateTable[OldBrowsingIndex];

                PruneStateChildren(OldState);
            }

            RemoveState(OldBrowsingIndex);
            AddState(NewBrowsingIndex, ChildState);

            BuildStateTable(inner, null, NewBrowsingIndex, ChildState);
        }

        /// <summary>
        /// Checks whether a range of blocks can be removed from a block list.
        /// </summary>
        /// <param name="inner">The inner with blocks to remove.</param>
        /// <param name="firstBlockIndex">Index of the first block to remove.</param>
        /// <param name="lastBlockIndex">Index following the last block to remove.</param>
        public virtual bool IsBlockRangeRemoveable(IWriteableBlockListInner inner, int firstBlockIndex, int lastBlockIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(firstBlockIndex >= 0 && firstBlockIndex < inner.BlockStateList.Count);
            Debug.Assert(lastBlockIndex >= 0 && lastBlockIndex <= inner.BlockStateList.Count);
            Debug.Assert(firstBlockIndex <= lastBlockIndex);

            int DeletedCount = lastBlockIndex - firstBlockIndex;
            if (inner.BlockStateList.Count > DeletedCount)
                return true;

            Debug.Assert(inner.BlockStateList.Count == DeletedCount);
            Debug.Assert(inner.Owner != null);

            INode Node = inner.Owner.Node;
            string PropertyName = inner.PropertyName;
            Debug.Assert(Node != null);

            bool Result = true;

            Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(inner.Owner.Node.GetType());
            IReadOnlyDictionary<Type, string[]> NeverEmptyCollectionTable = NodeHelper.NeverEmptyCollectionTable;
            if (NeverEmptyCollectionTable.ContainsKey(InterfaceType))
            {
                foreach (string Item in NeverEmptyCollectionTable[InterfaceType])
                    if (Item == PropertyName)
                        Result = false;
            }

            return Result;
        }

        /// <summary>
        /// Removes a range of blocks from a block list.
        /// </summary>
        /// <param name="inner">The inner for the block list from which blocks are removed.</param>
        /// <param name="firstBlockIndex">Index of the first block to remove.</param>
        /// <param name="lastBlockIndex">Index following the last block to remove.</param>
        public virtual void RemoveBlockRange(IWriteableBlockListInner inner, int firstBlockIndex, int lastBlockIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(firstBlockIndex >= 0 && firstBlockIndex < inner.BlockStateList.Count);
            Debug.Assert(lastBlockIndex >= 0 && lastBlockIndex <= inner.BlockStateList.Count);
            Debug.Assert(firstBlockIndex <= lastBlockIndex);

            Action<IWriteableOperation> HandlerRedoNode = (IWriteableOperation operation) => RedoRemoveNode(operation);
            Action<IWriteableOperation> HandlerUndoNode = (IWriteableOperation operation) => UndoRemoveNode(operation);
            Action<IWriteableOperation> HandlerRedoBlock = (IWriteableOperation operation) => RedoRemoveBlock(operation);
            Action<IWriteableOperation> HandlerUndoBlock = (IWriteableOperation operation) => UndoRemoveBlock(operation);

            IWriteableOperationList OperationList = CreateOperationList();

            for (int i = firstBlockIndex; i < lastBlockIndex; i++)
            {
                IWriteableBlockState BlockState = inner.BlockStateList[i];
                Debug.Assert(BlockState.StateList.Count >= 1);

                // Remove at firstBlockIndex since subsequent blocks are moved as the block at firstBlockIndex is deleted.
                // Same for nodes inside blokcks, delete them at 0.
                for (int j = 1; j < BlockState.StateList.Count; j++)
                {
                    IWriteableRemoveNodeOperation OperationNode = CreateRemoveNodeOperation(inner.Owner.Node, inner.PropertyName, firstBlockIndex, 0, HandlerRedoNode, HandlerUndoNode, isNested: true);
                    OperationList.Add(OperationNode);
                }

                IWriteableRemoveBlockOperation OperationBlock = CreateRemoveBlockOperation(inner.Owner.Node, inner.PropertyName, firstBlockIndex, HandlerRedoBlock, HandlerUndoBlock, isNested: true);
                OperationList.Add(OperationBlock);
            }

            if (OperationList.Count > 0)
            {
                IWriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRefresh(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
                IWriteableGenericRefreshOperation RefreshOperation = CreateGenericRefreshOperation(RootState, HandlerRedo, HandlerUndo, isNested: false);
                IWriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, RefreshOperation);

                OperationGroup.Redo();
                SetLastOperation(OperationGroup);
                CheckInvariant();
            }
        }

        /// <summary>
        /// Removes a range of blocks from a block list and replace them with other blocks.
        /// </summary>
        /// <param name="inner">The inner for the block list from which blocks are replaced.</param>
        /// <param name="firstBlockIndex">Index of the first block to remove.</param>
        /// <param name="lastBlockIndex">Index following the last block to remove.</param>
        /// <param name="indexList">List of nodes in blocks to insert.</param>
        public virtual void ReplaceBlockRange(IWriteableBlockListInner inner, int firstBlockIndex, int lastBlockIndex, IList<IWriteableInsertionBlockNodeIndex> indexList)
        {
            Debug.Assert(inner != null);
            Debug.Assert(firstBlockIndex >= 0 && firstBlockIndex < inner.BlockStateList.Count);
            Debug.Assert(lastBlockIndex >= 0 && lastBlockIndex <= inner.BlockStateList.Count);
            Debug.Assert(firstBlockIndex <= lastBlockIndex);
            Debug.Assert(indexList != null);

            int BlockIndex = firstBlockIndex - 1;
            int BlockNodeIndex = 0;

            foreach (IWriteableInsertionBlockNodeIndex NodeIndex in indexList)
            {
                bool IsHandled = false;

                if (NodeIndex is IWriteableInsertionNewBlockNodeIndex AsNewBlockNodeIndex)
                {
                    BlockIndex++;
                    BlockNodeIndex = 0;

                    Debug.Assert(AsNewBlockNodeIndex.BlockIndex == BlockIndex);

                    IsHandled = true;
                }
                else if (NodeIndex is IWriteableInsertionExistingBlockNodeIndex AsExistingBlockNodeIndex)
                {
                    BlockNodeIndex++;

                    Debug.Assert(AsExistingBlockNodeIndex.BlockIndex == BlockIndex);
                    Debug.Assert(AsExistingBlockNodeIndex.Index == BlockNodeIndex);

                    IsHandled = true;
                }

                Debug.Assert(IsHandled);
            }

            int FinalBlockIndex = BlockIndex + 1;

            Action<IWriteableOperation> HandlerRedoInsertNode = (IWriteableOperation operation) => RedoInsertNewNode(operation);
            Action<IWriteableOperation> HandlerUndoInsertNode = (IWriteableOperation operation) => UndoInsertNewNode(operation);
            Action<IWriteableOperation> HandlerRedoInsertBlock = (IWriteableOperation operation) => RedoInsertNewBlock(operation);
            Action<IWriteableOperation> HandlerUndoInsertBlock = (IWriteableOperation operation) => UndoInsertNewBlock(operation);
            Action<IWriteableOperation> HandlerRedoRemoveNode = (IWriteableOperation operation) => RedoRemoveNode(operation);
            Action<IWriteableOperation> HandlerUndoRemoveNode = (IWriteableOperation operation) => UndoRemoveNode(operation);
            Action<IWriteableOperation> HandlerRedoRemoveBlock = (IWriteableOperation operation) => RedoRemoveBlock(operation);
            Action<IWriteableOperation> HandlerUndoRemoveBlock = (IWriteableOperation operation) => UndoRemoveBlock(operation);

            IWriteableOperationList OperationList = CreateOperationList();

            // Insert first to prevent empty block lists.
            foreach (IWriteableInsertionBlockNodeIndex NodeIndex in indexList)
                if (NodeIndex is IWriteableInsertionNewBlockNodeIndex AsNewBlockNodeIndex)
                {
                    IBlock NewBlock = NodeTreeHelperBlockList.CreateBlock(inner.Owner.Node, inner.PropertyName, ReplicationStatus.Normal, AsNewBlockNodeIndex.PatternNode, AsNewBlockNodeIndex.SourceNode);
                    IWriteableInsertBlockOperation OperationInsertBlock = CreateInsertBlockOperation(inner.Owner.Node, inner.PropertyName, AsNewBlockNodeIndex.BlockIndex, NewBlock, AsNewBlockNodeIndex.Node, HandlerRedoInsertBlock, HandlerUndoInsertBlock, isNested: true);
                    OperationList.Add(OperationInsertBlock);
                }
                else if (NodeIndex is IWriteableInsertionExistingBlockNodeIndex AsExistingBlockNodeIndex)
                {
                    IndexToPositionAndNode(AsExistingBlockNodeIndex, out BlockIndex, out int Index, out INode Node);
                    IWriteableInsertNodeOperation OperationInsertNode = CreateInsertNodeOperation(inner.Owner.Node, inner.PropertyName, BlockIndex, Index, Node, HandlerRedoInsertNode, HandlerUndoInsertNode, isNested: true);
                    OperationList.Add(OperationInsertNode);
                }

            Debug.Assert(BlockIndex + 1 == FinalBlockIndex);

            for (int i = FinalBlockIndex; i < FinalBlockIndex + lastBlockIndex - firstBlockIndex; i++)
            {
                IWriteableBlockState BlockState = inner.BlockStateList[i + firstBlockIndex - FinalBlockIndex];
                Debug.Assert(BlockState.StateList.Count >= 1);

                // Remove at FinalBlockIndex since subsequent blocks are moved as the block at FinalBlockIndex is deleted.
                // Same for nodes inside blokcks, delete them at 0.
                for (int j = 1; j < BlockState.StateList.Count; j++)
                {
                    IWriteableRemoveNodeOperation OperationNode = CreateRemoveNodeOperation(inner.Owner.Node, inner.PropertyName, FinalBlockIndex, 0, HandlerRedoRemoveNode, HandlerUndoRemoveNode, isNested: true);
                    OperationList.Add(OperationNode);
                }

                IWriteableRemoveBlockOperation OperationBlock = CreateRemoveBlockOperation(inner.Owner.Node, inner.PropertyName, FinalBlockIndex, HandlerRedoRemoveBlock, HandlerUndoRemoveBlock, isNested: true);
                OperationList.Add(OperationBlock);
            }

            if (OperationList.Count > 0)
            {
                IWriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRefresh(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
                IWriteableGenericRefreshOperation RefreshOperation = CreateGenericRefreshOperation(RootState, HandlerRedo, HandlerUndo, isNested: false);
                IWriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, RefreshOperation);

                OperationGroup.Redo();
                SetLastOperation(OperationGroup);
                CheckInvariant();
            }
        }

        /// <summary>
        /// Checks whether a range of nodes can be removed from a list or block list.
        /// </summary>
        /// <param name="inner">The inner with nodes to remove.</param>
        /// <param name="blockIndex">Index of the block where to remove nodes, for a block list. -1 for a list.</param>
        /// <param name="firstNodeIndex">Index of the first node to remove.</param>
        /// <param name="lastNodeIndex">Index following the last node to remove.</param>
        public virtual bool IsNodeRangeRemoveable(IWriteableCollectionInner inner, int blockIndex, int firstNodeIndex, int lastNodeIndex)
        {
            Debug.Assert(inner != null);

            bool IsHandled = false;
            bool Result = false;

            if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                Result = IsNodeRangeRemoveable(AsBlockListInner, blockIndex, firstNodeIndex, lastNodeIndex);
                IsHandled = true;
            }

            else if (inner is IWriteableListInner AsListInner)
            {
                Debug.Assert(blockIndex == -1);
                Result = IsNodeRangeRemoveable(AsListInner, firstNodeIndex, lastNodeIndex);
                IsHandled = true;
            }

            Debug.Assert(IsHandled);

            if (!Result)
            {
                Debug.Assert(inner.Owner != null);

                INode Node = inner.Owner.Node;
                string PropertyName = inner.PropertyName;
                Debug.Assert(Node != null);

                Result = true;

                Type InterfaceType = NodeTreeHelper.NodeTypeToInterfaceType(inner.Owner.Node.GetType());
                IReadOnlyDictionary<Type, string[]> NeverEmptyCollectionTable = NodeHelper.NeverEmptyCollectionTable;
                if (NeverEmptyCollectionTable.ContainsKey(InterfaceType))
                {
                    foreach (string Item in NeverEmptyCollectionTable[InterfaceType])
                        if (Item == PropertyName)
                            Result = false;
                }
            }

            return Result;
        }

        /// <summary></summary>
        private protected virtual bool IsNodeRangeRemoveable(IWriteableBlockListInner inner, int blockIndex, int firstNodeIndex, int lastNodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(blockIndex >= 0 && blockIndex < inner.BlockStateList.Count);

            IWriteableBlockState BlockState = inner.BlockStateList[blockIndex];
            Debug.Assert(firstNodeIndex >= 0 && firstNodeIndex < BlockState.StateList.Count);
            Debug.Assert(lastNodeIndex >= 0 && lastNodeIndex <= BlockState.StateList.Count);
            Debug.Assert(firstNodeIndex <= lastNodeIndex);

            int DeletedCount = lastNodeIndex - firstNodeIndex;
            return inner.BlockStateList.Count > 1 || BlockState.StateList.Count > DeletedCount;
        }

        /// <summary></summary>
        private protected virtual bool IsNodeRangeRemoveable(IWriteableListInner inner, int firstNodeIndex, int lastNodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(firstNodeIndex >= 0 && firstNodeIndex < inner.StateList.Count);
            Debug.Assert(lastNodeIndex >= 0 && lastNodeIndex <= inner.StateList.Count);
            Debug.Assert(firstNodeIndex <= lastNodeIndex);

            int DeletedCount = lastNodeIndex - firstNodeIndex;
            return inner.StateList.Count > DeletedCount;
        }

        /// <summary>
        /// Removes a range of nodes from a list or block list.
        /// </summary>
        /// <param name="inner">The inner with nodes to remove.</param>
        /// <param name="blockIndex">Index of the block where to remove nodes, for a block list. -1 for a list.</param>
        /// <param name="firstNodeIndex">Index of the first node to remove.</param>
        /// <param name="lastNodeIndex">Index following the last node to remove.</param>
        public virtual void RemoveNodeRange(IWriteableCollectionInner inner, int blockIndex, int firstNodeIndex, int lastNodeIndex)
        {
            Debug.Assert(inner != null);

            bool IsHandled = false;

            if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                RemoveNodeRange(AsBlockListInner, blockIndex, firstNodeIndex, lastNodeIndex);
                IsHandled = true;
            }

            else if (inner is IWriteableListInner AsListInner)
            {
                Debug.Assert(blockIndex == -1);
                RemoveNodeRange(AsListInner, firstNodeIndex, lastNodeIndex);
                IsHandled = true;
            }

            Debug.Assert(IsHandled);
        }

        /// <summary></summary>
        public virtual void RemoveNodeRange(IWriteableBlockListInner inner, int blockIndex, int firstNodeIndex, int lastNodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(blockIndex >= 0 && blockIndex < inner.BlockStateList.Count);

            IWriteableBlockState BlockState = inner.BlockStateList[blockIndex];
            Debug.Assert(firstNodeIndex >= 0 && firstNodeIndex < BlockState.StateList.Count);
            Debug.Assert(lastNodeIndex >= 0 && lastNodeIndex <= BlockState.StateList.Count);
            Debug.Assert(firstNodeIndex <= lastNodeIndex);

            int DeletedCount = lastNodeIndex - firstNodeIndex;

            Action<IWriteableOperation> HandlerRedoRemoveNode = (IWriteableOperation operation) => RedoRemoveNode(operation);
            Action<IWriteableOperation> HandlerUndoRemoveNode = (IWriteableOperation operation) => UndoRemoveNode(operation);
            Action<IWriteableOperation> HandlerRedoRemoveBlock = (IWriteableOperation operation) => RedoRemoveBlock(operation);
            Action<IWriteableOperation> HandlerUndoRemoveBlock = (IWriteableOperation operation) => UndoRemoveBlock(operation);

            IWriteableOperationList OperationList = CreateOperationList();

            for (int i = firstNodeIndex; i < lastNodeIndex; i++)
            {
                IWriteableRemoveOperation Operation;

                if (DeletedCount < BlockState.StateList.Count || i + 1 < lastNodeIndex)
                    Operation = CreateRemoveNodeOperation(inner.Owner.Node, inner.PropertyName, blockIndex, firstNodeIndex, HandlerRedoRemoveNode, HandlerUndoRemoveNode, isNested: true);
                else
                    Operation = CreateRemoveBlockOperation(inner.Owner.Node, inner.PropertyName, blockIndex, HandlerRedoRemoveBlock, HandlerUndoRemoveBlock, isNested: true);

                OperationList.Add(Operation);
            }

            if (OperationList.Count > 0)
            {
                IWriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRefresh(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
                IWriteableGenericRefreshOperation RefreshOperation = CreateGenericRefreshOperation(RootState, HandlerRedo, HandlerUndo, isNested: false);
                IWriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, RefreshOperation);

                OperationGroup.Redo();
                SetLastOperation(OperationGroup);
                CheckInvariant();
            }
        }

        /// <summary></summary>
        public virtual void RemoveNodeRange(IWriteableListInner inner, int firstNodeIndex, int lastNodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(firstNodeIndex >= 0 && firstNodeIndex < inner.StateList.Count);
            Debug.Assert(lastNodeIndex >= 0 && lastNodeIndex <= inner.StateList.Count);
            Debug.Assert(firstNodeIndex <= lastNodeIndex);

            Action<IWriteableOperation> HandlerRedoNode = (IWriteableOperation operation) => RedoRemoveNode(operation);
            Action<IWriteableOperation> HandlerUndoNode = (IWriteableOperation operation) => UndoRemoveNode(operation);

            IWriteableOperationList OperationList = CreateOperationList();

            for (int i = firstNodeIndex; i < lastNodeIndex; i++)
            {
                IWriteableRemoveNodeOperation OperationNode = CreateRemoveNodeOperation(inner.Owner.Node, inner.PropertyName, -1, firstNodeIndex, HandlerRedoNode, HandlerUndoNode, isNested: true);
                OperationList.Add(OperationNode);
            }

            if (OperationList.Count > 0)
            {
                IWriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRefresh(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
                IWriteableGenericRefreshOperation RefreshOperation = CreateGenericRefreshOperation(RootState, HandlerRedo, HandlerUndo, isNested: false);
                IWriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, RefreshOperation);

                OperationGroup.Redo();
                SetLastOperation(OperationGroup);
                CheckInvariant();
            }
        }

        /// <summary>
        /// Removes a range of nodes from a list or block list and replace them with other nodes.
        /// </summary>
        /// <param name="inner">The inner for the list or block list from which nodes are replaced.</param>
        /// <param name="blockIndex">Index of the block where to remove nodes, for a block list. -1 for a list.</param>
        /// <param name="firstNodeIndex">Index of the first node to remove.</param>
        /// <param name="lastNodeIndex">Index following the last node to remove.</param>
        /// <param name="indexList">List of nodes to insert.</param>
        public virtual void ReplaceNodeRange(IWriteableCollectionInner inner, int blockIndex, int firstNodeIndex, int lastNodeIndex, IList<IWriteableInsertionCollectionNodeIndex> indexList)
        {
            Debug.Assert(inner != null);

            bool IsHandled = false;

            if (inner is IWriteableBlockListInner AsBlockListInner)
            {
                ReplaceNodeRange(AsBlockListInner, blockIndex, firstNodeIndex, lastNodeIndex, indexList);
                IsHandled = true;
            }

            else if (inner is IWriteableListInner AsListInner)
            {
                Debug.Assert(blockIndex == -1);
                ReplaceNodeRange(AsListInner, firstNodeIndex, lastNodeIndex, indexList);
                IsHandled = true;
            }

            Debug.Assert(IsHandled);
        }

        /// <summary></summary>
        public virtual void ReplaceNodeRange(IWriteableBlockListInner inner, int blockIndex, int firstNodeIndex, int lastNodeIndex, IList<IWriteableInsertionCollectionNodeIndex> indexList)
        {
            Debug.Assert(inner != null);
            Debug.Assert(blockIndex >= 0 && blockIndex < inner.BlockStateList.Count);

            IWriteableBlockState BlockState = inner.BlockStateList[blockIndex];
            Debug.Assert(firstNodeIndex >= 0 && firstNodeIndex < BlockState.StateList.Count);
            Debug.Assert(lastNodeIndex >= 0 && lastNodeIndex <= BlockState.StateList.Count);
            Debug.Assert(firstNodeIndex <= lastNodeIndex);

            int BlockNodeIndex = firstNodeIndex;

            foreach (IWriteableInsertionCollectionNodeIndex NodeIndex in indexList)
            {
                bool IsHandled = false;

                if (NodeIndex is IWriteableInsertionExistingBlockNodeIndex AsExistingBlockNodeIndex)
                {
                    Debug.Assert(AsExistingBlockNodeIndex.BlockIndex == blockIndex);
                    Debug.Assert(AsExistingBlockNodeIndex.Index == BlockNodeIndex);

                    BlockNodeIndex++;
                    IsHandled = true;
                }

                Debug.Assert(IsHandled);
            }

            int FinalNodeIndex = BlockNodeIndex;
            int DeletedCount = lastNodeIndex - firstNodeIndex;

            Action<IWriteableOperation> HandlerRedoInsertNode = (IWriteableOperation operation) => RedoInsertNewNode(operation);
            Action<IWriteableOperation> HandlerUndoInsertNode = (IWriteableOperation operation) => UndoInsertNewNode(operation);
            Action<IWriteableOperation> HandlerRedoRemoveNode = (IWriteableOperation operation) => RedoRemoveNode(operation);
            Action<IWriteableOperation> HandlerUndoRemoveNode = (IWriteableOperation operation) => UndoRemoveNode(operation);
            Action<IWriteableOperation> HandlerRedoRemoveBlock = (IWriteableOperation operation) => RedoRemoveBlock(operation);
            Action<IWriteableOperation> HandlerUndoRemoveBlock = (IWriteableOperation operation) => UndoRemoveBlock(operation);

            IWriteableOperationList OperationList = CreateOperationList();

            // Insert first to prevent empty block lists.
            foreach (IWriteableInsertionCollectionNodeIndex NodeIndex in indexList)
                if (NodeIndex is IWriteableInsertionExistingBlockNodeIndex AsExistingBlockNodeIndex)
                {
                    IndexToPositionAndNode(AsExistingBlockNodeIndex, out blockIndex, out int Index, out INode Node);
                    IWriteableInsertNodeOperation OperationInsertNode = CreateInsertNodeOperation(inner.Owner.Node, inner.PropertyName, blockIndex, Index, Node, HandlerRedoInsertNode, HandlerUndoInsertNode, isNested: true);
                    OperationList.Add(OperationInsertNode);
                }

            for (int i = FinalNodeIndex; i < FinalNodeIndex + lastNodeIndex - firstNodeIndex; i++)
            {
                IWriteableRemoveOperation Operation;

                if (DeletedCount < BlockState.StateList.Count || indexList.Count > 0 || i + 1 < FinalNodeIndex + lastNodeIndex - firstNodeIndex)
                    Operation = CreateRemoveNodeOperation(inner.Owner.Node, inner.PropertyName, blockIndex, FinalNodeIndex, HandlerRedoRemoveNode, HandlerUndoRemoveNode, isNested: true);
                else
                    Operation = CreateRemoveBlockOperation(inner.Owner.Node, inner.PropertyName, blockIndex, HandlerRedoRemoveBlock, HandlerUndoRemoveBlock, isNested: true);

                OperationList.Add(Operation);
            }

            if (OperationList.Count > 0)
            {
                IWriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRefresh(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
                IWriteableGenericRefreshOperation RefreshOperation = CreateGenericRefreshOperation(RootState, HandlerRedo, HandlerUndo, isNested: false);
                IWriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, RefreshOperation);

                OperationGroup.Redo();
                SetLastOperation(OperationGroup);
                CheckInvariant();
            }
        }

        /// <summary></summary>
        public virtual void ReplaceNodeRange(IWriteableListInner inner, int firstNodeIndex, int lastNodeIndex, IList<IWriteableInsertionCollectionNodeIndex> indexList)
        {
            Debug.Assert(inner != null);
            Debug.Assert(firstNodeIndex >= 0 && firstNodeIndex < inner.StateList.Count);
            Debug.Assert(lastNodeIndex >= 0 && lastNodeIndex <= inner.StateList.Count);
            Debug.Assert(firstNodeIndex <= lastNodeIndex);

            int BlockNodeIndex = firstNodeIndex;

            foreach (IWriteableInsertionCollectionNodeIndex NodeIndex in indexList)
            {
                bool IsHandled = false;

                if (NodeIndex is IWriteableInsertionListNodeIndex AsListNodeIndex)
                {
                    Debug.Assert(AsListNodeIndex.Index == BlockNodeIndex);

                    BlockNodeIndex++;
                    IsHandled = true;
                }

                Debug.Assert(IsHandled);
            }

            int FinalNodeIndex = BlockNodeIndex;

            Action<IWriteableOperation> HandlerRedoInsertNode = (IWriteableOperation operation) => RedoInsertNewNode(operation);
            Action<IWriteableOperation> HandlerUndoInsertNode = (IWriteableOperation operation) => UndoInsertNewNode(operation);
            Action<IWriteableOperation> HandlerRedoRemoveNode = (IWriteableOperation operation) => RedoRemoveNode(operation);
            Action<IWriteableOperation> HandlerUndoRemoveNode = (IWriteableOperation operation) => UndoRemoveNode(operation);

            IWriteableOperationList OperationList = CreateOperationList();

            // Insert first to prevent empty block lists.
            foreach (IWriteableInsertionCollectionNodeIndex NodeIndex in indexList)
                if (NodeIndex is IWriteableInsertionListNodeIndex AsListNodeIndex)
                {
                    IndexToPositionAndNode(AsListNodeIndex, out int BlockIndex, out int Index, out INode Node);
                    IWriteableInsertNodeOperation OperationInsertNode = CreateInsertNodeOperation(inner.Owner.Node, inner.PropertyName, BlockIndex, Index, Node, HandlerRedoInsertNode, HandlerUndoInsertNode, isNested: true);
                    OperationList.Add(OperationInsertNode);
                }

            for (int i = FinalNodeIndex; i < FinalNodeIndex + lastNodeIndex - firstNodeIndex; i++)
            {
                IWriteableRemoveNodeOperation OperationNode = CreateRemoveNodeOperation(inner.Owner.Node, inner.PropertyName, -1, FinalNodeIndex, HandlerRedoRemoveNode, HandlerUndoRemoveNode, isNested: true);
                OperationList.Add(OperationNode);
            }

            if (OperationList.Count > 0)
            {
                IWriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRefresh(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
                IWriteableGenericRefreshOperation RefreshOperation = CreateGenericRefreshOperation(RootState, HandlerRedo, HandlerUndo, isNested: false);
                IWriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, RefreshOperation);

                OperationGroup.Redo();
                SetLastOperation(OperationGroup);
                CheckInvariant();
            }
        }

        /// <summary>
        /// Assign the optional node.
        /// </summary>
        /// <param name="nodeIndex">Index of the optional node.</param>
        /// <param name="isChanged">True upon return if the node was changed. False if the node was already assigned.</param>
        public virtual void Assign(IWriteableBrowsingOptionalNodeIndex nodeIndex, out bool isChanged)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(StateTable.ContainsKey(nodeIndex));

            IWriteableOptionalNodeState State = StateTable[nodeIndex] as IWriteableOptionalNodeState;
            Debug.Assert(State != null);

            IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> Inner = State.ParentInner as IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex>;
            Debug.Assert(Inner != null);

            if (!Inner.IsAssigned)
            {
                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoAssign(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoAssign(operation);
                IWriteableAssignmentOperation Operation = CreateAssignmentOperation(Inner.Owner.Node, Inner.PropertyName, HandlerRedo, HandlerUndo, isNested: false);

                Operation.Redo();
                SetLastOperation(Operation);
                CheckInvariant();

                isChanged = true;
            }
            else
                isChanged = false;
        }

        /// <summary></summary>
        private protected virtual void RedoAssign(IWriteableOperation operation)
        {
            IWriteableAssignmentOperation AssignmentOperation = (IWriteableAssignmentOperation)operation;
            ExecuteAssign(AssignmentOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteAssign(IWriteableAssignmentOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex>;

            Inner.Assign(operation);

            Stats.AssignedOptionalNodeCount++;

            NotifyStateAssigned(operation);
        }

        /// <summary></summary>
        private protected virtual void UndoAssign(IWriteableOperation operation)
        {
            IWriteableAssignmentOperation AssignmentOperation = (IWriteableAssignmentOperation)operation;
            AssignmentOperation = AssignmentOperation.ToInverseAssignment();

            ExecuteUnassign(AssignmentOperation);
        }

        /// <summary>
        /// Unassign the optional node.
        /// </summary>
        /// <param name="nodeIndex">Index of the optional node.</param>
        /// <param name="isChanged">True upon return if the node was changed. False if the node was already not assigned.</param>
        public virtual void Unassign(IWriteableBrowsingOptionalNodeIndex nodeIndex, out bool isChanged)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(StateTable.ContainsKey(nodeIndex));

            IWriteableOptionalNodeState State = StateTable[nodeIndex] as IWriteableOptionalNodeState;
            Debug.Assert(State != null);

            IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> Inner = State.ParentInner as IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex>;
            Debug.Assert(Inner != null);

            if (Inner.IsAssigned)
            {
                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoUnassign(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoUnassign(operation);
                IWriteableAssignmentOperation Operation = CreateAssignmentOperation(Inner.Owner.Node, Inner.PropertyName, HandlerRedo, HandlerUndo, isNested: false);

                Operation.Redo();
                SetLastOperation(Operation);
                CheckInvariant();

                isChanged = true;
            }
            else
                isChanged = false;
        }

        /// <summary></summary>
        private protected virtual void RedoUnassign(IWriteableOperation operation)
        {
            IWriteableAssignmentOperation AssignmentOperation = (IWriteableAssignmentOperation)operation;
            ExecuteUnassign(AssignmentOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteUnassign(IWriteableAssignmentOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex>;

            Inner.Unassign(operation);

            Stats.AssignedOptionalNodeCount--;

            NotifyStateUnassigned(operation);
        }

        /// <summary></summary>
        private protected virtual void UndoUnassign(IWriteableOperation operation)
        {
            IWriteableAssignmentOperation AssignmentOperation = (IWriteableAssignmentOperation)operation;
            AssignmentOperation = AssignmentOperation.ToInverseAssignment();

            ExecuteAssign(AssignmentOperation);
        }

        /// <summary>
        /// Changes the replication state of a block.
        /// </summary>
        /// <param name="inner">The inner where the blok is changed.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="replication">New replication value.</param>
        public virtual void ChangeReplication(IWriteableBlockListInner inner, int blockIndex, ReplicationStatus replication)
        {
            Debug.Assert(inner != null);
            Debug.Assert(blockIndex >= 0 && blockIndex < inner.BlockStateList.Count);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoChangeReplication(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoChangeReplication(operation);
            IWriteableChangeBlockOperation Operation = CreateChangeBlockOperation(inner.Owner.Node, inner.PropertyName, blockIndex, replication, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        /// <summary></summary>
        private protected virtual void RedoChangeReplication(IWriteableOperation operation)
        {
            IWriteableChangeBlockOperation ChangeBlockOperation = (IWriteableChangeBlockOperation)operation;
            ExecuteChangeReplication(ChangeBlockOperation);
        }

        /// <summary></summary>
        private protected virtual void UndoChangeReplication(IWriteableOperation operation)
        {
            IWriteableChangeBlockOperation ChangeBlockOperation = (IWriteableChangeBlockOperation)operation;
            ChangeBlockOperation = ChangeBlockOperation.ToInverseChange();

            ExecuteChangeReplication(ChangeBlockOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteChangeReplication(IWriteableChangeBlockOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>;

            Inner.ChangeReplication(operation);

            NotifyBlockStateChanged(operation);
        }

        /// <summary>
        /// Changes the value of an enum or boolean.
        /// If the value exceeds allowed values, it is rounded to fit.
        /// </summary>
        /// <param name="nodeIndex">Index of the state with the enum to change.</param>
        /// <param name="propertyName">Name of the property to change.</param>
        /// <param name="value">The new value.</param>
        public virtual void ChangeDiscreteValue(IWriteableIndex nodeIndex, string propertyName, int value)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(StateTable.ContainsKey(nodeIndex));
            Debug.Assert(value >= 0);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoChangeDiscreteValue(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoChangeDiscreteValue(operation);
            IWriteableNodeState State = StateTable[nodeIndex];
            IWriteableChangeDiscreteValueOperation Operation = CreateChangeDiscreteValueOperation(State.Node, propertyName, value, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        /// <summary></summary>
        private protected virtual void RedoChangeDiscreteValue(IWriteableOperation operation)
        {
            IWriteableChangeDiscreteValueOperation ChangeDiscreteValueOperation = (IWriteableChangeDiscreteValueOperation)operation;
            ExecuteChangeDiscreteValue(ChangeDiscreteValueOperation);
        }

        /// <summary></summary>
        private protected virtual void UndoChangeDiscreteValue(IWriteableOperation operation)
        {
            IWriteableChangeDiscreteValueOperation ChangeDiscreteValueOperation = (IWriteableChangeDiscreteValueOperation)operation;
            ChangeDiscreteValueOperation = ChangeDiscreteValueOperation.ToInverseChange();

            ExecuteChangeDiscreteValue(ChangeDiscreteValueOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteChangeDiscreteValue(IWriteableChangeDiscreteValueOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            int NewValue = operation.NewValue;

            IWriteableNodeState State = (IWriteableNodeState)GetState(ParentNode);
            Debug.Assert(State != null);
            Debug.Assert(State.ValuePropertyTypeTable.ContainsKey(PropertyName));
            Debug.Assert(State.ValuePropertyTypeTable[PropertyName] == Constants.ValuePropertyType.Boolean || State.ValuePropertyTypeTable[PropertyName] == Constants.ValuePropertyType.Enum);

            int OldValue = NodeTreeHelper.GetEnumValue(State.Node, PropertyName);

            NodeTreeHelper.GetEnumRange(State.Node.GetType(), PropertyName, out int Min, out int Max);

            Debug.Assert(NewValue >= Min && NewValue <= Max);

            NodeTreeHelper.SetEnumValue(State.Node, PropertyName, NewValue);

            operation.Update(State, OldValue);

            NotifyDiscreteValueChanged(operation);
        }

        /// <summary>
        /// Changes the value of a text.
        /// </summary>
        /// <param name="nodeIndex">Index of the state with the string to change.</param>
        /// <param name="propertyName">Name of the property to change.</param>
        /// <param name="text">The new text.</param>
        public virtual void ChangeText(IWriteableIndex nodeIndex, string propertyName, string text)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(StateTable.ContainsKey(nodeIndex));
            Debug.Assert(text != null);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoChangeText(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoChangeText(operation);
            IWriteableNodeState State = StateTable[nodeIndex];
            IWriteableChangeTextOperation Operation = CreateChangeTextOperation(State.Node, propertyName, text, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        /// <summary></summary>
        private protected virtual void RedoChangeText(IWriteableOperation operation)
        {
            IWriteableChangeTextOperation ChangeTextOperation = (IWriteableChangeTextOperation)operation;
            ExecuteChangeText(ChangeTextOperation);
        }

        /// <summary></summary>
        private protected virtual void UndoChangeText(IWriteableOperation operation)
        {
            IWriteableChangeTextOperation ChangeTextOperation = (IWriteableChangeTextOperation)operation;
            ChangeTextOperation = ChangeTextOperation.ToInverseChange();

            ExecuteChangeText(ChangeTextOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteChangeText(IWriteableChangeTextOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            string NewText = operation.NewText;

            IWriteableNodeState State = (IWriteableNodeState)GetState(ParentNode);
            Debug.Assert(State != null);
            Debug.Assert(State.ValuePropertyTypeTable.ContainsKey(PropertyName));
            Debug.Assert(State.ValuePropertyTypeTable[PropertyName] == Constants.ValuePropertyType.String);

            string OldText = NodeTreeHelper.GetString(State.Node, PropertyName);
            Debug.Assert(OldText != null);

            NodeTreeHelper.SetString(State.Node, PropertyName, NewText);

            operation.Update(State, OldText);

            NotifyTextChanged(operation);
        }

        /// <summary>
        /// Changes the value of a text.
        /// </summary>
        /// <param name="nodeIndex">Index of the state with the comment to change.</param>
        /// <param name="text">The new comment.</param>
        public virtual void ChangeComment(IWriteableIndex nodeIndex, string text)
        {
            Debug.Assert(nodeIndex != null);
            Debug.Assert(StateTable.ContainsKey(nodeIndex));
            Debug.Assert(text != null);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoChangeComment(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoChangeComment(operation);
            IWriteableNodeState State = StateTable[nodeIndex];
            IWriteableChangeCommentOperation Operation = CreateChangeCommentOperation(State.Node, text, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        /// <summary></summary>
        private protected virtual void RedoChangeComment(IWriteableOperation operation)
        {
            IWriteableChangeCommentOperation ChangeCommentOperation = (IWriteableChangeCommentOperation)operation;
            ExecuteChangeComment(ChangeCommentOperation);
        }

        /// <summary></summary>
        private protected virtual void UndoChangeComment(IWriteableOperation operation)
        {
            IWriteableChangeCommentOperation ChangeCommentOperation = (IWriteableChangeCommentOperation)operation;
            ChangeCommentOperation = ChangeCommentOperation.ToInverseChange();

            ExecuteChangeComment(ChangeCommentOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteChangeComment(IWriteableChangeCommentOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string NewText = operation.NewText;

            IWriteableNodeState State = (IWriteableNodeState)GetState(ParentNode);
            Debug.Assert(State != null);

            string OldText = NodeTreeHelper.GetCommentText(State.Node);
            Debug.Assert(OldText != null);

            NodeTreeHelper.SetCommentText(State.Node, NewText);

            operation.Update(State, OldText);

            NotifyCommentChanged(operation);
        }

        /// <summary>
        /// Checks whether a block can be split at the given index.
        /// </summary>
        /// <param name="inner">The inner where the block would be split.</param>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        public virtual bool IsSplittable(IWriteableBlockListInner inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            return inner.IsSplittable(nodeIndex);
        }

        /// <summary>
        /// Splits a block in two at the given index.
        /// </summary>
        /// <param name="inner">The inner where the block is split.</param>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        public virtual void SplitBlock(IWriteableBlockListInner inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);
            Debug.Assert(inner.IsSplittable(nodeIndex));

            IWriteableBlockState BlockState = inner.BlockStateList[nodeIndex.BlockIndex];
            ReplicationStatus Replication = BlockState.ChildBlock.Replication;
            IPattern NewPatternNode = NodeHelper.CreateSimplePattern(BlockState.ChildBlock.ReplicationPattern.Text);
            IIdentifier NewSourceNode = NodeHelper.CreateSimpleIdentifier(BlockState.ChildBlock.SourceIdentifier.Text);
            IBlock NewBlock = NodeTreeHelperBlockList.CreateBlock(inner.Owner.Node, inner.PropertyName, Replication, NewPatternNode, NewSourceNode);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoSplitBlock(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoSplitBlock(operation);
            IWriteableSplitBlockOperation Operation = CreateSplitBlockOperation(inner.Owner.Node, inner.PropertyName, nodeIndex.BlockIndex, nodeIndex.Index, NewBlock, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        /// <summary></summary>
        private protected virtual void RedoSplitBlock(IWriteableOperation operation)
        {
            IWriteableSplitBlockOperation SplitBlockOperation = (IWriteableSplitBlockOperation)operation;
            ExecuteSplitBlock(SplitBlockOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteSplitBlock(IWriteableSplitBlockOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>;

            IWriteableBlockState OldBlockState = Inner.BlockStateList[operation.BlockIndex];
            Debug.Assert(operation.Index < OldBlockState.StateList.Count);

            int OldNodeCount = OldBlockState.StateList.Count;

            Inner.SplitBlock(operation);
            Stats.BlockCount++;

            IWriteableBlockState NewBlockState = operation.BlockState;

            Debug.Assert(OldBlockState.StateList.Count + NewBlockState.StateList.Count == OldNodeCount);
            Debug.Assert(NewBlockState.StateList.Count > 0);

            IReadOnlyBrowsingPatternIndex PatternIndex = NewBlockState.PatternIndex;
            IReadOnlyPatternState PatternState = NewBlockState.PatternState;
            AddState(PatternIndex, PatternState);
            Stats.PlaceholderNodeCount++;

            IReadOnlyBrowsingSourceIndex SourceIndex = NewBlockState.SourceIndex;
            IReadOnlySourceState SourceState = NewBlockState.SourceState;
            AddState(SourceIndex, SourceState);
            Stats.PlaceholderNodeCount++;

            NotifyBlockSplit(operation);
        }

        /// <summary></summary>
        private protected virtual void UndoSplitBlock(IWriteableOperation operation)
        {
            IWriteableSplitBlockOperation SplitBlockOperation = (IWriteableSplitBlockOperation)operation;
            IWriteableMergeBlocksOperation MergeBlocksOperation = SplitBlockOperation.ToMergeBlocksOperation();

            ExecuteMergeBlocks(MergeBlocksOperation);
        }

        /// <summary>
        /// Checks whether a block can be merged at the given index.
        /// </summary>
        /// <param name="inner">The inner where the block would be split.</param>
        /// <param name="nodeIndex">Index of the first node in the block to merge.</param>
        public virtual bool IsMergeable(IWriteableBlockListInner inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            return inner.IsMergeable(nodeIndex);
        }

        /// <summary>
        /// Merges two blocks at the given index.
        /// </summary>
        /// <param name="inner">The inner where blocks are merged.</param>
        /// <param name="nodeIndex">Index of the first node in the block to merge.</param>
        public virtual void MergeBlocks(IWriteableBlockListInner inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);
            Debug.Assert(inner.IsMergeable(nodeIndex));

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoMergeBlocks(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoMergeBlocks(operation);
            IWriteableMergeBlocksOperation Operation = CreateMergeBlocksOperation(inner.Owner.Node, inner.PropertyName, nodeIndex.BlockIndex, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        /// <summary></summary>
        private protected virtual void RedoMergeBlocks(IWriteableOperation operation)
        {
            IWriteableMergeBlocksOperation MergeBlocksOperation = (IWriteableMergeBlocksOperation)operation;
            ExecuteMergeBlocks(MergeBlocksOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteMergeBlocks(IWriteableMergeBlocksOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>;

            int BlockIndex = operation.BlockIndex;
            IWriteableBlockState FirstBlockState = Inner.BlockStateList[BlockIndex - 1];
            IWriteableBlockState SecondBlockState = Inner.BlockStateList[BlockIndex];

            IReadOnlyBrowsingSourceIndex SourceIndex = FirstBlockState.SourceIndex;
            RemoveState(SourceIndex);
            Stats.PlaceholderNodeCount--;

            IReadOnlyBrowsingPatternIndex PatternIndex = FirstBlockState.PatternIndex;
            RemoveState(PatternIndex);
            Stats.PlaceholderNodeCount--;

            int OldNodeCount = FirstBlockState.StateList.Count + SecondBlockState.StateList.Count;
            int FirstNodeIndex = FirstBlockState.StateList.Count;

            Inner.MergeBlocks(operation);
            Stats.BlockCount--;

            IWriteableBlockState BlockState = Inner.BlockStateList[BlockIndex - 1];

            Debug.Assert(BlockState.StateList.Count == OldNodeCount);
            Debug.Assert(FirstNodeIndex < BlockState.StateList.Count);

            NotifyBlocksMerged(operation);
        }

        /// <summary></summary>
        private protected virtual void UndoMergeBlocks(IWriteableOperation operation)
        {
            IWriteableMergeBlocksOperation MergeBlocksOperation = (IWriteableMergeBlocksOperation)operation;
            IWriteableSplitBlockOperation SplitBlockOperation = MergeBlocksOperation.ToSplitBlockOperation();

            ExecuteSplitBlock(SplitBlockOperation);
        }

        /// <summary>
        /// Checks whether a node can be moved in a list.
        /// </summary>
        /// <param name="inner">The inner where the node is.</param>
        /// <param name="nodeIndex">Index of the node that would be moved.</param>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        public virtual bool IsMoveable(IWriteableCollectionInner inner, IWriteableBrowsingCollectionNodeIndex nodeIndex, int direction)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            IWriteableNodeState State = StateTable[nodeIndex];
            Debug.Assert(State != null);

            bool Result = inner.IsMoveable(nodeIndex, direction);

            return Result;
        }

        /// <summary>
        /// Moves a node around in a list or block list. In a block list, the node stays in same block.
        /// </summary>
        /// <param name="inner">The inner for the list or block list in which the node is moved.</param>
        /// <param name="nodeIndex">Index for the moved node.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        public virtual void Move(IWriteableCollectionInner inner, IWriteableBrowsingCollectionNodeIndex nodeIndex, int direction)
        {
            Debug.Assert(inner != null);
            Debug.Assert(nodeIndex != null);

            IWriteableNodeState State = StateTable[nodeIndex];
            Debug.Assert(State != null);

            IndexToPositionAndNode(nodeIndex, out int BlockIndex, out int Index, out INode Node);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoMove(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoMove(operation);
            IWriteableMoveNodeOperation Operation = CreateMoveNodeOperation(inner.Owner.Node, inner.PropertyName, BlockIndex, Index, direction, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        /// <summary></summary>
        private protected virtual void RedoMove(IWriteableOperation operation)
        {
            IWriteableMoveNodeOperation MoveNodeOperation = (IWriteableMoveNodeOperation)operation;
            ExecuteMove(MoveNodeOperation);
        }

        /// <summary></summary>
        private protected virtual void UndoMove(IWriteableOperation operation)
        {
            IWriteableMoveNodeOperation MoveNodeOperation = (IWriteableMoveNodeOperation)operation;
            MoveNodeOperation = MoveNodeOperation.ToInverseMove();

            ExecuteMove(MoveNodeOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteMove(IWriteableMoveNodeOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableCollectionInner<IWriteableBrowsingCollectionNodeIndex>;

            Inner.Move(operation);
            NotifyStateMoved(operation);
        }

        /// <summary>
        /// Checks whether a block can be moved in a block list.
        /// </summary>
        /// <param name="inner">The inner where the block is.</param>
        /// <param name="blockIndex">Index of the block that would be moved.</param>
        /// <param name="direction">Direction of the move, relative to the current position of the item.</param>
        public virtual bool IsBlockMoveable(IWriteableBlockListInner inner, int blockIndex, int direction)
        {
            Debug.Assert(inner != null);
            Debug.Assert(blockIndex >= 0 && blockIndex < inner.BlockStateList.Count);

            return blockIndex + direction >= 0 && blockIndex + direction < inner.BlockStateList.Count;
        }

        /// <summary>
        /// Moves a block around in a block list.
        /// </summary>
        /// <param name="inner">The inner where the block is moved.</param>
        /// <param name="blockIndex">Index of the block to move.</param>
        /// <param name="direction">The change in position, relative to the current block position.</param>
        public virtual void MoveBlock(IWriteableBlockListInner inner, int blockIndex, int direction)
        {
            Debug.Assert(inner != null);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoMoveBlock(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoMoveBlock(operation);
            IWriteableMoveBlockOperation Operation = CreateMoveBlockOperation(inner.Owner.Node, inner.PropertyName, blockIndex, direction, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        /// <summary></summary>
        private protected virtual void RedoMoveBlock(IWriteableOperation operation)
        {
            IWriteableMoveBlockOperation MoveBlockOperation = (IWriteableMoveBlockOperation)operation;
            ExecuteMoveBlock(MoveBlockOperation);
        }

        /// <summary></summary>
        private protected virtual void UndoMoveBlock(IWriteableOperation operation)
        {
            IWriteableMoveBlockOperation MoveBlockOperation = (IWriteableMoveBlockOperation)operation;
            MoveBlockOperation = MoveBlockOperation.ToInverseMoveBlock();

            ExecuteMoveBlock(MoveBlockOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteMoveBlock(IWriteableMoveBlockOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>;

            IWriteableBlockState BlockState = Inner.BlockStateList[operation.BlockIndex];
            Debug.Assert(BlockState != null);
            Debug.Assert(BlockState.StateList.Count > 0);

            IWriteableNodeState State = BlockState.StateList[0];
            Debug.Assert(State != null);

            IWriteableBrowsingExistingBlockNodeIndex NodeIndex = State.ParentIndex as IWriteableBrowsingExistingBlockNodeIndex;
            Debug.Assert(NodeIndex != null);

            Inner.MoveBlock(operation);

            NotifyBlockStateMoved(operation);
        }

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

        protected private virtual void Expand(IWriteableNodeIndex expandedIndex, IWriteableOperationList operationList)
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

        protected private virtual IWriteablePlaceholderNodeState FindBestExpandReduceState(IWriteablePlaceholderNodeState state)
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

        /// <summary></summary>
        private protected virtual void RedoExpandBlockList(IWriteableOperation operation)
        {
            IWriteableExpandArgumentOperation ExpandArgumentOperation = (IWriteableExpandArgumentOperation)operation;
            ExecuteInsertNewBlock(ExpandArgumentOperation);
        }

        /// <summary></summary>
        private protected virtual void UndoExpandBlockList(IWriteableOperation operation)
        {
            IWriteableExpandArgumentOperation ExpandArgumentOperation = (IWriteableExpandArgumentOperation)operation;
            IWriteableRemoveBlockOperation RemoveBlockOperation = ExpandArgumentOperation.ToRemoveBlockOperation();

            ExecuteRemoveBlock(RemoveBlockOperation);
        }

        /// <summary>
        /// Reduces an existing node. Opposite of <see cref="Expand"/>.
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

        /// <summary></summary>
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

        /// <summary></summary>
        private protected virtual void Canonicalize(IWriteableNodeState state, IWriteableOperationList operationList)
        {
            IWriteableNodeIndex NodeIndex = state.ParentIndex as IWriteableNodeIndex;
            Debug.Assert(NodeIndex != null);

            CanonicalizeChildren(state, operationList);

            Reduce(NodeIndex, operationList, isNested: state != RootState);
        }

        /// <summary></summary>
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

        /// <summary></summary>
        private protected virtual void RedoRefresh(IWriteableOperation operation)
        {
            IWriteableGenericRefreshOperation GenericRefreshOperation = (IWriteableGenericRefreshOperation)operation;
            ExecuteRefresh(GenericRefreshOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteRefresh(IWriteableGenericRefreshOperation operation)
        {
            NotifyGenericRefresh(operation);
        }

        /// <summary>
        /// Undo the last operation.
        /// </summary>
        public virtual void Undo()
        {
            Debug.Assert(CanUndo);

            RedoIndex--;
            IWriteableOperationGroup OperationGroup = OperationStack[RedoIndex];
            OperationGroup.Undo();

            CheckInvariant();
        }

        /// <summary>
        /// Redo the last operation undone.
        /// </summary>
        public virtual void Redo()
        {
            Debug.Assert(CanRedo);

            IWriteableOperationGroup OperationGroup = OperationStack[RedoIndex];
            OperationGroup.Redo();
            RedoIndex++;

            CheckInvariant();
        }

        /// <summary>
        /// Split an identifier with replace and insert indexes.
        /// </summary>
        /// <param name="inner">The inner where the node is replaced.</param>
        /// <param name="replaceIndex">Index for the replace operation.</param>
        /// <param name="insertIndex">Index for the insert operation.</param>
        /// <param name="firstIndex">Index of the replacing node upon return.</param>
        /// <param name="secondIndex">Index of the inserted node upon return.</param>
        public virtual void SplitIdentifier(IWriteableListInner inner, IWriteableInsertionListNodeIndex replaceIndex, IWriteableInsertionListNodeIndex insertIndex, out IWriteableBrowsingListNodeIndex firstIndex, out IWriteableBrowsingListNodeIndex secondIndex)
        {
            Debug.Assert(inner != null);
            Debug.Assert(replaceIndex != null);
            Debug.Assert(insertIndex != null);
            IWriteableNodeState Owner = inner.Owner;
            IWriteableIndex ParentIndex = Owner.ParentIndex;
            Debug.Assert(Contains(ParentIndex));
            Debug.Assert(IndexToState(ParentIndex) == Owner);
            IWriteableInnerReadOnlyDictionary<string> InnerTable = Owner.InnerTable;
            Debug.Assert(InnerTable.ContainsKey(inner.PropertyName));
            Debug.Assert(InnerTable[inner.PropertyName] == inner);

            int Index = replaceIndex.Index;
            Debug.Assert(insertIndex.Index == Index + 1);
            INode ReplacingNode = replaceIndex.Node;
            INode InsertedNode = insertIndex.Node;

            Action<IWriteableOperation> HandlerRedoReplace = (IWriteableOperation operation) => RedoReplace(operation);
            Action<IWriteableOperation> HandlerUndoReplace = (IWriteableOperation operation) => UndoReplace(operation);
            IWriteableReplaceOperation ReplaceOperation = CreateReplaceOperation(inner.Owner.Node, inner.PropertyName, -1, Index, ReplacingNode, HandlerRedoReplace, HandlerUndoReplace, isNested: true);

            Action<IWriteableOperation> HandlerRedoInsert = (IWriteableOperation operation) => RedoInsertNewNode(operation);
            Action<IWriteableOperation> HandlerUndoInsert = (IWriteableOperation operation) => UndoInsertNewNode(operation);
            IWriteableInsertNodeOperation InsertOperation = CreateInsertNodeOperation(inner.Owner.Node, inner.PropertyName, -1, Index + 1, InsertedNode, HandlerRedoInsert, HandlerUndoInsert, isNested: true);

            ReplaceOperation.Redo();
            InsertOperation.Redo();

            Action<IWriteableOperation> HandlerRedoRefresh = (IWriteableOperation operation) => RedoRefresh(operation);
            Action<IWriteableOperation> HandlerUndoRefresh = (IWriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
            IWriteableGenericRefreshOperation RefreshOperation = CreateGenericRefreshOperation(RootState, HandlerRedoRefresh, HandlerUndoRefresh, isNested: false);

            RefreshOperation.Redo();

            IWriteableOperationList OperationList = CreateOperationList();
            OperationList.Add(ReplaceOperation);
            OperationList.Add(InsertOperation);
            IWriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
            IWriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, RefreshOperation);
            SetLastOperation(OperationGroup);

            CheckInvariant();

            firstIndex = ReplaceOperation.NewBrowsingIndex as IWriteableBrowsingListNodeIndex;
            Debug.Assert(firstIndex != null);

            secondIndex = InsertOperation.BrowsingIndex as IWriteableBrowsingListNodeIndex;
            Debug.Assert(secondIndex != null);
        }

        /// <summary></summary>
        private protected virtual void IndexToPositionAndNode(IWriteableIndex nodeIndex, out int blockIndex, out int index, out INode node)
        {
            blockIndex = -1;
            index = -1;
            node = null;
            bool IsHandled = false;

            switch (nodeIndex)
            {
                case IWriteableInsertionPlaceholderNodeIndex AsPlaceholderNodeIndex:
                    node = AsPlaceholderNodeIndex.Node;
                    IsHandled = true;
                    break;

                case IWriteableInsertionOptionalNodeIndex AsOptionalNodeIndex:
                    node = AsOptionalNodeIndex.Node;
                    IsHandled = true;
                    break;

                case IWriteableInsertionOptionalClearIndex AsOptionalClearIndex:
                    IsHandled = true;
                    break;

                case IWriteableInsertionListNodeIndex AsListNodeIndex:
                    index = AsListNodeIndex.Index;
                    node = AsListNodeIndex.Node;
                    IsHandled = true;
                    break;

                case IWriteableInsertionExistingBlockNodeIndex AsExistingBlockNodeIndex:
                    blockIndex = AsExistingBlockNodeIndex.BlockIndex;
                    index = AsExistingBlockNodeIndex.Index;
                    node = AsExistingBlockNodeIndex.Node;
                    IsHandled = true;
                    break;

                case IWriteableBrowsingListNodeIndex AsListNodeIndex:
                    index = AsListNodeIndex.Index;
                    node = AsListNodeIndex.Node;
                    IsHandled = true;
                    break;

                case IWriteableBrowsingExistingBlockNodeIndex AsExistingBlockNodeIndex:
                    blockIndex = AsExistingBlockNodeIndex.BlockIndex;
                    index = AsExistingBlockNodeIndex.Index;
                    node = AsExistingBlockNodeIndex.Node;
                    IsHandled = true;
                    break;
            }

            Debug.Assert(IsHandled);
        }
        #endregion

        #region Descendant Interface
        /// <summary></summary>
        private protected override void CheckContextConsistency(IReadOnlyBrowseContext browseContext)
        {
            ((WriteableBrowseContext)browseContext).CheckConsistency();
        }

        /// <summary></summary>
        private protected virtual void PruneState(IWriteableNodeState state)
        {
            PruneStateChildren(state);
            RemoveState(state.ParentIndex);
        }

        /// <summary></summary>
        private protected virtual void PruneStateChildren(IWriteableNodeState state)
        {
            foreach (KeyValuePair<string, IWriteableInner> Entry in state.InnerTable)
            {
                bool IsHandled = false;

                if (Entry.Value is IWriteablePlaceholderInner<IWriteableBrowsingPlaceholderNodeIndex> AsPlaceholderInner)
                {
                    PrunePlaceholderInner(AsPlaceholderInner);
                    IsHandled = true;
                }
                else if (Entry.Value is IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> AsOptionalInner)
                {
                    PruneOptionalInner(AsOptionalInner);
                    IsHandled = true;
                }
                else if (Entry.Value is IWriteableListInner<IWriteableBrowsingListNodeIndex> AsListInner)
                {
                    PruneListInner(AsListInner);
                    IsHandled = true;
                }
                else if (Entry.Value is IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> AsBlockListInner)
                {
                    PruneBlockListInner(AsBlockListInner);
                    IsHandled = true;
                }

                Debug.Assert(IsHandled);
            }
        }

        /// <summary></summary>
        private protected virtual void PrunePlaceholderInner(IWriteablePlaceholderInner<IWriteableBrowsingPlaceholderNodeIndex> inner)
        {
            PruneState(inner.ChildState);

            Stats.PlaceholderNodeCount--;
        }

        /// <summary></summary>
        private protected virtual void PruneOptionalInner(IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> inner)
        {
            PruneState(inner.ChildState);

            Stats.OptionalNodeCount--;
            if (inner.IsAssigned)
                Stats.AssignedOptionalNodeCount--;
        }

        /// <summary></summary>
        private protected virtual void PruneListInner(IWriteableListInner<IWriteableBrowsingListNodeIndex> inner)
        {
            foreach (IWriteableNodeState State in inner.StateList)
            {
                PruneState(State);

                Stats.PlaceholderNodeCount--;
            }

            Stats.ListCount--;
        }

        /// <summary></summary>
        private protected virtual void PruneBlockListInner(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner)
        {
            for (int BlockIndex = inner.BlockStateList.Count; BlockIndex > 0; BlockIndex--)
            {
                Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoRemoveBlockView(operation);
                Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => throw new NotImplementedException(); // Undo is not possible.
                IWriteableRemoveBlockViewOperation Operation = CreateRemoveBlockViewOperation(inner.Owner.Node, inner.PropertyName, BlockIndex - 1, HandlerRedo, HandlerUndo, isNested: true);

                Operation.Redo();
            }

            Stats.BlockListCount--;
        }

        /// <summary></summary>
        private protected virtual void RedoRemoveBlockView(IWriteableOperation operation)
        {
            IWriteableRemoveBlockViewOperation RemoveBlockViewOperation = (IWriteableRemoveBlockViewOperation)operation;
            ExecuteRemoveBlockView(RemoveBlockViewOperation);
        }

        /// <summary></summary>
        private protected virtual void ExecuteRemoveBlockView(IWriteableRemoveBlockViewOperation operation)
        {
            INode ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>;
            IWriteableBlockState RemovedBlockState = Inner.BlockStateList[operation.BlockIndex];

            for (int Index = 0; Index < RemovedBlockState.StateList.Count; Index++)
            {
                IWriteableNodeState State = RemovedBlockState.StateList[Index];

                PruneState(State);
                Stats.PlaceholderNodeCount--;
            }

            IWriteableBrowsingPatternIndex PatternIndex = RemovedBlockState.PatternIndex;
            IWriteableBrowsingSourceIndex SourceIndex = RemovedBlockState.SourceIndex;

            Debug.Assert(PatternIndex != null);
            Debug.Assert(StateTable.ContainsKey(PatternIndex));
            Debug.Assert(SourceIndex != null);
            Debug.Assert(StateTable.ContainsKey(SourceIndex));

            RemoveState(PatternIndex);
            Stats.PlaceholderNodeCount--;

            RemoveState(SourceIndex);
            Stats.PlaceholderNodeCount--;

            Inner.NotifyBlockStateRemoved(RemovedBlockState);

            Stats.BlockCount--;

            operation.Update(RemovedBlockState);

            NotifyBlockViewRemoved(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyBlockStateInserted(IWriteableInsertBlockOperation operation)
        {
            BlockStateInsertedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyBlockStateRemoved(IWriteableRemoveBlockOperation operation)
        {
            BlockStateRemovedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyBlockViewRemoved(IWriteableRemoveBlockViewOperation operation)
        {
            BlockViewRemovedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyStateInserted(IWriteableInsertNodeOperation operation)
        {
            StateInsertedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyStateRemoved(IWriteableRemoveNodeOperation operation)
        {
            StateRemovedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyStateReplaced(IWriteableReplaceOperation operation)
        {
            StateReplacedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyStateAssigned(IWriteableAssignmentOperation operation)
        {
            StateAssignedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyStateUnassigned(IWriteableAssignmentOperation operation)
        {
            StateUnassignedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyDiscreteValueChanged(IWriteableChangeDiscreteValueOperation operation)
        {
            DiscreteValueChangedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyTextChanged(IWriteableChangeTextOperation operation)
        {
            TextChangedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyCommentChanged(IWriteableChangeCommentOperation operation)
        {
            CommentChangedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyBlockStateChanged(IWriteableChangeBlockOperation operation)
        {
            BlockStateChangedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyStateMoved(IWriteableMoveNodeOperation operation)
        {
            StateMovedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyBlockStateMoved(IWriteableMoveBlockOperation operation)
        {
            BlockStateMovedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyBlockSplit(IWriteableSplitBlockOperation operation)
        {
            BlockSplitHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyBlocksMerged(IWriteableMergeBlocksOperation operation)
        {
            BlocksMergedHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void NotifyGenericRefresh(IWriteableGenericRefreshOperation operation)
        {
            GenericRefreshHandler?.Invoke(operation);
        }

        /// <summary></summary>
        private protected virtual void SetLastOperation(IWriteableOperation operation)
        {
            IWriteableOperationList OperationList = CreateOperationList();
            OperationList.Add(operation);
            IWriteableOperationReadOnlyList OperationReadOnlyList = OperationList.ToReadOnly();
            IWriteableOperationGroup OperationGroup = CreateOperationGroup(OperationReadOnlyList, null);

            SetLastOperation(OperationGroup);
        }

        /// <summary></summary>
        private protected virtual void SetLastOperation(IWriteableOperationGroup operationGroup)
        {
            Debug.Assert(RedoIndex >= 0 && RedoIndex <= _OperationStack.Count);

            while (_OperationStack.Count > RedoIndex)
                _OperationStack.RemoveAt(RedoIndex);

            Debug.Assert(RedoIndex == _OperationStack.Count);

            _OperationStack.Add(operationGroup);
            RedoIndex = _OperationStack.Count;

            Debug.Assert(RedoIndex == _OperationStack.Count);
        }
        #endregion

        #region Invariant
        /// <summary></summary>
        private protected override void CheckInvariant()
        {
            base.CheckInvariant();

            bool IsValid = IsNodeTreeValid(RootState.Node);
            Debug.Assert(IsValid);
        }

        /// <summary></summary>
        private protected virtual bool IsNodeTreeValid(INode node)
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

        /// <summary></summary>
        private protected virtual bool IsNodeTreeChildNodeValid(INode node, string propertyName)
        {
            NodeTreeHelperChild.GetChildNode(node, propertyName, out INode ChildNode);
            Debug.Assert(ChildNode != null);

            bool IsValid = InvariantFailed(IsNodeTreeValid(ChildNode));

            return IsValid;
        }

        /// <summary></summary>
        private protected virtual bool IsNodeTreeOptionalNodeValid(INode node, string propertyName)
        {
            bool IsValid = true;

            NodeTreeHelperOptional.GetChildNode(node, propertyName, out bool IsAssigned, out INode ChildNode);
            if (IsAssigned)
            {
                IsValid &= InvariantFailed(IsNodeTreeValid(ChildNode));
            }

            return IsValid;
        }

        /// <summary></summary>
        private protected virtual bool IsNodeTreeListValid(INode node, string propertyName)
        {
            NodeTreeHelperList.GetChildNodeList(node, propertyName, out IReadOnlyList<INode> ChildNodeList);
            Debug.Assert(ChildNodeList != null);

            bool IsValid = true;

            if (ChildNodeList.Count == 0)
            {
                IsValid &= InvariantFailed(IsEmptyListValid(node, propertyName));
            }

            for (int Index = 0; Index < ChildNodeList.Count; Index++)
            {
                INode ChildNode = ChildNodeList[Index];
                Debug.Assert(ChildNode != null);

                IsValid &= InvariantFailed(IsNodeTreeValid(ChildNode));
            }

            return IsValid;
        }

        /// <summary></summary>
        private protected virtual bool IsNodeTreeBlockListValid(INode node, string propertyName)
        {
            NodeTreeHelperBlockList.GetChildBlockList(node, propertyName, out IReadOnlyList<INodeTreeBlock> ChildBlockList);
            Debug.Assert(ChildBlockList != null);

            bool IsValid = true;

            if (ChildBlockList.Count == 0)
            {
                IsValid &= InvariantFailed(IsEmptyBlockListValid(node, propertyName));
            }

            for (int BlockIndex = 0; BlockIndex < ChildBlockList.Count; BlockIndex++)
            {
                INodeTreeBlock Block = ChildBlockList[BlockIndex];
                Debug.Assert(Block.NodeList.Count > 0);

                for (int Index = 0; Index < Block.NodeList.Count; Index++)
                {
                    INode ChildNode = Block.NodeList[Index];
                    Debug.Assert(ChildNode != null);

                    IsValid &= InvariantFailed(IsNodeTreeValid(ChildNode));
                }
            }

            return IsValid;
        }

        /// <summary></summary>
        private protected virtual bool IsEmptyListValid(INode node, string propertyName)
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

        /// <summary></summary>
        private protected virtual bool IsEmptyBlockListValid(INode node, string propertyName)
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

        /// <summary></summary>
        private protected bool InvariantFailed(bool condition)
        {
            Debug.Assert(condition, "Invariant");
            return condition;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxIndexNodeStateDictionary object.
        /// </summary>
        private protected override IReadOnlyIndexNodeStateDictionary CreateStateTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableIndexNodeStateDictionary();
        }

        /// <summary>
        /// Creates a IxxxInnerDictionary{string} object.
        /// </summary>
        private protected override IReadOnlyInnerDictionary<string> CreateInnerTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableInnerDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxIndexNodeStateDictionary object.
        /// </summary>
        private protected override IReadOnlyIndexNodeStateDictionary CreateChildStateTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableIndexNodeStateDictionary();
        }

        /// <summary>
        /// Creates a IxxxxBrowseContext object.
        /// </summary>
        private protected override IReadOnlyBrowseContext CreateBrowseContext(IReadOnlyBrowseContext parentBrowseContext, IReadOnlyNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableBrowseContext((IWriteableNodeState)state);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderInner{IxxxBrowsingPlaceholderNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> CreatePlaceholderInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingPlaceholderNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteablePlaceholderInner<IWriteableBrowsingPlaceholderNodeIndex, WriteableBrowsingPlaceholderNodeIndex>((IWriteableNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxOptionalInner{IxxxBrowsingOptionalNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyOptionalInner<IReadOnlyBrowsingOptionalNodeIndex> CreateOptionalInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingOptionalNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex, WriteableBrowsingOptionalNodeIndex>((IWriteableNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxListInner{IxxxBrowsingListNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyListInner<IReadOnlyBrowsingListNodeIndex> CreateListInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingListNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableListInner<IWriteableBrowsingListNodeIndex, WriteableBrowsingListNodeIndex>((IWriteableNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxBlockListInner{IxxxBrowsingBlockNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyBlockListInner<IReadOnlyBrowsingBlockNodeIndex> CreateBlockListInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingBlockNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableBlockListInner<IWriteableBrowsingBlockNodeIndex, WriteableBrowsingBlockNodeIndex>((IWriteableNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        private protected override IReadOnlyPlaceholderNodeState CreateRootNodeState(IReadOnlyRootNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteablePlaceholderNodeState<IWriteableInner<IWriteableBrowsingChildIndex>>((IWriteableRootNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxWriteableInsertionOptionalNodeIndex object.
        /// </summary>
        private protected virtual IWriteableInsertionOptionalNodeIndex CreateNewOptionalNodeIndex(INode parentNode, string propertyName, INode node)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableInsertionOptionalNodeIndex(parentNode, propertyName, node);
        }

        /// <summary>
        /// Creates a IxxxInsertNodeOperation object.
        /// </summary>
        private protected virtual IWriteableInsertNodeOperation CreateInsertNodeOperation(INode parentNode, string propertyName, int blockIndex, int index, INode node, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableInsertNodeOperation(parentNode, propertyName, blockIndex, index, node, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxInsertBlockOperation object.
        /// </summary>
        private protected virtual IWriteableInsertBlockOperation CreateInsertBlockOperation(INode parentNode, string propertyName, int blockIndex, IBlock block, INode node, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableInsertBlockOperation(parentNode, propertyName, blockIndex, block, node, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxRemoveBlockOperation object.
        /// </summary>
        private protected virtual IWriteableRemoveBlockOperation CreateRemoveBlockOperation(INode parentNode, string propertyName, int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableRemoveBlockOperation(parentNode, propertyName, blockIndex, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxRemoveBlockViewOperation object.
        /// </summary>
        private protected virtual IWriteableRemoveBlockViewOperation CreateRemoveBlockViewOperation(INode parentNode, string propertyName, int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableRemoveBlockViewOperation(parentNode, propertyName, blockIndex, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxRemoveNodeOperation object.
        /// </summary>
        private protected virtual IWriteableRemoveNodeOperation CreateRemoveNodeOperation(INode parentNode, string propertyName, int blockIndex, int index, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableRemoveNodeOperation(parentNode, propertyName, blockIndex, index, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxReplaceOperation object.
        /// </summary>
        private protected virtual IWriteableReplaceOperation CreateReplaceOperation(INode parentNode, string propertyName, int blockIndex, int index, INode newNode, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableReplaceOperation(parentNode, propertyName, blockIndex, index, newNode, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxAssignmentOperation object.
        /// </summary>
        private protected virtual IWriteableAssignmentOperation CreateAssignmentOperation(INode parentNode, string propertyName, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableAssignmentOperation(parentNode, propertyName, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxChangeDiscreteValueOperation object.
        /// </summary>
        private protected virtual IWriteableChangeDiscreteValueOperation CreateChangeDiscreteValueOperation(INode parentNode, string propertyName, int value, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableChangeDiscreteValueOperation(parentNode, propertyName, value, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxChangeTextOperation object.
        /// </summary>
        private protected virtual IWriteableChangeTextOperation CreateChangeTextOperation(INode parentNode, string propertyName, string text, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableChangeTextOperation(parentNode, propertyName, text, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxChangeCommentOperation object.
        /// </summary>
        private protected virtual IWriteableChangeCommentOperation CreateChangeCommentOperation(INode parentNode, string text, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableChangeCommentOperation(parentNode, text, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxChangeBlockOperation object.
        /// </summary>
        private protected virtual IWriteableChangeBlockOperation CreateChangeBlockOperation(INode parentNode, string propertyName, int blockIndex, ReplicationStatus replication, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableChangeBlockOperation(parentNode, propertyName, blockIndex, replication, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxSplitBlockOperation object.
        /// </summary>
        private protected virtual IWriteableSplitBlockOperation CreateSplitBlockOperation(INode parentNode, string propertyName, int blockIndex, int index, IBlock newBlock, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableSplitBlockOperation(parentNode, propertyName, blockIndex, index, newBlock, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMergeBlocksOperation object.
        /// </summary>
        private protected virtual IWriteableMergeBlocksOperation CreateMergeBlocksOperation(INode parentNode, string propertyName, int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableMergeBlocksOperation(parentNode, propertyName, blockIndex, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMoveNodeOperation object.
        /// </summary>
        private protected virtual IWriteableMoveNodeOperation CreateMoveNodeOperation(INode parentNode, string propertyName, int blockIndex, int index, int direction, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableMoveNodeOperation(parentNode, propertyName, blockIndex, index, direction, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMoveBlockOperation object.
        /// </summary>
        private protected virtual IWriteableMoveBlockOperation CreateMoveBlockOperation(INode parentNode, string propertyName, int blockIndex, int direction, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableMoveBlockOperation(parentNode, propertyName, blockIndex, direction, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxExpandArgumentOperation object.
        /// </summary>
        private protected virtual IWriteableExpandArgumentOperation CreateExpandArgumentOperation(INode parentNode, string propertyName, IBlock block, INode node, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableExpandArgumentOperation(parentNode, propertyName, block, node, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxGenericRefreshOperation object.
        /// </summary>
        private protected virtual IWriteableGenericRefreshOperation CreateGenericRefreshOperation(IWriteableNodeState refreshState, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableGenericRefreshOperation(refreshState, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxOperationGroupList object.
        /// </summary>
        private protected virtual IWriteableOperationGroupList CreateOperationGroupStack()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableOperationGroupList();
        }

        /// <summary>
        /// Creates a IxxxOperationList object.
        /// </summary>
        private protected virtual IWriteableOperationList CreateOperationList()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableOperationList();
        }

        /// <summary>
        /// Creates a IxxxOperationGroup object.
        /// </summary>
        private protected virtual IWriteableOperationGroup CreateOperationGroup(IWriteableOperationReadOnlyList operationList, IWriteableGenericRefreshOperation refresh)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableController));
            return new WriteableOperationGroup(operationList, refresh);
        }
        #endregion
    }
}

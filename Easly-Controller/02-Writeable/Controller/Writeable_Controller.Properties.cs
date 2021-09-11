namespace EaslyController.Writeable
{
    using System;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Controller for a node tree.
    /// This controller supports operations to modify the tree.
    /// </summary>
    public partial class WriteableController : ReadOnlyController
    {
        /// <summary>
        /// Index of the root node.
        /// </summary>
        public new WriteableRootNodeIndex RootIndex { get { return (WriteableRootNodeIndex)base.RootIndex; } }

        /// <summary>
        /// State of the root node.
        /// </summary>
        public new IWriteablePlaceholderNodeState RootState { get { return (IWriteablePlaceholderNodeState)base.RootState; } }

        /// <summary>
        /// State table.
        /// </summary>
        public new WriteableNodeStateReadOnlyDictionary StateTable { get { return (WriteableNodeStateReadOnlyDictionary)base.StateTable; } }

        /// <summary>
        /// List of operations that have been performed, and can be undone or redone.
        /// </summary>
        public WriteableOperationGroupReadOnlyList OperationStack { get; }
        private WriteableOperationGroupList _OperationStack;

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
        public event Action<WriteableRemoveBlockOperation> BlockStateRemoved
        {
            add { AddBlockStateRemovedDelegate(value); }
            remove { RemoveBlockStateRemovedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<WriteableRemoveBlockOperation> BlockStateRemovedHandler;
        private protected virtual void AddBlockStateRemovedDelegate(Action<WriteableRemoveBlockOperation> handler) { BlockStateRemovedHandler += handler; }
        private protected virtual void RemoveBlockStateRemovedDelegate(Action<WriteableRemoveBlockOperation> handler) { BlockStateRemovedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a block view must be removed.
        /// </summary>
        public event Action<WriteableRemoveBlockViewOperation> BlockViewRemoved
        {
            add { AddBlockViewRemovedDelegate(value); }
            remove { RemoveBlockViewRemovedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<WriteableRemoveBlockViewOperation> BlockViewRemovedHandler;
        private protected virtual void AddBlockViewRemovedDelegate(Action<WriteableRemoveBlockViewOperation> handler) { BlockViewRemovedHandler += handler; }
        private protected virtual void RemoveBlockViewRemovedDelegate(Action<WriteableRemoveBlockViewOperation> handler) { BlockViewRemovedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a state is inserted.
        /// </summary>
        public event Action<WriteableInsertNodeOperation> StateInserted
        {
            add { AddStateInsertedDelegate(value); }
            remove { RemoveStateInsertedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<WriteableInsertNodeOperation> StateInsertedHandler;
        private protected virtual void AddStateInsertedDelegate(Action<WriteableInsertNodeOperation> handler) { StateInsertedHandler += handler; }
        private protected virtual void RemoveStateInsertedDelegate(Action<WriteableInsertNodeOperation> handler) { StateInsertedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a state is removed.
        /// </summary>
        public event Action<WriteableRemoveNodeOperation> StateRemoved
        {
            add { AddStateRemovedDelegate(value); }
            remove { RemoveStateRemovedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<WriteableRemoveNodeOperation> StateRemovedHandler;
        private protected virtual void AddStateRemovedDelegate(Action<WriteableRemoveNodeOperation> handler) { StateRemovedHandler += handler; }
        private protected virtual void RemoveStateRemovedDelegate(Action<WriteableRemoveNodeOperation> handler) { StateRemovedHandler -= handler; }
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
        public event Action<WriteableAssignmentOperation> StateAssigned
        {
            add { AddStateAssignedDelegate(value); }
            remove { RemoveStateAssignedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<WriteableAssignmentOperation> StateAssignedHandler;
        private protected virtual void AddStateAssignedDelegate(Action<WriteableAssignmentOperation> handler) { StateAssignedHandler += handler; }
        private protected virtual void RemoveStateAssignedDelegate(Action<WriteableAssignmentOperation> handler) { StateAssignedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a state is unassigned.
        /// </summary>
        public event Action<WriteableAssignmentOperation> StateUnassigned
        {
            add { AddStateUnassignedDelegate(value); }
            remove { RemoveStateUnassignedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<WriteableAssignmentOperation> StateUnassignedHandler;
        private protected virtual void AddStateUnassignedDelegate(Action<WriteableAssignmentOperation> handler) { StateUnassignedHandler += handler; }
        private protected virtual void RemoveStateUnassignedDelegate(Action<WriteableAssignmentOperation> handler) { StateUnassignedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a discrete value is changed.
        /// </summary>
        public event Action<WriteableChangeDiscreteValueOperation> DiscreteValueChanged
        {
            add { AddDiscreteValueChangedDelegate(value); }
            remove { RemoveDiscreteValueChangedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<WriteableChangeDiscreteValueOperation> DiscreteValueChangedHandler;
        private protected virtual void AddDiscreteValueChangedDelegate(Action<WriteableChangeDiscreteValueOperation> handler) { DiscreteValueChangedHandler += handler; }
        private protected virtual void RemoveDiscreteValueChangedDelegate(Action<WriteableChangeDiscreteValueOperation> handler) { DiscreteValueChangedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when text is changed.
        /// </summary>
        public event Action<WriteableChangeTextOperation> TextChanged
        {
            add { AddTextChangedDelegate(value); }
            remove { RemoveTextChangedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<WriteableChangeTextOperation> TextChangedHandler;
        private protected virtual void AddTextChangedDelegate(Action<WriteableChangeTextOperation> handler) { TextChangedHandler += handler; }
        private protected virtual void RemoveTextChangedDelegate(Action<WriteableChangeTextOperation> handler) { TextChangedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a comment is changed.
        /// </summary>
        public event Action<WriteableChangeCommentOperation> CommentChanged
        {
            add { AddCommentChangedDelegate(value); }
            remove { RemoveCommentChangedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<WriteableChangeCommentOperation> CommentChangedHandler;
        private protected virtual void AddCommentChangedDelegate(Action<WriteableChangeCommentOperation> handler) { CommentChangedHandler += handler; }
        private protected virtual void RemoveCommentChangedDelegate(Action<WriteableChangeCommentOperation> handler) { CommentChangedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a block state is changed.
        /// </summary>
        public event Action<WriteableChangeBlockOperation> BlockStateChanged
        {
            add { AddBlockStateChangedDelegate(value); }
            remove { RemoveBlockStateChangedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<WriteableChangeBlockOperation> BlockStateChangedHandler;
        private protected virtual void AddBlockStateChangedDelegate(Action<WriteableChangeBlockOperation> handler) { BlockStateChangedHandler += handler; }
        private protected virtual void RemoveBlockStateChangedDelegate(Action<WriteableChangeBlockOperation> handler) { BlockStateChangedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a state is moved.
        /// </summary>
        public event Action<WriteableMoveNodeOperation> StateMoved
        {
            add { AddStateMovedDelegate(value); }
            remove { RemoveStateMovedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<WriteableMoveNodeOperation> StateMovedHandler;
        private protected virtual void AddStateMovedDelegate(Action<WriteableMoveNodeOperation> handler) { StateMovedHandler += handler; }
        private protected virtual void RemoveStateMovedDelegate(Action<WriteableMoveNodeOperation> handler) { StateMovedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a block state is moved.
        /// </summary>
        public event Action<WriteableMoveBlockOperation> BlockStateMoved
        {
            add { AddBlockStateMovedDelegate(value); }
            remove { RemoveBlockStateMovedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<WriteableMoveBlockOperation> BlockStateMovedHandler;
        private protected virtual void AddBlockStateMovedDelegate(Action<WriteableMoveBlockOperation> handler) { BlockStateMovedHandler += handler; }
        private protected virtual void RemoveBlockStateMovedDelegate(Action<WriteableMoveBlockOperation> handler) { BlockStateMovedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when a block is split.
        /// </summary>
        public event Action<WriteableSplitBlockOperation> BlockSplit
        {
            add { AddBlockSplitDelegate(value); }
            remove { RemoveBlockSplitDelegate(value); }
        }
#pragma warning disable 1591
        private Action<WriteableSplitBlockOperation> BlockSplitHandler;
        private protected virtual void AddBlockSplitDelegate(Action<WriteableSplitBlockOperation> handler) { BlockSplitHandler += handler; }
        private protected virtual void RemoveBlockSplitDelegate(Action<WriteableSplitBlockOperation> handler) { BlockSplitHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called when two blocks are merged.
        /// </summary>
        public event Action<WriteableMergeBlocksOperation> BlocksMerged
        {
            add { AddBlocksMergedDelegate(value); }
            remove { RemoveBlocksMergedDelegate(value); }
        }
#pragma warning disable 1591
        private Action<WriteableMergeBlocksOperation> BlocksMergedHandler;
        private protected virtual void AddBlocksMergedDelegate(Action<WriteableMergeBlocksOperation> handler) { BlocksMergedHandler += handler; }
        private protected virtual void RemoveBlocksMergedDelegate(Action<WriteableMergeBlocksOperation> handler) { BlocksMergedHandler -= handler; }
#pragma warning restore 1591

        /// <summary>
        /// Called to refresh views.
        /// </summary>
        public event Action<WriteableGenericRefreshOperation> GenericRefresh
        {
            add { AddGenericRefreshDelegate(value); }
            remove { RemoveGenericRefreshDelegate(value); }
        }
#pragma warning disable 1591
        private Action<WriteableGenericRefreshOperation> GenericRefreshHandler;
        private protected virtual void AddGenericRefreshDelegate(Action<WriteableGenericRefreshOperation> handler) { GenericRefreshHandler += handler; }
        private protected virtual void RemoveGenericRefreshDelegate(Action<WriteableGenericRefreshOperation> handler) { GenericRefreshHandler -= handler; }
#pragma warning restore 1591
    }
}

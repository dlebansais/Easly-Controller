namespace EaslyController.Writeable
{
    using System;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Controller for a node tree.
    /// This controller supports operations to modify the tree.
    /// </summary>
    public partial class WriteableController : ReadOnlyController, IWriteableController
    {
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
    }
}

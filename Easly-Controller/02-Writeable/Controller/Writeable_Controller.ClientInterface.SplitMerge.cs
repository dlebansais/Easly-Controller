namespace EaslyController.Writeable
{
    using System;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using Contracts;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Controller for a node tree.
    /// This controller supports operations to modify the tree.
    /// </summary>
    public partial class WriteableController : ReadOnlyController
    {
        /// <summary>
        /// Checks whether a block can be split at the given index.
        /// </summary>
        /// <param name="inner">The inner where the block would be split.</param>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        public virtual bool IsSplittable(IWriteableBlockListInner inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex)
        {
            Contract.RequireNotNull(inner, out IWriteableBlockListInner Inner);
            Contract.RequireNotNull(nodeIndex, out IWriteableBrowsingExistingBlockNodeIndex NodeIndex);

            return Inner.IsSplittable(NodeIndex);
        }

        /// <summary>
        /// Splits a block in two at the given index.
        /// </summary>
        /// <param name="inner">The inner where the block is split.</param>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        public virtual void SplitBlock(IWriteableBlockListInner inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex)
        {
            Contract.RequireNotNull(inner, out IWriteableBlockListInner Inner);
            Contract.RequireNotNull(nodeIndex, out IWriteableBrowsingExistingBlockNodeIndex NodeIndex);
            Debug.Assert(Inner.IsSplittable(NodeIndex));

            IWriteableBlockState BlockState = (IWriteableBlockState)Inner.BlockStateList[NodeIndex.BlockIndex];
            ReplicationStatus Replication = BlockState.ChildBlock.Replication;
            Pattern NewPatternNode = NodeHelper.CreateSimplePattern(BlockState.ChildBlock.ReplicationPattern.Text);
            Identifier NewSourceNode = NodeHelper.CreateSimpleIdentifier(BlockState.ChildBlock.SourceIdentifier.Text);
            IBlock NewBlock = NodeTreeHelperBlockList.CreateBlock(Inner.Owner.Node, Inner.PropertyName, Replication, NewPatternNode, NewSourceNode);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoSplitBlock(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoSplitBlock(operation);
            WriteableSplitBlockOperation Operation = CreateSplitBlockOperation(Inner.Owner.Node, Inner.PropertyName, NodeIndex.BlockIndex, NodeIndex.Index, NewBlock, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        private protected virtual void RedoSplitBlock(IWriteableOperation operation)
        {
            WriteableSplitBlockOperation SplitBlockOperation = (WriteableSplitBlockOperation)operation;
            ExecuteSplitBlock(SplitBlockOperation);
        }

        private protected virtual void ExecuteSplitBlock(WriteableSplitBlockOperation operation)
        {
            Node ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>;

            IWriteableBlockState OldBlockState = (IWriteableBlockState)Inner.BlockStateList[operation.BlockIndex];
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

        private protected virtual void UndoSplitBlock(IWriteableOperation operation)
        {
            WriteableSplitBlockOperation SplitBlockOperation = (WriteableSplitBlockOperation)operation;
            WriteableMergeBlocksOperation MergeBlocksOperation = SplitBlockOperation.ToMergeBlocksOperation();

            ExecuteMergeBlocks(MergeBlocksOperation);
        }

        /// <summary>
        /// Checks whether a block can be merged at the given index.
        /// </summary>
        /// <param name="inner">The inner where the block would be split.</param>
        /// <param name="nodeIndex">Index of the first node in the block to merge.</param>
        public virtual bool IsMergeable(IWriteableBlockListInner inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex)
        {
            Contract.RequireNotNull(inner, out IWriteableBlockListInner Inner);
            Contract.RequireNotNull(nodeIndex, out IWriteableBrowsingExistingBlockNodeIndex NodeIndex);

            return Inner.IsMergeable(NodeIndex);
        }

        /// <summary>
        /// Merges two blocks at the given index.
        /// </summary>
        /// <param name="inner">The inner where blocks are merged.</param>
        /// <param name="nodeIndex">Index of the first node in the block to merge.</param>
        public virtual void MergeBlocks(IWriteableBlockListInner inner, IWriteableBrowsingExistingBlockNodeIndex nodeIndex)
        {
            Contract.RequireNotNull(inner, out IWriteableBlockListInner Inner);
            Contract.RequireNotNull(nodeIndex, out IWriteableBrowsingExistingBlockNodeIndex NodeIndex);
            Debug.Assert(Inner.IsMergeable(NodeIndex));

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => RedoMergeBlocks(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoMergeBlocks(operation);
            WriteableMergeBlocksOperation Operation = CreateMergeBlocksOperation(Inner.Owner.Node, Inner.PropertyName, NodeIndex.BlockIndex, HandlerRedo, HandlerUndo, isNested: false);

            Operation.Redo();
            SetLastOperation(Operation);
            CheckInvariant();
        }

        private protected virtual void RedoMergeBlocks(IWriteableOperation operation)
        {
            WriteableMergeBlocksOperation MergeBlocksOperation = (WriteableMergeBlocksOperation)operation;
            ExecuteMergeBlocks(MergeBlocksOperation);
        }

        private protected virtual void ExecuteMergeBlocks(WriteableMergeBlocksOperation operation)
        {
            Node ParentNode = operation.ParentNode;
            string PropertyName = operation.PropertyName;
            IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> Inner = GetInner(ParentNode, PropertyName) as IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex>;

            int BlockIndex = operation.BlockIndex;
            IWriteableBlockState FirstBlockState = (IWriteableBlockState)Inner.BlockStateList[BlockIndex - 1];
            IWriteableBlockState SecondBlockState = (IWriteableBlockState)Inner.BlockStateList[BlockIndex];

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

            IWriteableBlockState BlockState = (IWriteableBlockState)Inner.BlockStateList[BlockIndex - 1];

            Debug.Assert(BlockState.StateList.Count == OldNodeCount);
            Debug.Assert(FirstNodeIndex < BlockState.StateList.Count);

            NotifyBlocksMerged(operation);
        }

        private protected virtual void UndoMergeBlocks(IWriteableOperation operation)
        {
            WriteableMergeBlocksOperation MergeBlocksOperation = (WriteableMergeBlocksOperation)operation;
            WriteableSplitBlockOperation SplitBlockOperation = MergeBlocksOperation.ToSplitBlockOperation();

            ExecuteSplitBlock(SplitBlockOperation);
        }
    }
}

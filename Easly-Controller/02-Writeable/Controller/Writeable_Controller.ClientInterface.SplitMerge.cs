﻿namespace EaslyController.Writeable
{
    using System;
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

        private protected virtual void RedoSplitBlock(IWriteableOperation operation)
        {
            IWriteableSplitBlockOperation SplitBlockOperation = (IWriteableSplitBlockOperation)operation;
            ExecuteSplitBlock(SplitBlockOperation);
        }

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

        private protected virtual void RedoMergeBlocks(IWriteableOperation operation)
        {
            IWriteableMergeBlocksOperation MergeBlocksOperation = (IWriteableMergeBlocksOperation)operation;
            ExecuteMergeBlocks(MergeBlocksOperation);
        }

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

        private protected virtual void UndoMergeBlocks(IWriteableOperation operation)
        {
            IWriteableMergeBlocksOperation MergeBlocksOperation = (IWriteableMergeBlocksOperation)operation;
            IWriteableSplitBlockOperation SplitBlockOperation = MergeBlocksOperation.ToSplitBlockOperation();

            ExecuteSplitBlock(SplitBlockOperation);
        }
    }
}

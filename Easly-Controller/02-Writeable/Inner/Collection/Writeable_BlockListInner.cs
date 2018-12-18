using BaseNode;
using BaseNodeHelper;
using EaslyController.ReadOnly;
using System;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Inner for a block list.
    /// </summary>
    public interface IWriteableBlockListInner : IReadOnlyBlockListInner, IWriteableCollectionInner
    {
        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        new IWriteableBlockStateReadOnlyList BlockStateList { get; }

        /// <summary>
        /// Called when a block state is created.
        /// </summary>
        new event Action<IWriteableBlockState> BlockStateCreated;

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        new event Action<IWriteableBlockState> BlockStateRemoved;
    }

    /// <summary>
    /// Inner for a block list.
    /// </summary>
    public interface IWriteableBlockListInner<out IIndex> : IReadOnlyBlockListInner<IIndex>, IWriteableCollectionInner<IIndex>
        where IIndex : IWriteableBrowsingBlockNodeIndex
    {
        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        new IWriteableBlockStateReadOnlyList BlockStateList { get; }

        /// <summary>
        /// Called when a block state is created.
        /// </summary>
        new event Action<IWriteableBlockState> BlockStateCreated;

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        new event Action<IWriteableBlockState> BlockStateRemoved;
    }

    /// <summary>
    /// Inner for a block list.
    /// </summary>
    public class WriteableBlockListInner<IIndex, TIndex> : ReadOnlyBlockListInner<IIndex, TIndex>, IWriteableBlockListInner<IIndex>, IWriteableBlockListInner
        where IIndex : IWriteableBrowsingBlockNodeIndex
        where TIndex : WriteableBrowsingBlockNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBlockListInner{IIndex, TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public WriteableBlockListInner(IWriteableNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        public new IWriteableNodeState Owner { get { return (IWriteableNodeState)base.Owner; } }

        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        public new IWriteableBlockStateReadOnlyList BlockStateList { get { return (IWriteableBlockStateReadOnlyList)base.BlockStateList; } }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        public new IWriteablePlaceholderNodeState FirstNodeState { get { return (IWriteablePlaceholderNodeState)base.FirstNodeState; } }

        /// <summary>
        /// Called when a block state is created.
        /// </summary>
        public new event Action<IWriteableBlockState> BlockStateCreated;

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        public new event Action<IWriteableBlockState> BlockStateRemoved;
        #endregion

        #region Client Interface
        /// <summary>
        /// Inserts a new node in a list or block list.
        /// </summary>
        /// <param name="nodeIndex">Index of the node to insert.</param>
        /// <param name="browsingIndex">Index of the inserted node upon return.</param>
        /// <param name="childState">The inserted node state upon return.</param>
        public virtual void Insert(IWriteableInsertionCollectionNodeIndex nodeIndex, out IWriteableBrowsingCollectionNodeIndex browsingIndex, out IWriteablePlaceholderNodeState childState)
        {
            Debug.Assert(nodeIndex != null);

            switch (nodeIndex)
            {
                case IWriteableInsertionNewBlockNodeIndex AsNewBlockIndex:
                    InsertNew(AsNewBlockIndex, out browsingIndex, out childState);
                    break;

                case IWriteableInsertionExistingBlockNodeIndex AsExistingBlockIndex:
                    InsertExisting(AsExistingBlockIndex, out browsingIndex, out childState);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(nodeIndex));
            }
        }

        protected virtual void InsertNew(IWriteableInsertionNewBlockNodeIndex newBlockIndex, out IWriteableBrowsingCollectionNodeIndex browsingIndex, out IWriteablePlaceholderNodeState childState)
        {
            Debug.Assert(newBlockIndex != null);
            Debug.Assert(newBlockIndex.BlockIndex >= 0);
            Debug.Assert(newBlockIndex.BlockIndex <= BlockStateList.Count);

            INode ParentNode = Owner.Node;
            NodeTreeHelper.InsertIntoBlockList(ParentNode, PropertyName, newBlockIndex.BlockIndex, ReplicationStatus.Normal, newBlockIndex.PatternNode, newBlockIndex.SourceNode, out IBlock ChildBlock);
            NodeTreeHelper.InsertIntoBlock(ChildBlock, 0, newBlockIndex.Node);

            IWriteableBrowsingNewBlockNodeIndex BrowsingNewBlockIndex = (IWriteableBrowsingNewBlockNodeIndex)newBlockIndex.ToBrowsingIndex(ParentNode);
            browsingIndex = BrowsingNewBlockIndex;

            IWriteableBlockState BlockState = (IWriteableBlockState)CreateBlockState(BrowsingNewBlockIndex, ChildBlock);
            InsertInBlockStateList(newBlockIndex.BlockIndex, BlockState);

            childState = (IWriteablePlaceholderNodeState)CreateNodeState(BrowsingNewBlockIndex);
            BlockState.Insert(BrowsingNewBlockIndex, childState, 0);
        }

        protected virtual void InsertExisting(IWriteableInsertionExistingBlockNodeIndex existingBlockIndex, out IWriteableBrowsingCollectionNodeIndex browsingIndex, out IWriteablePlaceholderNodeState childState)
        {
            Debug.Assert(existingBlockIndex != null);
            Debug.Assert(existingBlockIndex.BlockIndex < BlockStateList.Count);

            IWriteableBlockState BlockState = BlockStateList[existingBlockIndex.BlockIndex];

            Debug.Assert(existingBlockIndex.Index <= BlockState.StateList.Count);

            INode ParentNode = Owner.Node;
            IBlock ChildBlock = BlockState.ChildBlock;

            NodeTreeHelper.InsertIntoBlock(ChildBlock, existingBlockIndex.Index, existingBlockIndex.Node);

            IWriteableBrowsingExistingBlockNodeIndex BrowsingExistingBlockIndex = (IWriteableBrowsingExistingBlockNodeIndex)existingBlockIndex.ToBrowsingIndex(Owner.Node);
            browsingIndex = BrowsingExistingBlockIndex;

            childState = (IWriteablePlaceholderNodeState)CreateNodeState(BrowsingExistingBlockIndex);
            BlockState.Insert(BrowsingExistingBlockIndex, childState, BrowsingExistingBlockIndex.Index);
        }
        #endregion

        #region Descendant Interface
        protected override void NotifyBlockStateCreated(IReadOnlyBlockState blockState)
        {
            BlockStateCreated?.Invoke((IWriteableBlockState)blockState);
        }

        protected override void NotifyBlockStateRemoved(IReadOnlyBlockState blockState)
        {
            BlockStateRemoved?.Invoke((IWriteableBlockState)blockState);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBlockStateList object.
        /// </summary>
        protected override IReadOnlyBlockStateList CreateBlockStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockListInner<IIndex, TIndex>));
            return new WriteableBlockStateList();
        }

        /// <summary>
        /// Creates a IxxxBlockStateReadOnlyList object.
        /// </summary>
        protected override IReadOnlyBlockStateReadOnlyList CreateBlockStateListReadOnly(IReadOnlyBlockStateList blockStateList)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockListInner<IIndex, TIndex>));
            return new WriteableBlockStateReadOnlyList((IWriteableBlockStateList)blockStateList);
        }

        /// <summary>
        /// Creates a IxxxBlockState object.
        /// </summary>
        protected override IReadOnlyBlockState CreateBlockState(IReadOnlyBrowsingNewBlockNodeIndex nodeIndex, IBlock childBlock)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockListInner<IIndex, TIndex>));
            return new WriteableBlockState(this, (IWriteableBrowsingNewBlockNodeIndex)nodeIndex, childBlock);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        protected override IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockListInner<IIndex, TIndex>));
            return new WriteablePlaceholderNodeState((IWriteableNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates an index object.
        /// </summary>
        protected override IIndex CreateNodeIndex(IReadOnlyPlaceholderNodeState state, string propertyName, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBlockListInner<IIndex, TIndex>));
            return (TIndex)(IWriteableBrowsingBlockNodeIndex)new WriteableBrowsingExistingBlockNodeIndex(Owner.Node, state.Node, propertyName, blockIndex, index);
        }
        #endregion
    }
}

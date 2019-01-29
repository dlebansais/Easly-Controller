using BaseNode;
using BaseNodeHelper;
using EaslyController.ReadOnly;
using EaslyController.Writeable;
using System;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Inner for a block list.
    /// </summary>
    public interface IFrameBlockListInner : IWriteableBlockListInner, IFrameCollectionInner
    {
        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        new IFrameBlockStateReadOnlyList BlockStateList { get; }

        /// <summary>
        /// Called when a block state is created.
        /// </summary>
        new event Action<IFrameBlockState> BlockStateCreated;

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        new event Action<IFrameBlockState> BlockStateRemoved;
    }

    /// <summary>
    /// Inner for a block list.
    /// </summary>
    public interface IFrameBlockListInner<out IIndex> : IWriteableBlockListInner<IIndex>, IFrameCollectionInner<IIndex>
        where IIndex : IFrameBrowsingBlockNodeIndex
    {
        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        new IFrameBlockStateReadOnlyList BlockStateList { get; }

        /// <summary>
        /// Called when a block state is created.
        /// </summary>
        new event Action<IFrameBlockState> BlockStateCreated;

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        new event Action<IFrameBlockState> BlockStateRemoved;
    }

    /// <summary>
    /// Inner for a block list.
    /// </summary>
    public class FrameBlockListInner<IIndex, TIndex> : WriteableBlockListInner<IIndex, TIndex>, IFrameBlockListInner<IIndex>, IFrameBlockListInner
        where IIndex : IFrameBrowsingBlockNodeIndex
        where TIndex : FrameBrowsingBlockNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBlockListInner{IIndex, TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public FrameBlockListInner(IFrameNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        public new IFrameNodeState Owner { get { return (IFrameNodeState)base.Owner; } }

        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        public new IFrameBlockStateReadOnlyList BlockStateList { get { return (IFrameBlockStateReadOnlyList)base.BlockStateList; } }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        public new IFramePlaceholderNodeState FirstNodeState { get { return (IFramePlaceholderNodeState)base.FirstNodeState; } }

        /// <summary>
        /// Called when a block state is created.
        /// </summary>
        public new event Action<IFrameBlockState> BlockStateCreated
        {
            add { AddBlockStateCreatedDelegate((Action<IReadOnlyBlockState>)value); }
            remove { RemoveBlockStateCreatedDelegate((Action<IReadOnlyBlockState>)value); }
        }

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        public new event Action<IFrameBlockState> BlockStateRemoved
        {
            add { AddBlockStateRemovedDelegate((Action<IReadOnlyBlockState>)value); }
            remove { RemoveBlockStateRemovedDelegate((Action<IReadOnlyBlockState>)value); }
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBlockStateList object.
        /// </summary>
        protected override IReadOnlyBlockStateList CreateBlockStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBlockListInner<IIndex, TIndex>));
            return new FrameBlockStateList();
        }

        /// <summary>
        /// Creates a IxxxBlockStateReadOnlyList object.
        /// </summary>
        protected override IReadOnlyBlockStateReadOnlyList CreateBlockStateListReadOnly(IReadOnlyBlockStateList blockStateList)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBlockListInner<IIndex, TIndex>));
            return new FrameBlockStateReadOnlyList((IFrameBlockStateList)blockStateList);
        }

        /// <summary>
        /// Creates a IxxxBlockState object.
        /// </summary>
        protected override IReadOnlyBlockState CreateBlockState(IReadOnlyBrowsingNewBlockNodeIndex nodeIndex, IBlock childBlock)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBlockListInner<IIndex, TIndex>));
            return new FrameBlockState(this, (IFrameBrowsingNewBlockNodeIndex)nodeIndex, childBlock);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        protected override IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBlockListInner<IIndex, TIndex>));
            return new FramePlaceholderNodeState((IFrameNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        protected override IWriteableBrowsingExistingBlockNodeIndex CreateBrowsingNodeIndex(INode node, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBlockListInner<IIndex, TIndex>));
            return new FrameBrowsingExistingBlockNodeIndex(Owner.Node, node, PropertyName, blockIndex, index);
        }

        /// <summary>
        /// Creates a IxxxBrowsingNewBlockNodeIndex object.
        /// </summary>
        protected override IWriteableBrowsingNewBlockNodeIndex CreateNewBlockNodeIndex(INode node, int blockIndex, IPattern patternNode, IIdentifier sourceNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBlockListInner<IIndex, TIndex>));
            return new FrameBrowsingNewBlockNodeIndex(Owner.Node, node, PropertyName, blockIndex, patternNode, sourceNode);
        }
        #endregion
    }
}

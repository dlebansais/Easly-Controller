namespace EaslyController.Frame
{
    using System;
    using BaseNode;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public interface IFrameBlockListInner : IWriteableBlockListInner, IFrameCollectionInner
    {
        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        new FrameBlockStateReadOnlyList BlockStateList { get; }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        new IFramePlaceholderNodeState FirstNodeState { get; }
    }

    /// <inheritdoc/>
    internal interface IFrameBlockListInner<out IIndex> : IWriteableBlockListInner<IIndex>, IFrameCollectionInner<IIndex>
        where IIndex : IFrameBrowsingBlockNodeIndex
    {
        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        new FrameBlockStateReadOnlyList BlockStateList { get; }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        new IFramePlaceholderNodeState FirstNodeState { get; }
    }

    /// <inheritdoc/>
    internal class FrameBlockListInner<IIndex> : WriteableBlockListInner<IIndex>, IFrameBlockListInner<IIndex>, IFrameBlockListInner
        where IIndex : IFrameBrowsingBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="FrameBlockListInner{IIndex}"/> object.
        /// </summary>
        public static new FrameBlockListInner<IIndex> Empty { get; } = new FrameBlockListInner<IIndex>();

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBlockListInner{IIndex}"/> class.
        /// </summary>
        protected FrameBlockListInner()
            : this(FrameNodeState<IFrameInner<IFrameBrowsingChildIndex>>.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBlockListInner{IIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        protected FrameBlockListInner(IFrameNodeState owner)
            : base(owner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBlockListInner{IIndex}"/> class.
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
        public new FrameBlockStateReadOnlyList BlockStateList { get { return (FrameBlockStateReadOnlyList)base.BlockStateList; } }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        public new IFramePlaceholderNodeState FirstNodeState { get { return (IFramePlaceholderNodeState)base.FirstNodeState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBlockStateList object.
        /// </summary>
        private protected override ReadOnlyBlockStateList CreateBlockStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBlockListInner<IIndex>));
            return new FrameBlockStateList();
        }

        /// <summary>
        /// Creates a IxxxBlockState object.
        /// </summary>
        private protected override IReadOnlyBlockState CreateBlockState(IReadOnlyBrowsingNewBlockNodeIndex nodeIndex, IBlock childBlock)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBlockListInner<IIndex>));
            return new FrameBlockState<IFrameInner<IFrameBrowsingChildIndex>>(this, (IFrameBrowsingNewBlockNodeIndex)nodeIndex, childBlock);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        private protected override IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBlockListInner<IIndex>));
            return new FramePlaceholderNodeState<IFrameInner<IFrameBrowsingChildIndex>>((IFrameNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingBlockNodeIndexList.
        /// </summary>
        private protected override ReadOnlyBrowsingBlockNodeIndexList CreateBlockNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBlockListInner<IIndex>));
            return new FrameBrowsingBlockNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        private protected override IWriteableBrowsingExistingBlockNodeIndex CreateBrowsingNodeIndex(Node node, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBlockListInner<IIndex>));
            return new FrameBrowsingExistingBlockNodeIndex(Owner.Node, node, PropertyName, blockIndex, index);
        }

        /// <summary>
        /// Creates a IxxxBrowsingNewBlockNodeIndex object.
        /// </summary>
        private protected override IWriteableBrowsingNewBlockNodeIndex CreateNewBlockNodeIndex(Node node, int blockIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBlockListInner<IIndex>));
            return new FrameBrowsingNewBlockNodeIndex(Owner.Node, node, PropertyName, blockIndex);
        }
        #endregion
    }
}

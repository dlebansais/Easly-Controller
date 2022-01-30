namespace EaslyController.Focus
{
    using System;
    using BaseNode;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public interface IFocusBlockListInner : IFrameBlockListInner, IFocusCollectionInner
    {
        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        new FocusBlockStateReadOnlyList BlockStateList { get; }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        new IFocusPlaceholderNodeState FirstNodeState { get; }
    }

    /// <inheritdoc/>
    internal interface IFocusBlockListInner<out IIndex> : IFrameBlockListInner<IIndex>, IFocusCollectionInner<IIndex>
        where IIndex : IFocusBrowsingBlockNodeIndex
    {
        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        new FocusBlockStateReadOnlyList BlockStateList { get; }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        new IFocusPlaceholderNodeState FirstNodeState { get; }
    }

    /// <inheritdoc/>
    internal class FocusBlockListInner<IIndex> : FrameBlockListInner<IIndex>, IFocusBlockListInner<IIndex>, IFocusBlockListInner
        where IIndex : IFocusBrowsingBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="FocusBlockListInner{IIndex}"/> object.
        /// </summary>
        public static new FocusBlockListInner<IIndex> Empty { get; } = new FocusBlockListInner<IIndex>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FocusBlockListInner{IIndex}"/> class.
        /// </summary>
        protected FocusBlockListInner()
            : this(FocusNodeState<IFocusInner<IFocusBrowsingChildIndex>>.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FocusBlockListInner{IIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        protected FocusBlockListInner(IFocusNodeState owner)
            : base(owner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FocusBlockListInner{IIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public FocusBlockListInner(IFocusNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        public new IFocusNodeState Owner { get { return (IFocusNodeState)base.Owner; } }

        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        public new FocusBlockStateReadOnlyList BlockStateList { get { return (FocusBlockStateReadOnlyList)base.BlockStateList; } }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        public new IFocusPlaceholderNodeState FirstNodeState { get { return (IFocusPlaceholderNodeState)base.FirstNodeState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBlockStateList object.
        /// </summary>
        private protected override ReadOnlyBlockStateList CreateBlockStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBlockListInner<IIndex>));
            return new FocusBlockStateList();
        }

        /// <summary>
        /// Creates a IxxxBlockState object.
        /// </summary>
        private protected override IReadOnlyBlockState CreateBlockState(IReadOnlyBrowsingNewBlockNodeIndex nodeIndex, IBlock childBlock)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBlockListInner<IIndex>));
            return new FocusBlockState<IFocusInner<IFocusBrowsingChildIndex>>(this, (IFocusBrowsingNewBlockNodeIndex)nodeIndex, childBlock);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        private protected override IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBlockListInner<IIndex>));
            return new FocusPlaceholderNodeState<IFocusInner<IFocusBrowsingChildIndex>>((IFocusNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingBlockNodeIndexList.
        /// </summary>
        private protected override ReadOnlyBrowsingBlockNodeIndexList CreateBlockNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBlockListInner<IIndex>));
            return new FocusBrowsingBlockNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        private protected override IWriteableBrowsingExistingBlockNodeIndex CreateBrowsingNodeIndex(Node node, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBlockListInner<IIndex>));
            return new FocusBrowsingExistingBlockNodeIndex(Owner.Node, node, PropertyName, blockIndex, index);
        }

        /// <summary>
        /// Creates a IxxxBrowsingNewBlockNodeIndex object.
        /// </summary>
        private protected override IWriteableBrowsingNewBlockNodeIndex CreateNewBlockNodeIndex(Node node, int blockIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBlockListInner<IIndex>));
            return new FocusBrowsingNewBlockNodeIndex(Owner.Node, node, PropertyName, blockIndex);
        }
        #endregion
    }
}

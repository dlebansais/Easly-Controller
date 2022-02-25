namespace EaslyController.Layout
{
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <inheritdoc/>
    public interface ILayoutBlockListInner : IFocusBlockListInner, ILayoutCollectionInner
    {
        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        new LayoutBlockStateReadOnlyList BlockStateList { get; }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        new ILayoutPlaceholderNodeState FirstNodeState { get; }
    }

    /// <inheritdoc/>
    internal interface ILayoutBlockListInner<out IIndex> : IFocusBlockListInner<IIndex>, ILayoutCollectionInner<IIndex>
        where IIndex : ILayoutBrowsingBlockNodeIndex
    {
        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        new LayoutBlockStateReadOnlyList BlockStateList { get; }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        new ILayoutPlaceholderNodeState FirstNodeState { get; }
    }

    /// <inheritdoc/>
    internal class LayoutBlockListInner<IIndex> : FocusBlockListInner<IIndex>, ILayoutBlockListInner<IIndex>, ILayoutBlockListInner
        where IIndex : ILayoutBrowsingBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="LayoutBlockListInner{IIndex}"/> object.
        /// </summary>
        public static new LayoutBlockListInner<IIndex> Empty { get; } = new LayoutBlockListInner<IIndex>();

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutBlockListInner{IIndex}"/> class.
        /// </summary>
        protected LayoutBlockListInner()
            : this(LayoutNodeState<ILayoutInner<ILayoutBrowsingChildIndex>>.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutBlockListInner{IIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        protected LayoutBlockListInner(ILayoutNodeState owner)
            : base(owner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutBlockListInner{IIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public LayoutBlockListInner(ILayoutNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        public new ILayoutNodeState Owner { get { return (ILayoutNodeState)base.Owner; } }

        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        public new LayoutBlockStateReadOnlyList BlockStateList { get { return (LayoutBlockStateReadOnlyList)base.BlockStateList; } }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        public new ILayoutPlaceholderNodeState FirstNodeState { get { return (ILayoutPlaceholderNodeState)base.FirstNodeState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBlockStateList object.
        /// </summary>
        private protected override ReadOnlyBlockStateList CreateBlockStateList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutBlockListInner<IIndex>>());
            return new LayoutBlockStateList();
        }

        /// <summary>
        /// Creates a IxxxBlockState object.
        /// </summary>
        private protected override IReadOnlyBlockState CreateBlockState(IReadOnlyBrowsingNewBlockNodeIndex nodeIndex, IBlock childBlock)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutBlockListInner<IIndex>>());
            return new LayoutBlockState<ILayoutInner<ILayoutBrowsingChildIndex>>(this, (ILayoutBrowsingNewBlockNodeIndex)nodeIndex, childBlock);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        private protected override IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutBlockListInner<IIndex>>());
            return new LayoutPlaceholderNodeState<ILayoutInner<ILayoutBrowsingChildIndex>>((ILayoutNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingBlockNodeIndexList.
        /// </summary>
        private protected override ReadOnlyBrowsingBlockNodeIndexList CreateBlockNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutBlockListInner<IIndex>>());
            return new LayoutBrowsingBlockNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        private protected override IWriteableBrowsingExistingBlockNodeIndex CreateBrowsingNodeIndex(Node node, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutBlockListInner<IIndex>>());
            return new LayoutBrowsingExistingBlockNodeIndex(Owner.Node, node, PropertyName, blockIndex, index);
        }

        /// <summary>
        /// Creates a IxxxBrowsingNewBlockNodeIndex object.
        /// </summary>
        private protected override IWriteableBrowsingNewBlockNodeIndex CreateNewBlockNodeIndex(Node node, int blockIndex)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutBlockListInner<IIndex>>());
            return new LayoutBrowsingNewBlockNodeIndex(Owner.Node, node, PropertyName, blockIndex);
        }
        #endregion
    }
}

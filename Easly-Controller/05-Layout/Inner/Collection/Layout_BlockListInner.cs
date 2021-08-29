namespace EaslyController.Layout
{
    using System;
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Inner for a block list.
    /// </summary>
    public interface ILayoutBlockListInner : IFocusBlockListInner, ILayoutCollectionInner
    {
        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        new ILayoutBlockStateReadOnlyList BlockStateList { get; }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        new ILayoutPlaceholderNodeState FirstNodeState { get; }
    }

    /// <summary>
    /// Inner for a block list.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal interface ILayoutBlockListInner<out IIndex> : IFocusBlockListInner<IIndex>, ILayoutCollectionInner<IIndex>
        where IIndex : ILayoutBrowsingBlockNodeIndex
    {
        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        new ILayoutBlockStateReadOnlyList BlockStateList { get; }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        new ILayoutPlaceholderNodeState FirstNodeState { get; }
    }

    /// <summary>
    /// Inner for a block list.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index as interface.</typeparam>
    /// <typeparam name="TIndex">Type of the index as class.</typeparam>
    internal class LayoutBlockListInner<IIndex, TIndex> : FocusBlockListInner<IIndex, TIndex>, ILayoutBlockListInner<IIndex>, ILayoutBlockListInner
        where IIndex : ILayoutBrowsingBlockNodeIndex
        where TIndex : LayoutBrowsingBlockNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutBlockListInner{IIndex, TIndex}"/> class.
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
        public new ILayoutBlockStateReadOnlyList BlockStateList { get { return (ILayoutBlockStateReadOnlyList)base.BlockStateList; } }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        public new ILayoutPlaceholderNodeState FirstNodeState { get { return (ILayoutPlaceholderNodeState)base.FirstNodeState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBlockStateList object.
        /// </summary>
        private protected override IReadOnlyBlockStateList CreateBlockStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutBlockListInner<IIndex, TIndex>));
            return new LayoutBlockStateList();
        }

        /// <summary>
        /// Creates a IxxxBlockState object.
        /// </summary>
        private protected override IReadOnlyBlockState CreateBlockState(IReadOnlyBrowsingNewBlockNodeIndex nodeIndex, IBlock childBlock)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutBlockListInner<IIndex, TIndex>));
            return new LayoutBlockState<ILayoutInner<ILayoutBrowsingChildIndex>>(this, (ILayoutBrowsingNewBlockNodeIndex)nodeIndex, childBlock);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        private protected override IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutBlockListInner<IIndex, TIndex>));
            return new LayoutPlaceholderNodeState<ILayoutInner<ILayoutBrowsingChildIndex>>((ILayoutNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingBlockNodeIndexList.
        /// </summary>
        private protected override IReadOnlyBrowsingBlockNodeIndexList CreateBlockNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutBlockListInner<IIndex, TIndex>));
            return new LayoutBrowsingBlockNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        private protected override IWriteableBrowsingExistingBlockNodeIndex CreateBrowsingNodeIndex(Node node, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutBlockListInner<IIndex, TIndex>));
            return new LayoutBrowsingExistingBlockNodeIndex(Owner.Node, node, PropertyName, blockIndex, index);
        }

        /// <summary>
        /// Creates a IxxxBrowsingNewBlockNodeIndex object.
        /// </summary>
        private protected override IWriteableBrowsingNewBlockNodeIndex CreateNewBlockNodeIndex(Node node, int blockIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutBlockListInner<IIndex, TIndex>));
            return new LayoutBrowsingNewBlockNodeIndex(Owner.Node, node, PropertyName, blockIndex);
        }
        #endregion
    }
}

namespace EaslyController.Focus
{
    using System;
    using BaseNode;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Inner for a block list.
    /// </summary>
    public interface IFocusBlockListInner : IFrameBlockListInner, IFocusCollectionInner
    {
        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        new IFocusBlockStateReadOnlyList BlockStateList { get; }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        new IFocusPlaceholderNodeState FirstNodeState { get; }
    }

    /// <summary>
    /// Inner for a block list.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal interface IFocusBlockListInner<out IIndex> : IFrameBlockListInner<IIndex>, IFocusCollectionInner<IIndex>
        where IIndex : IFocusBrowsingBlockNodeIndex
    {
        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        new IFocusBlockStateReadOnlyList BlockStateList { get; }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        new IFocusPlaceholderNodeState FirstNodeState { get; }
    }

    /// <summary>
    /// Inner for a block list.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index as interface.</typeparam>
    /// <typeparam name="TIndex">Type of the index as class.</typeparam>
    internal class FocusBlockListInner<IIndex, TIndex> : FrameBlockListInner<IIndex, TIndex>, IFocusBlockListInner<IIndex>, IFocusBlockListInner
        where IIndex : IFocusBrowsingBlockNodeIndex
        where TIndex : FocusBrowsingBlockNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusBlockListInner{IIndex, TIndex}"/> class.
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
        public new IFocusBlockStateReadOnlyList BlockStateList { get { return (IFocusBlockStateReadOnlyList)base.BlockStateList; } }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        public new IFocusPlaceholderNodeState FirstNodeState { get { return (IFocusPlaceholderNodeState)base.FirstNodeState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBlockStateList object.
        /// </summary>
        private protected override IReadOnlyBlockStateList CreateBlockStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBlockListInner<IIndex, TIndex>));
            return new FocusBlockStateList();
        }

        /// <summary>
        /// Creates a IxxxBlockState object.
        /// </summary>
        private protected override IReadOnlyBlockState CreateBlockState(IReadOnlyBrowsingNewBlockNodeIndex nodeIndex, IBlock childBlock)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBlockListInner<IIndex, TIndex>));
            return new FocusBlockState<IFocusInner<IFocusBrowsingChildIndex>>(this, (IFocusBrowsingNewBlockNodeIndex)nodeIndex, childBlock);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        private protected override IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBlockListInner<IIndex, TIndex>));
            return new FocusPlaceholderNodeState<IFocusInner<IFocusBrowsingChildIndex>>((IFocusNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingBlockNodeIndexList.
        /// </summary>
        private protected override IReadOnlyBrowsingBlockNodeIndexList CreateBlockNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBlockListInner<IIndex, TIndex>));
            return new FocusBrowsingBlockNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        private protected override IWriteableBrowsingExistingBlockNodeIndex CreateBrowsingNodeIndex(INode node, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBlockListInner<IIndex, TIndex>));
            return new FocusBrowsingExistingBlockNodeIndex(Owner.Node, node, PropertyName, blockIndex, index);
        }

        /// <summary>
        /// Creates a IxxxBrowsingNewBlockNodeIndex object.
        /// </summary>
        private protected override IWriteableBrowsingNewBlockNodeIndex CreateNewBlockNodeIndex(INode node, int blockIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBlockListInner<IIndex, TIndex>));
            return new FocusBrowsingNewBlockNodeIndex(Owner.Node, node, PropertyName, blockIndex);
        }
        #endregion
    }
}

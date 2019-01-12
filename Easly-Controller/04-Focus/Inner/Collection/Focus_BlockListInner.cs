﻿using BaseNode;
using EaslyController.Frame;
using EaslyController.ReadOnly;
using EaslyController.Writeable;
using System;

namespace EaslyController.Focus
{
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
        /// Called when a block state is created.
        /// </summary>
        new event Action<IFocusBlockState> BlockStateCreated;

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        new event Action<IFocusBlockState> BlockStateRemoved;
    }

    /// <summary>
    /// Inner for a block list.
    /// </summary>
    public interface IFocusBlockListInner<out IIndex> : IFrameBlockListInner<IIndex>, IFocusCollectionInner<IIndex>
        where IIndex : IFocusBrowsingBlockNodeIndex
    {
        /// <summary>
        /// States of blocks in the block list.
        /// </summary>
        new IFocusBlockStateReadOnlyList BlockStateList { get; }

        /// <summary>
        /// Called when a block state is created.
        /// </summary>
        new event Action<IFocusBlockState> BlockStateCreated;

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        new event Action<IFocusBlockState> BlockStateRemoved;
    }

    /// <summary>
    /// Inner for a block list.
    /// </summary>
    public class FocusBlockListInner<IIndex, TIndex> : FrameBlockListInner<IIndex, TIndex>, IFocusBlockListInner<IIndex>, IFocusBlockListInner
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

        /// <summary>
        /// Called when a block state is created.
        /// </summary>
        public new event Action<IFocusBlockState> BlockStateCreated
        {
            add { AddBlockStateCreatedDelegate((Action<IReadOnlyBlockState>)value); }
            remove { RemoveBlockStateCreatedDelegate((Action<IReadOnlyBlockState>)value); }
        }

        /// <summary>
        /// Called when a block state is removed.
        /// </summary>
        public new event Action<IFocusBlockState> BlockStateRemoved
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
            ControllerTools.AssertNoOverride(this, typeof(FocusBlockListInner<IIndex, TIndex>));
            return new FocusBlockStateList();
        }

        /// <summary>
        /// Creates a IxxxBlockStateReadOnlyList object.
        /// </summary>
        protected override IReadOnlyBlockStateReadOnlyList CreateBlockStateListReadOnly(IReadOnlyBlockStateList blockStateList)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBlockListInner<IIndex, TIndex>));
            return new FocusBlockStateReadOnlyList((IFocusBlockStateList)blockStateList);
        }

        /// <summary>
        /// Creates a IxxxBlockState object.
        /// </summary>
        protected override IReadOnlyBlockState CreateBlockState(IReadOnlyBrowsingNewBlockNodeIndex nodeIndex, IBlock childBlock)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBlockListInner<IIndex, TIndex>));
            return new FocusBlockState(this, (IFocusBrowsingNewBlockNodeIndex)nodeIndex, childBlock);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        protected override IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBlockListInner<IIndex, TIndex>));
            return new FocusPlaceholderNodeState((IFocusNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates an index object.
        /// </summary>
        protected override IIndex CreateNodeIndex(IReadOnlyPlaceholderNodeState state, string propertyName, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBlockListInner<IIndex, TIndex>));
            return (TIndex)(IFocusBrowsingBlockNodeIndex)new FocusBrowsingExistingBlockNodeIndex(Owner.Node, state.Node, propertyName, blockIndex, index);
        }

        /// <summary>
        /// Creates a IxxxBrowsingNewBlockNodeIndex object.
        /// </summary>
        protected override IWriteableBrowsingNewBlockNodeIndex CreateNewBlockNodeIndex(INode node, int blockIndex, IPattern patternNode, IIdentifier sourceNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBlockListInner<IIndex, TIndex>));
            return new FocusBrowsingNewBlockNodeIndex(Owner.Node, node, PropertyName, blockIndex, patternNode, sourceNode);
        }
        #endregion
    }
}
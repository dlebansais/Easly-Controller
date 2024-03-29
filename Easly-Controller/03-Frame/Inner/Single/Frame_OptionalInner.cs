﻿namespace EaslyController.Frame
{
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <inheritdoc/>
    public interface IFrameOptionalInner : IWriteableOptionalInner, IFrameSingleInner
    {
        /// <summary>
        /// The state of the optional node.
        /// </summary>
        new IFrameOptionalNodeState ChildState { get; }
    }

    /// <inheritdoc/>
    internal interface IFrameOptionalInner<out IIndex> : IWriteableOptionalInner<IIndex>, IFrameSingleInner<IIndex>
        where IIndex : IFrameBrowsingOptionalNodeIndex
    {
        /// <summary>
        /// The state of the optional node.
        /// </summary>
        new IFrameOptionalNodeState ChildState { get; }
    }

    /// <inheritdoc/>
    internal class FrameOptionalInner<IIndex> : WriteableOptionalInner<IIndex>, IFrameOptionalInner<IIndex>, IFrameOptionalInner
        where IIndex : IFrameBrowsingOptionalNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameOptionalInner{IIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public FrameOptionalInner(IFrameNodeState owner, string propertyName)
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
        /// The state of the optional node.
        /// </summary>
        public new IFrameOptionalNodeState ChildState { get { return (IFrameOptionalNodeState)base.ChildState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxOptionalNodeState object.
        /// </summary>
        private protected override IReadOnlyOptionalNodeState CreateNodeState(IReadOnlyBrowsingOptionalNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameOptionalInner<IIndex>>());
            return new FrameOptionalNodeState<IFrameInner<IFrameBrowsingChildIndex>>((IFrameBrowsingOptionalNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxBrowsingOptionalNodeIndex object.
        /// </summary>
        private protected override IWriteableBrowsingOptionalNodeIndex CreateBrowsingNodeIndex()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameOptionalInner<IIndex>>());
            return new FrameBrowsingOptionalNodeIndex(Owner.Node, PropertyName);
        }
        #endregion
    }
}

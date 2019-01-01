using BaseNode;
using BaseNodeHelper;
using EaslyController.ReadOnly;
using EaslyController.Writeable;
using System;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Inner for an optional node.
    /// </summary>
    public interface IFrameOptionalInner : IWriteableOptionalInner, IFrameSingleInner
    {
        /// <summary>
        /// The state of the optional node.
        /// </summary>
        new IFrameOptionalNodeState ChildState { get; }
    }

    /// <summary>
    /// Inner for an optional node.
    /// </summary>
    public interface IFrameOptionalInner<out IIndex> : IWriteableOptionalInner<IIndex>, IFrameSingleInner<IIndex>
        where IIndex : IFrameBrowsingOptionalNodeIndex
    {
        /// <summary>
        /// The state of the optional node.
        /// </summary>
        new IFrameOptionalNodeState ChildState { get; }
    }

    /// <summary>
    /// Inner for an optional node.
    /// </summary>
    public class FrameOptionalInner<IIndex, TIndex> : WriteableOptionalInner<IIndex, TIndex>, IFrameOptionalInner<IIndex>, IFrameOptionalInner
        where IIndex : IFrameBrowsingOptionalNodeIndex
        where TIndex : FrameBrowsingOptionalNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameOptionalInner{IIndex, TIndex}"/> class.
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
        IFrameNodeState IFrameSingleInner.ChildState { get { return ChildState; } }
        IFrameNodeState IFrameSingleInner<IIndex>.ChildState { get { return ChildState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxOptionalNodeState object.
        /// </summary>
        protected override IReadOnlyOptionalNodeState CreateNodeState(IReadOnlyBrowsingOptionalNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameOptionalInner<IIndex, TIndex>));
            return new FrameOptionalNodeState((IFrameBrowsingOptionalNodeIndex)nodeIndex);
        }
        #endregion
    }
}

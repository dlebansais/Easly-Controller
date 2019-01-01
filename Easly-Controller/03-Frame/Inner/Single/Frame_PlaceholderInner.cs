using BaseNode;
using BaseNodeHelper;
using EaslyController.ReadOnly;
using EaslyController.Writeable;
using System;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Inner for a child node.
    /// </summary>
    public interface IFramePlaceholderInner : IWriteablePlaceholderInner, IFrameSingleInner
    {
    }

    /// <summary>
    /// Inner for a child node.
    /// </summary>
    public interface IFramePlaceholderInner<out IIndex> : IWriteablePlaceholderInner<IIndex>, IFrameSingleInner<IIndex>
        where IIndex : IFrameBrowsingPlaceholderNodeIndex
    {
    }

    /// <summary>
    /// Inner for a child node.
    /// </summary>
    public class FramePlaceholderInner<IIndex, TIndex> : WriteablePlaceholderInner<IIndex, TIndex>, IFramePlaceholderInner<IIndex>, IFramePlaceholderInner
        where IIndex : IFrameBrowsingPlaceholderNodeIndex
        where TIndex : FrameBrowsingPlaceholderNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FramePlaceholderInner{IIndex, TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public FramePlaceholderInner(IFrameNodeState owner, string propertyName)
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
        public new IFrameNodeState ChildState { get { return (IFrameNodeState)base.ChildState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        protected override IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyBrowsingPlaceholderNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePlaceholderInner<IIndex, TIndex>));
            return new FramePlaceholderNodeState((IFrameBrowsingPlaceholderNodeIndex)nodeIndex);
        }
        #endregion
    }
}

using BaseNode;
using BaseNodeHelper;
using EaslyController.ReadOnly;
using EaslyController.Frame;
using System;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// Inner for an optional node.
    /// </summary>
    public interface IFocusOptionalInner : IFrameOptionalInner, IFocusSingleInner
    {
        /// <summary>
        /// The state of the optional node.
        /// </summary>
        new IFocusOptionalNodeState ChildState { get; }
    }

    /// <summary>
    /// Inner for an optional node.
    /// </summary>
    public interface IFocusOptionalInner<out IIndex> : IFrameOptionalInner<IIndex>, IFocusSingleInner<IIndex>
        where IIndex : IFocusBrowsingOptionalNodeIndex
    {
        /// <summary>
        /// The state of the optional node.
        /// </summary>
        new IFocusOptionalNodeState ChildState { get; }
    }

    /// <summary>
    /// Inner for an optional node.
    /// </summary>
    public class FocusOptionalInner<IIndex, TIndex> : FrameOptionalInner<IIndex, TIndex>, IFocusOptionalInner<IIndex>, IFocusOptionalInner
        where IIndex : IFocusBrowsingOptionalNodeIndex
        where TIndex : FocusBrowsingOptionalNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusOptionalInner{IIndex, TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public FocusOptionalInner(IFocusNodeState owner, string propertyName)
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
        /// The state of the optional node.
        /// </summary>
        public new IFocusOptionalNodeState ChildState { get { return (IFocusOptionalNodeState)base.ChildState; } }
        IFocusNodeState IFocusSingleInner.ChildState { get { return ChildState; } }
        IFocusNodeState IFocusSingleInner<IIndex>.ChildState { get { return ChildState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxOptionalNodeState object.
        /// </summary>
        protected override IReadOnlyOptionalNodeState CreateNodeState(IReadOnlyBrowsingOptionalNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusOptionalInner<IIndex, TIndex>));
            return new FocusOptionalNodeState((IFocusBrowsingOptionalNodeIndex)nodeIndex);
        }
        #endregion
    }
}

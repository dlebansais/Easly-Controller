using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Interface for all inners.
    /// </summary>
    public interface IFocusInner : IFrameInner
    {
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        new IFocusNodeState Owner { get; }
    }

    /// <summary>
    /// Interface for all inners.
    /// </summary>
    public interface IFocusInner<out IIndex> : IFrameInner<IIndex>
        where IIndex : IFocusBrowsingChildIndex
    {
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        new IFocusNodeState Owner { get; }
    }

    /// <summary>
    /// Interface for all inners.
    /// </summary>
    public abstract class FocusInner<IIndex> : FrameInner<IIndex>, IFocusInner<IIndex>, IFocusInner
        where IIndex : IFocusBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusInner{IIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public FocusInner(IFocusNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        public new IFocusNodeState Owner { get { return (IFocusNodeState)base.Owner; } }
        #endregion
    }
}

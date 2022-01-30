namespace EaslyController.Focus
{
    using BaseNode;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public interface IFocusEmptyInner : IFrameEmptyInner, IFocusSingleInner
    {
        /// <summary>
        /// The state of the node.
        /// </summary>
        new IFocusEmptyNodeState ChildState { get; }
    }

    /// <inheritdoc/>
    internal interface IFocusEmptyInner<out IIndex> : IFrameEmptyInner<IIndex>, IFocusSingleInner<IIndex>
        where IIndex : IFocusBrowsingChildIndex
    {
        /// <summary>
        /// The state of the node.
        /// </summary>
        new IFocusEmptyNodeState ChildState { get; }
    }

    /// <inheritdoc/>
    internal class FocusEmptyInner<IIndex> : FrameEmptyInner<IIndex>, IFocusEmptyInner<IIndex>, IFocusEmptyInner
        where IIndex : IFocusBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusEmptyInner{IIndex}"/> class.
        /// </summary>
        public FocusEmptyInner()
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
        public new IFocusEmptyNodeState ChildState { get { return (IFocusEmptyNodeState)base.ChildState; } }
        #endregion
    }
}

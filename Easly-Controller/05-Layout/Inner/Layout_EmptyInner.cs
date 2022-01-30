namespace EaslyController.Layout
{
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public interface ILayoutEmptyInner : IFocusEmptyInner, ILayoutSingleInner
    {
        /// <summary>
        /// The state of the node.
        /// </summary>
        new ILayoutEmptyNodeState ChildState { get; }
    }

    /// <inheritdoc/>
    internal interface ILayoutEmptyInner<out IIndex> : IFocusEmptyInner<IIndex>, ILayoutSingleInner<IIndex>
        where IIndex : ILayoutBrowsingChildIndex
    {
        /// <summary>
        /// The state of the node.
        /// </summary>
        new ILayoutEmptyNodeState ChildState { get; }
    }

    /// <inheritdoc/>
    internal class LayoutEmptyInner<IIndex> : FocusEmptyInner<IIndex>, ILayoutEmptyInner<IIndex>, ILayoutEmptyInner
        where IIndex : ILayoutBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutEmptyInner{IIndex}"/> class.
        /// </summary>
        public LayoutEmptyInner()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        public new ILayoutNodeState Owner { get { return (ILayoutNodeState)base.Owner; } }

        /// <summary>
        /// The state of the optional node.
        /// </summary>
        public new ILayoutEmptyNodeState ChildState { get { return (ILayoutEmptyNodeState)base.ChildState; } }
        #endregion
    }
}

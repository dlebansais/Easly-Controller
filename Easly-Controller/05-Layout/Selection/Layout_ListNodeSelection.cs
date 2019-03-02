namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// A selection of nodes in a list.
    /// </summary>
    public interface ILayoutListNodeSelection : IFocusListNodeSelection, ILayoutContentSelection
    {
    }

    /// <summary>
    /// A selection of nodes in a list.
    /// </summary>
    public class LayoutListNodeSelection : FocusListNodeSelection, ILayoutListNodeSelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutListNodeSelection"/> class.
        /// </summary>
        /// <param name="stateView">The state view that encompasses the selection.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="startIndex">Index of the first selected node in the list.</param>
        /// <param name="endIndex">Index of the last selected node in the list.</param>
        public LayoutListNodeSelection(ILayoutNodeStateView stateView, string propertyName, int startIndex, int endIndex)
            : base(stateView, propertyName, startIndex, endIndex)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The state view that encompasses the selection.
        /// </summary>
        public new ILayoutNodeStateView StateView { get { return (ILayoutNodeStateView)base.StateView; } }
        #endregion
    }
}

namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// A selection of nodes in a block of a list block.
    /// </summary>
    public interface ILayoutBlockListNodeSelection : IFocusBlockListNodeSelection, ILayoutContentSelection
    {
    }

    /// <summary>
    /// A selection of nodes in a block of a list block.
    /// </summary>
    public class LayoutBlockListNodeSelection : FocusBlockListNodeSelection, ILayoutBlockListNodeSelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutBlockListNodeSelection"/> class.
        /// </summary>
        /// <param name="stateView">The state view that encompasses the selection.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="blockIndex">Index of the block.</param>
        /// <param name="startIndex">Index of the first selected node in the list.</param>
        /// <param name="endIndex">Index of the last selected node in the list.</param>
        public LayoutBlockListNodeSelection(ILayoutNodeStateView stateView, string propertyName, int blockIndex, int startIndex, int endIndex)
            : base(stateView, propertyName, blockIndex, startIndex, endIndex)
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

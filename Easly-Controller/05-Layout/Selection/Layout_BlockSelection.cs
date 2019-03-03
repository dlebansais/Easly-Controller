namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// A selection of blocks in a block list.
    /// </summary>
    public interface ILayoutBlockSelection : IFocusBlockSelection, ILayoutContentSelection
    {
    }

    /// <summary>
    /// A selection of blocks in a block list.
    /// </summary>
    public class LayoutBlockSelection : FocusBlockSelection, ILayoutBlockSelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutBlockSelection"/> class.
        /// </summary>
        /// <param name="stateView">The state view that encompasses the selection.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="startIndex">Index of the first selected block.</param>
        /// <param name="endIndex">Index of the last selected block.</param>
        public LayoutBlockSelection(ILayoutNodeStateView stateView, string propertyName, int startIndex, int endIndex)
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

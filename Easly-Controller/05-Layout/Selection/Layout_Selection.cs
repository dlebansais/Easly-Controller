namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// A selection of part of a node value, or a selection of nodes.
    /// </summary>
    public interface ILayoutSelection : IFocusSelection
    {
        /// <summary>
        /// The state view that encompasses the selection.
        /// </summary>
        new ILayoutNodeStateView StateView { get; }

        /// <summary>
        /// Prints the selection.
        /// </summary>
        void Print();
    }

    /// <summary>
    /// A selection of part of a node value, or a selection of nodes.
    /// </summary>
    public abstract class LayoutSelection : FocusSelection, ILayoutSelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutSelection"/> class.
        /// </summary>
        /// <param name="stateView">The state view that encompasses the selection.</param>
        public LayoutSelection(ILayoutNodeStateView stateView)
            : base(stateView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The state view that encompasses the selection.
        /// </summary>
        public new ILayoutNodeStateView StateView { get { return (ILayoutNodeStateView)base.StateView; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Prints the selection.
        /// </summary>
        public abstract void Print();
        #endregion
    }
}

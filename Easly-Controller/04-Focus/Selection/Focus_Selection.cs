namespace EaslyController.Focus
{
    /// <summary>
    /// A selection of part of a node value, or a selection of nodes.
    /// </summary>
    public interface IFocusSelection
    {
        /// <summary>
        /// The state view that encompasses the selection.
        /// </summary>
        IFocusNodeStateView StateView { get; }
    }

    /// <summary>
    /// A selection of part of a node value, or a selection of nodes.
    /// </summary>
    public abstract class FocusSelection : IFocusSelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusSelection"/> class.
        /// </summary>
        /// <param name="stateView">The state view that encompasses the selection.</param>
        public FocusSelection(IFocusNodeStateView stateView)
        {
            StateView = stateView;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The state view that encompasses the selection.
        /// </summary>
        public IFocusNodeStateView StateView { get; }
        #endregion
    }
}

namespace EaslyController.Focus
{
    using System.Windows;

    /// <summary>
    /// A selection of part of a node value, or a selection of nodes.
    /// </summary>
    public interface IFocusSelection
    {
        /// <summary>
        /// The state view that encompasses the selection.
        /// </summary>
        IFocusNodeStateView StateView { get; }

        /// <summary>
        /// Copy the selection in the clipboard.
        /// </summary>
        /// <param name="dataObject">The clipboard data object that can already contain other custom formats.</param>
        void Copy(IDataObject dataObject);

        /// <summary>
        /// Copy the selection in the clipboard then removes it.
        /// </summary>
        void Cut();

        /// <summary>
        /// Replaces the selection with the content of the clipboard.
        /// </summary>
        void Paste();
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

        #region Client Interface
        /// <summary>
        /// Copy the selection in the clipboard.
        /// </summary>
        /// <param name="dataObject">The clipboard data object that can already contain other custom formats.</param>
        public abstract void Copy(IDataObject dataObject);

        /// <summary>
        /// Copy the selection in the clipboard then removes it.
        /// </summary>
        public abstract void Cut();

        /// <summary>
        /// Replaces the selection with the content of the clipboard.
        /// </summary>
        public abstract void Paste();
        #endregion
    }
}

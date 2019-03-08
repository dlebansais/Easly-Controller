namespace EaslyController.Focus
{
    using System.Windows;
    using EaslyController.Controller;

    /// <summary>
    /// A selection of a node an all its content and children.
    /// </summary>
    public interface IFocusNodeSelection : IFocusSelection
    {
    }

    /// <summary>
    /// A selection of a node an all its content and children.
    /// </summary>
    public class FocusNodeSelection : FocusSelection, IFocusNodeSelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusNodeSelection"/> class.
        /// </summary>
        /// <param name="stateView">The selected state view.</param>
        public FocusNodeSelection(IFocusNodeStateView stateView)
            : base(stateView)
        {
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Copy the selection in the clipboard.
        /// </summary>
        /// <param name="dataObject">The clipboard data object that can already contain other custom formats.</param>
        public override void Copy(IDataObject dataObject)
        {
            ClipboardHelper.WriteNode(dataObject, StateView.State.Node);
        }

        /// <summary>
        /// Copy the selection in the clipboard then removes it.
        /// </summary>
        /// <param name="dataObject">The clipboard data object that can already contain other custom formats.</param>
        /// <param name="isDeleted">True if something was deleted.</param>
        public override void Cut(IDataObject dataObject, out bool isDeleted)
        {
            isDeleted = false;
        }

        /// <summary>
        /// Replaces the selection with the content of the clipboard.
        /// </summary>
        /// <param name="isChanged">True if something was replaced or added.</param>
        public override void Paste(out bool isChanged)
        {
            isChanged = false;
            ((IFocusInternalControllerView)StateView.ControllerView).ReplaceWithClipboardContent(StateView.State);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return StateView.State.ToString();
        }
        #endregion
    }
}

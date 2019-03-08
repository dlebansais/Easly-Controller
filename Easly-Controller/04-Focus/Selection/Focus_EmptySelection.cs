namespace EaslyController.Focus
{
    using System.Windows;

    /// <summary>
    /// An empty selection.
    /// </summary>
    public interface IFocusEmptySelection : IFocusSelection
    {
    }

    /// <summary>
    /// An empty selection.
    /// </summary>
    public class FocusEmptySelection : FocusSelection, IFocusEmptySelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusEmptySelection"/> class.
        /// </summary>
        /// <param name="stateView">The selected state view.</param>
        public FocusEmptySelection(IFocusNodeStateView stateView)
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
        }

        /// <summary>
        /// Copy the selection in the clipboard then removes it.
        /// </summary>
        public override void Cut()
        {
        }

        /// <summary>
        /// Replaces the selection with the content of the clipboard.
        /// </summary>
        public override void Paste()
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return "Empty";
        }
        #endregion
    }
}

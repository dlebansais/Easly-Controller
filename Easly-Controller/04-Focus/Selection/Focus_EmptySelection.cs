namespace EaslyController.Focus
{
    using System.Diagnostics;
    using System.Windows;
    using EaslyController.Controller;

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

            if (ClipboardHelper.TryReadText(out string Text) && Text.Length > 0)
            {
                IFocusControllerView ControllerView = StateView.ControllerView;
                if (ControllerView.Focus is IFocusCommentFocus AsCommentFocus)
                {
                    string FocusedText = ControllerView.FocusedText;
                    int CaretPosition = ControllerView.CaretPosition;
                    Debug.Assert(CaretPosition >= 0 && CaretPosition <= FocusedText.Length);

                    string Content = FocusedText.Substring(0, CaretPosition) + Text + FocusedText.Substring(CaretPosition);

                    IFocusController Controller = StateView.ControllerView.Controller;
                    Controller.ChangeComment(AsCommentFocus.CellView.StateView.State.ParentIndex, Content);

                    isChanged = true;
                }
                else if (ControllerView.Focus is IFocusStringContentFocus AsStringContentFocus)
                {
                    string FocusedText = ControllerView.FocusedText;
                    int CaretPosition = ControllerView.CaretPosition;
                    Debug.Assert(CaretPosition >= 0 && CaretPosition <= FocusedText.Length);

                    string Content = FocusedText.Substring(0, CaretPosition) + Text + FocusedText.Substring(CaretPosition);

                    IFocusStringContentFocusableCellView CellView = AsStringContentFocus.CellView;

                    IFocusController Controller = StateView.ControllerView.Controller;
                    Controller.ChangeText(CellView.StateView.State.ParentIndex, CellView.PropertyName, Content);

                    isChanged = true;
                }
            }
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

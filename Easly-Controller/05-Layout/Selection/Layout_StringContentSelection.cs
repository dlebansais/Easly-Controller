namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// A selection of part of a string property.
    /// </summary>
    public interface ILayoutStringContentSelection : IFocusStringContentSelection, ILayoutContentSelection, ILayoutTextSelection
    {
    }

    /// <summary>
    /// A selection of part of a comment.
    /// </summary>
    public class LayoutStringContentSelection : FocusStringContentSelection, ILayoutStringContentSelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutStringContentSelection"/> class.
        /// </summary>
        /// <param name="stateView">The state view that encompasses the selection.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="start">Index of the first character in the selected text.</param>
        /// <param name="end">Index following the last character in the selected text.</param>
        public LayoutStringContentSelection(ILayoutNodeStateView stateView, string propertyName, int start, int end)
            : base(stateView, propertyName, start, end)
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

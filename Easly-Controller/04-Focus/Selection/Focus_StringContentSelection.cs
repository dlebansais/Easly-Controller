namespace EaslyController.Focus
{
    /// <summary>
    /// A selection of part of a string property.
    /// </summary>
    public interface IFocusStringContentSelection : IFocusContentSelection, IFocusTextSelection
    {
    }

    /// <summary>
    /// A selection of part of a comment.
    /// </summary>
    public class FocusStringContentSelection : FocusSelection, IFocusStringContentSelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusStringContentSelection"/> class.
        /// </summary>
        /// <param name="stateView">The state view that encompasses the selection.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="start">Index of the first character in the selected text.</param>
        /// <param name="end">Index following the last character in the selected text.</param>
        public FocusStringContentSelection(IFocusNodeStateView stateView, string propertyName, int start, int end)
            : base(stateView)
        {
            Start = start;
            End = end;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The property name.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Index of the first character in the selected text.
        /// </summary>
        public int Start { get; private set; }

        /// <summary>
        /// Index following the last character in the selected text.
        /// </summary>
        public int End { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Updates the text selection with new start and end values.
        /// </summary>
        /// <param name="start">The new start value.</param>
        /// <param name="end">The new end value.</param>
        public virtual void Update(int start, int end)
        {
            Start = start;
            End = end;
        }
        #endregion
    }
}

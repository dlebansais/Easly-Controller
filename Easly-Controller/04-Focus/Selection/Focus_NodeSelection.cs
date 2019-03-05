namespace EaslyController.Focus
{
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

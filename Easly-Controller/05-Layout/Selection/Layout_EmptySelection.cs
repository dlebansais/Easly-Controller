namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// An empty selection.
    /// </summary>
    public interface ILayoutEmptySelection : IFocusEmptySelection, ILayoutSelection
    {
    }

    /// <summary>
    /// An empty selection.
    /// </summary>
    public class LayoutEmptySelection : FocusEmptySelection, ILayoutEmptySelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutEmptySelection"/> class.
        /// </summary>
        /// <param name="stateView">The selected state view.</param>
        public LayoutEmptySelection(ILayoutNodeStateView stateView)
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
        public virtual void Print()
        {
        }
        #endregion
    }
}

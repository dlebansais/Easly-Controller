namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// A selection of discrete property (enum or boolean).
    /// </summary>
    public interface ILayoutDiscreteContentSelection : IFocusDiscreteContentSelection, ILayoutContentSelection
    {
    }

    /// <summary>
    /// A selection of discrete property (enum or boolean).
    /// </summary>
    public class LayoutDiscreteContentSelection : FocusDiscreteContentSelection, ILayoutDiscreteContentSelection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutDiscreteContentSelection"/> class.
        /// </summary>
        /// <param name="stateView">The state view that encompasses the selection.</param>
        /// <param name="propertyName">The property name.</param>
        public LayoutDiscreteContentSelection(ILayoutNodeStateView stateView, string propertyName)
            : base(stateView, propertyName)
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

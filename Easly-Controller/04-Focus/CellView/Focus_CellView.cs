namespace EaslyController.Focus
{
    using System.Diagnostics;
    using EaslyController.Frame;

    /// <summary>
    /// Atomic cell view of a component in a node.
    /// </summary>
    public interface IFocusCellView : IFrameCellView
    {
        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        new IFocusNodeStateView StateView { get; }

        /// <summary>
        /// Updates the focus chain with cells in the tree.
        /// </summary>
        /// <param name="focusChain">The list of focusable cell views found in the tree.</param>
        void UpdateFocusChain(IFocusFocusableCellViewList focusChain);
    }

    /// <summary>
    /// Atomic cell view of a component in a node.
    /// </summary>
    internal abstract class FocusCellView : FrameCellView, IFocusCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        public FocusCellView(IFocusNodeStateView stateView)
            : base(stateView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        public new IFocusNodeStateView StateView { get { return (IFocusNodeStateView)base.StateView; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Updates the focus chain with cells in the tree.
        /// </summary>
        /// <param name="focusChain">The list of focusable cell views found in the tree.</param>
        public abstract void UpdateFocusChain(IFocusFocusableCellViewList focusChain);
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusCellView AsCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsCellView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}

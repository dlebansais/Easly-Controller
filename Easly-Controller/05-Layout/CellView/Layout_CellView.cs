namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Focus;

    /// <summary>
    /// Atomic cell view of a component in a node.
    /// </summary>
    public interface ILayoutCellView : IFocusCellView
    {
        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        new ILayoutNodeStateView StateView { get; }
    }

    /// <summary>
    /// Atomic cell view of a component in a node.
    /// </summary>
    internal abstract class LayoutCellView : FocusCellView, ILayoutCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        public LayoutCellView(ILayoutNodeStateView stateView)
            : base(stateView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        public new ILayoutNodeStateView StateView { get { return (ILayoutNodeStateView)base.StateView; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ILayoutCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutCellView AsCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsCellView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}

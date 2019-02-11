namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Focus;

    /// <summary>
    /// A collection of cell views organized in a column.
    /// </summary>
    public interface ILayoutColumn : IFocusColumn, ILayoutCellViewCollection
    {
    }

    /// <summary>
    /// A collection of cell views organized in a column.
    /// </summary>
    internal class LayoutColumn : FocusColumn, ILayoutColumn
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutColumn"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="cellViewList">The list of child cell views.</param>
        public LayoutColumn(ILayoutNodeStateView stateView, ILayoutCellViewList cellViewList)
            : base(stateView, cellViewList)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The collection of child cells.
        /// </summary>
        public new ILayoutCellViewList CellViewList { get { return (ILayoutCellViewList)base.CellViewList; } }

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

            if (!comparer.IsSameType(other, out LayoutColumn AsColumn))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsColumn))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}

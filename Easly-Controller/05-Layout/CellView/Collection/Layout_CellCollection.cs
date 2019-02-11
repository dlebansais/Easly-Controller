namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Controller;
    using EaslyController.Focus;

    /// <summary>
    /// Base interface for collection of cell views.
    /// </summary>
    public interface ILayoutCellViewCollection : IFocusCellViewCollection, ILayoutCellView, ILayoutAssignableCellView
    {
        /// <summary>
        /// The collection of child cells.
        /// </summary>
        new ILayoutCellViewList CellViewList { get; }
    }

    /// <summary>
    /// Base interface for collection of cell views.
    /// </summary>
    internal abstract class LayoutCellViewCollection : FocusCellViewCollection, ILayoutCellViewCollection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutCellViewCollection"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="cellViewList">The list of child cell views.</param>
        public LayoutCellViewCollection(ILayoutNodeStateView stateView, ILayoutCellViewList cellViewList)
            : base(stateView, cellViewList)
        {
            CellSize = MeasureHelper.InvalidSize;
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

        /// <summary>
        /// Size of the cell.
        /// </summary>
        public Size CellSize { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Measures the cell.
        /// </summary>
        public abstract void Measure();
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

            if (!comparer.IsSameType(other, out LayoutCellViewCollection AsCellViewCollection))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsCellViewCollection))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}

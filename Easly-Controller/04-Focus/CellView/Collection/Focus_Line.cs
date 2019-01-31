namespace EaslyController.Focus
{
    using System.Diagnostics;
    using EaslyController.Frame;

    /// <summary>
    /// A collection of cell views organized in a column.
    /// </summary>
    public interface IFocusLine : IFrameLine, IFocusCellViewCollection
    {
    }

    /// <summary>
    /// A collection of cell views organized in a column.
    /// </summary>
    internal class FocusLine : FrameLine, IFocusLine
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusLine"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="cellViewList">The list of child cell views.</param>
        public FocusLine(IFocusNodeStateView stateView, IFocusCellViewList cellViewList)
            : base(stateView, cellViewList)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The collection of child cells.
        /// </summary>
        public new IFocusCellViewList CellViewList { get { return (IFocusCellViewList)base.CellViewList; } }

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
        public virtual void UpdateFocusChain(IFocusFocusableCellViewList focusChain)
        {
            foreach (IFocusCellView Item in CellViewList)
                Item.UpdateFocusChain(focusChain);
        }
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

            if (!comparer.IsSameType(other, out FocusLine AsLine))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsLine))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}

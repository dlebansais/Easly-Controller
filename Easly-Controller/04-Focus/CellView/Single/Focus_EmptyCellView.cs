namespace EaslyController.Focus
{
    using System.Diagnostics;
    using EaslyController.Frame;

    /// <summary>
    /// Cell view with no content and that is not displayed.
    /// </summary>
    public interface IFocusEmptyCellView : IFrameEmptyCellView, IFocusCellView
    {
    }

    /// <summary>
    /// Cell view with no content and that is not displayed.
    /// </summary>
    internal class FocusEmptyCellView : FrameEmptyCellView, IFocusEmptyCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusEmptyCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        public FocusEmptyCellView(IFocusNodeStateView stateView, IFocusCellViewCollection parentCellView)
            : base(stateView, parentCellView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        public new IFocusNodeStateView StateView { get { return (IFocusNodeStateView)base.StateView; } }

        /// <summary>
        /// The collection of cell views containing this view. Null for the root of the cell tree.
        /// </summary>
        public new IFocusCellViewCollection ParentCellView { get { return (IFocusCellViewCollection)base.ParentCellView; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Updates the focus chain with cells in the tree.
        /// </summary>
        /// <param name="focusChain">The list of focusable cell views found in the tree.</param>
        public virtual void UpdateFocusChain(IFocusFocusList focusChain)
        {
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

            if (!comparer.IsSameType(other, out FocusEmptyCellView AsEmptyCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsEmptyCellView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}

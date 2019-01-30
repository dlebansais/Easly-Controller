namespace EaslyController.Focus
{
    using System.Diagnostics;
    using EaslyController.Frame;

    /// <summary>
    /// A cell view for a block state.
    /// </summary>
    public interface IFocusBlockCellView : IFrameBlockCellView, IFocusCellView
    {
        /// <summary>
        /// The collection of cell views containing this view.
        /// </summary>
        new IFocusCellViewCollection ParentCellView { get; }

        /// <summary>
        /// The block state view of the state associated to this cell.
        /// </summary>
        new IFocusBlockStateView BlockStateView { get; }
    }

    /// <summary>
    /// A leaf of the cell view tree for a child state.
    /// </summary>
    public class FocusBlockCellView : FrameBlockCellView, IFocusBlockCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusBlockCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view.</param>
        /// <param name="blockStateView">The block state view of the state associated to this cell.</param>
        public FocusBlockCellView(IFocusNodeStateView stateView, IFocusCellViewCollection parentCellView, IFocusBlockStateView blockStateView)
            : base(stateView, parentCellView, blockStateView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The collection of cell views containing this view.
        /// </summary>
        public new IFocusCellViewCollection ParentCellView { get { return (IFocusCellViewCollection)base.ParentCellView; } }

        /// <summary>
        /// The block state view of the state associated to this cell.
        /// </summary>
        public new IFocusBlockStateView BlockStateView { get { return (IFocusBlockStateView)base.BlockStateView; } }

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
            BlockStateView.UpdateFocusChain(focusChain);
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

            if (!(other is IFocusBlockCellView AsBlockCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBlockCellView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}

using EaslyController.Frame;
using System.Diagnostics;

namespace EaslyController.Focus
{
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
        /// Initializes an instance of <see cref="FocusBlockCellView"/>.
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
                return false;

            if (!base.IsEqual(comparer, AsBlockCellView))
                return false;

            return true;
        }
        #endregion
    }
}

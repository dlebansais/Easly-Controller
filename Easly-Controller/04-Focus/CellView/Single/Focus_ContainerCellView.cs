using EaslyController.Frame;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// A leaf of the cell view tree for a child state.
    /// </summary>
    public interface IFocusContainerCellView : IFrameContainerCellView, IFocusCellView, IFocusAssignableCellView
    {
        /// <summary>
        /// The collection of cell views containing this view.
        /// </summary>
        new IFocusCellViewCollection ParentCellView { get; }

        /// <summary>
        /// The state view of the state associated to this cell.
        /// </summary>
        new IFocusNodeStateView ChildStateView { get; }
    }

    /// <summary>
    /// A leaf of the cell view tree for a child state.
    /// </summary>
    public class FocusContainerCellView : FrameContainerCellView, IFocusContainerCellView
    {
        #region Init
        /// <summary>
        /// Initializes an instance of <see cref="FocusContainerCellView"/>.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view.</param>
        /// <param name="childStateView">The state view of the state associated to this cell.</param>
        public FocusContainerCellView(IFocusNodeStateView stateView, IFocusCellViewCollection parentCellView, IFocusNodeStateView childStateView)
            : base(stateView, parentCellView, childStateView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The collection of cell views containing this view.
        /// </summary>
        public new IFocusCellViewCollection ParentCellView { get { return (IFocusCellViewCollection)base.ParentCellView; } }

        /// <summary>
        /// The state view of the state associated to this cell.
        /// </summary>
        public new IFocusNodeStateView ChildStateView { get { return (IFocusNodeStateView)base.ChildStateView; } }

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
            ChildStateView.UpdateFocusChain(focusChain);
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

            if (!(other is IFocusContainerCellView AsContainerCellView))
                return false;

            if (!base.IsEqual(comparer, AsContainerCellView))
                return false;

            return true;
        }
        #endregion
    }
}

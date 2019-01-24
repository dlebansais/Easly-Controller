using EaslyController.Frame;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// Cell view for components that are displayed.
    /// </summary>
    public interface IFocusVisibleCellView : IFrameVisibleCellView, IFocusCellView
    {
        /// <summary>
        /// The frame that created this cell view.
        /// </summary>
        new IFocusFrame Frame { get; }
    }

    /// <summary>
    /// Cell view for components that are displayed.
    /// </summary>
    public class FocusVisibleCellView : FrameVisibleCellView, IFocusVisibleCellView
    {
        #region Init
        /// <summary>
        /// Initializes an instance of <see cref="FocusVisibleCellView"/>.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="frame">The frame that created this cell view.</param>
        public FocusVisibleCellView(IFocusNodeStateView stateView, IFocusFrame frame)
            : base(stateView, frame)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        public new IFocusNodeStateView StateView { get { return (IFocusNodeStateView)base.StateView; } }

        /// <summary>
        /// The frame that created this cell view.
        /// </summary>
        public new IFocusFrame Frame { get { return (IFocusFrame)base.Frame; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Updates the focus chain with cells in the tree.
        /// </summary>
        /// <param name="focusChain">The list of focusable cell views found in the tree.</param>
        public virtual void UpdateFocusChain(IFocusFocusableCellViewList focusChain)
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

            if (!(other is IFocusVisibleCellView AsVisibleCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsVisibleCellView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}

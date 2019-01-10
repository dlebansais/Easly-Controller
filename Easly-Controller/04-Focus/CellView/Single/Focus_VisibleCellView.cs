using EaslyController.Frame;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// Cell view for components that are displayed.
    /// </summary>
    public interface IFocusVisibleCellView : IFrameVisibleCellView, IFocusCellView
    {
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
        public FocusVisibleCellView(IFocusNodeStateView stateView)
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
                return false;

            if (!base.IsEqual(comparer, AsVisibleCellView))
                return false;

            return true;
        }
        #endregion
    }
}

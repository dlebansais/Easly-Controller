using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Cell view for components that can receive the focus.
    /// </summary>
    public interface IFrameFocusableCellView : IFrameVisibleCellView
    {
    }

    /// <summary>
    /// Cell view for components that can receive the focus.
    /// </summary>
    public abstract class FrameFocusableCellView : FrameVisibleCellView, IFrameFocusableCellView
    {
        #region Init
        /// <summary>
        /// Initializes an instance of <see cref="FrameFocusableCellView"/>.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        public FrameFocusableCellView(IFrameNodeStateView stateView)
            : base(stateView)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameCellView"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameFocusableCellView AsFocusableCellView))
                return false;

            if (!base.IsEqual(comparer, AsFocusableCellView))
                return false;

            return true;
        }
        #endregion
    }
}

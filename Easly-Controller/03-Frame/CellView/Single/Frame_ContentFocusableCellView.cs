using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Cell view for components that can receive the focus and be modified.
    /// </summary>
    public interface IFrameContentFocusableCellView : IFrameFocusableCellView
    {
    }

    /// <summary>
    /// Cell view for components that can receive the focus and be modified.
    /// </summary>
    public abstract class FrameContentFocusableCellView : FrameFocusableCellView, IFrameContentFocusableCellView
    {
        #region Init
        /// <summary>
        /// Initializes an instance of <see cref="FrameFocusableCellView"/>.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        public FrameContentFocusableCellView(IFrameNodeStateView stateView)
            : base(stateView)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameContentFocusableCellView AsContentFocusableCellView))
                return false;

            if (!base.IsEqual(comparer, AsContentFocusableCellView))
                return false;

            return true;
        }
        #endregion
    }
}

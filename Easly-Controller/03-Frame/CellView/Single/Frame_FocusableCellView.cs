using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Cell view for discrete elements that can receive the focus but are not the component of a node (insertion points, keywords and other decorations)
    /// </summary>
    public interface IFrameFocusableCellView : IFrameVisibleCellView
    {
    }

    /// <summary>
    /// Cell view for discrete elements that can receive the focus but are not the component of a node (insertion points, keywords and other decorations)
    /// </summary>
    public class FrameFocusableCellView : FrameVisibleCellView, IFrameFocusableCellView
    {
        #region Init
        /// <summary>
        /// Initializes an instance of <see cref="FrameFocusableCellView"/>.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="frame">The frame that created this cell view.</param>
        public FrameFocusableCellView(IFrameNodeStateView stateView, IFrameFrame frame)
            : base(stateView, frame)
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

            if (!(other is IFrameFocusableCellView AsFocusableCellView))
                return false;

            if (!base.IsEqual(comparer, AsFocusableCellView))
                return false;

            return true;
        }
        #endregion
    }
}
